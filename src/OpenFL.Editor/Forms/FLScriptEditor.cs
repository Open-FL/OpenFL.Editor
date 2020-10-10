using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using OpenFL.Editor.Forms.Debug;
using OpenFL.Editor.Properties;
using OpenFL.Editor.Utils;

namespace OpenFL.Editor.Forms
{
    public partial class FLScriptEditor : Form
    {

        #region Utility

        public void OpenFileDialog()
        {
            if (openFLScript.ShowDialog() == DialogResult.OK)
            {
                Path = openFLScript.FileName;
                rtbIn.Text = File.ReadAllText(Path);
            }
        }

        #endregion

        #region Fields / Properties

        public Color ErrorColor { get; set; } = Color.Red;

        public Color SuccessColor { get; set; } = Color.Green;

        public Color FLComments { get; set; } = Color.Aqua;

        public Color FLDefines { get; set; } = Color.Orange;

        public Color FLFunctions { get; set; } = Color.Crimson;

        public Color PPKeys { get; set; } = Color.Green;

        public event Action OnLoadComplete;

        public static readonly string TempEditorContentPath = System.IO.Path.Combine(
             System.IO.Path.GetDirectoryName(
                                             Assembly
                                                 .GetExecutingAssembly()
                                                 .Location
                                            ),
             "tempeditorcontent_" +
             System.IO.Path
                   .GetFileNameWithoutExtension(
                                                System.IO.Path
                                                      .GetRandomFileName()
                                               ) +
             ".fl"
            );

        public MenuStrip Toolbar => msToolbar;

        private static readonly string DEFAULT_SCRIPT =
            $"{FLKeywords.EntryFunctionKey}:\n\tsetactive 3\n\tSet_v 1\n\tsetactive 0 1 2\n\tSet_v 1";


        public readonly FLEditorPluginHost PluginHost;

        public static string ConfigPath
        {
            get
            {
                string ret = System.IO.Path.Combine(PluginPaths.InternalSystemConfigPath, "editor");
                Directory.CreateDirectory(ret);
                return ret;
            }
        }

        public bool IsFullyLoaded { get; private set; }

        public string Path
        {
            get
            {
                if (_path == null)
                {
                    _path = TempEditorContentPath;
                }

                return _path;
            }
            set => _path = value;
        }

        private event Action<string> OnLog;

        public event Action<string> OnLoadStartupFile;


        private string _path;

