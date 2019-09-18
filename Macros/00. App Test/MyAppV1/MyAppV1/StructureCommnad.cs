using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAppV1
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]


    //Sample......
    public class StructureCommand : IExternalCommand
    {
        // Set GUID [Guid("4A202994-8AA4-40F9-810D-3A0F663A6CA8")]
        static AddInId appId = new AddInId(new Guid("4A202994-8AA4-40F9-810D-3A0F663A6CA8"));

        /// <summary>
        /// Main Function
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="message"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Thưc thi hàm createElevationByRooms
            try
            {
                helloWorld(commandData.Application.ActiveUIDocument);
                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch { return Autodesk.Revit.UI.Result.Succeeded; }

        }
        public void helloWorld(UIDocument uiDoc)
        {
            TaskDialog.Show("Done", "Hello World");

        }
    }




}
