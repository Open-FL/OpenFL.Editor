using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using OpenFL.Editor.Properties;

namespace OpenFL.Editor.Forms
{
    public partial class ExportViewer : Form
    {

        private readonly string file;
        public FLDataContainer FLContainer;

        public ExportViewer(string file)
        {
            InitializeComponent();
            Icon = Resources.OpenFL_Icon;
            this.file = file;
        }

        private void ExportViewer_Load(object sender, EventArgs e)
        {
            StyleManager.RegisterControls(this);


            Stream s = File.OpenRead(file);


            FLProgram p = FLSerializer.LoadProgram(s, FLContainer.InstructionSet).Initialize(FLContainer);
            FLBuffer input = FLContainer.CreateBuffer(512, 512, 1, "Input");
            p.Run(input, true);

            Bitmap bmp = p.GetActiveBitmap();

            pbExportView.Image = bmp;
        }

    }
}