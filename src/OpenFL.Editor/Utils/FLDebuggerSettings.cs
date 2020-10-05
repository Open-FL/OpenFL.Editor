using System;
using System.IO;
using System.Xml.Serialization;

namespace OpenFL.Editor.Utils
{
    public class FLDebuggerSettings
    {

        public bool Abort;
        public bool ExperimentalDebuggerMultiThread;
        public bool ExperimentalKernelLoading;
        public string KernelPath = "resources/kernel";
        public bool LogParserStacktrace;
        public bool LogProgramStacktrace;
        public int ResX = 512;
        public int ResY = 512;
        public int ResZ = 1;
        public string ScriptPath;
        public string WorkingDir;
        public string PluginInit;

        public string[] InitFiles => PluginInit.Split(';');


        public static FLDebuggerSettings Load(string path)
        {
            Stream s = null;
            try
            {
                s = File.OpenRead(path);
                XmlSerializer xs = new XmlSerializer(typeof(FLDebuggerSettings));
                FLDebuggerSettings settings = (FLDebuggerSettings) xs.Deserialize(s);
                s.Close();
                return settings;
            }
            catch (Exception)
            {
                s?.Close();
                throw;
            }
        }

        public void Save(string path)
        {
            Stream s = null;
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(FLDebuggerSettings));
                s = File.OpenWrite(path);
                xs.Serialize(s, this);
                s.Close();
            }
            catch (Exception)
            {
                s?.Close();
                throw;
            }
        }

    }
}