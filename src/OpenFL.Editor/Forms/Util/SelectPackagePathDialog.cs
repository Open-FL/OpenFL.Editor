using System;
using System.IO;
using System.Windows.Forms;

using PluginSystem.Core;
using PluginSystem.FileSystem.Packer;
using PluginSystem.StartupActions;

using ThemeEngine;

namespace OpenFL.Editor.Forms.Util
{
    public partial class SelectPackagePathDialog : Form
    {

        public SelectPackagePathDialog()
        {
            InitializeComponent();

            StyleManager.RegisterControls(this);
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            if (ofdSelectFile.ShowDialog() == DialogResult.OK)
            {
                tbPath.Text = ofdSelectFile.FileName;
            }
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            if (fbdSelectFolder.ShowDialog() == DialogResult.OK)
            {
                tbPath.Text = fbdSelectFolder.SelectedPath;
            }
        }

        private void tbPath_TextChanged(object sender, EventArgs e)
        {
            btnLoad.Enabled = //(Directory.Exists(tbPath.Text) ||
            //                   File.Exists(tbPath.Text) ||
            //                   tbPath.Text.StartsWith("http://") ||
            //                   tbPath.Text.StartsWith("https://")) &&
                              PluginPacker.CanLoad(tbPath.Text);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            AddPackage(tbPath.Text, cbActivate.Checked);
            MessageBox.Show("Will be installed on restart");
            Close();
        }

        private static void AddPackage(string package, bool activate)
        {
            string action = activate
                                ? ActionRunner.ADD_ACTIVATE_PACKAGE_ACTION
                                : ActionRunner.ADD_PACKAGE_ACTION;
            ActionRunner.AddActionToStartup($"{action} {package}");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ofdSelectFile.Multiselect = true;
            if (ofdSelectFile.ShowDialog() == DialogResult.OK)
            {
                string[] files = ofdSelectFile.FileNames;
                for (int i = 0; i < files.Length; i++)
                {
                    string file = files[i];
                    AddPackage(file, cbActivate.Checked);
                }

                Close();
            }
            ofdSelectFile.Multiselect = false;
            MessageBox.Show("Will be installed on restart");
        }

    }
}