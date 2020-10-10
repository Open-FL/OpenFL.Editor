using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenFL.Editor.Properties;
using OpenFL.Editor.Utils;

using ThemeEngine;

using Utility.Exceptions;
using Utility.WindowsForms.CustomControls;
using Utility.WindowsForms.Forms;

namespace OpenFL.Editor.Forms.Util
{
    public partial class Loader : Form
    {

        private readonly Task WaitTask;

        private ColoredProgressBar pbProgress;

        public Loader(Task waitTask, string title = "Loading") : this(waitTask, Color.GreenYellow, title)
        {
        }

        public Loader(Task waitTask, Color pcColor, string title = "Loading")
        {
            InitializeComponent();

            SetupProgressBar(pcColor);

            lblLoad.Text = title;

            Icon = Resources.OpenFL_Icon;

            CheckForIllegalCrossThreadCalls = false;
            WaitTask = waitTask;


            StyleManager.RegisterControls(this, "loader");

            //FLScriptEditor.RegisterDefaultTheme(rtbLog);
            //FLScriptEditor.RegisterDefaultTheme(panelLog);
            //FLScriptEditor.RegisterDefaultTheme(pbProgress);
            //FLScriptEditor.RegisterDefaultTheme(lblStatus);
            //FLScriptEditor.RegisterDefaultTheme(lblLoad);
            //FLScriptEditor.RegisterDefaultTheme(panel1);
        }


        public void SetStatus(string status)
        {
            lblStatus.Text = status;
        }

        public void SetTitle(string title)
        {
            lblLoad.Text = title;
        }

        public void SetProgress(int value, int maxValue)
        {
            pbProgress.Maximum = maxValue;
            pbProgress.Value = value;
        }

        public void Log(string log, Color foreColor)
        {
            rtbLog.AppendLine(log, foreColor, rtbLog.BackColor);
        }

        private void KernelLoader_Load(object sender, EventArgs e)
        {
            Point pos = Screen.PrimaryScreen.Bounds.Location +
                        new Size(
                                 Screen.PrimaryScreen.Bounds.Width / 2,
                                 Screen.PrimaryScreen.Bounds.Height / 2
                                ) -
                        new Size(Bounds.Width / 2, Bounds.Height / 2);
            Location = pos;

            WaitTask.Start();
            checkFinishTimer.Start();
        }

        private void checkFinishTimer_Tick(object sender, EventArgs e)
        {
            if (WaitTask.IsCompleted)
            {
                checkFinishTimer.Stop();
                if (WaitTask.IsFaulted)
                {
                    if (WaitTask.Exception.InnerException is SoftException)
                    {
                        ExceptionViewer ev = new ExceptionViewer(WaitTask.Exception, false);
                        ev.ShowDialog();
                        FLScriptEditor.Settings.Abort = true;
                        Application.Exit();
                        return;
                    }

                    throw WaitTask.Exception;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
        }


        private void SetupProgressBar(Color pbColor)
        {
            if (pbProgress != null)
            {
                return;
            }

            pbProgress = new ColoredProgressBar();
            pbProgress.FillColor = pbColor;
            pbProgress.BackColor = Color.DimGray;
            pbProgress.Location = new Point(209, 56);
            pbProgress.Name = "pbProgress";
            pbProgress.Size = new Size(396, 10);
            pbProgress.TabIndex = 3;
            Controls.Add(pbProgress);
            StyleManager.RegisterControl(pbProgress, "loader", "progress");
        }

    }
}