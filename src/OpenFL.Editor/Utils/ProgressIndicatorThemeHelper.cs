namespace OpenFL.Editor.Utils
{
    public static class ProgressIndicatorThemeHelper
    {

        public static void ApplyTheme(ProgressIndicator indicator)
        {
            StyleManager.RegisterControl(indicator.gbSubTask, "progress");
            StyleManager.RegisterControl(indicator.lblStatus, "progress");
            StyleManager.RegisterControl(indicator.mainProgressPanel, "default", "progress");
            StyleManager.RegisterControl(indicator.panelMain, "progress");
            StyleManager.RegisterControl(indicator.pbProgress, "progress");
            StyleManager.RegisterControl(indicator.subTaskPanel, "progress");

            //FLScriptEditor.RegisterDefaultTheme(indicator.gbSubTask);
            //FLScriptEditor.RegisterDefaultTheme(indicator.lblStatus);
            //FLScriptEditor.RegisterDefaultTheme(indicator.mainProgressPanel);
            //FLScriptEditor.RegisterDefaultTheme(indicator.panelMain);
            //FLScriptEditor.RegisterDefaultTheme(indicator.pbProgress);
            //FLScriptEditor.RegisterDefaultTheme(indicator.subTaskPanel);
        }

    }
}