using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenFL.Core;
using OpenFL.Core.DataObjects.ExecutableDataObjects;
using OpenFL.Core.DataObjects.SerializableDataObjects;
using OpenFL.Core.Parsing.StageResults;
using OpenFL.Core.ProgramChecks;
using OpenFL.Debugging;
using OpenFL.Editor.Properties;
using OpenFL.Editor.Utils;

using ThemeEngine;

using Utility.Exceptions;
using Utility.FastString;
using Utility.WindowsForms.Forms;

namespace OpenFL.Editor.Forms
{
    public class FL
    {

        public readonly FLDataContainer FLContainer;
        private Func<Color> ErrorColor;
        private Func<Color> SuccessColor;
        private Func<FLDebuggerSettings> Settings;

        private ContainerForm previewForm;

        private PictureBox previewPicture;

        private Task previewTask;

        #region FL Build

        public FL(Func<FLDebuggerSettings> settings, FLDataContainer container, Func<Color> errorColor, Func<Color> successColor)
        {
            Settings = settings;
            FLContainer = container;
            ErrorColor = errorColor;
            SuccessColor = successColor;
        }


        private Task<SerializableFLProgram> ParseProgram(List<FLProgramCheck> checks, string filename, string[] defines)
        {
            FLContainer.CheckBuilder.Detach(false);
            FLContainer.CheckBuilder.RemoveAllProgramChecks();
            List<FLProgramCheck> si = checks;
            si.ForEach(x => FLContainer.CheckBuilder.AddProgramCheck(x));
            FLContainer.CheckBuilder.Attach(FLContainer.Parser, true);


            Task<SerializableFLProgram> loadT =
                new Task<SerializableFLProgram>(
                                                () =>
                                                    FLContainer.Parser.Process(
                                                                               new FLParserInput(
                                                                                                 filename,
                                                                                                 defines
                                                                                                )
                                                                              )
                                               );
            return loadT;
        }


        public bool Build(string file, List<FLProgramCheck> checks, string[] defines, Action<string> output, Action<string, Color> logMessage)
        {
            if (FLContainer.CheckBuilder.IsAttached)
            {
                FLContainer.CheckBuilder.Detach(false);
            }


            string source = File.ReadAllText(file);
            Task<SerializableFLProgram> loadT = ParseProgram(checks, file, defines);

            loadT.Start();
            while (!loadT.IsCompleted)
            {
                Application.DoEvents();
            }

            if (loadT.IsFaulted)
            {
                FLContainer.SerializedProgram = null;

                string s = $"Errors: {loadT.Exception.InnerExceptions.Count}\n";

                foreach (Exception exceptionInnerException in loadT.Exception.InnerExceptions)
                {
                    Exception ex = TaskUtils.GetInnerIfAggregate(exceptionInnerException);
                    s += $"PARSE ERROR:\n\t{ex.Message}\n";
                    if (Settings().LogParserStacktrace)
                    {
                        s += $"\t\t{ex.StackTrace.Split('\n').Unpack("\n\t\t")}\n";
                    }
                }

                output(source);
                logMessage(s, ErrorColor());
                return false;
            }
            else
            {
                FLContainer.SerializedProgram = loadT.Result;
                string s = FLContainer.SerializedProgram.ToString();

                output(s);
                logMessage("Parsed Successfully.\n", SuccessColor());
                return true;
            }
        }

        public void ClosePreview()
        {
            previewForm?.Close();
            previewForm = null;
            previewPicture = null;
        }

        public void OpenPreview(Action<string, Color> logMessage)
        {
            if (previewForm == null || previewForm.IsDisposed)
            {
                previewPicture = new PictureBox();
                previewPicture.Dock = DockStyle.Fill;
                previewPicture.Image = Resources.OpenFL;
                previewPicture.SizeMode = PictureBoxSizeMode.Zoom;


                StyleManager.RegisterControl(previewPicture, "default", "preview");

                previewForm = ContainerForm.CreateContainer(
                                                            previewPicture,
                                                            null,
                                                            "Preview: ",
                                                            Resources.OpenFL_Icon,
                                                            FormBorderStyle.SizableToolWindow
                                                           );

                ComputePreview(logMessage);
            }
            else
            {
                previewForm.Show();
            }
        }

