using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
// Lib Revit API
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI.Selection;

namespace IncreaseParameterForm
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class mainCS : IExternalCommand
    {
        // Set GUID [Guid("F68829EF-0540-42B8-9F5C-37972E069E6B")]
        static AddInId appId = new AddInId(new Guid("F68829EF-0540-42B8-9F5C-37972E069E6B"));

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

                increaseValueParameter(commandData.Application.ActiveUIDocument);
                return Autodesk.Revit.UI.Result.Succeeded;

            }

            catch {

                return Result.Succeeded;

            }
        }



        public void increaseValueParameter(UIDocument uiDoc)
        {
            Document doc = uiDoc.Document;

            string prefix = "9B";

            int beginNumber = 1;

            string suffix = "";


            string parameterName = "INNO_Ten-CK";

            using (IncreaseParameterForm myFormIncreaseParameter = new IncreaseParameterForm())

            {
                // Add list parameter to cb
                myFormIncreaseParameter.ShowDialog();

                //if the user hits cancel just drop out of macro
                if (myFormIncreaseParameter.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    //else do all this :)    
                    myFormIncreaseParameter.Close();
                    return;
                }

                if (myFormIncreaseParameter.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    parameterName = myFormIncreaseParameter.paraName_Tb.Text;

                    prefix = myFormIncreaseParameter.Pre_Tb.Text;
                    beginNumber = Convert.ToInt32(myFormIncreaseParameter.startNum_Tb.Text);
                    suffix = myFormIncreaseParameter.Suf_Tb.Text;
                    myFormIncreaseParameter.Close();
                }


            }

            while (true)

            {
                List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, "Select Elements...") as List<Reference>;

                using (Transaction trans = new Transaction(doc, "Increase Parameter..."))
                {
                    trans.Start();

                    foreach (Reference myRef in myListRef)
                    {
                        Element myElem = doc.GetElement(myRef);
                        Parameter myParameter = myElem.LookupParameter(parameterName);
                        if (myParameter == null)
                        {
                            TaskDialog.Show("Error!!", "Has no parpation name: " + parameterName);
                        }

                        else
                        {
                            myParameter.Set(prefix + beginNumber.ToString() + suffix);

                        }

                    }
                    trans.Commit();
                }
                beginNumber++;
            }

        }




    }


    public class FilterByNameElementType : ISelectionFilter
    {

        //Cac bien thanh vien
        List<string> myListNameFilter = new List<string>();

        // Bo khoi dung
        public FilterByNameElementType(List<string> myListName)
        {
            myListNameFilter = myListName;
        }

        public bool AllowElement(Element e)
        {
            string typeE = e.GetType().Name.ToString();

            if (this.myListNameFilter.Contains(typeE))
            {
                return true;
            }

            return false;
        }

        public bool AllowReference(Reference r, XYZ myXYZ)
        {
            return true;
        }

    }

}
