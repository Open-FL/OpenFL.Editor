namespace OpenFL.Editor.Forms.Util
{
    partial class ListPluginsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbPluginHosts = new System.Windows.Forms.ListBox();
            this.gbHosts = new System.Windows.Forms.GroupBox();
            this.gbPlugins = new System.Windows.Forms.GroupBox();
            this.lbPlugins = new System.Windows.Forms.ListBox();
            this.gbHosts.SuspendLayout();
            this.gbPlugins.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbPluginHosts
            // 
            this.lbPluginHosts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPluginHosts.FormattingEnabled = true;
            this.lbPluginHosts.Location = new System.Drawing.Point(3, 16);
            this.lbPluginHosts.Name = "lbPluginHosts";
            this.lbPluginHosts.Size = new System.Drawing.Size(218, 268);
            this.lbPluginHosts.TabIndex = 0;
            this.lbPluginHosts.SelectedIndexChanged += new System.EventHandler(this.lbPluginHosts_SelectedIndexChanged);
            // 
            // gbHosts
            // 
            this.gbHosts.Controls.Add(this.lbPluginHosts);
            this.gbHosts.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbHosts.Location = new System.Drawing.Point(0, 0);
            this.gbHosts.Name = "gbHosts";
            this.gbHosts.Size = new System.Drawing.Size(224, 287);
            this.gbHosts.TabIndex = 1;
            this.gbHosts.TabStop = false;
            this.gbHosts.Text = "Plugin Hosts";
            // 
            // gbPlugins
            // 
            this.gbPlugins.Controls.Add(this.lbPlugins);
            this.gbPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPlugins.Location = new System.Drawing.Point(224, 0);
            this.gbPlugins.Name = "gbPlugins";
            this.gbPlugins.Size = new System.Drawing.Size(292, 287);
            this.gbPlugins.TabIndex = 2;
            this.gbPlugins.TabStop = false;
            this.gbPlugins.Text = "Plugins";
            // 
            // lbPlugins
            // 
            this.lbPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPlugins.FormattingEnabled = true;
            this.lbPlugins.Location = new System.Drawing.Point(3, 16);
            this.lbPlugins.Name = "lbPlugins";
            this.lbPlugins.Size = new System.Drawing.Size(286, 268);
            this.lbPlugins.TabIndex = 0;
            this.lbPlugins.DoubleClick += ToggleLoadState;
            // 
            // ListPluginsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 287);
            this.Controls.Add(this.gbPlugins);
            this.Controls.Add(this.gbHosts);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ListPluginsForm";
            this.Text = "ListPluginsForm";
            this.gbHosts.ResumeLayout(false);
            this.gbPlugins.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbPluginHosts;
        private System.Windows.Forms.GroupBox gbHosts;
        private System.Windows.Forms.GroupBox gbPlugins;
        private System.Windows.Forms.ListBox lbPlugins;
    }
}