using System;
using System.IO;
using System.Windows.Forms;

using PluginSystem.Core;
using PluginSystem.FileSystem.Packer;

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
            btnLoad.Enabled = (Directory.Exists(tbPath.Text) || File.Exists(tbPath.Text)) &&
                              PluginPacker.CanLoad(tbPath.Text);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            PluginManager.AddPackage(tbPath.Text, out string name);
            if (cbActivate.Checked)
            {
                PluginManager.ActivatePackage(name, cbAddToExistingHosts.Checked);
            }
            Close();
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
                    PluginManager.AddPackage(file, out string name);
                    if (cbActivate.Checked)
                    {
                        PluginManager.ActivatePackage(name, cbAddToExistingHosts.Checked);
                    }
                }

                Close();
            }

            ofdSelectFile.Multiselect = false;
        }

    }
}