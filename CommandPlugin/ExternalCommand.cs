using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Diagnostics;

namespace CommandPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ExternalCommand : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            if (uiApp.ActiveUIDocument == null || uiApp.ActiveUIDocument.ActiveGraphicalView == null)
            {
                return Result.Failed;
            }

            var view = uiApp.ActiveUIDocument.ActiveGraphicalView;
            Debug.WriteLine("ActiveGraphicalView: " + view.ToDebugString());
            ShowInfo(view);
            return Result.Succeeded;
        }

        private static void ShowInfo(View view)
        {
            string message = "View: " + view.UniqueId;

            //get the name of the view
            message += "\nType: " + view.ViewType;

            message += "\nName: " + view.ViewName;

            //The crop box sets display bounds of the view
            BoundingBoxXYZ cropBox = view.CropBox;
            XYZ max = cropBox.Max; //Maximum coordinates (upper-right-front corner of the box).
            XYZ min = cropBox.Min; //Minimum coordinates (lower-left-rear corner of the box).
            message += "\nCrop Box: ";
            message += "\nMaximum coordinates: (" + max.X + ", " + max.Y + ", " + max.Z + ")";
            message += "\nMinimum coordinates: (" + min.X + ", " + min.Y + ", " + min.Z + ")";


            //get the origin of the screen
            XYZ origin = view.Origin;
            message += "\nOrigin: (" + origin.X + ", " + origin.Y + ", " + origin.Z + ")";


            //The bounds of the view in paper space (in inches).
            BoundingBoxUV outline = view.Outline;
            UV maxUv = outline.Max; //Maximum coordinates (upper-right corner of the box).
            UV minUv = outline.Min; //Minimum coordinates (lower-left corner of the box).
            message += "\nOutline: ";
            message += "\nMaximum coordinates: (" + maxUv.U + ", " + maxUv.V + ")";
            message += "\nMinimum coordinates: (" + minUv.U + ", " + minUv.V + ")";

            //The direction towards the right side of the screen
            XYZ rightDirection = view.RightDirection;
            message += "\nRight direction: (" + rightDirection.X + ", " +
                           rightDirection.Y + ", " + rightDirection.Z + ")";

            //The direction towards the top of the screen
            XYZ upDirection = view.UpDirection;
            message += "\nUp direction: (" + upDirection.X + ", " +
                           upDirection.Y + ", " + upDirection.Z + ")";

            //The direction towards the viewer
            XYZ viewDirection = view.ViewDirection;
            message += "\nView direction: (" + viewDirection.X + ", " +
                           viewDirection.Y + ", " + viewDirection.Z + ")";

            //The scale of the view
            message += "\nScale: " + view.Scale;
            // Scale is meaningless for Schedules
            //if (view.ViewType != ViewType.Schedule)
            //{
            //    int testScale = 5;
            //    //set the scale of the view
            //    view.Scale = testScale;
            //    message += "\nScale after set: " + view.Scale;
            //}

            TaskDialog.Show("Revit", message);
        }
    }

    public static class Extensions
    {
        public static string ToDebugString(this View view)
        {
            return $"{view.UniqueId} {view.ViewName} {view.ViewType}";
        }
    }
}
