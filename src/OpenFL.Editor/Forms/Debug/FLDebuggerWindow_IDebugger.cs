using System;
using System.Diagnostics;
using System.Windows.Forms;

using OpenFL.Core;
using OpenFL.Core.DataObjects.ExecutableDataObjects;
using OpenFL.Debugging;

namespace OpenFL.Editor.Forms.Debug
{
    public partial class FLDebuggerWindow : IProgramDebugger
    {

        private readonly Stopwatch instrTimer = new Stopwatch();
        private readonly Stopwatch totalTimer = new Stopwatch();

        public bool FollowScripts { get; private set; }


        public void OnProgramStart(FLDebuggerEvents.ProgramStartEventArgs args)
        {
            continueEx = false;
            Show();
            Text = "WAITING FOR PROGRAM START";
            while (!continueEx && !exitDirect)
            {
                Application.DoEvents();
            }

            btnContinue.Enabled = false;
            btnContinue.Text = "Continue";
        }

        public void OnProgramExit(FLDebuggerEvents.ProgramExitEventArgs args)
        {
            UpdateSidePanel();
            btnContinue.Text = "Close";
            btnContinue.Enabled = true;

            continueEx = false;
            Text = "DEBUGGING FINISHED";
            while (!continueEx && !exitDirect)
            {
                Application.DoEvents();
            }

            btnContinue.Enabled = false;

            Close();
        }

        public void OnSubProgramStart(FLDebuggerEvents.SubProgramStartEventArgs args)
        {
            ProcessEvent(args.ExternalSymbol);

            instrTimer.Stop();
            totalTimer.Stop();
            if (nohalt)
            {
                return;
            }

            Enabled = false;
            continueEx = false;
            Show();
            btnContinue.Enabled = true;
            Text = "WAITING FOR SUBPROGRAM";
        }

        public void OnSubProgramExit(FLDebuggerEvents.SubProgramExitEventArgs args)
        {
            Enabled = true;
            continueEx = true;
            btnContinue.Enabled = false;
            btnContinue.Text = "Continue";
            instrTimer.Start();
            totalTimer.Start();
        }

        public void OnBufferWarm(FLDebuggerEvents.WarmEventArgs args)
        {
        }

        public void OnInternalBufferLoad(FLDebuggerEvents.InternalBufferLoadEventArgs args)
        {
            lbInternalBuffers.Items.Add(args.LoadedBuffer);
        }

        public void OnFunctionStepInto(FLDebuggerEvents.FunctionRunEventArgs args)
        {
            ProcessEvent(args.Function);
        }

        public void OnInstructionStepInto(FLDebuggerEvents.InstructionRunEventArgs args)
        {
            ProcessEvent(args.Instruction);
        }

        public void AfterFunction(FLDebuggerEvents.FunctionRunEventArgs args)
        {
            ProcessEvent(args.Function);
        }

        public void AfterInstruction(FLDebuggerEvents.InstructionRunEventArgs args)
        {
            ProcessEvent(args.Instruction);
        }


        public void ProcessEvent(FLParsedObject obj)
        {
            if (nohalt)
            {
                return;
            }

            instrTimer.Stop();
            totalTimer.Stop();
            double millis = instrTimer.Elapsed.TotalMilliseconds;
            double totalMillis = totalTimer.Elapsed.TotalMilliseconds;
            int line = GetLineOfObject(obj);


            string newLine =
                $"{clbCode.Items[line].ToString().TrimEnd()}\t| {Math.Round(totalMillis, 4)} ms ({Math.Round(millis, 4)} ms)";
            clbCode.Items[line] = newLine;

            clbCode.Invalidate();

            if (!clbCode.GetItemChecked(line))
            {
                instrTimer.Restart();
                totalTimer.Start();
                return;
            }

            btnContinue.Text = "Start";

            UpdateSidePanel();
            btnContinue.Enabled = true;
            btnRunToEnd.Enabled = true;
            MarkInstruction(line);
            clbCode.Invalidate();

            continueEx = false;
            Text = "IN HALT MODE";
            while (!continueEx && !nohalt)
            {
                Application.DoEvents();
            }

            btnContinue.Enabled = false;
            btnRunToEnd.Enabled = true;

            Text = "Debugging";
            instrTimer.Restart();
            totalTimer.Start();
        }

    }
}