using ThemeEngine;

namespace OpenFL.Editor.Forms
{
    partial class FLScriptEditor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLScriptEditor));
            this.panelInput = new System.Windows.Forms.Panel();
            this.btnPopOutInput = new System.Windows.Forms.Button();
            this.rtbIn = new System.Windows.Forms.RichTextBox();
            this.panelToolbar = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblBuildMode = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDebug = new System.Windows.Forms.Button();
            this.cbBuildMode = new System.Windows.Forms.ComboBox();
            this.cbLiveView = new System.Windows.Forms.CheckBox();
            this.cbAutoBuild = new System.Windows.Forms.CheckBox();
            this.lbOptimizations = new System.Windows.Forms.CheckedListBox();
            this.openFLScript = new System.Windows.Forms.OpenFileDialog();
            this.rtbOut = new System.Windows.Forms.RichTextBox();
            this.panelConsoleOut = new System.Windows.Forms.Panel();
            this.btnPopOutBuildLog = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.rtbParserOutput = new System.Windows.Forms.RichTextBox();
            this.panelCodeArea = new System.Windows.Forms.Panel();
            this.panelOutput = new System.Windows.Forms.Panel();
            this.btnPopOutOutput = new System.Windows.Forms.Button();
            this.tmrConsoleRefresh = new System.Windows.Forms.Timer(this.components);
            this.tmrConsoleColors = new System.Windows.Forms.Timer(this.components);
            this.sfdScript = new System.Windows.Forms.SaveFileDialog();
            this.sfdExport = new System.Windows.Forms.SaveFileDialog();
            this.ofdPackageTarget = new System.Windows.Forms.OpenFileDialog();
            this.msToolbar = new System.Windows.Forms.MenuStrip();
            this.panelInput.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelConsoleOut.SuspendLayout();
            this.panelCodeArea.SuspendLayout();
            this.panelOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelInput
            // 
            this.panelInput.BackColor = System.Drawing.Color.DimGray;
            this.panelInput.Controls.Add(this.btnPopOutInput);
            this.panelInput.Controls.Add(this.rtbIn);
            this.panelInput.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelInput.Location = new System.Drawing.Point(0, 0);
            this.panelInput.Name = "panelInput";
            this.panelInput.Size = new System.Drawing.Size(334, 323);
            this.panelInput.TabIndex = 0;
            // 
            // btnPopOutInput
            // 
            this.btnPopOutInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPopOutInput.BackColor = System.Drawing.Color.DimGray;
            this.btnPopOutInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPopOutInput.Location = new System.Drawing.Point(247, 297);
            this.btnPopOutInput.Name = "btnPopOutInput";
            this.btnPopOutInput.Size = new System.Drawing.Size(84, 23);
            this.btnPopOutInput.TabIndex = 14;
            this.btnPopOutInput.Text = "Pop Out";
            this.btnPopOutInput.UseVisualStyleBackColor = false;
            this.btnPopOutInput.Click += new System.EventHandler(this.btnPopOutInput_Click);
            // 
            // rtbIn
            // 
            this.rtbIn.AcceptsTab = true;
            this.rtbIn.BackColor = System.Drawing.Color.DimGray;
            this.rtbIn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbIn.ForeColor = System.Drawing.Color.Black;
            this.rtbIn.Location = new System.Drawing.Point(0, 0);
            this.rtbIn.Name = "rtbIn";
            this.rtbIn.Size = new System.Drawing.Size(334, 323);
            this.rtbIn.TabIndex = 0;
            this.rtbIn.Text = "";
            this.rtbIn.TextChanged += new System.EventHandler(this.rtbIn_TextChanged);
            // 
            // panelToolbar
            // 
            this.panelToolbar.BackColor = System.Drawing.Color.DimGray;
            this.panelToolbar.Controls.Add(this.panel1);
            this.panelToolbar.Controls.Add(this.lbOptimizations);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelToolbar.Location = new System.Drawing.Point(0, 4);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Size = new System.Drawing.Size(236, 508);
            this.panelToolbar.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblBuildMode);
            this.panel1.Controls.Add(this.lblVersion);
            this.panel1.Controls.Add(this.btnUpdate);
            this.panel1.Controls.Add(this.btnDebug);
            this.panel1.Controls.Add(this.cbBuildMode);
            this.panel1.Controls.Add(this.cbLiveView);
            this.panel1.Controls.Add(this.cbAutoBuild);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 416);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(236, 92);
            this.panel1.TabIndex = 21;
            // 
            // lblBuildMode
            // 
            this.lblBuildMode.AutoSize = true;
            this.lblBuildMode.Location = new System.Drawing.Point(12, 35);
            this.lblBuildMode.Name = "lblBuildMode";
            this.lblBuildMode.Size = new System.Drawing.Size(63, 13);
            this.lblBuildMode.TabIndex = 18;
            this.lblBuildMode.Text = "Build Mode:";
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVersion.AutoSize = true;
            this.lblVersion.ForeColor = System.Drawing.Color.Silver;
            this.lblVersion.Location = new System.Drawing.Point(-1, 79);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(42, 13);
            this.lblVersion.TabIndex = 20;
            this.lblVersion.Text = "Version";
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.DimGray;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Location = new System.Drawing.Point(0, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(116, 23);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "Parse";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDebug
            // 
            this.btnDebug.BackColor = System.Drawing.Color.DimGray;
            this.btnDebug.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDebug.Location = new System.Drawing.Point(120, 3);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(116, 23);
            this.btnDebug.TabIndex = 3;
            this.btnDebug.Text = "Debug";
            this.btnDebug.UseVisualStyleBackColor = false;
            this.btnDebug.Click += new System.EventHandler(this.btnDebug_Click);
            // 
            // cbBuildMode
            // 
            this.cbBuildMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBuildMode.FormattingEnabled = true;
            this.cbBuildMode.Location = new System.Drawing.Point(81, 32);
            this.cbBuildMode.Name = "cbBuildMode";
            this.cbBuildMode.Size = new System.Drawing.Size(149, 21);
            this.cbBuildMode.TabIndex = 17;
            this.cbBuildMode.SelectedIndexChanged += new System.EventHandler(this.cbBuildMode_SelectedIndexChanged);
            // 
            // cbLiveView
            // 
            this.cbLiveView.AutoSize = true;
            this.cbLiveView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbLiveView.Location = new System.Drawing.Point(26, 59);
            this.cbLiveView.Name = "cbLiveView";
            this.cbLiveView.Size = new System.Drawing.Size(70, 18);
            this.cbLiveView.TabIndex = 13;
            this.cbLiveView.Text = "Preview";
            this.cbLiveView.UseVisualStyleBackColor = true;
            this.cbLiveView.CheckedChanged += new System.EventHandler(this.cbLiveView_CheckedChanged);
            // 
            // cbAutoBuild
            // 
            this.cbAutoBuild.AutoSize = true;
            this.cbAutoBuild.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbAutoBuild.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbAutoBuild.Location = new System.Drawing.Point(139, 59);
            this.cbAutoBuild.Name = "cbAutoBuild";
            this.cbAutoBuild.Size = new System.Drawing.Size(80, 18);
            this.cbAutoBuild.TabIndex = 14;
            this.cbAutoBuild.Text = "Auto Build";
            this.cbAutoBuild.UseVisualStyleBackColor = true;
            // 
            // lbOptimizations
            // 
            this.lbOptimizations.BackColor = System.Drawing.Color.DimGray;
            this.lbOptimizations.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbOptimizations.CheckOnClick = true;
            this.lbOptimizations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbOptimizations.FormattingEnabled = true;
            this.lbOptimizations.Location = new System.Drawing.Point(0, 0);
            this.lbOptimizations.Name = "lbOptimizations";
            this.lbOptimizations.Size = new System.Drawing.Size(236, 508);
            this.lbOptimizations.TabIndex = 1;
            // 
            // openFLScript
            // 
            this.openFLScript.FileName = "openFileDialog1";
            // 
            // rtbOut
            // 
            this.rtbOut.BackColor = System.Drawing.Color.DimGray;
            this.rtbOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbOut.ForeColor = System.Drawing.Color.Black;
            this.rtbOut.Location = new System.Drawing.Point(0, 0);
            this.rtbOut.Name = "rtbOut";
            this.rtbOut.ReadOnly = true;
            this.rtbOut.Size = new System.Drawing.Size(350, 323);
            this.rtbOut.TabIndex = 0;
            this.rtbOut.Text = "";
            // 
            // panelConsoleOut
            // 
            this.panelConsoleOut.BackColor = System.Drawing.Color.DimGray;
            this.panelConsoleOut.Controls.Add(this.btnPopOutBuildLog);
            this.panelConsoleOut.Controls.Add(this.btnClear);
            this.panelConsoleOut.Controls.Add(this.rtbParserOutput);
            this.panelConsoleOut.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelConsoleOut.Location = new System.Drawing.Point(236, 327);
            this.panelConsoleOut.Name = "panelConsoleOut";
            this.panelConsoleOut.Size = new System.Drawing.Size(684, 185);
            this.panelConsoleOut.TabIndex = 1;
            // 
            // btnPopOutBuildLog
            // 
            this.btnPopOutBuildLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPopOutBuildLog.BackColor = System.Drawing.Color.DimGray;
            this.btnPopOutBuildLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPopOutBuildLog.Location = new System.Drawing.Point(578, 132);
            this.btnPopOutBuildLog.Name = "btnPopOutBuildLog";
            this.btnPopOutBuildLog.Size = new System.Drawing.Size(84, 23);
            this.btnPopOutBuildLog.TabIndex = 12;
            this.btnPopOutBuildLog.Text = "Pop Out";
            this.btnPopOutBuildLog.UseVisualStyleBackColor = false;
            this.btnPopOutBuildLog.Click += new System.EventHandler(this.btnPopOutBuildLog_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.BackColor = System.Drawing.Color.DimGray;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Location = new System.Drawing.Point(578, 159);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(84, 23);
            this.btnClear.TabIndex = 13;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // rtbParserOutput
            // 
            this.rtbParserOutput.BackColor = System.Drawing.Color.DimGray;
            this.rtbParserOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbParserOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbParserOutput.ForeColor = System.Drawing.Color.Maroon;
            this.rtbParserOutput.Location = new System.Drawing.Point(0, 0);
            this.rtbParserOutput.Name = "rtbParserOutput";
            this.rtbParserOutput.ReadOnly = true;
            this.rtbParserOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.rtbParserOutput.Size = new System.Drawing.Size(684, 185);
            this.rtbParserOutput.TabIndex = 0;
            this.rtbParserOutput.Text = "";
            // 
            // panelCodeArea
            // 
            this.panelCodeArea.BackColor = System.Drawing.Color.DimGray;
            this.panelCodeArea.Controls.Add(this.panelOutput);
            this.panelCodeArea.Controls.Add(this.panelInput);
            this.panelCodeArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCodeArea.Location = new System.Drawing.Point(236, 4);
            this.panelCodeArea.Name = "panelCodeArea";
            this.panelCodeArea.Size = new System.Drawing.Size(684, 323);
            this.panelCodeArea.TabIndex = 2;
            // 
            // panelOutput
            // 
            this.panelOutput.BackColor = System.Drawing.Color.DimGray;
            this.panelOutput.Controls.Add(this.btnPopOutOutput);
            this.panelOutput.Controls.Add(this.rtbOut);
            this.panelOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOutput.Location = new System.Drawing.Point(334, 0);
            this.panelOutput.Name = "panelOutput";
            this.panelOutput.Size = new System.Drawing.Size(350, 323);
            this.panelOutput.TabIndex = 1;
            // 
            // btnPopOutOutput
            // 
            this.btnPopOutOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPopOutOutput.BackColor = System.Drawing.Color.DimGray;
            this.btnPopOutOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPopOutOutput.Location = new System.Drawing.Point(3, 297);
            this.btnPopOutOutput.Name = "btnPopOutOutput";
            this.btnPopOutOutput.Size = new System.Drawing.Size(84, 23);
            this.btnPopOutOutput.TabIndex = 13;
            this.btnPopOutOutput.Text = "Pop Out";
            this.btnPopOutOutput.UseVisualStyleBackColor = false;
            this.btnPopOutOutput.Click += new System.EventHandler(this.btnPopOutOutput_Click);
            // 
            // tmrConsoleRefresh
            // 
            this.tmrConsoleRefresh.Tick += new System.EventHandler(this.tmrConsoleRefresh_Tick);
            // 
            // tmrConsoleColors
            // 
            this.tmrConsoleColors.Interval = 1000;
            this.tmrConsoleColors.Tick += new System.EventHandler(this.tmrConsoleColors_Tick);
            // 
            // sfdScript
            // 
            this.sfdScript.DefaultExt = "fl";
            this.sfdScript.FileName = "FLFile";
            this.sfdScript.Filter = "FLScript|*.fl|Exported FL Script|*.flc|Exported Image|*.png";
            this.sfdScript.Title = "Save FL Script";
            // 
            // sfdExport
            // 
            this.sfdExport.DefaultExt = "flc";
            this.sfdExport.FileName = "ExportedScript";
            this.sfdExport.Filter = "Exported FL Scripts|*.flc";
            this.sfdExport.Title = "Select Export Destination";
            // 
            // ofdPackageTarget
            // 
            this.ofdPackageTarget.FileName = "openFileDialog1";
            // 
            // msToolbar
            // 
            this.msToolbar.BackColor = System.Drawing.Color.DimGray;
            this.msToolbar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.msToolbar.Location = new System.Drawing.Point(0, 0);
            this.msToolbar.Name = "msToolbar";
            this.msToolbar.Size = new System.Drawing.Size(920, 4);
            this.msToolbar.TabIndex = 16;
            this.msToolbar.Text = "menuStrip1";
            // 
            // FLScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 512);
            this.Controls.Add(this.panelCodeArea);
            this.Controls.Add(this.panelConsoleOut);
            this.Controls.Add(this.panelToolbar);
            this.Controls.Add(this.msToolbar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(252, 551);
            this.Name = "FLScriptEditor";
            this.Text = "FL Parse Output Viewer";
            this.Load += new System.EventHandler(this.EditorLoad);
            this.panelInput.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelConsoleOut.ResumeLayout(false);
            this.panelCodeArea.ResumeLayout(false);
            this.panelOutput.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelInput;
        private System.Windows.Forms.Panel panelToolbar;
        private System.Windows.Forms.OpenFileDialog openFLScript;
        private System.Windows.Forms.RichTextBox rtbIn;
        private System.Windows.Forms.RichTextBox rtbOut;
        [StyleSelector("build-out")]
        private System.Windows.Forms.Panel panelConsoleOut;
        private System.Windows.Forms.Panel panelCodeArea;
        private System.Windows.Forms.Panel panelOutput;
        private System.Windows.Forms.RichTextBox rtbParserOutput;
        private System.Windows.Forms.Timer tmrConsoleRefresh;
        private System.Windows.Forms.CheckedListBox lbOptimizations;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Timer tmrConsoleColors;
        private System.Windows.Forms.Button btnDebug;
        private System.Windows.Forms.SaveFileDialog sfdScript;
        private System.Windows.Forms.Button btnPopOutBuildLog;
        private System.Windows.Forms.Button btnPopOutInput;
        private System.Windows.Forms.Button btnPopOutOutput;
        private System.Windows.Forms.CheckBox cbLiveView;
        private System.Windows.Forms.CheckBox cbAutoBuild;
        private System.Windows.Forms.Label lblBuildMode;
        private System.Windows.Forms.ComboBox cbBuildMode;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.SaveFileDialog sfdExport;
        private System.Windows.Forms.OpenFileDialog ofdPackageTarget;
        private System.Windows.Forms.MenuStrip msToolbar;
        private System.Windows.Forms.Panel panel1;
    }
}

