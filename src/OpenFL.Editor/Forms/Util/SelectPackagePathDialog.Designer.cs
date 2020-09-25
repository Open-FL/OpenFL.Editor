namespace OpenFL.Editor.Forms.Util
{
    partial class SelectPackagePathDialog
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
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.btnLoad = new System.Windows.Forms.Button();
            this.fbdSelectFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.ofdSelectFile = new System.Windows.Forms.OpenFileDialog();
            this.btnBatchLoad = new System.Windows.Forms.Button();
            this.cbActivate = new System.Windows.Forms.CheckBox();
            this.cbAddToExistingHosts = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(404, 32);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelectFile.TabIndex = 0;
            this.btnSelectFile.Text = "File";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(485, 32);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(75, 23);
            this.btnSelectFolder.TabIndex = 1;
            this.btnSelectFolder.Text = "Folder";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // tbPath
            // 
            this.tbPath.Location = new System.Drawing.Point(47, 6);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(513, 20);
            this.tbPath.TabIndex = 2;
            this.tbPath.TextChanged += new System.EventHandler(this.tbPath_TextChanged);
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(12, 9);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(29, 13);
            this.lblPath.TabIndex = 3;
            this.lblPath.Text = "Path";
            // 
            // btnLoad
            // 
            this.btnLoad.Enabled = false;
            this.btnLoad.Location = new System.Drawing.Point(485, 61);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // fbdSelectFolder
            // 
            this.fbdSelectFolder.Description = "Select a Folder that contains the plugin";
            this.fbdSelectFolder.ShowNewFolderButton = false;
            // 
            // ofdSelectFile
            // 
            this.ofdSelectFile.FileName = "openFileDialog1";
            // 
            // btnBatchLoad
            // 
            this.btnBatchLoad.Location = new System.Drawing.Point(404, 61);
            this.btnBatchLoad.Name = "btnBatchLoad";
            this.btnBatchLoad.Size = new System.Drawing.Size(75, 23);
            this.btnBatchLoad.TabIndex = 5;
            this.btnBatchLoad.Text = "Batch Load";
            this.btnBatchLoad.UseVisualStyleBackColor = true;
            this.btnBatchLoad.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbActivate
            // 
            this.cbActivate.AutoSize = true;
            this.cbActivate.Location = new System.Drawing.Point(275, 36);
            this.cbActivate.Name = "cbActivate";
            this.cbActivate.Size = new System.Drawing.Size(102, 17);
            this.cbActivate.TabIndex = 6;
            this.cbActivate.Text = "Activate on Add";
            this.cbActivate.UseVisualStyleBackColor = true;
            // 
            // cbAddToExistingHosts
            // 
            this.cbAddToExistingHosts.AutoSize = true;
            this.cbAddToExistingHosts.Location = new System.Drawing.Point(275, 65);
            this.cbAddToExistingHosts.Name = "cbAddToExistingHosts";
            this.cbAddToExistingHosts.Size = new System.Drawing.Size(123, 17);
            this.cbAddToExistingHosts.TabIndex = 7;
            this.cbAddToExistingHosts.Text = "Add to existing hosts";
            this.cbAddToExistingHosts.UseVisualStyleBackColor = true;
            // 
            // SelectPackagePathDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 89);
            this.Controls.Add(this.cbAddToExistingHosts);
            this.Controls.Add(this.cbActivate);
            this.Controls.Add(this.btnBatchLoad);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.tbPath);
            this.Controls.Add(this.btnSelectFolder);
            this.Controls.Add(this.btnSelectFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectPackagePathDialog";
            this.Text = "Select Package Path";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.FolderBrowserDialog fbdSelectFolder;
        private System.Windows.Forms.OpenFileDialog ofdSelectFile;
        private System.Windows.Forms.Button btnBatchLoad;
        private System.Windows.Forms.CheckBox cbActivate;
        private System.Windows.Forms.CheckBox cbAddToExistingHosts;
    }
}