        public void ComputePreview(Action<string, Color> logMessage)
        {
            if (previewPicture != null && (previewTask == null || previewTask.IsCompleted))
            {
                previewTask = new Task(
                                       () =>
                                       {
                                           if (FLContainer.SerializedProgram == null)
                                           {
                                               throw new InvalidOperationException();
                                           }

                                           FLProgram pro = null;
                                           try
                                           {
                                               pro = FLContainer.SerializedProgram.Initialize(
                                                                                              FLContainer
                                                                                             );

                                               pro.Run(FLContainer.CreateBuffer(
                                                                                Settings().ResX,
                                                                                Settings().ResY,
                                                                                Settings().ResZ,
                                                                                "Preview Buffer"
                                                                               ),
                                                       true
                                                      );
                                               if (previewPicture != null)
                                               {
                                                   previewPicture.Image = pro.GetActiveBitmap();
                                               }

                                               pro.FreeResources();
                                           }
                                           catch (Exception ex)
                                           {
                                               pro?.FreeResources();
                                               if (previewPicture != null)
                                               {
                                                   previewPicture.Image = SystemIcons.Error.ToBitmap();
                                               }

                                               throw ex;
                                           }
                                       }
                                      );
                Task t = new Task(
                                  () =>
                                  {
                                      while (!previewTask.IsCompleted)
                                      {
                                          Thread.Sleep(100);
                                      }

                                      if (previewTask.IsFaulted)
                                      {
                                          string s = $"Errors: {previewTask.Exception.InnerExceptions.Count}\n";

                                          foreach (Exception exceptionInnerException in previewTask
                                                                                        .Exception.InnerExceptions)
                                          {
                                              Exception ex = TaskUtils.GetInnerIfAggregate(exceptionInnerException);
                                              s +=
                                                  $"PROGRAM ERROR:\n\t{ex.Message}\n";
                                              if (Settings().LogProgramStacktrace)
                                              {
                                                  s += $"\t\t{ex.StackTrace.Split('\n').Unpack("\n\t\t")}\n";
                                              }
                                          }

                                          logMessage(s, ErrorColor());
                                      }
                                  }
                                 );
                previewTask.Start();
                t.Start();
            }
        }

        public bool Parse(string file, string inputSource, List<FLProgramCheck> checks, string[] defines, Action<string> output, Action<string, Color> logMessage)
        {
            bool ret = WriteAndBuild(file, inputSource, checks, defines, output, logMessage);
            ComputePreview(logMessage);
            return ret;
        }

        public bool WriteAndBuild(string file, string inputSource, List<FLProgramCheck> checks, string[] defines, Action<string> output, Action<string, Color> logMessage)
        {
            File.WriteAllText(file, inputSource);
            return Build(file, checks, defines, output, logMessage);
        }


        public bool StartDebugger(string file, string inputSource, List<FLProgramCheck> checks, string[] defines, Action<string> output, Action<string, Color> logMessage)
        {
            if (FLContainer.SerializedProgram == null)
            {
                bool ret = WriteAndBuild(file, inputSource, checks, defines, output, logMessage);
                if (!ret) return false;
            }

            if (FLContainer.SerializedProgram == null)
            {
                return false;
            }

            if (Settings().ExperimentalDebuggerMultiThread)
            {
                Task t = new Task(
                                  () =>
                                  {
                                      FLDebugger.StartContainer(
                                                                FLContainer,
                                                                FLContainer.SerializedProgram.Initialize(
                                                                                                         FLContainer
                                                                                                        ),
                                                                Settings().ResX,
                                                                Settings().ResY,
                                                                Settings().ResZ
                                                               );
                                  }
                                 );
                t.Start();
                while (!t.IsCompleted)
                {
                    Application.DoEvents();
                }

                if (t.IsFaulted)
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    FLDebugger.StartContainer(
                                              FLContainer,
                                              FLContainer.SerializedProgram.Initialize(
                                                                                       FLContainer
                                                                                      ),
                                              Settings().ResX,
                                              Settings().ResY,
                                              Settings().ResZ
                                             );
                    logMessage($"Program Build Succeeded.\n", SuccessColor());
                }
                catch (Byt3Exception exception)
                {
                    logMessage($"Program Build Failed: '{exception.Message}'\n", ErrorColor());
                    return false;
                }
            }

            return true;
        }

        #endregion

    }
}