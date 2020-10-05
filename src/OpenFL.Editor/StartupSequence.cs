using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenFL.Core;
using OpenFL.Editor.Forms;
using OpenFL.Editor.Forms.Util;
using OpenFL.Editor.Utils;
using OpenFL.Editor.Utils.Plugins;
using OpenFL.ResourceManagement;

using PluginSystem.Core;
using PluginSystem.Core.Pointer;
using PluginSystem.Events.Args;
using PluginSystem.FileSystem;
using PluginSystem.StartupActions;
using PluginSystem.Utility;

using ThemeEngine;
using ThemeEngine.Forms;

using Utility.ADL;
using Utility.ADL.Configs;
using Utility.ADL.Streams;
using Utility.CommandRunner;
using Utility.CommandRunner.BuiltInCommands.SetSettings;
using Utility.Exceptions;
using Utility.IO.VirtualFS;
using Utility.TypeManagement;
using Utility.WindowsForms.CustomControls;
using Utility.WindowsForms.Forms;

namespace OpenFL.Editor
{
    public static class StartupSequence
    {

        private static readonly ADLLogger<LogType> pluginLogger = new ADLLogger<LogType>(
             new ProjectDebugConfig("PM", -1, PrefixLookupSettings.AddPrefixIfAvailable)
            );

        public static TextReader LogReader;
        private static readonly PipeStream LogStream = new PipeStream();
        public static FLDataContainer FlContainer;

        public static Loader loaderForm;
        private static Form mainForm;

        public static event Action CustomStartupActions;

        public static void Startup(string[] args)
        {
            OverrideSettingsFromArguments(ParseStartupArguments(args));


            if (loaderForm == null || loaderForm.IsDisposed)
            {
                loaderForm = new Loader(new Task(() => TaskBasedStartup(args)));
            }

            if (loaderForm.ShowDialog() == DialogResult.Cancel)
            {
                PluginManager.OnLog -= PluginManagerLoadLog;
                return;
            }

            if (FlContainer == null)
            {
                FlContainer = new FLDataContainer();
            }

            ResourceManager.AddUnpacker(new FL2FLCUnpacker(FlContainer));
            ResourceManager.AddUnpacker(new FL2TexUnpacker(FlContainer));
            ResourceManager.AddUnpacker(new FLC2TexUnpacker(FlContainer));
            ResourceManager.AddUnpacker(new FLRESUnpacker());


            PluginManager.OnLog -= PluginManagerLoadLog;
            mainForm = GetRequestedForm();
            if (mainForm is FLScriptEditor ed)
            {
                LoadPlugins(ed);
            }

            PrepareForRun();
            RunForm(mainForm);
        }

        private static void WarmStartup()
        {
            InitializePluginSystem();

            mainForm = GetRequestedForm();
            if (mainForm is FLScriptEditor ed)
            {
                LoadPlugins(ed);
            }

            if (loaderForm == null || loaderForm.IsDisposed)
            {
                loaderForm = new Loader(new Task(() => WarmTaskBasedStartup(8)));
            }

            if (loaderForm.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            PrepareForRun();
            RunFormWarm(mainForm);
        }

        private static void PrepareForRun()
        {
            if (mainForm is FLScriptEditor ed)
            {
                ed.FLContainer = FlContainer;
            }
            else if (mainForm is ExportViewer ev)
            {
                ev.FLContainer = FlContainer;
            }
        }

        private static void SetProgress(string title, int current, int max)
        {
            loaderForm.SetTitle($"{title} {current}/{max}");
            loaderForm.SetProgress(current, max);
        }

        private static void WarmTaskBasedStartup(int maxTasks)
        {
            loaderForm.SetStatus("Loading Settings...");

            loaderForm.Log("Loading Editor Settings.", Color.White);

            if (!LoadEditorSettings())
            {
                return;
            }


            SetProgress("Running Custom Actions", 5, maxTasks);
            loaderForm.Log("Running Custom Actions", Color.White);
            CustomStartupActions?.Invoke();

            SetProgress("Finished", 6, maxTasks);
            loaderForm.SetStatus("Launching Window.");
            loaderForm.SetTitle("Initialization Finished.");
            loaderForm.Log("Opening Program...", Color.White);
        }

        private static void TaskBasedStartup(string[] args)
        {
            int maxTasks = 6;

            loaderForm.SetStatus("Starting up...");

            loaderForm.Log("Initializing FS", Color.White);
            PrepareFileSystem();

            SetProgress("Initializing", 1, maxTasks);


            SetProgress("Initializing", 2, maxTasks);

            InitializePluginSystem();

            loaderForm.Log("Loading Theme Engine.", Color.White);
            LoadThemeEngine();


            loaderForm.Log("Loading Logging System.", Color.White);
            InitializeLogging();
            SetProgress("Initializing", 3, maxTasks);

            loaderForm.Log("Loading Resource System.", Color.White);
            InitializeResourceSystem();
            SetProgress("Initializing", 4, maxTasks);

            WarmTaskBasedStartup(maxTasks);
        }

        public static void PrepareFileSystem()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        }


