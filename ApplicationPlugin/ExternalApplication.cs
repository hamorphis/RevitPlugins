using Autodesk.Revit.UI;

namespace ApplicationPlugin
{
    class ExternalApplication : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            TaskDialog.Show("External Application", "Hello from External Application");
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
