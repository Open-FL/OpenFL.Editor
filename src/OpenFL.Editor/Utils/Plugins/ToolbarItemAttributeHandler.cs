using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using OpenFL.Editor.Forms;
using OpenFL.Editor.Forms.Util;

namespace OpenFL.Editor.Utils.Plugins
{
    public class DefaultToolbarItems : APlugin<FLEditorPluginHost>
    {

        private ListPluginsForm pluginList;

        [ToolbarItem("Plugins", 5)]
        private void PluginDummy()
        {
        }

        [ToolbarItem("Plugins/Add Plugin Package..", 0)]
        private void OnPluginAdd()
        {
            new SelectPackagePathDialog().ShowDialog();
        }

        [ToolbarItem("Editor", 6)]
        private void EditorTopic()
        {
        }

        [ToolbarItem("Editor/Restart..", 0)]
        private void OnRestart()
        {
            Application.Restart();
        }

        [ToolbarItem("Plugins/List Plugins", 2)]
        private void OnListPlugins()
        {
            if (pluginList == null || pluginList.IsDisposed)
            {
                pluginList = new ListPluginsForm();
            }

            pluginList.Show();
        }

    }

    public class ToolbarItemAttributeHandler : IAttributeHandler<Attribute>
    {

        private readonly FLScriptEditor Editor;
        private readonly List<ToolStripDropDownItem> pluginToolbarItems = new List<ToolStripDropDownItem>();
        private readonly Dictionary<ToolStripDropDownItem, int> sortData = new Dictionary<ToolStripDropDownItem, int>();

        private readonly Queue<(IPlugin plugin, PluginAssemblyPointer ptr, MemberInfo mi, ToolbarItemAttribute obj)>
            toolbarQueue =
                new Queue<(IPlugin plugin, PluginAssemblyPointer ptr, MemberInfo mi, ToolbarItemAttribute obj)>();

        public ToolbarItemAttributeHandler(FLScriptEditor editor)
        {
            Editor = editor;
            Editor.OnLoadComplete += Editor_OnLoadComplete;
            Editor.Closing += Editor_Closing;
        }

        public void Handle(IPlugin plugin, PluginAssemblyPointer ptr, MemberInfo mi, Attribute obj)
        {
            if (obj is ToolbarItemAttribute tbia)
            {
                AddMenuItemPath(Editor.Toolbar, plugin, ptr, mi, tbia);
            }
        }

        private void Editor_Closing(object sender, CancelEventArgs e)
        {
            pluginToolbarItems.ForEach(x => x?.Dispose());
            pluginToolbarItems.Clear();
        }

        private void Editor_OnLoadComplete()
        {
            foreach ((IPlugin plugin, PluginAssemblyPointer ptr, MemberInfo mi, ToolbarItemAttribute obj) valueTuple in
                toolbarQueue)
            {
                AddMenuItemPath(Editor.Toolbar, valueTuple.plugin, valueTuple.ptr, valueTuple.mi, valueTuple.obj);
            }
        }


        private void OnMenuItemCreate(ToolStripDropDownItem item)
        {
            StyleManager.RegisterControl(item.DropDown, "menu");

            //RegisterDefaultTheme(item.DropDown);
            item.Image = FLEditorPluginHost.FLEditorImage;
            sortData[item] = int.MaxValue / 2;
        }

        public void AddMenuItemPath(
            MenuStrip toolbar, IPlugin plugin, PluginAssemblyPointer ptr, MemberInfo mi, ToolbarItemAttribute obj)
        {
            if (!Editor.IsFullyLoaded)
            {
                toolbarQueue.Enqueue((plugin, ptr, mi, obj));
                return;
            }

            ToolStripDropDownItem dd = toolbar.GetItem(obj.Path, OnMenuItemCreate, pluginToolbarItems);

            sortData[dd] = obj.Order;

            ApplyToolbarItemOrder(dd, obj.Order);

            Action method = CreateAction(plugin, mi);

            dd.Click += (sender, args) => method?.Invoke();
        }

        private void ApplyToolbarItemOrder(ToolStripDropDownItem dd, int selfOrder)
        {
            ToolStripItem parent = dd.OwnerItem;
            if (parent is ToolStripDropDownItem ddp)
            {
                if (selfOrder >= 0)
                {
                    List<ToolStripDropDownItem> t = ddp.DropDownItems.Cast<ToolStripDropDownItem>().ToList();
                    t.Sort((a, b) => sortData[a].CompareTo(sortData[b]));
                    ddp.DropDownItems.Clear();
                    ddp.DropDownItems.AddRange(t.Cast<ToolStripItem>().ToArray());
                }
                else
                {
                    List<ToolStripDropDownItem> t = ddp.DropDownItems.Cast<ToolStripDropDownItem>().ToList();
                    t.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.InvariantCulture));
                    ddp.DropDownItems.Clear();
                    ddp.DropDownItems.AddRange(t.Cast<ToolStripItem>().ToArray());
                }
            }
            else
            {
                List<ToolStripDropDownItem> t = Editor.Toolbar.Items.Cast<ToolStripDropDownItem>().ToList();
                t.Sort((a, b) => sortData[a].CompareTo(sortData[b]));
                Editor.Toolbar.Items.Clear();
                Editor.Toolbar.Items.AddRange(t.Cast<ToolStripItem>().ToArray());
            }
        }

        private Action CreateAction(
            object instance, MemberInfo mi,
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            Action method = null;
            if (mi is MethodInfo mthd)
            {
                method = () => mthd.Invoke(instance, null);
            }
            else if (mi is EventInfo ev)
            {
                method = FromEvent(instance, instance.GetType(), ev, flags);
            }
            else
            {
                method = () =>
                         {
                             StyledMessageBox.Show("Error", "Could not bind", MessageBoxButtons.OK, SystemIcons.Error);
                         };
            }

            return method;
        }

        private Action FromEvent(
            object instance, Type instanceType, EventInfo info, BindingFlags flags, params object[] parameter)
        {
            FieldInfo evField = instanceType.GetField(info.Name, flags);
            PropertyInfo evProperty = instanceType.GetProperty(info.Name, flags);
            if (evField != null)
            {
                MulticastDelegate del = (MulticastDelegate) evField.GetValue(instance);
                return () =>
                       {
                           foreach (Delegate handler in del.GetInvocationList())
                           {
                               handler.Method.Invoke(handler.Target, parameter);
                           }
                       };
            }

            if (evProperty != null)
            {
                MulticastDelegate del = (MulticastDelegate) evProperty.GetValue(instance);
                return () =>
                       {
                           foreach (Delegate handler in del.GetInvocationList())
                           {
                               handler.Method.Invoke(handler.Target, null);
                           }
                       };
            }

            return () => { StyledMessageBox.Show("Error", "Could not bind", MessageBoxButtons.OK, SystemIcons.Error); };
        }

    }
}