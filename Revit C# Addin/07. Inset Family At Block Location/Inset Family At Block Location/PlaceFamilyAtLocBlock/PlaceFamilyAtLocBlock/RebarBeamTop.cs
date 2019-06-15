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
using Autodesk.Revit.DB.Structure;
using System.Windows.Forms;

namespace PlaceFamilyAtLocBlock
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class PlaceFamilyAtLocBlock : IExternalCommand
    {
        // Set [Guid("D7A773B8-2D0E-4B78-B70E-0DF8929F96E5")]
        static AddInId appId = new AddInId(new Guid("D7A773B8-2D0E-4B78-B70E-0DF8929F96E5"));

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
                PlaceFamily(commandData.Application.ActiveUIDocument);
                return Result.Succeeded;
            }

            catch
            {
                //TaskDialog.Show("Error", "OK ");
                return Result.Succeeded;

            }
        }



        public void PlaceFamily(UIDocument uiDoc)
        {

            Document doc = uiDoc.Document;

            ElementId eId = uiDoc.Selection.PickObject(ObjectType.Element).ElementId;

            Element e = doc.GetElement(eId);

            List<XYZ> myCoors = new List<XYZ>();

            string myTextClipBoard = Clipboard.GetText();

            //Split each line in
            string[] lines = myTextClipBoard.Split(';');
            foreach (string line in lines)
            {
                string[] myItem = line.Split('|');
                if (myItem.Length - 1 == 2)
                {
                    if (myItem[0] != "0" || myItem[1] != "0")
                    {
                        myCoors.Add(new XYZ(Convert.ToDouble(myItem[0]) / 304.8, Convert.ToDouble(myItem[1]) / 304.8, Convert.ToDouble(myItem[2]) / 304.8));
                    }
                }
            }

            if (myCoors.Count < 1)
            {
                TaskDialog.Show("Empty Data", "ClipBoard không có dữ liệu phù hợp...");
                return;
            }

            using (Transaction myTrans = new Transaction(doc, "CopyElementByCoordinate"))

            {
                myTrans.Start();
                foreach (XYZ myXYZ in myCoors)
                {
                    ElementTransformUtils.CopyElement(doc, eId, myXYZ);
                }
                myTrans.Commit();
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


    public class FilterByIdCategory : ISelectionFilter
    {
        //Cac bien thanh vien
        List<int> ListIdCategory = new List<int>();


        // Bo khoi dung
        public FilterByIdCategory(List<int> myListIdCategory)
        {
            this.ListIdCategory = myListIdCategory;
        }

        public bool AllowElement(Element e)
        {
            int categoryE = e.Category.Id.IntegerValue;

            if (this.ListIdCategory.Contains(categoryE))
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
