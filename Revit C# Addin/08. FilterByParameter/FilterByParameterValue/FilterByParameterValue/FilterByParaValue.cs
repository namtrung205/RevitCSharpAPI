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

namespace FilterByParameterValue
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class FilterByParameterValue : IExternalCommand
    {
        // Set GUID [Guid("472FE4ED-8F4A-4E9B-8DDA-B90D674F9171")]
        static AddInId appId = new AddInId(new Guid("472FE4ED-8F4A-4E9B-8DDA-B90D674F9171"));

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
            selectByParaSpecial(commandData.Application.ActiveUIDocument);
            return Autodesk.Revit.UI.Result.Succeeded;
        }

        //
        public void selectByParaSpecial(UIDocument uiDoc)
        {

            // pick first element to get all parameters
            Document doc = uiDoc.Document;

            Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element, "Select an Element...");

            Element myFirstElem = doc.GetElement(myRef);

            // Get parameters of element
            ParameterSet myParaSet = myFirstElem.Parameters;



            List<string> myListParaName = new List<string>();
            List<string> myListParaValueString = new List<string>();

            bool saveSelection = false;

            foreach (Parameter myPara in myParaSet)
            {
                //TaskDialog.Show("abc", myPara.Definition.Name + "Value: " + myPara.AsValueString());
                myListParaName.Add(myPara.Definition.Name);

                if (myPara.StorageType == StorageType.String)
                {
                    myListParaValueString.Add(myPara.AsString());
                }

                else
                {
                    myListParaValueString.Add(myPara.AsValueString());
                }

            }

            //TaskDialog.Show("abc","num value: " + myListParaName.Count.ToString());
            //Reference myRef2 = uiDoc.Selection.PickObject(ObjectType.Element, new FilterByNameElementType(myListType));

            List<ElementId> myListIdElem = new List<ElementId>();

            string paraNameSelected = "a";
            string paraValueSelected = "b";

            using (FilterForm myFormSelect = new FilterForm(myListParaName, myListParaValueString))

            {
                // Add list parameter to cb
                myFormSelect.ShowDialog();

                //if the user hits cancel just drop out of macro
                if (myFormSelect.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    //else do all this :)    
                    myFormSelect.Close();
                    return;
                }

                if (myFormSelect.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    paraNameSelected = myFormSelect.paraName_Cb.Text;
                    paraValueSelected = myFormSelect.value_Tb.Text;
                    saveSelection = myFormSelect.saveSelection;
                    myFormSelect.Close();
                }


            }

            // Select by Filter

            List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element,
                                                                    new FilterByParameterValueSelectClass(paraNameSelected, paraValueSelected),
                                                                    "Select Element") as List<Reference>;

            foreach (Reference myRefe in myListRef) // Iterate through list of selected elements
            {
                // Add the ElementId of each selected element to the selection filter
                myListIdElem.Add(myRefe.ElementId);
            }

            if (saveSelection)
            {
                using (Transaction t = new Transaction(doc, "Create & Add To Selection Filter"))
                {
                    t.Start(); // Start the transaction

                    // Create a SelectionFilterElement
                    SelectionFilterElement selFilter = SelectionFilterElement.Create(doc, paraNameSelected + " - " + paraValueSelected);



                    foreach (Reference r in myListRef) // Iterate through list of selected elements
                    {
                        // Add the ElementId of each selected element to the selection filter
                        selFilter.AddSingle(r.ElementId);
                    }

                    // Commit the transaction
                    t.Commit();
                }

            }
            uiDoc.Selection.SetElementIds(myListIdElem);

            //TaskDialog.Show("abc", "number element: " + myListRef.Count);

        }


    }



    public class FilterByParameterValueSelectClass : ISelectionFilter
    {
        public string prName;
        public string prValue;

        public FilterByParameterValueSelectClass(string paraName, string paraValue)
        {
            this.prName = paraName;
            this.prValue = paraValue;

        }

        public bool AllowElement(Element e)
        {
            //ParameterSet myParas = e.Parameters;

            Parameter myPara = e.GetParameters(this.prName).FirstOrDefault();

            if (myPara.StorageType == StorageType.String)
            {
                if (myPara.AsString() == this.prValue)
                {
                    return true;
                }

            }


            if (myPara.AsValueString() == this.prValue)
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