        public FLDataContainer FLContainer
        {
            get => FLImplementation?.FLContainer;
            set
            {
                if (FLImplementation == null)
                {
                    FLImplementation = new FL(() => Settings, value, () => ErrorColor, () => SuccessColor);
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        public FL FLImplementation { get; private set; }

        private bool ignoreChanged;

        public string WrittenText => rtbIn.Text;

        public Control PanelToolbar => panelToolbar;

        public static FLDebuggerSettings Settings { get; internal set; } = new FLDebuggerSettings();

        public List<FLProgramCheck> ProgramChecks => lbOptimizations.CheckedItems.Cast<FLProgramCheck>().ToList();

        public string[] Defines => new[] { cbBuildMode.SelectedItem.ToString().ToUpper() };

        #endregion

        #region Constructor

        public FLScriptEditor()
        {
            PluginHost = new FLEditorPluginHost(this);
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        public FLScriptEditor(string path) : this()
        {
            Path = path;
            if (path.EndsWith(".fl"))
            {
                rtbIn.WriteSource(File.ReadAllText(Path), FLDefines, PPKeys, FLFunctions, FLComments);
            }
        }

        public FLScriptEditor(string path, string workingDir) : this(path)
        {
            Directory.SetCurrentDirectory(workingDir);
        }

        #endregion

        #region Logging

        public void AddLogger(Action<string> onChunkReceive)
        {
            OnLog += onChunkReceive;
        }

        public void RemoveLogger(Action<string> onChunkReceive)
        {
            OnLog -= onChunkReceive;
        }

        private void SetLogOutput(string r, Color color)
        {
            string pr = "";
            pr += $"File: {Path}\n";
            pr += r;
            pr += "____________________________________________________\n";
            rtbParserOutput.AppendText(pr, color, rtbParserOutput.BackColor);
        }

        #endregion

        #region Setup / Startup

        private void EditorLoad(object sender, EventArgs e)
        {
            StyleManager.RegisterControls(this, "editor");

            lblVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            rtbIn.WriteSource(DEFAULT_SCRIPT, FLDefines, PPKeys, FLFunctions, FLComments);
            CheckForIllegalCrossThreadCalls = false;


            #region MyRegion

            Icon = Resources.OpenFL_Icon;
            Closing += FLScriptEditor_Closing;

            FLDebugger.Initialize(
                                  program =>
                                  {
                                      FLDebuggerWindow cv = new FLDebuggerWindow(program);
                                      PluginManager.LoadPlugins(cv);
                                      return cv;
                                  }
                                 );
            DoubleBuffered = true;

            MakeResizable(panelConsoleOut, ResizeableControl.EdgeEnum.Top, rtbParserOutput);
            MakeResizable(panelInput, ResizeableControl.EdgeEnum.Right, rtbIn);


            panelCodeArea.Resize += PanelCodeArea_Resize;

            tmrConsoleRefresh.Start();


            if (Settings.Abort)
            {
                Application.Exit();
                return;
            }


            OnLoadStartupFile?.Invoke(Path);


            rtbParserOutput.TextChanged += RtbParserOutputTextChanged;

            FLContainer.SetCheckBuilder(
                                        new FLProgramCheckBuilder(
                                                                  FLContainer.InstructionSet,
                                                                  FLContainer.BufferCreator,
                                                                  FLProgramCheckType.All
                                                                 )
                                       );

            for (int i = 0; i < FLContainer.CheckBuilder.ProgramChecks.Count; i++)
            {
                FLProgramCheck type = FLContainer.CheckBuilder.ProgramChecks[i];
                lbOptimizations.Items.Add(type);
            }

            cbBuildMode.Items.AddRange(
                                       Enum.GetNames(typeof(FLProgramCheckType)).Select(x => x.Trim('[', ']')).ToArray()
                                      );
            cbBuildMode.SelectedIndex = 0;

            #endregion


            IsFullyLoaded = true;

            OnLoadComplete?.Invoke();


            //StartupSequence.LoadPlugins(this);
        }

        private void CloseEditor()
        {
            if (File.Exists(TempEditorContentPath))
            {
                File.Delete(TempEditorContentPath);
            }

            File.Delete(PluginPaths.InitPluginListFile);

            Application.Exit();
        }

        private void CreatePopOut(Button button, Panel container, Control child, string name)
        {
            button.Visible = false;
            int originalHeight = container.Height;
            ContainerForm.CreateContainer(
                                          child,
                                          (control, args) =>
                                          {
                                              container.Height = originalHeight;
                                              button.Visible = true;
                                              button.BringToFront();
                                          },
                                          name,
                                          Icon,
                                          FormBorderStyle.Sizable
                                         );
            container.Height = 0;
        }

        private static void MakeResizable(
            Control control, ResizeableControl.EdgeEnum activeEdges,
            params Control[] dockedChildren)
        {
            ResizeableControl ret = new ResizeableControl(control, dockedChildren);
            ret.DrawOutline = true;
            ret.OutlineColor = CodeViewHelper.SourceBackColor;
            ret.Edges = activeEdges;
        }

        #endregion

        #region UI

        private void RefreshInput()
        {
            ignoreChanged = true;
            string src = rtbIn.Text;
            if (src.Count(c => c == '\n') > 100)
            {
                rtbIn.Text = src; //no colorcoding to prevent loading forever
                return;
            }

            rtbIn.Enabled = false;
            rtbIn.WriteSource(src, FLDefines, PPKeys, FLFunctions, FLComments);
            rtbIn.Enabled = true;

            //Path = "";
            ignoreChanged = false;

            if (cbAutoBuild.Checked)
            {
                WriteAndBuild();
                ComputePreview();
            }
        }

        public void Build()
        {
            FLImplementation.Build(Path, ProgramChecks, Defines, SetOutput, SetLogOutput);
        }

        public void Parse()
        {
            FLImplementation.Parse(Path, rtbIn.Text, ProgramChecks, Defines, SetOutput, SetLogOutput);
        }

        public void StartDebugger()
        {
            FLImplementation.StartDebugger(Path, rtbIn.Text, ProgramChecks, Defines, SetOutput, SetLogOutput);
        }

        public void WriteAndBuild()
        {
            FLImplementation.WriteAndBuild(Path, rtbIn.Text, ProgramChecks, Defines, SetOutput, SetLogOutput);
        }

        public void ComputePreview()
        {
            FLImplementation.ComputePreview(SetLogOutput);
        }

        private void SetOutput(string output)
        {
            rtbOut.WriteSource(output, FLDefines, PPKeys, FLFunctions, FLComments);
        }

        #region Preview

        private void OpenPreview()
        {
            WriteAndBuild();
            FLImplementation.OpenPreview(SetLogOutput);
        }

        private void ClosePreview()
        {
            FLImplementation.ClosePreview();
        }

        #endregion

        #region Build Modes

        private void SelectBuildMode(FLProgramCheckType checkType)
        {
            for (int i = 0; i < lbOptimizations.Items.Count; i++)
            {
                object lbOptimizationsItem = lbOptimizations.Items[i];
                FLProgramCheck pc = (FLProgramCheck) lbOptimizationsItem;
                lbOptimizations.SetItemChecked(
                                               i,
                                               (pc.CheckType & checkType) != 0
                                              );
            }
        }

        #endregion

        #endregion

        #region UI Event Handlers

        private void tmrConsoleColors_Tick(object sender, EventArgs e)
        {
            tmrConsoleColors.Stop();
            RefreshInput();
        }

        private void FLScriptEditor_Closing(object sender, CancelEventArgs e)
        {
            CloseEditor();
        }


        private void cbLiveView_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbLiveView.Checked)
            {
                ClosePreview();
            }
            else
            {
                OpenPreview();
            }
        }

        private void cbBuildMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectBuildMode(
                            (FLProgramCheckType) Enum.Parse(
                                                            typeof(FLProgramCheckType),
                                                            cbBuildMode.SelectedItem.ToString()
                                                           )
                           );
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbParserOutput.Text = "";
        }

        private void PanelCodeArea_Resize(object sender, EventArgs e)
        {
            panelInput.Width = panelOutput.Width = panelCodeArea.Width / 2;
        }

        private void RtbParserOutputTextChanged(object sender, EventArgs e)
        {
            rtbParserOutput.ScrollToCaret();
        }

        private void rtbIn_TextChanged(object sender, EventArgs e)
        {
            if (ignoreChanged)
            {
                return;
            }

            if (tmrConsoleColors.Enabled)
            {
                tmrConsoleColors.Stop();
            }

            tmrConsoleColors.Start();
        }

        private void tmrConsoleRefresh_Tick(object sender, EventArgs e)
        {
            TextReader reader = StartupSequence.LogReader;

            string r = reader.ReadToEnd();

            OnLog?.Invoke(r);
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {
            StartDebugger();
        }


        private void btnPopOutBuildLog_Click(object sender, EventArgs e)
        {
            CreatePopOut(btnPopOutBuildLog, panelConsoleOut, rtbParserOutput, "Parser Build Output");
        }

        private void btnPopOutInput_Click(object sender, EventArgs e)
        {
            CreatePopOut(btnPopOutInput, panelInput, rtbIn, "FL Editor");
        }

        private void btnPopOutOutput_Click(object sender, EventArgs e)
        {
            CreatePopOut(btnPopOutOutput, panelOutput, rtbOut, "Parser Output");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Parse();
        }

        #endregion

    }
}