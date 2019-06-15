using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Lib Revit API
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI.Selection;

namespace PaintFaceAuto
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class PaintFaceAuto : IExternalCommand
    {
        // Set GUID [Guid("79E1A28D-F513-4A26-A004-D0233C975E28")]
        static AddInId appId = new AddInId(new Guid("79E1A28D-F513-4A26-A004-D0233C975E28"));

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
            paintFaceAuto(commandData.Application.ActiveUIDocument);
            return Autodesk.Revit.UI.Result.Succeeded;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiDoc">Active UIDocument</param> 
        private void paintFaceAuto(UIDocument uiDoc)
        {

            Document doc = uiDoc.Document;

            //Pick to get material from face(get MaterialId)			
            Reference myRef1 = uiDoc.Selection.PickObject(ObjectType.Face, "Select Face to get material...");

            // Get element by pickface
            Element e = doc.GetElement(myRef1) as Element;

            //Get GeoObject from element;
            GeometryObject myGeoObj = e.GetGeometryObjectFromReference(myRef1) as Face;

            //Get face from element Object:
            Face myPickedFace = myGeoObj as Face;
            ElementId myMaterialEleId = myPickedFace.MaterialElementId;

            //filtered
            List<string> myFil = new List<string>() { "Wall", "Floor" };

            // Select multiple Wall
            List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element,
                             new FilterByNameElementType(myFil), "Select Walls, Floor...") as List<Reference>;


            foreach (Reference myRef in myListRef)
            {

                Element myElem = doc.GetElement(myRef);
                if (myElem.GetType().Name == "Wall")
                {

                    Wall myWall = myElem as Wall;
                    paintFaceOfWall(doc,myWall, myMaterialEleId);
                }
                else if (myElem.GetType().Name == "Floor")
                {
                    if (myElem.Category.Name == "Structural Foundations")
                    {
                        //TaskDialog.Show("abc", "Paint Foundation");
                        Floor mySlab = myElem as Floor;
                        paintFaceOfFoundation(doc, mySlab, myMaterialEleId);
                    }
                    else
                    {
                        Floor myFloor = myElem as Floor;
                        paintFaceOfFloor(doc,myFloor, myMaterialEleId);
                        //						TaskDialog.Show("abc", "Paint Floor");
                    }
                }
            }
        }


        private void paintFaceOfFoundation(Document doc, Floor myFoundation, ElementId myMatId)
        {

            GeometryElement geometryElement = myFoundation.get_Geometry(new Options());

            using (Transaction myTrans = new Transaction(doc, "filter face of foundation and Paint"))
            {

                myTrans.Start();
                UV myPoint = new UV(0, 0);

                foreach (GeometryObject geometryObject in geometryElement)
                {
                    if (geometryObject is Solid)
                    {
                        Solid solid = geometryObject as Solid;
                        XYZ myNormVec = new XYZ();
                        foreach (Face myFace in solid.Faces)
                        {
                            myNormVec = myFace.ComputeNormal(myPoint);
                            if (Math.Round(Math.Abs(myNormVec.Z), 1) != 1.0)
                            {
                                doc.Paint(myFoundation.Id, myFace, myMatId);
                            }
                        }
                    }
                }
                myTrans.Commit();
            }

        }


        private void paintFaceOfFloor(Document doc, Floor myFloor, ElementId myMatId)
        {

            GeometryElement geometryElement = myFloor.get_Geometry(new Options());

            using (Transaction myTrans = new Transaction(doc, "filter face of Floor and Paint"))
            {

                myTrans.Start();
                UV myPoint = new UV(0, 0);

                foreach (GeometryObject geometryObject in geometryElement)
                {
                    if (geometryObject is Solid)
                    {
                        Solid solid = geometryObject as Solid;
                        XYZ myNormVec = new XYZ();
                        foreach (Face myFace in solid.Faces)
                        {
                            myNormVec = myFace.ComputeNormal(myPoint);
                            if (Math.Round(myNormVec.Z, 1) != 1.0)
                            {
                                doc.Paint(myFloor.Id, myFace, myMatId);
                            }
                        }
                    }
                }
                myTrans.Commit();
            }

        }


        private void paintFaceOfWall(Document doc, Wall myWall, ElementId myMatId)
        {

            GeometryElement geometryElement = myWall.get_Geometry(new Options());

            double widthWall = myWall.Width;
            // Lay cuver of location cuver

            LocationCurve myWallLc = myWall.Location as LocationCurve;

            Curve myWallCurve = myWallLc.Curve as Curve;

            double myAreaOfWall = myWallCurve.Length * widthWall;


            using (Transaction myTrans = new Transaction(doc, "fil face of wall and Paint"))
            {

                myTrans.Start();
                UV myPoint = new UV(0, 0);

                foreach (GeometryObject geometryObject in geometryElement)
                {
                    if (geometryObject is Solid)
                    {
                        Solid solid = geometryObject as Solid;
                        XYZ myNormVec = new XYZ();
                        foreach (Face myFace in solid.Faces)
                        {
                            myNormVec = myFace.ComputeNormal(myPoint);

                            // If normal vector of face has Z value != -1 paint
                            if (Math.Abs(Math.Round(myNormVec.Z, 1)) != 1.0)
                            {
                                doc.Paint(myWall.Id, myFace, myMatId);
                            }

                            // else 
                            else
                            {
                                if (Math.Round(myNormVec.Z, 1) == -1.0 && !isBottomFace(doc, myFace, myWall))
                                {
                                    doc.Paint(myWall.Id, myFace, myMatId);
                                }
                            }

                        }
                    }
                }
                myTrans.Commit();
            }

        }


        private bool isBottomFace(Document doc, Face myFace, Wall myWall)

        {

            BoundingBoxXYZ myBoundWall = myWall.get_BoundingBox(null);

            double zMinValue = myBoundWall.Min.Z;

            if (Math.Abs(zMinValue - getElevetionOfFace(myFace)) < 0.001)
            {
                return true;
            }

            return false;

        }


        private double getElevetionOfFace(Face myFAce)
        {
            //get array EdgeArray
            EdgeArrayArray myEdgeArAr = myFAce.EdgeLoops;

            XYZ testPoint = new XYZ();

            foreach (EdgeArray edgeAr in myEdgeArAr)
            {
                foreach (Edge myEdge in edgeAr)
                {
                    // Get one test point
                    testPoint = myEdge.Evaluate(0.5);
                    break;
                }
                break;
            }
            return testPoint.Z;

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
