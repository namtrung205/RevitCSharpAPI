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

namespace copyNPlaceFamilyAtBlock
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class copyNPlaceFamilyAtBlock : IExternalCommand
    {
        // Set [Guid("61C5853A-CBD2-46A3-9F70-3717F8B3106F")]
        static AddInId appId = new AddInId(new Guid("61C5853A-CBD2-46A3-9F70-3717F8B3106F"));

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
                copyAndPlaceFamilyAtDwgBlock(commandData.Application.ActiveUIDocument);
                return Result.Succeeded;
            }

            catch
            {
                //TaskDialog.Show("Error", "OK ");
                return Result.Succeeded;

            }
        }




        public void copyAndPlaceFamilyAtDwgBlock(UIDocument uiDoc)
        {
            Document doc = uiDoc.Document;


            //Select ImportInstance

            //List<string> myListImportDwg = new List<string>() {"Import Symbol"};

            Reference myRefImportDWG = uiDoc.Selection.PickObject(ObjectType.Element,
                                                              "Pick a Import DWG...");
            ImportInstance myImportInstance = doc.GetElement(myRefImportDWG) as ImportInstance;
            Element dwgImportElement = doc.GetElement(myRefImportDWG);


            string myTextClipBoard = Clipboard.GetText();


            Dictionary<XYZ, double> myDicCoorAndRot = getOriginAndRotByBlock(dwgImportElement, myTextClipBoard);
            if (myDicCoorAndRot.Keys.Count < 1)
            {
                TaskDialog.Show("Error!!", "CLipboard không có dữ liệu, hoặc file .DWG không chứa block có tên như trong clipboard");
                return;
            }

            Reference myRefFamily = uiDoc.Selection.PickObject(ObjectType.Element, "Select Instance Family...");

            Element myFamilyElement = doc.GetElement(myRefFamily);


            XYZ originInstance = myImportInstance.GetTransform().Origin;

            LocationCurve locCurve = myFamilyElement.Location as LocationCurve;
            if (null == locCurve)
            {
                XYZ pointRef = ((LocationPoint)myFamilyElement.Location).Point;


                XYZ deltaXYZ = new XYZ();
                List<ElementId> myElemIdCopiedColTotal = new List<ElementId>();
                foreach (XYZ myXYZ in myDicCoorAndRot.Keys)
                {
                    List<ElementId> myElemIdCopiedCol = new List<ElementId>();

                    //Copy Element
                    using (Transaction myTrans = new Transaction(doc, "Copy Element"))
                    {
                        myTrans.Start();
                        deltaXYZ = originInstance + myXYZ - pointRef;
                        myElemIdCopiedCol = ElementTransformUtils.CopyElement(doc, myFamilyElement.Id, deltaXYZ).ToList();
                        myTrans.Commit();
                    }

                    using (Transaction myTrans = new Transaction(doc, "RotateElement Location Point"))
                    {
                        myTrans.Start();
                        // Code here
                        //ElementTransformUtils.RotateElement(doc, myEle.Id, axis, DegreesToRadians(degrees));

                        foreach (ElementId myIdEleCopied in myElemIdCopiedCol)
                        {
                            Element myElemCopied = doc.GetElement(myIdEleCopied);
                            XYZ point = ((LocationPoint)myElemCopied.Location).Point;
                            XYZ point2 = point.Add(XYZ.BasisZ);

                            Line axis = Line.CreateBound(point, point2);

                            ElementTransformUtils.RotateElement(doc, myIdEleCopied, axis, myDicCoorAndRot[myXYZ]);
                            myElemIdCopiedColTotal.Add(myIdEleCopied);

                        }
                        myTrans.Commit();

                    }
 
                }

                // Make group From element Cp=opied
                using (Transaction trans = new Transaction(doc, "Make group from Copied Element"))
                {
                    trans.Start();
                    if (myElemIdCopiedColTotal.Count > 0)
                    {
                        Group myGroupRebar = doc.Create.NewGroup(myElemIdCopiedColTotal);
                        //	myGroupRebar.GroupType.Name = rebarGroupName;

                    }
                    else
                    {
                        TaskDialog.Show("Warning!", "No rebar was hosted by this element, so no any group was created!");
                    }
                    trans.Commit();
                }
            }
        }




        private Dictionary<XYZ, double> getOriginAndRotByBlock(Element myDwgImportElement, string nameOfBlock)
        {

            Dictionary<XYZ, double> myReturnDic = new Dictionary<XYZ, double>();
            GeometryElement myGeoElem = myDwgImportElement.get_Geometry(new Options());
            foreach (GeometryObject GeoOj in myGeoElem)
            {
                GeometryInstance instance = GeoOj as GeometryInstance;
                if (instance != null)
                {
                    foreach (GeometryObject instObj in instance.SymbolGeometry)
                    {
                        if (instObj is GeometryInstance)
                        {
                            GeometryInstance blockInstance = instObj as GeometryInstance;

                            string name = blockInstance.Symbol.Name;

                            if (name == nameOfBlock)
                            {
                                Transform transform = blockInstance.Transform;

                                XYZ origin = transform.Origin;
                                if (!myReturnDic.Keys.Contains(origin))
                                {
                                    XYZ vectorTran = transform.OfVector(transform.BasisX.Normalize());
                                    double rot = transform.BasisX.AngleOnPlaneTo(vectorTran, transform.BasisZ.Normalize()); // radians
                                                                                                                            //rot = rot * (180 / Math.PI); // degrees

                                    myReturnDic.Add(origin, rot);
                                }
                            }
                        }
                    }
                }

            }
            return myReturnDic;
        }


    }
    //By name type of class. Ex: "AngularDimension", Wall

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
