using System;

namespace OpenFL.Editor.Utils.Plugins
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ToolbarItemAttribute : Attribute
    {

        public ToolbarItemAttribute(string path, int order = -1)
        {
            Path = path;
            Order = order;
        }

        public string Path { get; }

        public int Order { get; }

    }
}