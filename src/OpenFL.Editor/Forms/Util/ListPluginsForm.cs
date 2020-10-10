using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenFL.Editor.Forms.Util
{
    public partial class ListPluginsForm : Form
    {

        public ListPluginsForm()
        {
            InitializeComponent();

            List<IPluginHost> hosts = PluginManager.GetHosts<IPluginHost>();
            lbPluginHosts.Items.Add("[Inactive Packages]");
            lbPluginHosts.Items.Add("[Active Packages]");
            foreach (IPluginHost pluginHost in hosts)
            {
                lbPluginHosts.Items.Add(pluginHost);
            }


            StyleManager.RegisterControls(this);
        }

        private void lbPluginHosts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbPluginHosts.SelectedIndex != -1)
            {
                lbPlugins.Items.Clear();
                if (lbPluginHosts.SelectedItem is IPluginHost host)
                {
                    List<IPlugin> plugins = PluginManager.GetPlugins<IPlugin>(host);
                    foreach (IPlugin plugin in plugins)
                    {
                        lbPlugins.Items.Add(plugin);
                    }
                }
                else
                {
                    List<string> packages = ListHelper.LoadList(PluginPaths.GlobalPluginListFile).ToList();
                    List<string> installed = ListHelper.LoadList(PluginPaths.PluginListFile).ToList();
                    if (lbPluginHosts.SelectedItem.ToString() == "[Inactive Packages]")
                    {
                        foreach (string package in packages.Where(x => !installed.Contains(x)))
                        {
                            lbPlugins.Items.Add(package);
                        }
                    }
                    else if (lbPluginHosts.SelectedItem.ToString() == "[Active Packages]")
                    {
                        foreach (string package in packages.Where(x => installed.Contains(x)))
                        {
                            lbPlugins.Items.Add(package);
                        }
                    }
                }
            }
        }

        private void ToggleLoadState(object sender, EventArgs e)
        {
            if (lbPlugins.SelectedIndex != -1)
            {
                if (lbPlugins.SelectedItem is IPlugin plugin)
                {
                }
                else
                {
                    bool isActive = lbPluginHosts.SelectedItem.ToString() != "[Inactive Packages]";
                    BasePluginPointer ptr = new BasePluginPointer(lbPlugins.SelectedItem.ToString());
                    if (!isActive)
                    {
                        ActionRunner.AddActionToStartup($"{ActionRunner.ACTIVATE_PACKAGE_ACTION} {ptr.PluginName}");
                    }
                    else
                    {
                        ActionRunner.AddActionToStartup($"{ActionRunner.DEACTIVATE_PACKAGE_ACTION} {ptr.PluginName}");
                    }

                    lbPlugins.Items.Remove(lbPlugins.SelectedItem);
                }
            }
        }

    }
}