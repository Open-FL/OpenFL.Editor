using System.Drawing;

using OpenFL.Editor.Forms;
using OpenFL.Editor.Properties;

namespace OpenFL.Editor
{
    public class FLEditorPluginHost : IPluginHost
    {

        public FLEditorPluginHost(FLScriptEditor editor)
        {
            Editor = editor;
        }

        public static Bitmap FLEditorImage => Resources.OpenFL;

        public static Icon FLEditorIcon => Resources.OpenFL_Icon;

        public static Bitmap LoadingImage => Resources.loading_pac;

        public FLScriptEditor Editor { get; }

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

    }
}