        public static string[] ParseStartupArguments(string[] args)
        {
            string[] argss = args.Length > 0 && args[0] == "-no-update"
                                 ? args.Reverse().Take(Math.Max(args.Length - 1, 0)).Reverse().ToArray()
                                 : args;
            if (argss.Length == 1 &&
                File.Exists(argss[0]) &&
                (argss[0].EndsWith(".flc") || argss[0].EndsWith(".fl") || argss[0].EndsWith(".flres")))
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), argss[0]);
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));
                argss = new[] { "-ss", "Debugger.ScriptPath:" + path };
            }

            return argss;
        }

        public static void OverrideSettingsFromArguments(string[] args)
        {
            Runner r = new Runner();
            SetSettingsCommand ss = SetSettingsCommand.CreateSettingsCommand(
                                                                             "Debugger",
                                                                             new[] { "--set-settings", "-ss" },
                                                                             FLScriptEditor.Settings
                                                                            );
            r._AddCommand(ss);
            r._RunCommands(args);
        }


        public static void LoadThemeEngine()
        {
            ProgressIndicator.OnCreate = ProgressIndicatorThemeHelper.ApplyTheme;
            if (File.Exists(Path.Combine(PluginPaths.InternalSystemConfigPath, "last_style.txt")))
            {
                string file = File.ReadAllText(
                                               Path.Combine(PluginPaths.InternalSystemConfigPath, "last_style.txt")
                                              );
                StyleManager.ApplyThemeFile(file);
            }
        }

        public static void InitializeLogging()
        {
            OpenFLDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            InternalADLProjectDebugConfig.Settings.MinSeverity = Verbosity.Level1;
            ManifestIODebugConfig.Settings.MinSeverity = Verbosity.Level1;

            LogTextStream ls = new LogTextStream(LogStream);
            Debug.DefaultInitialization();
            Debug.AddOutputStream(ls);
            LogReader = new StreamReader(LogStream);
        }

        public static void InitializeResourceSystem()
        {
            TypeAccumulator.RegisterAssembly(typeof(OpenFLDebugConfig).Assembly);
            ManifestReader.RegisterAssembly(Assembly.GetExecutingAssembly());
            ManifestReader.RegisterAssembly(typeof(FLRunner).Assembly);
            ManifestReader.PrepareManifestFiles(false);
            ManifestReader.PrepareManifestFiles(true);
            EmbeddedFileIOManager.Initialize();
        }


        public static void InitializePluginSystem()
        {
            loaderForm.TopMost = false;
            PluginManager.OnLog += args => pluginLogger.Log(LogType.Log, args.Message, 1);
            PluginManager.OnLog += PluginManagerLoadLog;
            PluginManager.OnInitialized += PluginManagerOnOnInitialized;
            PluginManager.Initialize(
                                     Path.Combine(PluginPaths.EntryDirectory, "data"),
                                     "internal",
                                     "plugins",
                                     (msg, title) =>
                                         StyledMessageBox.Show(
                                                               title,
                                                               msg,
                                                               MessageBoxButtons.YesNo,
                                                               SystemIcons.Question
                                                              ) ==
                                         DialogResult.Yes,
                                     SetProgress,
                                     Path.Combine(PluginPaths.EntryDirectory, "static-data.sd")
                                    );

            loaderForm.TopMost = true;
        }

        private static void PluginManagerOnOnInitialized()
        {
            if (FLScriptEditor.Settings.PluginInit != null)
            {
                loaderForm.Log("Running Initialization", Color.OrangeRed);
                foreach (string s in FLScriptEditor.Settings.InitFiles)
                {
                    if (Directory.Exists(Path.GetFullPath(s)))
                    {
                        string[] plugins = Directory.GetFiles(
                                                              Path.GetFullPath(s),
                                                              "*",
                                                              SearchOption.AllDirectories
                                                             );
                        foreach (string plugin in plugins)
                        {
                            ActionRunner.AddActionToStartup($"{ActionRunner.ADD_ACTIVATE_PACKAGE_ACTION} {plugin}");
                        }
                    }
                }
            }
        }

        private static void PluginManagerLoadLog(LogMessageEventArgs eventargs)
        {
            if (loaderForm != null && !loaderForm.IsDisposed)
            {
                loaderForm.Log(eventargs.Message, Color.Purple);
            }
        }

        public static void LoadPlugins(FLScriptEditor editor)
        {
            AttributeManager.AddAttributeHandler(new ToolbarItemAttributeHandler(editor));


            PluginManager.AddPlugin(
                                    new DefaultToolbarItems(),
                                    new PluginAssemblyPointer(
                                                              "fl-editor-toolbar",
                                                              "",
                                                              "",
                                                              "9.9.9.9",
                                                              editor.PluginHost
                                                             )
                                   );


            PluginManager.LoadPlugins(editor.PluginHost);
        }

        public static bool LoadEditorSettings()
        {
            if (File.Exists(Path.Combine(FLScriptEditor.ConfigPath, "fleditor.settings.xml")))
            {
                try
                {
                    FLDebuggerSettings settings =
                        FLDebuggerSettings.Load(Path.Combine(FLScriptEditor.ConfigPath, "fleditor.settings.xml"));
                    FLScriptEditor.Settings = settings;
                }
                catch (Exception)
                {
                    DialogResult res = StyledMessageBox.Show(
                                                             "Settings Load Error",
                                                             $"Could not load the Settings file at path: {Path.Combine(FLScriptEditor.ConfigPath, "fleditor.settings.xml")}\nTo Fix this issue the editor can delete the file.\nDo you want to delete the file?",
                                                             MessageBoxButtons.YesNoCancel,
                                                             SystemIcons.Error
                                                            );
                    if (res == DialogResult.Yes)
                    {
                        try
                        {
                            File.Delete(Path.Combine(FLScriptEditor.ConfigPath, "fleditor.settings.xml"));
                        }
                        catch (Exception)
                        {
                            StyledMessageBox.Show(
                                                  "Warning",
                                                  "Could not delete the File.",
                                                  MessageBoxButtons.OK,
                                                  SystemIcons.Exclamation
                                                 );
                        }
                    }
                    else if (res == DialogResult.Cancel)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static Form GetRequestedForm()
        {
            Form ed = null;


            if (string.IsNullOrEmpty(FLScriptEditor.Settings.WorkingDir) &&
                !string.IsNullOrEmpty(FLScriptEditor.Settings.ScriptPath))
            {
                if (FLScriptEditor.Settings.ScriptPath.EndsWith(".flc"))
                {
                    ed = new ExportViewer(FLScriptEditor.Settings.ScriptPath);
                }
                else
                {
                    ed = new FLScriptEditor(FLScriptEditor.Settings.ScriptPath);
                }
            }
            else if (!string.IsNullOrEmpty(FLScriptEditor.Settings.ScriptPath) &&
                     !string.IsNullOrEmpty(FLScriptEditor.Settings.WorkingDir))
            {
                if (FLScriptEditor.Settings.ScriptPath.EndsWith(".flc"))
                {
                    ed = new ExportViewer(FLScriptEditor.Settings.ScriptPath);
                }
                else
                {
                    ed = new FLScriptEditor(FLScriptEditor.Settings.ScriptPath, FLScriptEditor.Settings.WorkingDir);
                }
            }
            else
            {
                ed = new FLScriptEditor();
            }

            return ed;
        }

        public static void RunFormWarm(Form form)
        {
            try
            {
                for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
                {
                    Form openForm = Application.OpenForms[i];
                    openForm.Close();
                }

                form.ShowDialog(); //Show as dialog to have a blocking call so we do not leave the try catch block
            }
            catch (Exception e)
            {
                if (e is SoftException)
                {
                    ExceptionViewer ev = new ExceptionViewer(e);
                    if (ev.ShowDialog() == DialogResult.Retry)
                    {
                        RunFormWarm(GetRequestedForm());
                    }
                }
                else
                {
                    throw;
                }
            }
        }

        public static void RunForm(Form form)
        {
            try
            {
                Application.Run(form);
            }
            catch (Exception e)
            {
                if (e is SoftException)
                {
                    ExceptionViewer ev = new ExceptionViewer(e);
                    if (ev.ShowDialog() == DialogResult.Retry)
                    {
                        WarmStartup();
                    }
                }
                else
                {
                    throw;
                }
            }
        }

    }
}