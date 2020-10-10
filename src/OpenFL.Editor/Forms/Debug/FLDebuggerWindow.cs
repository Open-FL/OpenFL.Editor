using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using OpenFL.Core.Buffers;
using OpenFL.Core.DataObjects.ExecutableDataObjects;
using OpenFL.Editor.Properties;
using OpenFL.Editor.Utils;

using PluginSystem.Core.Interfaces;
using PluginSystem.Core.Pointer;

using ThemeEngine;

using Utility.WindowsForms.CustomControls;

namespace OpenFL.Editor.Forms.Debug
{
    public partial class FLDebuggerWindow : Form, IPluginHost
    {

        private readonly CustomCheckedListBox clbCode;
        private readonly Dictionary<IParsedObject, int> marks;
        private readonly FLProgram Program;
        private readonly string Source;


        private bool continueEx;
        private bool exitDirect;
        private bool nohalt;
        private int selectedLine;
        private bool started;


        public FLDebuggerWindow(FLProgram program)
        {
            Program = program;
            marks = Program.ToString(out string source);
            Source = source;
            InitializeComponent();

            clbCode = new CustomCheckedListBox(GetCodeItemBackColor, GetCodeItemForeColor);
            Controls.Add(clbCode);
            clbCode.BackColor = Color.DimGray;
            clbCode.ForeColor = Color.Black;
            clbCode.CheckOnClick = true;
            clbCode.Dock = DockStyle.Fill;
            Closing += CodeView_Closing;


            StyleManager.RegisterControls(this);
        }

        public Color DebuggerBreakpointColor { get; set; } = Color.DarkRed;

        public Color DebuggerBreakpointHitColor { get; set; } = Color.Orange;

        public bool IsAllowedPlugin(IPlugin plugin)
        {
            return true;
        }

        public void OnPluginLoad(IPlugin plugin, BasePluginPointer ptr)
        {
        }

        public void OnPluginUnload(IPlugin plugin)
        {
        }

        public event Action<FLProgram, FLBuffer> OnInternalBufferClick;

        public event Action<FLProgram, FLBuffer> OnBufferClick;


        private void CodeView_Load(object sender, EventArgs e)
        {
            Icon = Resources.OpenFL_Icon;


            lbInternalBuffers.MouseDoubleClick += LbInternalBuffersOnMouseDoubleClick;
            lbBuffers.MouseDoubleClick += LbBuffers_MouseDoubleClick;
            DoubleBuffered = true;


            clbCode.Items.AddRange(Source.Split('\n'));

            UpdateSidePanel();
            btnContinue.Text = "Start";
            btnContinue.Enabled = true;
        }

        private void LbInternalBuffersOnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lbInternalBuffers.SelectedItem == null || !started)
            {
                return;
            }

            FLBuffer buf = lbInternalBuffers.SelectedItem as FLBuffer;

            OnInternalBufferClick?.Invoke(Program, buf);
        }

        private Color GetCodeItemForeColor(CustomCheckedListBox listbox, DrawItemEventArgs e)
        {
            return listbox.ForeColor;
        }


        private Color GetCodeItemBackColor(CustomCheckedListBox listbox, DrawItemEventArgs e)
        {
            if (e.Index == selectedLine)
            {
                return DebuggerBreakpointHitColor;
            }

            if (listbox.GetItemChecked(e.Index))
            {
                return DebuggerBreakpointColor;
            }

            return listbox.BackColor;
        }

        private void CodeView_Closing(object sender, CancelEventArgs e)
        {
            exitDirect = true;
            nohalt = true;
        }

        private void LbBuffers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lbBuffers.SelectedItem == null || !started)
            {
                return;
            }

            OnBufferClick?.Invoke(Program, Program.GetBufferWithName(lbBuffers.SelectedItem.ToString(), false));
        }

        private int GetLineOfObject(IParsedObject obj)
        {
            return marks[obj];
        }

        private void SelectLine(int line)
        {
            selectedLine = line;
        }


        private void UpdateSidePanel()
        {
            FLBuffer active = Program.GetActiveBuffer(false);
            string activeBufferName = active == null ? "NULL" : active.DefinedBufferName;
            lblActiveBuffer.Text = "Active Buffer:\n" + activeBufferName;
            string s = "Active Channels:";
            if (Program.ActiveChannels != null)
            {
                for (int i = 0; i < Program.ActiveChannels.Length; i++)
                {
                    s += " " + Program.ActiveChannels[i];
                }
            }
            else
            {
                s += " NULL";
            }

            lblActiveChannels.Text = s;

            lbBuffers.Items.Clear();
            lbBuffers.Items.AddRange(Program.BufferNames.Cast<object>().ToArray());
        }

        public void MarkInstruction(int instr)
        {
            SelectLine(instr);
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            started = true;
            continueEx = true;
        }

        private void btnShowSidePanel_Click(object sender, EventArgs e)
        {
            panelSidePanel.Visible = true;
            btnShowSidePanel.SendToBack();
        }

        private void btnHideSidePanel_Click(object sender, EventArgs e)
        {
            panelSidePanel.Visible = false;
            btnShowSidePanel.BringToFront();
        }

        private void btnRunToEnd_Click(object sender, EventArgs e)
        {
            nohalt = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            FollowScripts = cbFollowScripts.Checked;
        }

        private void lbBuffers_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

    }
}