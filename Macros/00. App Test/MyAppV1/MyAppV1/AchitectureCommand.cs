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
    public class AchitectureCommand : IExternalCommand
    {
        // Set GUID [Guid("3EA9BB68-6846-43AF-9192-782F4A2CDF03")]
        static AddInId appId = new AddInId(new Guid("3EA9BB68-6846-43AF-9192-782F4A2CDF03"));

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
            TaskDialog.Show("Achitecture", "Hello From Achitecture Button");

        }
    }



}
