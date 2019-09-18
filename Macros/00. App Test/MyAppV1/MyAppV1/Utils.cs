using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAppV1
{

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



    //Chi chon duoc cac canh tren mot so loai doi tuong ma vuong goc voi mat phang View
    public class FilterEdgePerpencularWithViewAndCatergory : ISelectionFilter
    {

        //Cac bien thanh vien
        List<string> ListNameCategory = new List<string>();
        Document doc = null;

        // Bo khoi dung
        public FilterEdgePerpencularWithViewAndCatergory(List<string> myListNameCatergory, Document doc_para)
        {
            this.ListNameCategory = myListNameCatergory;
            this.doc = doc_para;
        }


        public bool AllowElement(Element e)
        {

            if (this.ListNameCategory.Count == 0)
            {
                return true;
            }


            string categoryE = e.Category.Name;

            if (this.ListNameCategory.Contains(categoryE))
            {
                return true;
            }
            return false;
        }



        public bool AllowReference(Reference myRef, XYZ myXYZ)
        {
            //Chi chap nhan cac edge cutting by View Plane
            //						
            //			if(myRef.ElementReferenceType == ElementReferenceType.REFERENCE_TYPE_CUT_EDGE)
            //			{
            //				return true;
            //			}
            //			

            // Get element by pickface
            View myView = doc.ActiveView;
            Plane myPlaneFromView = Plane.CreateByNormalAndOrigin(myView.ViewDirection, myView.Origin);


            //Get face from element Object:
            Options myOptions = new Options();
            myOptions.ComputeReferences = true;
            Edge myPickedEdge = null;
            Element myElement = doc.GetElement(myRef);
            GeometryElement geometryElement = myElement.get_Geometry(myOptions);
            if (geometryElement.Count() < 2)
            {
                if (!(geometryElement.ElementAt(0) is Solid))
                {
                    //TaskDialog.Show("abc", "XYZ");
                    myPickedEdge = GetInstanceEdgeFromSymbolRef(myRef, doc);
                }
                else
                {
                    //TaskDialog.Show("abc", " Else Z");
                    myPickedEdge = doc.GetElement(myRef).GetGeometryObjectFromReference(myRef) as Edge;
                }

            }
            else
            {
                //TaskDialog.Show("abc", "BCN");
                myPickedEdge = doc.GetElement(myRef).GetGeometryObjectFromReference(myRef) as Edge;

            }


            Curve myEdgeCurve = myPickedEdge.AsCurve();

            Line edgeAsLine = myEdgeCurve as Line;

            if (edgeAsLine != null)
            {
                //Section Point Edge and View
                XYZ mySectionPoint = Utils_Geometry_OXYZ.getSectionPointWithPlane(edgeAsLine, myPlaneFromView);


                if (Utils_Geometry_OXYZ.check2VectorParallel(edgeAsLine.Direction, myPlaneFromView.Normal))
                {
                    //					TaskDialog.Show("abc", "Perpencular...");
                    return true;
                }


                return false;
            }
            else
            {
                return false;
            }

        }



        public Edge GetInstanceEdgeFromSymbolRef(Reference symbolRef, Autodesk.Revit.DB.Document dbDoc)
        {
            Edge instEdge = null;

            Options gOptions = new Options();
            gOptions.ComputeReferences = true;
            gOptions.DetailLevel = ViewDetailLevel.Undefined;
            gOptions.IncludeNonVisibleObjects = false;

            Element elem = dbDoc.GetElement(symbolRef.ElementId);
            string stableRefSymbol = symbolRef.ConvertToStableRepresentation(dbDoc);
            string[] tokenList = stableRefSymbol.Split(new char[] { ':' });
            string stableRefInst = tokenList[3] + ":" + tokenList[4] + ":" + tokenList[5];

            GeometryElement geomElem = elem.get_Geometry(gOptions);
            foreach (GeometryObject geomElemObj in geomElem)
            {
                GeometryInstance geomInst = geomElemObj as GeometryInstance;
                if (geomInst != null)
                {
                    GeometryElement gInstGeom = geomInst.GetInstanceGeometry();
                    foreach (GeometryObject gGeomObject in gInstGeom)
                    {
                        Solid solid = gGeomObject as Solid;
                        if (solid != null)
                        {
                            foreach (Edge edge in solid.Edges)
                            {
                                string stableRef = edge.Reference.ConvertToStableRepresentation(dbDoc);

                                if (stableRef == stableRefInst)
                                {
                                    instEdge = edge;
                                    break;
                                }
                            }
                        }

                        if (instEdge != null)
                        {
                            // already found, exit early
                            break;
                        }
                    }
                }
                if (instEdge != null)
                {
                    // already found, exit early
                    break;
                }
            }
            return instEdge;
        }


    }





    //Các lớp riêng để tập hợp các thông tin cần thiết
    public class CustomElement2DInfo
    {
        public double H;
        public XYZ CenterPoint;
        public CurveLoop CurveLoopBot;
        public ReferenceArray SideRefArray;
        public ReferenceArray TopRefArray;
        public double Elevation;

        public CustomElement2DInfo(double h, XYZ centerPoint, ReferenceArray sideRefArray, ReferenceArray topRefArray, double elevation)
        {
            this.H = h;
            this.CenterPoint = centerPoint;
            this.SideRefArray = sideRefArray;
            this.TopRefArray = topRefArray;
            this.Elevation = elevation;
        }
    }


    //Các lớp riêng để tập hợp các thông tin cần thiết
    public class CustomElement1DInfo
    {
        public double H;
        public double B;
        public XYZ CenterPoint;
        public ReferenceArray SideRefArray;
        public ReferenceArray TopRefArray;
        public double Elevation;

        public CustomElement1DInfo(double h, double b, XYZ centerPoint, ReferenceArray sideRefArray, ReferenceArray topRefArray, double elevation)
        {
            this.H = h;
            this.B = b;
            this.CenterPoint = centerPoint;
            this.SideRefArray = sideRefArray;
            this.TopRefArray = topRefArray;
            this.Elevation = elevation;
        }
    }



    //Các lớp riêng để tập hợp các thông tin cần thiết
    public class CustomElement0DInfo
    {
        public double H;
        public XYZ CenterPoint;
        public CurveLoop CurveLoopBot;
        public ReferenceArray SideRefArray;
        public ReferenceArray TopRefArray;

        public double Elevation;

        public CustomElement0DInfo(double h, XYZ centerPoint, ReferenceArray sideRefArray, ReferenceArray topRefArray, double elevation)
        {
            this.H = h;
            this.CenterPoint = centerPoint;
            this.SideRefArray = sideRefArray;
            this.TopRefArray = topRefArray;
            this.Elevation = elevation;
        }
    }


    public class Utils
    {

        //Tính khoảng cách từ điểm tới mặt phẳng(có dấu)
        public double getDisFromPointToPlane(XYZ myPoint, Plane myPlane)
        {
            // Vector from Origin to my point
            XYZ v = myPlane.Origin - myPoint;

            double distance = myPlane.Normal.DotProduct(v);
            return distance;
        }

        //Get Point project onto plane
        //Lấy hình chiếu của 1 điểm trên 1 plane
        public XYZ getPointProjectFromPointOntoPlane(XYZ myPoint, Plane myPlane)
        {
            // Vector from Origin to my point
            XYZ v = myPlane.Origin - myPoint;

            double d = myPlane.Normal.DotProduct(v);
            if (Math.Round(d, 8) == 0.0)
            {
                return myPoint;
            }

            XYZ q = myPoint + d * myPlane.Normal;

            return q;
        }


        //Tính khoảng cách từ điểm tới Face(có dấu)
        public double getDisFromPointToFace(XYZ myPoint, Face myFace)
        {

            PlanarFace myPlanarface = myFace as PlanarFace;
            Plane myPlane = Plane.CreateByNormalAndOrigin(myPlanarface.FaceNormal, myPlanarface.Origin);

            // Vector from Origin to my point
            XYZ v = myPlane.Origin - myPoint;

            double distance = myPlane.Normal.DotProduct(v);
            return distance;
        }



        //Get Point project onto plane
        //Lấy hình chiếu của 1 điểm trên 1 plane
        public XYZ getPointProjectFromPointOntoFace(XYZ myPoint, Face myFace)
        {
            PlanarFace myPlanarface = myFace as PlanarFace;


            Plane myPlane = Plane.CreateByNormalAndOrigin(myPlanarface.FaceNormal, myPlanarface.Origin);

            // Vector from Origin to my point
            XYZ v = myPlane.Origin - myPoint;

            double d = myPlane.Normal.DotProduct(v);
            if (Math.Round(d, 8) == 0.0)
            {
                return myPoint;
            }

            XYZ q = myPoint + d * myPlane.Normal;

            return q;
        }



        //Print XYZ
        public void printXYZ(string prompt, XYZ myXYZ)
        {
            TaskDialog.Show("Info", prompt + " X: " + myXYZ.X + "\n" +
                            "Y: " + myXYZ.Y + "\n" +
                            "Z: " + myXYZ.Z);

        }



        public XYZ getMiddlePoint(XYZ p1, XYZ p2)
        {
            return (p1 + p2) / 2;
        }


        /// <summary>
        /// Nhóm hàm các đối tượng hình học : Sàn, Móng băng... dạng giản đơn
        /// </summary>
        public CustomElement2DInfo getGeometryLocation2D(Element myElement)
        {


            List<double> mylistDistance = new List<double>();

            List<Reference> myRefList = new List<Reference>();

            LocationPoint myElementLp = myElement.Location as LocationPoint;
            LocationCurve myElementLc = myElement.Location as LocationCurve;

            if (myElementLc != null || myElementLp != null)
            {
                TaskDialog.Show("Error!", "Doi tuong ban chon phai 2 phuong");
                return null;

            }


            //Get boundingbox

            BoundingBoxXYZ myElement_BB = myElement.get_BoundingBox(null);

            XYZ minPointElement = myElement_BB.Min;
            XYZ maxPointElement = myElement_BB.Max;
            XYZ centerPointElement = (minPointElement + maxPointElement) / 2;
            double h = maxPointElement.Z - minPointElement.Z;


            Options myOptions = new Options();
            myOptions.ComputeReferences = true;

            GeometryElement geometryElement = myElement.get_Geometry(myOptions);

            UV oPoint = new UV(0, 0);

            List<Face> listSideFace = new List<Face>();
            List<Face> listTopFace = new List<Face>();

            foreach (GeometryObject myGeometryObject in geometryElement)
            {
                if (myGeometryObject is Solid)
                {
                    Solid solid = myGeometryObject as Solid;

                    if (solid.Volume > 0)
                    {
                        XYZ myNormVec = new XYZ();
                        foreach (Face myFace in solid.Faces)
                        {
                            myNormVec = myFace.ComputeNormal(oPoint);
                            if (Math.Round(myNormVec.DotProduct(new XYZ(0, 0, 1)), 6) == 0)
                            {
                                listSideFace.Add(myFace);
                            }
                            else
                            {
                                listTopFace.Add(myFace);
                            }
                        }
                    }

                }
            }

            ReferenceArray mySideRefArray = new ReferenceArray();
            foreach (Face myFace in listSideFace)
            {

                mySideRefArray.Append(myFace.Reference);
            }

            ReferenceArray myTopRefArray = new ReferenceArray();
            foreach (Face myFace in listTopFace)
            {
                myTopRefArray.Append(myFace.Reference);
            }


            return new CustomElement2DInfo(h, centerPointElement, mySideRefArray, myTopRefArray, maxPointElement.Z);

        }



        /// <summary>
        /// Nhóm hàm các đối tượng hình học : Dầm, Tường, Móng... dạng giản đơn
        /// </summary>
        public CustomElement1DInfo getGeometryLocation1D_BK(Element myElement)
        {

            double b_max = 0;
            double b_min = 0;
            List<double> mylistDistanceRight = new List<double>();
            List<double> mylistDistanceUp = new List<double>();
            double b = 0;
            List<Reference> myRefList = new List<Reference>();

            LocationCurve myElementlLc = myElement.Location as LocationCurve;

            if (myElementlLc == null)
            {
                TaskDialog.Show("Error!", "Doi tuong ban chon phai 1 phuong");
                return null;

            }

            Line myLcLine = myElementlLc.Curve as Line;
            XYZ rightDirect = new XYZ(myLcLine.Direction.Y, myLcLine.Direction.X * -1, myLcLine.Direction.Z);

            if (myLcLine == null)
            {
                TaskDialog.Show("Error!", "Doi tuong ban chon phai la duong thang");
                return null;
            }


            //Get boundingbox

            BoundingBoxXYZ myElement_BB = myElement.get_BoundingBox(null);

            XYZ minPointElement = myElement_BB.Min;
            XYZ maxPointElement = myElement_BB.Max;
            XYZ centerPointElement = (minPointElement + maxPointElement) / 2;
            XYZ centerPointElementOffsetRight = centerPointElement + 100 * rightDirect;

            XYZ centerPointElementOffsetUp = centerPointElement + 100 * (rightDirect.CrossProduct(myLcLine.Direction));
            double h = maxPointElement.Z - minPointElement.Z;


            Options myOptions = new Options();
            myOptions.ComputeReferences = true;

            GeometryElement geometryElement = myElement.get_Geometry(myOptions);

            UV oPoint = new UV(0, 0);

            List<Face> listSideFace = new List<Face>();
            List<Face> listTopFace = new List<Face>();

            TaskDialog.Show("abc", "solids: " + getSolidsOfElement(myElement).Count);
            foreach (Solid solid in getSolidsOfElement(myElement))
            {
                TaskDialog.Show("abc", "volume: " + solid.Volume);
                XYZ myNormVec = new XYZ();
                foreach (Face myFace in solid.Faces)
                {
                    if (!(myFace is PlanarFace)) { continue; }

                    myNormVec = myFace.ComputeNormal(oPoint);

                    if (Math.Round(myNormVec.DotProduct(myLcLine.Direction), 6) == 0)
                    {
                        if (Math.Round(Math.Abs(myNormVec.Z), 6) != 1)
                        {
                            double bTemp = Math.Abs(getDisFromPointToFace(centerPointElementOffsetRight, myFace));
                            if (!mylistDistanceRight.Contains(bTemp))
                            {

                                mylistDistanceRight.Add(bTemp);
                                // Sort list distance
                                mylistDistanceRight.Sort();

                                //Neu bTemp < mylistDistanceRight, insert list face side vào đầu
                                if (bTemp <= mylistDistanceRight[0])
                                {
                                    listSideFace.Insert(0, myFace);
                                }
                                //Neu bTemp < mylistDistanceRight, insert list face side vào đầu
                                else
                                {
                                    if (bTemp > mylistDistanceRight[mylistDistanceRight.Count - 1])
                                    {
                                        listSideFace.Insert(listSideFace.Count - 1, myFace);
                                    }
                                    else
                                    {
                                        listSideFace.Add(myFace);
                                    }
                                }
                            }
                        }
                        else
                        {
                            double hTemp = Math.Abs(getDisFromPointToFace(centerPointElementOffsetUp, myFace));
                            if (!mylistDistanceUp.Contains(hTemp))
                            {
                                mylistDistanceUp.Add(hTemp);
                                listTopFace.Add(myFace);
                            }

                        }
                    }
                }
            }
            TaskDialog.Show("abc", "listface: " + mylistDistanceRight.Count);
            mylistDistanceRight.Sort();
            b_min = mylistDistanceRight[0];
            b_max = mylistDistanceRight[mylistDistanceRight.Count - 1];


            ReferenceArray mySideRefArray = new ReferenceArray();

            foreach (Face myFace in listSideFace)
            {
                mySideRefArray.Append(myFace.Reference);
            }

            ReferenceArray myTopRefArray = new ReferenceArray();
            foreach (Face myFace in listTopFace)
            {
                myTopRefArray.Append(myFace.Reference);
            }

            b = b_max - b_min;
            //			TaskDialog.Show("abc", "Width beam_max: " + b_max);
            TaskDialog.Show("abc", "Width beam: " + b);
            TaskDialog.Show("abc", "Height beam: " + h);

            return new CustomElement1DInfo(h, b, centerPointElement, mySideRefArray, myTopRefArray, maxPointElement.Z);

        }



        /// <summary>
        /// Nhóm hàm các đối tượng hình học : Dầm, Tường, Móng... dạng giản đơn
        /// </summary>
        public CustomElement1DInfo getGeometryLocation1D_BK2(Element myElement)
        {
            //De tinh chieu cao dam
            double h_max = 0;
            double h_min = 0;
            double h = 0;
            //De tinh be rong dam
            double b_max = 0;
            double b_min = 0;
            double b = 0;

            //Lu danh sach cac mat ||
            List<double> mylistDistanceRight = new List<double>();
            List<double> mylistDistanceUp = new List<double>();

            List<Reference> myRefList = new List<Reference>();

            LocationCurve myElementlLc = myElement.Location as LocationCurve;

            if (myElementlLc == null)
            {
                TaskDialog.Show("Error!", "Doi tuong ban chon phai 1 phuong");
                return null;

            }

            // Xác định các hướng của phần tử
            Line myLcLine = myElementlLc.Curve as Line;
            XYZ lineDrect = myLcLine.Direction.Normalize();
            XYZ rightDirect = (new XYZ(myLcLine.Direction.Y, myLcLine.Direction.X * -1, 0)).Normalize();
            XYZ upDirect = myLcLine.Direction.CrossProduct(rightDirect).Normalize();

            //Test print vector cua dam
            printXYZ("Line Direct", lineDrect);
            printXYZ("Right: ", rightDirect);
            printXYZ("Up: ", upDirect);


            if (myLcLine == null)
            {
                TaskDialog.Show("Error!", "Doi tuong ban chon phai la duong thang");
                return null;
            }


            //Get boundingbox to determine center point of beam
            BoundingBoxXYZ myElement_BB = myElement.get_BoundingBox(null);

            XYZ minPointElement = myElement_BB.Min;
            XYZ maxPointElement = myElement_BB.Max;
            XYZ centerPointElement = (minPointElement + maxPointElement) / 2;

            XYZ centerPointElementOffsetRight = centerPointElement + 100000 * rightDirect;
            XYZ centerPointElementOffsetUp = centerPointElement + 100000 * (upDirect);


            double h_Box = maxPointElement.Z - minPointElement.Z;


            Options myOptions = new Options();
            myOptions.ComputeReferences = true;

            GeometryElement geometryElement = myElement.get_Geometry(myOptions);

            UV oPoint = new UV(0, 0);

            List<Face> listSideFace = new List<Face>();
            List<Face> listTopFace = new List<Face>();

            //			TaskDialog.Show("abc", "solids: " + getSolidsOfElement(myElement).Count);
            foreach (Solid solid in getSolidsOfElement(myElement))
            {
                //				TaskDialog.Show("abc", "volume: " + solid.Volume);
                XYZ myNormVec = new XYZ();
                foreach (Face myFace in solid.Faces)
                {
                    if (!(myFace is PlanarFace)) { continue; }
                    myNormVec = myFace.ComputeNormal(oPoint).Normalize();

                    if (Math.Round(myNormVec.DotProduct(lineDrect), 6) == 0)
                    {
                        //Neu vector chi phuong cua face khong || voi updirect, thi xet truong hop la mat ben
                        if (Math.Round(Math.Abs(myNormVec.DotProduct(upDirect)), 6) != 1)
                        {
                            double bTemp = Math.Abs(getDisFromPointToFace(centerPointElementOffsetRight, myFace));
                            if (!mylistDistanceRight.Contains(bTemp))
                            {

                                mylistDistanceRight.Add(bTemp);
                                // Sort list distance
                                mylistDistanceRight.Sort();

                                //Neu bTemp < mylistDistanceRight, insert list face side vào đầu
                                if (bTemp <= mylistDistanceRight[0])
                                {
                                    listSideFace.Insert(0, myFace);
                                }
                                //Neu bTemp < mylistDistanceRight, insert list face side vào đầu
                                else
                                {
                                    if (bTemp > mylistDistanceRight[mylistDistanceRight.Count - 1])
                                    {
                                        listSideFace.Insert(listSideFace.Count - 1, myFace);
                                    }
                                    else
                                    {
                                        listSideFace.Add(myFace);
                                    }
                                }
                            }
                        }
                        // Xu li cac mat vuong goc voi truc direc
                        else
                        {
                            double hTemp = Math.Abs(getDisFromPointToFace(centerPointElementOffsetUp, myFace));
                            if (!mylistDistanceUp.Contains(hTemp))
                            {

                                mylistDistanceUp.Add(hTemp);
                                // Sort list distance
                                mylistDistanceUp.Sort();

                                //Neu hTemp < mylistDistanceUp, insert list face side vào đầu
                                if (hTemp <= mylistDistanceUp[0])
                                {
                                    listTopFace.Insert(0, myFace);
                                }
                                //Neu bTemp < mylistDistanceRight, insert list face side vào đầu
                                else
                                {
                                    if (hTemp > mylistDistanceUp[mylistDistanceUp.Count - 1])
                                    {
                                        listTopFace.Insert(mylistDistanceUp.Count - 1, myFace);
                                    }
                                    else
                                    {
                                        listTopFace.Add(myFace);
                                    }
                                }
                            }

                        }
                    }
                }
            }

            //Tinh toan h

            h_min = mylistDistanceUp[0];
            h_max = mylistDistanceUp[mylistDistanceUp.Count - 1];


            h = h_max - h_min;

            //			TaskDialog.Show("abc", "listface: " + mylistDistanceRight.Count);
            //			mylistDistanceRight.Sort();

            b_min = mylistDistanceRight[0];
            b_max = mylistDistanceRight[mylistDistanceRight.Count - 1];


            ReferenceArray mySideRefArray = new ReferenceArray();

            foreach (Face myFace in listSideFace)
            {
                mySideRefArray.Append(myFace.Reference);
            }

            ReferenceArray myTopRefArray = new ReferenceArray();
            foreach (Face myFace in listTopFace)
            {
                myTopRefArray.Append(myFace.Reference);
            }

            b = b_max - b_min;
            //			TaskDialog.Show("abc", "Width beam_max: " + b_max);
            TaskDialog.Show("abc", "Width beam: " + b);
            TaskDialog.Show("abc", "Height beam: " + h);

            return new CustomElement1DInfo(h, b, centerPointElement, mySideRefArray, myTopRefArray, maxPointElement.Z);

        }



        /// <summary>
        /// Nhóm hàm các đối tượng hình học : Dầm, Tường, Móng... dạng giản đơn
        /// </summary>
        public CustomElement1DInfo getGeometryLocation1D(Element myElement)
        {
            //De tinh chieu cao dam
            double h_max = 0;
            double h_min = 0;
            double h = 0;
            //De tinh be rong dam
            double b_max = 0;
            double b_min = 0;
            double b = 0;

            //Lu danh sach cac mat ||
            List<double> mylistDistanceRight = new List<double>();
            List<double> mylistDistanceUp = new List<double>();

            List<Reference> myRefList = new List<Reference>();

            LocationCurve myElementlLc = myElement.Location as LocationCurve;

            if (myElementlLc == null)
            {
                TaskDialog.Show("Error!", "Doi tuong ban chon phai 1 phuong");
                return null;

            }

            // Xác định các hướng của phần tử
            Line myLcLine = myElementlLc.Curve as Line;
            XYZ lineDrect = myLcLine.Direction.Normalize();
            XYZ rightDirect = (new XYZ(myLcLine.Direction.Y, myLcLine.Direction.X * -1, 0)).Normalize();
            XYZ upDirect = myLcLine.Direction.CrossProduct(rightDirect).Normalize();

            //Test print vector cua dam
            //printXYZ("Line Direct", lineDrect);
            //printXYZ("Right: ", rightDirect);
            //printXYZ("Up: ", upDirect);


            if (myLcLine == null)
            {
                TaskDialog.Show("Error!", "Doi tuong ban chon phai la duong thang");
                return null;
            }




            //Get boundingbox to determine center point of beam
            BoundingBoxXYZ myElement_BB = myElement.get_BoundingBox(null);

            XYZ minPointElement = myElement_BB.Min;
            XYZ maxPointElement = myElement_BB.Max;
            XYZ centerPointElement = (minPointElement + maxPointElement) / 2;

            XYZ middlePointOnLc = (myLcLine.GetEndPoint(0) + myLcLine.GetEndPoint(1)) / 2;

            centerPointElement = middlePointOnLc;

            XYZ centerPointElementOffsetRight = centerPointElement - 100000 * rightDirect;
            XYZ centerPointElementOffsetUp = centerPointElement - 100000 * (upDirect);


            double h_Box = maxPointElement.Z - minPointElement.Z;


            Options myOptions = new Options();
            myOptions.ComputeReferences = true;

            GeometryElement geometryElement = myElement.get_Geometry(myOptions);

            UV oPoint = new UV(0, 0);

            List<Face> listSideFace = new List<Face>();
            List<Face> listTopFace = new List<Face>();

            //			TaskDialog.Show("abc", "solids: " + getSolidsOfElement(myElement).Count);
            foreach (Solid solid in getSolidsOfElement(myElement))
            {
                //				TaskDialog.Show("abc", "volume: " + solid.Volume);
                XYZ myNormVec = new XYZ();

                try
                {


                    foreach (Face myFace in solid.Faces)
                    {
                        if (!(myFace is PlanarFace)) { continue; }

                        myNormVec = myFace.ComputeNormal(oPoint).Normalize();

                        if (Math.Round(myNormVec.DotProduct(lineDrect), 6) == 0)
                        {
                            //Neu vector chi phuong cua face khong || voi updirect, thi xet truong hop la mat ben
                            if (Math.Round(Math.Abs(myNormVec.DotProduct(upDirect)), 6) != 1)
                            {
                                double bTemp = Math.Abs(getDisFromPointToFace(centerPointElementOffsetRight, myFace));
                                if (!mylistDistanceRight.Contains(bTemp))
                                {

                                    mylistDistanceRight.Add(bTemp);
                                    // Sort list distance
                                    mylistDistanceRight.Sort();

                                    //Neu bTemp < mylistDistanceRight, insert list face side vào đầu
                                    if (bTemp <= mylistDistanceRight[0])
                                    {
                                        listSideFace.Insert(0, myFace);
                                    }
                                    //Neu bTemp < mylistDistanceRight, insert list face side vào đầu
                                    else
                                    {
                                        if (bTemp > mylistDistanceRight[mylistDistanceRight.Count - 1])
                                        {
                                            listSideFace.Insert(listSideFace.Count - 1, myFace);
                                        }
                                        else
                                        {
                                            listSideFace.Add(myFace);
                                        }
                                    }
                                }
                            }
                            // Xu li cac mat vuong goc voi truc direc
                            else
                            {
                                double hTemp = Math.Abs(getDisFromPointToFace(centerPointElementOffsetUp, myFace));
                                if (!mylistDistanceUp.Contains(hTemp))
                                {

                                    mylistDistanceUp.Add(hTemp);
                                    // Sort list distance
                                    mylistDistanceUp.Sort();

                                    //Neu hTemp < mylistDistanceUp, insert list face side vào đầu
                                    if (hTemp <= mylistDistanceUp[0])
                                    {
                                        listTopFace.Insert(0, myFace);
                                    }
                                    //Neu bTemp < mylistDistanceRight, insert list face side vào đầu
                                    else
                                    {
                                        if (hTemp > mylistDistanceUp[mylistDistanceUp.Count - 1])
                                        {
                                            listTopFace.Insert(mylistDistanceUp.Count - 1, myFace);
                                        }
                                        else
                                        {
                                            listTopFace.Add(myFace);
                                        }
                                    }
                                }

                            }
                        }
                    }

                }
                catch (Exception e)
                {
                    TaskDialog.Show("abc", e.Message.ToString());
                    continue;

                }
            }

            //Tinh toan h

            h_min = mylistDistanceUp[0];
            h_max = mylistDistanceUp[mylistDistanceUp.Count - 1];


            h = h_max - h_min;
            double h_d = (h_max + h_min) / 2;
            XYZ centerByHTemp = centerPointElementOffsetUp + h_d * upDirect;


            //			TaskDialog.Show("abc", "listface: " + mylistDistanceRight.Count);
            //			mylistDistanceRight.Sort();

            b_min = mylistDistanceRight[0];
            b_max = mylistDistanceRight[mylistDistanceRight.Count - 1];
            b = b_max - b_min;
            XYZ centerPointElementOffsetRight_New = centerByHTemp - 100000 * rightDirect;
            double b_d = (b_max + b_min) / 2;

            XYZ centerByH_BTemp = centerPointElementOffsetRight_New + b_d * rightDirect;

            centerPointElement = centerByH_BTemp;

            ReferenceArray mySideRefArray = new ReferenceArray();

            foreach (Face myFace in listSideFace)
            {
                mySideRefArray.Append(myFace.Reference);
            }

            ReferenceArray myTopRefArray = new ReferenceArray();
            foreach (Face myFace in listTopFace)
            {
                myTopRefArray.Append(myFace.Reference);
            }


            //			TaskDialog.Show("abc", "Width beam_max: " + b_max);
            //TaskDialog.Show("abc", "Width beam: " + b);
            //TaskDialog.Show("abc", "Height beam: " + h);

            return new CustomElement1DInfo(h, b, centerPointElement, mySideRefArray, myTopRefArray, maxPointElement.Z);

        }


        /// <summary>
        /// Nhóm hàm các đối tượng hình học :Cột, Móng (family 1 point)... dạng giản đơn
        /// </summary>
        public CustomElement0DInfo getGeometryLocation0D(Element myElement)
        {

            List<double> mylistDistance = new List<double>();

            List<Reference> myRefList = new List<Reference>();
            LocationPoint myElementLp = myElement.Location as LocationPoint;
            LocationCurve myElementLc = myElement.Location as LocationCurve;
            if (myElementLp == null)
            {
                if (myElementLc == null)
                {
                    TaskDialog.Show("Error!", "Doi tuong ban chon 0 phuong");
                    return null;
                }
                else
                {
                    TaskDialog.Show("Error!", "Doi tuong ban chon 2 phuong");
                    return null;
                }

            }

            XYZ lcPoint = myElementLp.Point;

            //Get boundingbox

            BoundingBoxXYZ myElement_BB = myElement.get_BoundingBox(null);

            XYZ minPointElement = myElement_BB.Min;
            XYZ maxPointElement = myElement_BB.Max;
            XYZ centerPointElement = (minPointElement + maxPointElement) / 2;
            double h = maxPointElement.Z - minPointElement.Z;


            Options myOptions = new Options();
            myOptions.ComputeReferences = true;

            GeometryElement geometryElement = myElement.get_Geometry(myOptions);

            UV oPoint = new UV(0, 0);

            List<Face> listSideFace = new List<Face>();
            List<Face> listTopFace = new List<Face>();


            List<Solid> myListSolid = new List<Solid>();

            foreach (GeometryObject myGeometryObject in geometryElement)
            {
                GeometryInstance myGeoInstance = myGeometryObject as GeometryInstance;

                if (myGeoInstance != null)
                {
                    foreach (GeometryObject instObj in myGeoInstance.SymbolGeometry)
                    {
                        Solid solid = instObj as Solid;
                        if (null == solid || solid.Volume < 0.0001)
                        {
                            continue;
                        }

                        myListSolid.Add(solid);

                    }
                }
                else
                {
                    Solid solid = myGeometryObject as Solid;
                    if (null == solid || solid.Volume < 0.0001)
                    {
                        continue;
                    }
                    myListSolid.Add(solid);
                }
            }

            foreach (Solid mySolid in myListSolid)
            {
                XYZ myNormVec_2 = new XYZ();
                foreach (Face myFace in mySolid.Faces)
                {
                    myNormVec_2 = myFace.ComputeNormal(oPoint);
                    if (Math.Round(myNormVec_2.DotProduct(new XYZ(0, 0, 1)), 6) == 0)
                    {
                        listSideFace.Add(myFace);
                        //TaskDialog.Show("abc", "area: " + myFace.Area);	
                    }
                    if (Math.Abs(Math.Round(myNormVec_2.DotProduct(new XYZ(0, 0, 1)), 6)) == 1)
                    {
                        listTopFace.Add(myFace);
                    }
                }
            }



            TaskDialog.Show("abc", "num top: " + listTopFace.Count);

            ReferenceArray mySideRefArray = new ReferenceArray();
            foreach (Face myFace in listSideFace)
            {
                mySideRefArray.Append(myFace.Reference);
            }

            ReferenceArray myTopRefArray = new ReferenceArray();
            foreach (Face myFace in listTopFace)
            {
                myTopRefArray.Append(myFace.Reference);
            }

            return new CustomElement0DInfo(h, centerPointElement, mySideRefArray, myTopRefArray, maxPointElement.Z);

        }


        private List<Solid> getListSolidFromObject(Element myElement)
        {

            Options myOptions = new Options();
            myOptions.ComputeReferences = true;

            GeometryElement geometryElement = myElement.get_Geometry(myOptions);

            List<Solid> myListSolid = new List<Solid>();

            foreach (GeometryObject myGeometryObject in geometryElement)
            {
                GeometryInstance myGeoInstance = myGeometryObject as GeometryInstance;

                if (myGeoInstance != null)
                {
                    foreach (GeometryObject instObj in myGeoInstance.SymbolGeometry)
                    {
                        Solid solid = instObj as Solid;
                        if (null == solid || solid.Volume < 0.0001)
                        {
                            continue;
                        }

                        myListSolid.Add(solid);

                    }
                }
                else
                {
                    Solid solid = myGeometryObject as Solid;
                    if (null == solid || solid.Volume < 0.0001)
                    {
                        continue;
                    }
                    myListSolid.Add(solid);
                }
            }
            return myListSolid;


        }




        /// <summary>
        /// Các hàm test


        public void DimBeam(UIDocument uiDoc)
        {
            //			UIDocument uiDoc = this.ActiveUIDocument;
            Document doc = uiDoc.Document;

            View myView = doc.ActiveView;

            //Calculate Geometry Parameter
            //Pick Beam

            Reference myRefBeam = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a Beam....");

            //TODO: Các bước tính toán hình học của dầm

            Element myBeam = doc.GetElement(myRefBeam);


            CustomElement1DInfo myCE1DI = getGeometryLocation1D(myBeam);

            XYZ centerPointBeam = myCE1DI.CenterPoint;
            XYZ centerPointProjectBeam = getPointProjectFromPointOntoPlane(centerPointBeam,
                                                                           Plane.CreateByNormalAndOrigin(myView.ViewDirection, myView.Origin));
            double beamHeight = myCE1DI.H;
            double beamWidth = myCE1DI.B;
            ReferenceArray myRefArrayTop = new ReferenceArray();
            myRefArrayTop.Append(myCE1DI.TopRefArray.get_Item(0));
            myRefArrayTop.Append(myCE1DI.TopRefArray.get_Item(myCE1DI.TopRefArray.Size - 1));

            ReferenceArray myRefArraySide = new ReferenceArray();
            myRefArraySide.Append(myCE1DI.SideRefArray.get_Item(0));
            myRefArraySide.Append(myCE1DI.SideRefArray.get_Item(myCE1DI.SideRefArray.Size - 1));

            TaskDialog.Show("abc", "Size: " + myCE1DI.SideRefArray.Size);


            using (Transaction trans = new Transaction(doc, "Create linear Dimension"))
            {
                trans.Start();


                Line lineDimTop = Line.CreateBound(centerPointProjectBeam - myView.RightDirection * (2.5 + beamWidth / 2),
                                                centerPointProjectBeam - myView.UpDirection - myView.RightDirection * (2.5 + beamWidth / 2));
                Dimension myDimTop = doc.Create.NewDimension(doc.ActiveView, lineDimTop, myCE1DI.TopRefArray);

                Line lineDimSide = Line.CreateBound(centerPointProjectBeam - myView.UpDirection * (2.5 + beamHeight / 2),
                                                centerPointProjectBeam - myView.UpDirection * (2.5 + beamHeight / 2) + myView.RightDirection);
                Dimension myDimSide = doc.Create.NewDimension(doc.ActiveView, lineDimSide, myCE1DI.SideRefArray);

                trans.Commit();
            }

            using (Transaction trans = new Transaction(doc, "Create linear Dimension"))
            {
                trans.Start();

                XYZ oPoint = centerPointProjectBeam - myCE1DI.H / 2 * myView.UpDirection;
                XYZ oPoint_1 = oPoint + 0.2 * myView.RightDirection;
                XYZ oPoint_2 = oPoint + 0.2 * myView.RightDirection;
                XYZ oPoint_4 = oPoint + 0.3 * myView.RightDirection;

                Reference myRef_2 = myRefArrayTop.get_Item(0);

                doc.Create.NewSpotElevation(myView, myRefArrayTop.get_Item(0), oPoint, oPoint, oPoint, oPoint, false);

                trans.Commit();
            }


        }


        public XYZ getIntersectionPointByLineAndPlane(Plane myPlane, Line myLine)
        {



            return new XYZ();
        }



        public void DimFloor(UIDocument uiDoc)
        {
            //			UIDocument uiDoc = this.ActiveUIDocument;
            Document doc = uiDoc.Document;

            View myView = doc.ActiveView;

            //Calculate Geometry Parameter
            //Pick Beam

            Reference myRefFloor = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a Beam....");

            //TODO: Các bước tính toán hình học của dầm

            Element myFloor = doc.GetElement(myRefFloor);

            CustomElement2DInfo myCE2DI = getGeometryLocation2D(myFloor);

            XYZ centerPointBeam = myCE2DI.CenterPoint;
            XYZ centerPointProjectBeam = getPointProjectFromPointOntoPlane(centerPointBeam,
                                                                           Plane.CreateByNormalAndOrigin(myView.ViewDirection, myView.Origin));
            double beamHeight = myCE2DI.H;
            double beamWidth = 0;
            TaskDialog.Show("abc", "Ref: " + myCE2DI.TopRefArray.Size);


            ReferenceArray myRefArrayTop = new ReferenceArray();
            //			foreach (Reference myRef in myCE2DI.TopRefArray) 
            //			{
            //				myRefArrayTop.Append(myRef);
            //			}


            myRefArrayTop.Append(myCE2DI.TopRefArray.get_Item(0));
            myRefArrayTop.Append(myCE2DI.TopRefArray.get_Item(myCE2DI.TopRefArray.Size - 1));

            ReferenceArray myRefArraySide = new ReferenceArray();
            myRefArraySide.Append(myCE2DI.SideRefArray.get_Item(0));
            myRefArraySide.Append(myCE2DI.SideRefArray.get_Item(myCE2DI.SideRefArray.Size - 1));

            TaskDialog.Show("abc", "Size: " + myCE2DI.SideRefArray.Size);


            using (Transaction trans = new Transaction(doc, "Create linear Dimension"))
            {
                trans.Start();


                Line lineDimTop = Line.CreateBound(centerPointProjectBeam - myView.RightDirection * (2.5 + beamWidth / 2),
                                                centerPointProjectBeam - myView.UpDirection - myView.RightDirection * (2.5 + beamWidth / 2));
                Dimension myDimTop = doc.Create.NewDimension(doc.ActiveView, lineDimTop, myRefArrayTop);
                //				
                //				Line lineDimSide = Line.CreateBound(centerPointProjectBeam - myView.UpDirection*(2.5+beamHeight/2),
                //				                                centerPointProjectBeam- myView.UpDirection*(2.5+beamHeight/2) + myView.RightDirection);
                //				Dimension myDimSide = doc.Create.NewDimension(doc.ActiveView, lineDimSide, myRefArraySide);

                trans.Commit();
            }


        }


        public void DimBeamAndFloor(UIDocument uiDoc)
        {
            //			UIDocument uiDoc = this.ActiveUIDocument;
            Document doc = uiDoc.Document;

            View myView = doc.ActiveView;

            //Calculate Geometry Parameter
            //Pick Beam

            Reference myRefBeam = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a Beam....");


            //TODO: Các bước tính toán hình học của dầm

            Element myBeam = doc.GetElement(myRefBeam);



            CustomElement1DInfo myCE1DI = getGeometryLocation1D(myBeam);

            XYZ centerPointBeam = myCE1DI.CenterPoint;
            XYZ centerPointProjectBeam = getPointProjectFromPointOntoPlane(centerPointBeam,
                                                                           Plane.CreateByNormalAndOrigin(myView.ViewDirection, myView.Origin));
            double beamHeight = myCE1DI.H;
            double beamWidth = myCE1DI.B;


            Reference myRefFloor = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a Beam....");

            //TODO: Các bước tính toán hình học của dầm

            Element myFloor = doc.GetElement(myRefFloor);

            CustomElement2DInfo myCE2DI = getGeometryLocation2D(myFloor);

            ReferenceArray myRefArrayTop = new ReferenceArray();


            myRefArrayTop.Append(myCE1DI.TopRefArray.get_Item(0));
            myRefArrayTop.Append(myCE1DI.TopRefArray.get_Item(myCE1DI.TopRefArray.Size - 1));

            //			foreach (Reference myRefFloorTop in myCE2DI.TopRefArray) 
            //			{
            //				myRefArrayTop.Append(myRefFloorTop);
            //			}


            myRefArrayTop.Append(myCE2DI.TopRefArray.get_Item(0));
            myRefArrayTop.Append(myCE2DI.TopRefArray.get_Item(myCE2DI.TopRefArray.Size - 1));

            foreach (Reference topRef in myRefArrayTop)
            {
                if (topRef.GlobalPoint != null)
                {
                    printXYZ("Global Point", topRef.GlobalPoint);
                }

            }


            ReferenceArray myRefArraySide = new ReferenceArray();
            myRefArraySide.Append(myCE1DI.SideRefArray.get_Item(0));
            myRefArraySide.Append(myCE1DI.SideRefArray.get_Item(myCE1DI.SideRefArray.Size - 1));

            TaskDialog.Show("abc", "Size: " + myCE1DI.SideRefArray.Size);


            using (Transaction trans = new Transaction(doc, "Create linear Dimension"))
            {
                trans.Start();


                Line lineDimTop = Line.CreateBound(centerPointProjectBeam - myView.RightDirection * (2.5 + beamWidth / 2),
                                                centerPointProjectBeam - myView.UpDirection - myView.RightDirection * (2.5 + beamWidth / 2));
                Dimension myDimTop = doc.Create.NewDimension(doc.ActiveView, lineDimTop, myRefArrayTop);

                Line lineDimSide = Line.CreateBound(centerPointProjectBeam - myView.UpDirection * (2.5 + beamHeight / 2),
                                                centerPointProjectBeam - myView.UpDirection * (2.5 + beamHeight / 2) + myView.RightDirection);
                Dimension myDimSide = doc.Create.NewDimension(doc.ActiveView, lineDimSide, myRefArraySide);

                trans.Commit();
            }


        }


        public void checkBeJoin(UIDocument uiDoc)
        {
            //			UIDocument uiDoc = this.ActiveUIDocument;
            Document doc = uiDoc.Document;

            View myView = doc.ActiveView;

            //Calculate Geometry Parameter
            //Pick Beam

            Reference myRefBeam = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a Beam....");


            //TODO: Các bước tính toán hình học của dầm

            Element myBeam = doc.GetElement(myRefBeam);



            CustomElement1DInfo myCE1DI = getGeometryLocation1D(myBeam);

            XYZ centerPointBeam = myCE1DI.CenterPoint;
            XYZ centerPointProjectBeam = getPointProjectFromPointOntoPlane(centerPointBeam,
                                                                           Plane.CreateByNormalAndOrigin(myView.ViewDirection, myView.Origin));
            double beamHeight = myCE1DI.H;
            double beamWidth = myCE1DI.B;


            Reference myRefFloor = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a Floor....");

            //TODO: Các bước tính toán hình học của dầm

            Element myFloor = doc.GetElement(myRefFloor);

            //Check Join
            TaskDialog.Show("abc", "Beam and Floor Join: " + JoinGeometryUtils.AreElementsJoined(doc, myBeam, myFloor));

            // neu joined va beam bi join
            if (JoinGeometryUtils.AreElementsJoined(doc, myBeam, myFloor) &&
               JoinGeometryUtils.IsCuttingElementInJoin(doc, myFloor, myBeam))
            {
                TaskDialog.Show("abc", "Beam was cutting");
                using (Transaction trans = new Transaction(doc, "Create linear Dimension"))
                {
                    trans.Start();

                    JoinGeometryUtils.SwitchJoinOrder(doc, myBeam, myFloor);

                    trans.Commit();
                }
            }


            using (Transaction trans = new Transaction(doc, "Create linear Dimension"))
            {
                trans.Start();


                //				Line lineDimTop = Line.CreateBound(centerPointProjectBeam - myView.RightDirection*(2.5+beamWidth/2),
                //				                                centerPointProjectBeam-myView.UpDirection - myView.RightDirection*(2.5+beamWidth/2));
                //				Dimension myDimTop = doc.Create.NewDimension(doc.ActiveView, lineDimTop, myRefArrayTop);
                //				
                //				Line lineDimSide = Line.CreateBound(centerPointProjectBeam - myView.UpDirection*(2.5+beamHeight/2),
                //				                                centerPointProjectBeam- myView.UpDirection*(2.5+beamHeight/2) + myView.RightDirection);
                //				Dimension myDimSide = doc.Create.NewDimension(doc.ActiveView, lineDimSide, myRefArraySide);

                trans.Commit();
            }


        }



        public void DimBeam_Para(UIDocument uiDoc, Element myBeam)
        {
            //			UIDocument uiDoc = this.ActiveUIDocument;
            Document doc = uiDoc.Document;

            View myView = doc.ActiveView;

            //			//Calculate Geometry Parameter
            //			//Pick Beam
            //			
            //			Reference myRefBeam = uiDoc.Selection.PickObject(ObjectType.Element,"Pick a Beam....");
            //			
            //			//TODO: Các bước tính toán hình học của dầm
            //			
            //			Element myBeam = doc.GetElement(myRefBeam);


            CustomElement1DInfo myCE1DI = getGeometryLocation1D(myBeam);

            XYZ centerPointBeam = myCE1DI.CenterPoint;
            XYZ centerPointProjectBeam = getPointProjectFromPointOntoPlane(centerPointBeam,
                                                                           Plane.CreateByNormalAndOrigin(myView.ViewDirection, myView.Origin));
            double beamHeight = myCE1DI.H;
            double beamWidth = myCE1DI.B;
            ReferenceArray myRefArrayTop = new ReferenceArray();
            myRefArrayTop.Append(myCE1DI.TopRefArray.get_Item(0));
            myRefArrayTop.Append(myCE1DI.TopRefArray.get_Item(myCE1DI.TopRefArray.Size - 1));

            ReferenceArray myRefArraySide = new ReferenceArray();
            myRefArraySide.Append(myCE1DI.SideRefArray.get_Item(0));
            myRefArraySide.Append(myCE1DI.SideRefArray.get_Item(myCE1DI.SideRefArray.Size - 1));

            TaskDialog.Show("abc", "Size: " + myCE1DI.SideRefArray.Size);


            using (Transaction trans = new Transaction(doc, "Create linear Dimension"))
            {
                trans.Start();


                Line lineDimTop = Line.CreateBound(centerPointProjectBeam - myView.RightDirection * (2.5 + beamWidth / 2),
                                                centerPointProjectBeam - myView.UpDirection - myView.RightDirection * (2.5 + beamWidth / 2));
                Dimension myDimTop = doc.Create.NewDimension(doc.ActiveView, lineDimTop, myCE1DI.TopRefArray);

                Line lineDimSide = Line.CreateBound(centerPointProjectBeam - myView.UpDirection * (2.5 + beamHeight / 2),
                                                centerPointProjectBeam - myView.UpDirection * (2.5 + beamHeight / 2) + myView.RightDirection);
                Dimension myDimSide = doc.Create.NewDimension(doc.ActiveView, lineDimSide, myCE1DI.SideRefArray);

                trans.Commit();
            }


        }


        public void DimBeamAndFloor_Para(UIDocument uiDoc, Element myBeam, Element myFloor)
        {
            //			UIDocument uiDoc = this.ActiveUIDocument;
            Document doc = uiDoc.Document;

            View myView = doc.ActiveView;


            CustomElement1DInfo myCE1DI = getGeometryLocation1D(myBeam);

            XYZ centerPointBeam = myCE1DI.CenterPoint;
            XYZ centerPointProjectBeam = getPointProjectFromPointOntoPlane(centerPointBeam,
                                                                           Plane.CreateByNormalAndOrigin(myView.ViewDirection, myView.Origin));
            double beamHeight = myCE1DI.H;
            double beamWidth = myCE1DI.B;


            CustomElement2DInfo myCE2DI = getGeometryLocation2D(myFloor);

            ReferenceArray myRefArrayTop = new ReferenceArray();



            if (Math.Round(myCE2DI.Elevation - myCE1DI.Elevation, 2) > 0)
            {
                TaskDialog.Show("abc", "Floor>Beam");

                myRefArrayTop.Append(myCE2DI.TopRefArray.get_Item(0));
                myRefArrayTop.Append(myCE2DI.TopRefArray.get_Item(myCE2DI.TopRefArray.Size - 1));
                myRefArrayTop.Append(myCE1DI.TopRefArray.get_Item(0));
                //				myRefArrayTop.Append(myCE1DI.TopRefArray.get_Item(myCE1DI.TopRefArray.Size - 1));
            }

            if (Math.Round(myCE2DI.Elevation - myCE1DI.Elevation, 2) == 0)
            {
                TaskDialog.Show("abc", "Floor==Beam");

                //				myRefArrayTop.Append(myCE1DI.TopRefArray.get_Item(myCE1DI.TopRefArray.Size - 1));

                myRefArrayTop.Append(myCE2DI.TopRefArray.get_Item(0));
                myRefArrayTop.Append(myCE2DI.TopRefArray.get_Item(myCE2DI.TopRefArray.Size - 1));
                myRefArrayTop.Append(myCE1DI.TopRefArray.get_Item(0));
            }

            if (Math.Round(myCE2DI.Elevation - myCE1DI.Elevation, 2) < 0)
            {
                TaskDialog.Show("abc", "Floor<Beam");
                myRefArrayTop.Append(myCE1DI.TopRefArray.get_Item(0));
                myRefArrayTop.Append(myCE1DI.TopRefArray.get_Item(myCE1DI.TopRefArray.Size - 1));

                myRefArrayTop.Append(myCE2DI.TopRefArray.get_Item(0));
                myRefArrayTop.Append(myCE2DI.TopRefArray.get_Item(myCE2DI.TopRefArray.Size - 1));
            }

            ReferenceArray myRefArraySide = new ReferenceArray();
            myRefArraySide.Append(myCE1DI.SideRefArray.get_Item(0));
            myRefArraySide.Append(myCE1DI.SideRefArray.get_Item(myCE1DI.SideRefArray.Size - 1));

            TaskDialog.Show("abc", "Size: " + myCE1DI.SideRefArray.Size);

            using (Transaction trans = new Transaction(doc, "Create linear Dimension"))
            {
                trans.Start();


                Line lineDimTop = Line.CreateBound(centerPointProjectBeam - myView.RightDirection * (2.5 + beamWidth / 2),
                                                centerPointProjectBeam - myView.UpDirection - myView.RightDirection * (2.5 + beamWidth / 2));
                Dimension myDimTop = doc.Create.NewDimension(doc.ActiveView, lineDimTop, myRefArrayTop);

                Line lineDimSide = Line.CreateBound(centerPointProjectBeam - myView.UpDirection * (2.5 + beamHeight / 2),
                                                centerPointProjectBeam - myView.UpDirection * (2.5 + beamHeight / 2) + myView.RightDirection);
                Dimension myDimSide = doc.Create.NewDimension(doc.ActiveView, lineDimSide, myRefArraySide);

                trans.Commit();
            }


        }


        public void AutodimBeamAndFloor(UIDocument uiDoc)
        {
            //			UIDocument uiDoc = this.ActiveUIDocument;
            Document doc = uiDoc.Document;

            View myView = doc.ActiveView;

            FilteredElementCollector filterBeam = new FilteredElementCollector(doc, myView.Id);

            //Get Beam
            Element myBeam = filterBeam.OfCategory(BuiltInCategory.OST_StructuralFraming)
                .WhereElementIsNotElementType().FirstOrDefault();
            TaskDialog.Show("abc", myBeam.Id.ToString());
            //GetSolids Floor

            FilteredElementCollector filterFloor = new FilteredElementCollector(doc, myView.Id);

            Element myFloor = filterFloor.OfCategory(BuiltInCategory.OST_Floors)
                .WhereElementIsNotElementType().FirstOrDefault();


            if (myBeam != null)
            {
                if (myFloor != null)
                {
                    DimBeamAndFloor_Para(uiDoc, myBeam, myFloor);

                }
                else
                {
                    DimBeam_Para(uiDoc, myBeam);
                }
            }





        }


        public List<Solid> getSolidsOfElement(Element myElement)
        {
            List<Solid> myListSolids = new List<Solid>();

            Options myOptions = new Options();
            myOptions.ComputeReferences = true;


            GeometryElement geometryElement = myElement.get_Geometry(myOptions);

            foreach (GeometryObject myGeometryObject in geometryElement)
            {
                if (myGeometryObject is Solid)
                {
                    //TaskDialog.Show("abc", "is solid");
                    Solid solid = myGeometryObject as Solid;

                    if (solid.Volume > 0)
                    {
                        myListSolids.Add(solid);
                    }

                }

                else
                {
                    GeometryInstance myGeoInstance = myGeometryObject as GeometryInstance;
                    if (myGeoInstance != null)
                    {
                        //TaskDialog.Show("abc", "is instance");
                        foreach (GeometryObject instObj in myGeoInstance.GetInstanceGeometry())
                        {
                            Solid solid = instObj as Solid;
                            if (solid.Volume != 0)
                            {
                                myListSolids.Add(solid);
                            }
                        }

                    }
                }

            }
            return myListSolids;
        }



        public List<Solid> getSolidsOfElement_2(Element myElement)
        {
            List<Solid> myListSolids = new List<Solid>();

            Options myOptions = new Options();
            myOptions.ComputeReferences = true;


            GeometryElement geometryElement = myElement.get_Geometry(myOptions);


            IEnumerable<Solid> myIESolid = GetSolids(geometryElement);

            foreach (Solid mySolid in myIESolid)
            {
                if (mySolid.Volume > 0)
                {
                    myListSolids.Add(mySolid);
                }
            }

            return myListSolids;
        }



        private static IEnumerable<Solid> GetSolids(IEnumerable<GeometryObject> geometryElement)
        {
            foreach (var geometry in geometryElement)
            {
                var solid = geometry as Solid;
                if (solid != null)
                    yield return solid;

                var instance = geometry as GeometryInstance;
                if (instance != null)
                    foreach (var instanceSolid in GetSolids(instance.GetSymbolGeometry()))
                        yield return instanceSolid;

                var element = geometry as GeometryElement;
                if (element != null)
                    foreach (var elementSolid in GetSolids(element))
                        yield return elementSolid;
            }
        }



        public void removeSegmentDim(UIDocument uiDoc)
        {
            //			UIDocument uiDoc = this.ActiveUIDocument;
            Document doc = uiDoc.Document;

            View myView = doc.ActiveView;

            //Calculate Geometry Parameter
            //Pick Beam

            Reference myRefDim = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a Dim....");


            //TODO: Các bước tính toán hình học của dầm

            Dimension myDim = doc.GetElement(myRefDim) as Dimension;
            foreach (Reference myRef in myDim.References)
            {

                //				TaskDialog.Show("point", myRef.GlobalPoint.ToString());
            }


        }




        public void setCurrentViewAsWorkPlan(UIDocument uiDoc)
        {
            //			UIDocument uiDoc = this.ActiveUIDocument;
            Document doc = uiDoc.Document;

            using (Transaction trans = new Transaction(doc, "WorkPlane"))
            {
                trans.Start();

                //				Plane plane = new Plane( uiDoc.Document.ActiveView.ViewDirection, uiDoc.Document.ActiveView.Origin);

                Plane plane = Plane.CreateByNormalAndOrigin(uiDoc.Document.ActiveView.ViewDirection, uiDoc.Document.ActiveView.Origin);


                SketchPlane sp = SketchPlane.Create(doc, plane);
                uiDoc.Document.ActiveView.SketchPlane = sp;
                trans.Commit();
            }
        }


    }


    public class SortByDisPointToFace : IComparer<Face>
    {
        XYZ myPoint = null;

        public SortByDisPointToFace(XYZ point)
        {
            this.myPoint = point;
        }


        public int Compare(Face lhs, Face rhs)
        {

            Utils myUtil = new Utils();


            double myDisLhs = Math.Abs(myUtil.getDisFromPointToFace(myPoint, lhs));

            double myDisRhs = Math.Abs(myUtil.getDisFromPointToFace(myPoint, rhs));


            return myDisLhs.CompareTo(myDisRhs);

        }
    }


    //Anotation Classes
    public class Utils_Dimension
    {

        public static void removeSegmentDim_Loop_Para(UIDocument uiDoc, Dimension myDim)
        {
            Document doc = uiDoc.Document;

            View myView = uiDoc.ActiveView;

            Line myOldDimLine = myDim.Curve as Line;

            Dimension myDimNext = myDim;


            int numZeroSeg = 0;
            while (true)
            {
                numZeroSeg = 0;
                foreach (DimensionSegment mySeg in myDimNext.Segments)
                {
                    if (mySeg.Value.Value < 0.001) { numZeroSeg++; }
                }
                if (numZeroSeg != 0)
                {
                    ReferenceArray myNewrefAr = new ReferenceArray();
                    for (int i = 0; i < myDim.Segments.Size; i++)
                    {
                        DimensionSegment myDimSeg = myDim.Segments.get_Item(i);
                        if (i == myDim.Segments.Size - 1)
                        {
                            if (myDimSeg.Value.Value < 0.0001)
                            {
                                myNewrefAr.Append(myDim.References.get_Item(i));
                            }
                            else
                            {
                                myNewrefAr.Append(myDim.References.get_Item(i));
                                myNewrefAr.Append(myDim.References.get_Item(i + 1));
                            }

                        }
                        else
                        {
                            if (myDimSeg.Value.Value > 0.0001)
                            {
                                myNewrefAr.Append(myDim.References.get_Item(i));
                            }
                            else
                            {

                            }
                        }
                    }
                    using (Transaction trans = new Transaction(doc, "Renew Dimension"))
                    {
                        trans.Start();
                        myDimNext = doc.Create.NewDimension(myView, myOldDimLine, myNewrefAr);
                        trans.Commit();
                    }

                }
                else { break; }
            }
            using (Transaction trans = new Transaction(doc, "Renew Dimension"))
            {
                trans.Start();

                numZeroSeg = 0;
                foreach (DimensionSegment mySeg in myDim.Segments)
                {
                    if (mySeg.Value.Value < 0.001) { numZeroSeg++; }
                }

                if (numZeroSeg != 0)
                {
                    doc.Delete(myDim.Id);
                }
                trans.Commit();
            }
        }


        public static void removeSegmentDim_For_Para(UIDocument uiDoc, Dimension myDim)
        {
            Document doc = uiDoc.Document;

            View myView = uiDoc.ActiveView;

            Line myOldDimLine = myDim.Curve as Line;

            Dimension myDimNext = myDim;

            int numZeroSeg = 0;
            foreach (DimensionSegment mySeg in myDimNext.Segments)
            {
                if (mySeg.Value.Value < 0.001) { numZeroSeg++; }
            }

            for (int k = 0; k < numZeroSeg + 1; k++)
            {
                numZeroSeg = 0;
                foreach (DimensionSegment mySeg in myDimNext.Segments)
                {
                    if (mySeg.Value.Value < 0.001) { numZeroSeg++; }
                }
                if (numZeroSeg != 0)
                {
                    ReferenceArray myNewrefAr = new ReferenceArray();
                    for (int i = 0; i < myDim.Segments.Size; i++)
                    {
                        DimensionSegment myDimSeg = myDim.Segments.get_Item(i);
                        if (i == myDim.Segments.Size - 1)
                        {
                            if (myDimSeg.Value.Value < 0.0001)
                            {
                                myNewrefAr.Append(myDim.References.get_Item(i));
                            }
                            else
                            {
                                myNewrefAr.Append(myDim.References.get_Item(i));
                                myNewrefAr.Append(myDim.References.get_Item(i + 1));
                            }

                        }
                        else
                        {
                            if (myDimSeg.Value.Value > 0.0001)
                            {
                                myNewrefAr.Append(myDim.References.get_Item(i));
                            }
                            else
                            {

                            }
                        }
                    }
                    using (Transaction trans = new Transaction(doc, "Renew Dimension"))
                    {
                        trans.Start();
                        myDimNext = doc.Create.NewDimension(myView, myOldDimLine, myNewrefAr);
                        trans.Commit();
                    }

                }
                else { break; }
            }
            using (Transaction trans = new Transaction(doc, "Renew Dimension"))
            {
                trans.Start();

                numZeroSeg = 0;
                foreach (DimensionSegment mySeg in myDim.Segments)
                {
                    if (mySeg.Value.Value < 0.001) { numZeroSeg++; }
                }

                if (numZeroSeg != 0)
                {
                    doc.Delete(myDim.Id);
                }
                trans.Commit();
            }
        }


        public static void overrideSegmentZeroDim_Para(UIDocument uiDoc, Dimension myDim, string overrideString)
        {
            //			UIDocument uiDoc = this.ActiveUIDocument;
            Document doc = uiDoc.Document;

            View myView = doc.ActiveView;

            foreach (DimensionSegment mySegDim in myDim.Segments)
            {
                using (Transaction myTrans = new Transaction(doc, "Override dim"))
                {
                    myTrans.Start();
                    if (mySegDim.Value < 0.0001)
                    {
                        mySegDim.ValueOverride = @overrideString;
                        //						TaskDialog.Show("abc", "override");
                    }
                    myTrans.Commit();
                }
            }
        }

        public static void overrideSegmentAtIndexDim_Para(UIDocument uiDoc, Dimension myDim, int index, string overrideString)
        {
            //			UIDocument uiDoc = this.ActiveUIDocument;
            Document doc = uiDoc.Document;

            View myView = doc.ActiveView;

            using (Transaction myTrans = new Transaction(doc, "Override dim"))
            {
                myTrans.Start();
                if (myDim.Segments.Size > 1)
                {
                    if (index > myDim.NumberOfSegments - 1)
                    {
                        TaskDialog.Show("Error!!!", "Index: " + index + " greater max index of segments");
                        return;
                    }

                    else
                    {
                        DimensionSegment myDimSeg = myDim.Segments.get_Item(index);
                        myDimSeg.ValueOverride = @overrideString;
                    }
                }
                else
                {
                    myDim.ValueOverride = @overrideString;
                }
                myTrans.Commit();
            }

            //Change graphic of dim

            OverrideGraphicSettings ogs = new OverrideGraphicSettings();
            ogs.SetProjectionLineColor(new Color(200, 0, 0));
            using (Transaction t = new Transaction(doc, "Set Element Override"))
            {
                t.Start();
                myView.SetElementOverrides(myDim.Id, ogs);
                t.Commit();
            }
        }

    }

    //Math and Type classes
    public class Utils_Convert
    {

        public static double unitConvertLeng_FeetToX(UIDocument uiDoc)
        {
            Document doc = uiDoc.Document;

            const double feetToMM = 304.8;

            Units units = doc.GetUnits();

            FormatOptions unitType = units.GetFormatOptions(UnitType.UT_Length); //you specify which unit you are interested in setting/reading

            string displayUnit = unitType.DisplayUnits.ToString();
            switch (displayUnit)
            {
                //mm
                case "DUT_MILLIMETERS":
                    return feetToMM;

                //cm
                case "DUT_CENTIMETERS":
                    return feetToMM / 10;

                //dm
                case "DUT_DECIMETERS ":
                    return feetToMM / 100;
                //m
                case "DUT_METERS":
                    return feetToMM / 1000;

                //inch
                case "DUT_DECIMAL_INCHES ":
                    return feetToMM / 10;

                //feet
                default:
                    return 1;
            }


        }


        public static bool canConvertStringToDouble(string myString)
        {
            try
            {
                Convert.ToDouble(myString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

    //Graphic Utils

    public class Utils_Graphic
    {

        public static void highlightElements(Document doc, List<ElementId> myListId)
        {
            OverrideGraphicSettings myOGS = new OverrideGraphicSettings();

            myOGS.SetProjectionLineColor(new Color(255, 0, 0));

            using (Transaction t = new Transaction(doc, "Set Element Override"))
            {
                t.Start();

                foreach (ElementId myElemId in myListId)
                {
                    Element myElem = doc.GetElement(myElemId);
                    doc.ActiveView.SetElementOverrides(myElemId, myOGS);
                }

                t.Commit();
            }



        }


        public static void resetHighlightElements(Document doc, List<ElementId> myListId)
        {
            OverrideGraphicSettings myOGS = new OverrideGraphicSettings();

            using (Transaction t = new Transaction(doc, "Set Element Override"))
            {
                t.Start();

                foreach (ElementId myElemId in myListId)
                {
                    Element myElem = doc.GetElement(myElemId);
                    doc.ActiveView.SetElementOverrides(myElemId, myOGS);
                }

                t.Commit();
            }

        }

    }


    //geometry Math Classes
    public class Utils_Geometry_OXYZ
    {
        /// <summary>
        /// Các hàm liên quan tới điểm và điểm
        /// </summary>
        /// <param name="p"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>


        //1. In tọa độ điểm
        public static void Print_XYZ(string outName, XYZ myXYZ)
        {
            if (myXYZ != null)
            {
                TaskDialog.Show("Info", outName + "\nX: " + myXYZ.X + "\nY: " + myXYZ.Y + "\nZ: " + myXYZ.Z);
            }
            else
            {
                TaskDialog.Show("Error!!!", "XYZ is null");
            }
        }


        //2. Kiem tra 3 diem thang hang

        public static bool is3PCollinear(XYZ p1, XYZ p2, XYZ p3)
        {
            //Kiem tra cac diem trung nhau va dua ve
            XYZ v12 = (p2 - p1);
            XYZ v13 = (p3 - p1);

            if (v12.IsAlmostEqualTo(v13) || v12.IsZeroLength() || v13.IsZeroLength())
            {
                return true;
            }
            else
            {
                if (Math.Round(Math.Abs(getCosOf2Vector(v12, v13)), 9) == 1.0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //Hàm kiểm tra 3 điểm thẳng hàng test


        /// <summary>
        /// Các hàm liên quan tới điểm và đường thẳng (hoặc điểm với vector)
        /// </summary>
        /// <param name="p"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>


        //Xac dinh 1 điểm nằm trên 1 đường thẳng

        public static bool checkPointOnLine(XYZ p, Line l)
        {
            if (is3PCollinear(p, l.GetEndPoint(0), l.GetEndPoint(1)))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        //Kiem tra 1 điểm có nằm trên 1 "đoạn thằng" (nằm giữa 2 đầu mút)
        public static bool checkPointOnSegmentLine(XYZ p, Line sl)
        {
            XYZ v1 = sl.GetEndPoint(0) - p;
            XYZ v2 = sl.GetEndPoint(1) - p;

            if (Math.Round(v1.GetLength() + v2.GetLength(), 9) == Math.Round(sl.Length, 9))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        //Xác định hình chiếu của 1 điểm "p" trên đường thằng tạo bởi 2 điểm "a", "b"
        //Overide 1: Nhập tọa độ 3 điểm
        public static XYZ getProjectPointOnLine(XYZ p, XYZ a, XYZ b)
        {
            //Checking Input
            if (a.IsAlmostEqualTo(b))
            {
                TaskDialog.Show("Error!!!", "A Point IsAlmoseEqual B, cannot create Line by A, B");
                return null;
            }
            // computer project point of p on ab
            else
            {
                XYZ ap = p - a;
                XYZ ab = b - a;
                return a + ap.DotProduct(ab) / ab.DotProduct(ab) * ab;
            }
        }



        //Xác định một điểm nằm bên trái hay bên phải 1 đường thằng p1-p2

        public static PositionOfPoint getPointAndLine(XYZ p1, XYZ p2, XYZ myPoint)
        {

            double cal = Math.Round(((p2.X - p1.X) * (myPoint.Y - p1.Y) - (p2.Y - p1.Y) * (myPoint.X - p1.X)), 9);
            //			TaskDialog.Show("abc", "Cal: " + cal);
            if (cal > 0)
            {
                return PositionOfPoint.left;
            }

            if (cal < 0)
            {
                return PositionOfPoint.right;
            }

            return PositionOfPoint.online;

        }


        public static PositionOfPoint getPointAndLine(Point2D p1, Point2D p2, Point2D myPoint)
        {

            double cal = Math.Round(((p2.X - p1.X) * (myPoint.Y - p1.Y) - (p2.Y - p1.Y) * (myPoint.X - p1.X)), 9);
            //			TaskDialog.Show("abc", "Cal: " + cal);
            if (cal > 0)
            {
                return PositionOfPoint.left;
            }

            if (cal < 0)
            {
                return PositionOfPoint.right;
            }

            return PositionOfPoint.online;

        }


        //Xác định hình chiếu của 1 điểm "p"(XYZ) trên đường thằng (Line type)
        //Overide 2: Nhập tọa độ 1 điểm và 1 đường thẳng
        public static XYZ getProjectPointOnLine(XYZ p, Line l)
        {
            XYZ a = l.GetEndPoint(0);
            XYZ b = l.GetEndPoint(1);

            //Checking Input
            if (a.IsAlmostEqualTo(b))
            {
                TaskDialog.Show("Error!!!", "A Point IsAlmoseEqual B, cannot create Line by A, B");
                return null;
            }
            // computer project point of p on ab
            else
            {
                XYZ ap = p - a;
                XYZ ab = b - a;
                return a + ap.DotProduct(ab) / ab.DotProduct(ab) * ab;
            }
        }




        //Xác định hình chiếu của 1 điểm "p"(XYZ) trên đoạn thằng (Line type)
        //Nếu nằm ngoài đoạn thẳng trả về null
        //Overide 2: Nhập tọa độ 1 điểm và 1 đường thẳng
        public static XYZ getProjectPointOnSegLine(XYZ p, Line l)
        {
            //			XYZ projectionPoint = null;

            XYZ a = l.GetEndPoint(0);
            XYZ b = l.GetEndPoint(1);

            //Checking Input
            if (a.IsAlmostEqualTo(b))
            {
                TaskDialog.Show("Error!!!", "A Point IsAlmoseEqual B, cannot create Line by A, B");
                return null;
            }
            // computer project point of p on ab
            else
            {
                XYZ ap = p - a;
                XYZ ab = b - a;
                XYZ projectionPoint = a + ap.DotProduct(ab) / ab.DotProduct(ab) * ab;
                if (checkPointOnSegmentLine(projectionPoint, l))
                {
                    return projectionPoint;
                }
                return null;
            }
        }



        /// <summary>
        /// Các hàm liên quan tới điểm và mặt phẳng (Hoặc planar Face)
        /// </summary>
        /// <param name="p"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>

        //reflink: https://stackoverflow.com/questions/9605556/how-to-project-a-point-onto-a-plane-in-3d
        //RefLink: https://thebuildingcoder.typepad.com/blog/2014/09/planes-projections-and-picking-points.html


        //Xác định khoảng cách từ 1 điểm tới một mặt phẳng (có thể âm hoặc dương)

        public static double getDisFromPointToPlane(XYZ p, Plane plane)
        {
            XYZ planeNvector = plane.Normal.Normalize();
            XYZ v = p - plane.Origin;
            return planeNvector.DotProduct(v);
        }

        public static double getAbsDisFromPointToPlane(XYZ p, Plane plane)
        {
            return Math.Abs(getDisFromPointToPlane(p, plane));
        }


        //Overide 1: Nhập 1 điểm và 1 plane
        public static XYZ getProjectPointOnPane(XYZ p, Plane plane)
        {
            //Vector giữa điểm và gốc mặt phẳng
            XYZ v = p - plane.Origin;

            //Tính khoảng cách từ điểm tới mặt phẳng
            double d = getDisFromPointToPlane(p, plane); //Có thể âm dương

            return p - d * plane.Normal;
        }


        //Xác định hình chiếu của 1 điểm "p" trên mặt phẳng tạo bởi 3 điểm "a", "b", "c"
        //Overide 2: Nhập tọa độ 4 điểm
        public static XYZ getProjectPointOnPane(XYZ p, XYZ a, XYZ b, XYZ c)
        {
            if (is3PCollinear(a, b, c))
            {
                TaskDialog.Show("Error!!!", "Cannot create plane from 3Point");
                return null;
            }
            else
            {
                Plane myPlane = Plane.CreateByThreePoints(a, b, c);
                return getProjectPointOnPane(p, myPlane);
            }
        }







        //Các hàm liên quan tới đường thẳng và đường thẳng (hoặc Vector và vector)

        //Tính góc giữa 2 vector
        public static double getCosOf2Vector(XYZ v1, XYZ v2)
        {
            return v1.Normalize().DotProduct(v2.Normalize());
        }


        //Kiểm tra 1 vector song song 
        public static bool check2VectorParallel(XYZ v1, XYZ v2)
        {
            if (Math.Round(Math.Abs(getCosOf2Vector(v1, v2)), 9) == 1.0)
            {
                return true;
            }

            else
            {
                return false;
            }

        }


        //Kiem tra 2 vector vuong goc
        public static bool check2VectorPerpencular(XYZ v1, XYZ v2)
        {
            if (Math.Round(Math.Abs(getCosOf2Vector(v1, v2)), 9) == 0.0)
            {
                return true;
            }

            else
            {
                return false;
            }

        }



        //Các hàm liên quan tới đường thẳng và mặt phẳng,

        public static XYZ getSectionPointWithPlane(Line l, Plane plane)
        {
            //Kiem tra line cắt mặt phẳng ko

            if (Math.Round(Math.Abs(getCosOf2Vector(l.Direction, plane.Normal)), 9) == 0)
            {
                //Kiem tra line nằm trên mặt phẳng ko!!!, nếu có trả về trung điểm của đường thằng
                if (Math.Round(getDisFromPointToPlane(l.GetEndPoint(0), plane), 9) == 0.0)
                {
                    //TaskDialog.Show("Info", "Line On the plane, will return middle Point of line.");
                    return (l.GetEndPoint(0) + l.GetEndPoint(1)) / 2;
                }
                else
                {
                    //TaskDialog.Show("Info", "Line parallel plane, will return Null instead a XYZ");
                    return null;
                }
            }


            else
            {
                // Lay cac thong tin cua mat phang va duong thang
                XYZ O = plane.Origin;
                XYZ n = plane.Normal;
                //Print_XYZ("O", O);
                //Print_XYZ("n", n);


                XYZ u = l.Direction;

                XYZ M = l.GetEndPoint(1);

                //Phuong trinh mat phang (1) Ax+By+Cz+D = 0
                //Voi D, A, B, C lan luot bang:
                double D = -n.X * O.X - n.Y * O.Y - n.Z * O.Z;
                double A = n.X;
                double B = n.Y;
                double C = n.Z;

                //Phuong trinh duong thang
                /*(2)
				 * x = x0 + a*t
				 * y = y0 + b*t
				 * z = z0 + c*t
				*/

                // trong do
                double x0 = M.X;
                double y0 = M.Y;
                double z0 = M.Z;

                double a = u.X;
                double b = u.Y;
                double c = u.Z;

                //Giai he phuong trinh tren bang thay 2 vao 1, tim ra t bang phuogn trinh sau
                // A(x0+at)+B(y0+bt)+C(z0+ct) + D = 0
                // t = (-D -A*x0 -B*y0 -C*z0)/(A*a+B*b+C*c)

                double t = (-D - A * x0 - B * y0 - C * z0) / (A * a + B * b + C * c);

                //Thay t vao 2 ta co toa do giao diem neu co

                return new XYZ(x0 + a * t, y0 + b * t, z0 + c * t);


            }
        }

        //Các hàm liên quan tới mặt phẳng và mặt phẳng

        //Các hàm liên quan tới Solid

    }

    //Type
    public enum PositionOfPoint
    {
        left, online, right

    }


    //Extension	

    public static class MyExtensions
    {

        //Transform

        public static Transform Clone(this Transform myTransform, Plane myPlane, XYZ origin, XYZ basisY)
        {
            myTransform.Origin = origin;

            myTransform.BasisY = basisY.Normalize();

            myTransform.BasisZ = (myPlane.Normal.Normalize() + origin).Normalize();

            myTransform.BasisX = myTransform.BasisY.CrossProduct(myTransform.BasisZ);
            return myTransform;
        }

        //XYZ
        public static XYZ asPoint3DInView(this XYZ myXYZ, View myView)
        {

            Plane myPlane = Plane.CreateByNormalAndOrigin(myView.ViewDirection, myView.Origin);


            Transform myNewCoorSysFromView = Transform.CreateReflection(myPlane);

            myNewCoorSysFromView.BasisX = myView.RightDirection;
            myNewCoorSysFromView.BasisY = myView.UpDirection;
            myNewCoorSysFromView.BasisZ = myView.ViewDirection;

            myNewCoorSysFromView.Origin = myView.Origin;

            return myNewCoorSysFromView.OfPoint(myXYZ);

        }


        public static Point2D asPoint2DInView(this XYZ myXYZ, View myView)
        {

            Plane myPlane = Plane.CreateByNormalAndOrigin(myView.ViewDirection, myView.Origin);


            Transform myNewCoorSysFromView = Transform.CreateReflection(myPlane);

            myNewCoorSysFromView.BasisX = myView.RightDirection;
            myNewCoorSysFromView.BasisY = myView.UpDirection;
            myNewCoorSysFromView.BasisZ = myView.ViewDirection;

            myNewCoorSysFromView.Origin = myView.Origin;

            XYZ point3DOnView = myNewCoorSysFromView.OfPoint(myXYZ);


            return new Point2D(point3DOnView.X, point3DOnView.Y);

        }


        //Extension For View
        public static XYZ OfPoint(this View myView, XYZ myXYZ)
        {
            Plane myPlane = Plane.CreateByNormalAndOrigin(myView.ViewDirection, myView.Origin);


            Transform myNewCoorSysFromView = Transform.CreateReflection(myPlane);

            myNewCoorSysFromView.BasisX = myView.RightDirection;
            myNewCoorSysFromView.BasisY = myView.UpDirection;
            myNewCoorSysFromView.BasisZ = myView.ViewDirection;

            myNewCoorSysFromView.Origin = myView.Origin;

            XYZ point3DOnView = myNewCoorSysFromView.OfPoint(myXYZ);


            return myNewCoorSysFromView.OfPoint(myXYZ);


        }


        public static List<XYZ> minMaxProjectPointInView(this View myView, List<XYZ> myListPoint)
        {

            Plane myPlane = Plane.CreateByNormalAndOrigin(myView.ViewDirection, myView.Origin);

            Transform myNewCoorSysFromView = Transform.CreateReflection(myPlane);

            myNewCoorSysFromView.BasisX = myView.RightDirection;
            myNewCoorSysFromView.BasisY = myView.UpDirection;
            myNewCoorSysFromView.BasisZ = myView.ViewDirection;

            myNewCoorSysFromView.Origin = myView.Origin;


            List<XYZ> myProjectXYZInView = new List<XYZ>();

            foreach (XYZ myPointInList in myListPoint)
            {
                XYZ projectPoint = Utils_Geometry_OXYZ.getProjectPointOnPane(myPointInList, myPlane);
                myProjectXYZInView.Add(projectPoint);

            }
            List<XYZ> outListPoint = new List<XYZ>();

            //Sort Right
            myProjectXYZInView.Sort(new SortXYZInView_ByRight(myView));
            outListPoint.Add(myProjectXYZInView[0]);
            outListPoint.Add(myProjectXYZInView[myProjectXYZInView.Count - 1]);

            //Sort Up
            myProjectXYZInView.Sort(new SortXYZInView_ByUp(myView));
            outListPoint.Add(myProjectXYZInView[0]);
            outListPoint.Add(myProjectXYZInView[myProjectXYZInView.Count - 1]);


            return myProjectXYZInView;

        }



    }


    //Custom Class

    public class Point2D
    {
        public double X;
        public double Y;

        public Point2D(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }




    //Cac lop sort

    //Cac lop sort

    //Sort By X;Y;Z of XYZ or Up Or Right
    public class SortXYZInView_ByX : IComparer<XYZ>
    {
        View myView = null;
        public SortXYZInView_ByX(View MyView)
        {
            this.myView = MyView;
        }
        public int Compare(XYZ lhs, XYZ rhs)
        {

            XYZ centerPointElementLhs = lhs;

            XYZ centerPointElementRhs = rhs;


            if (Math.Round(centerPointElementLhs.X, 4) == Math.Round(centerPointElementRhs.X, 4))
            {
                if (Math.Round(centerPointElementLhs.Y, 4) == Math.Round(centerPointElementRhs.Y, 4))
                {
                    return Math.Round(centerPointElementLhs.Z, 4).CompareTo(Math.Round(centerPointElementRhs.Z, 4));
                }
                else
                {
                    return Math.Round(centerPointElementLhs.Y, 4).CompareTo(Math.Round(centerPointElementRhs.Y, 4));
                }
            }
            else
            {
                return Math.Round(centerPointElementLhs.X, 4).CompareTo(Math.Round(centerPointElementRhs.X, 4));
            }
        }
    }



    public class SortXYZInView_ByY : IComparer<XYZ>
    {
        View myView = null;
        public SortXYZInView_ByY(View MyView)
        {
            this.myView = MyView;
        }
        public int Compare(XYZ lhs, XYZ rhs)
        {


            XYZ centerPointElementLhs = lhs;


            XYZ centerPointElementRhs = rhs;


            if (Math.Round(centerPointElementLhs.Y, 4) == Math.Round(centerPointElementRhs.Y, 4))
            {
                if (Math.Round(centerPointElementLhs.X, 4) == Math.Round(centerPointElementRhs.X, 4))
                {
                    return Math.Round(centerPointElementLhs.Z, 4).CompareTo(Math.Round(centerPointElementRhs.Z, 4));
                }
                else
                {
                    return Math.Round(centerPointElementLhs.X, 4).CompareTo(Math.Round(centerPointElementRhs.X, 4));
                }
            }
            else
            {
                return Math.Round(centerPointElementLhs.Y, 4).CompareTo(Math.Round(centerPointElementRhs.Y, 4));
            }
        }
    }



    public class SortXYZInView_ByZ : IComparer<XYZ>
    {
        View myView = null;
        public SortXYZInView_ByZ(View MyView)
        {
            this.myView = MyView;
        }
        public int Compare(XYZ lhs, XYZ rhs)
        {

            XYZ centerPointElementLhs = lhs;

            XYZ centerPointElementRhs = rhs;


            if (Math.Round(centerPointElementLhs.Z, 4) == Math.Round(centerPointElementRhs.Z, 4))
            {
                if (Math.Round(centerPointElementLhs.X, 4) == Math.Round(centerPointElementRhs.X, 4))
                {
                    return Math.Round(centerPointElementLhs.Y, 4).CompareTo(Math.Round(centerPointElementRhs.Y, 4));
                }
                else
                {
                    return Math.Round(centerPointElementLhs.X, 4).CompareTo(Math.Round(centerPointElementRhs.X, 4));
                }
            }
            else
            {
                return Math.Round(centerPointElementLhs.Z, 4).CompareTo(Math.Round(centerPointElementRhs.Z, 4));
            }
        }
    }



    public class SortXYZInView_ByUp : IComparer<XYZ>
    {
        View myView = null;
        public SortXYZInView_ByUp(View MyView)
        {
            this.myView = MyView;
        }
        public int Compare(XYZ lhs, XYZ rhs)
        {

            //Change transform to View
            XYZ centerPointElementInViewLhs = lhs.asPoint3DInView(myView);
            XYZ centerPointElementInViewRhs = rhs.asPoint3DInView(myView);


            if (Math.Round(centerPointElementInViewLhs.Y, 4) == Math.Round(centerPointElementInViewRhs.Y, 4))
            {
                if (Math.Round(centerPointElementInViewLhs.X, 4) == Math.Round(centerPointElementInViewRhs.X, 4))
                {
                    return Math.Round(centerPointElementInViewLhs.Z, 4).CompareTo(Math.Round(centerPointElementInViewRhs.Z, 4));
                }
                else
                {
                    return Math.Round(centerPointElementInViewLhs.X, 4).CompareTo(Math.Round(centerPointElementInViewRhs.X, 4));
                }
            }
            else
            {
                return Math.Round(centerPointElementInViewLhs.Y, 4).CompareTo(Math.Round(centerPointElementInViewRhs.Y, 4));
            }
        }
    }



    public class SortXYZInView_ByRight : IComparer<XYZ>
    {
        View myView = null;
        public SortXYZInView_ByRight(View MyView)
        {
            this.myView = MyView;
        }
        public int Compare(XYZ lhs, XYZ rhs)
        {

            //Change transform to View
            XYZ centerPointElementInViewLhs = lhs.asPoint3DInView(myView);
            XYZ centerPointElementInViewRhs = rhs.asPoint3DInView(myView);


            if (Math.Round(centerPointElementInViewLhs.X, 4) == Math.Round(centerPointElementInViewRhs.X, 4))
            {
                if (Math.Round(centerPointElementInViewLhs.Y, 4) == Math.Round(centerPointElementInViewRhs.Y, 4))
                {
                    return Math.Round(centerPointElementInViewLhs.Z, 4).CompareTo(Math.Round(centerPointElementInViewRhs.Z, 4));
                }
                else
                {
                    return Math.Round(centerPointElementInViewLhs.Y, 4).CompareTo(Math.Round(centerPointElementInViewRhs.Y, 4));
                }
            }
            else
            {
                return Math.Round(centerPointElementInViewLhs.X, 4).CompareTo(Math.Round(centerPointElementInViewRhs.X, 4));
            }
        }
    }



    //Sort Point Center Of Element

    public class SortElementInView_ByX : IComparer<Element>
    {
        View myView = null;
        public SortElementInView_ByX(View MyView)
        {
            this.myView = MyView;
        }
        public int Compare(Element lhs, Element rhs)
        {

            //Get bouding box of rebarSet lHS
            BoundingBoxXYZ myElementLhs_BB = lhs.get_BoundingBox(this.myView);

            XYZ minPointElementLhs = myElementLhs_BB.Min;
            XYZ maxPointElementLhs = myElementLhs_BB.Max;
            XYZ centerPointElementLhs = (minPointElementLhs + maxPointElementLhs) / 2;

            //Get bouding box of rebarSet lHS
            BoundingBoxXYZ myElementRhs_BB = rhs.get_BoundingBox(this.myView);
            XYZ minPointElementRhs = myElementRhs_BB.Min;
            XYZ maxPointElementRhs = myElementRhs_BB.Max;
            XYZ centerPointElementRhs = (minPointElementRhs + maxPointElementRhs) / 2;


            if (Math.Round(centerPointElementLhs.X, 4) == Math.Round(centerPointElementRhs.X, 4))
            {
                if (Math.Round(centerPointElementLhs.Y, 4) == Math.Round(centerPointElementRhs.Y, 4))
                {
                    return Math.Round(centerPointElementLhs.Z, 4).CompareTo(Math.Round(centerPointElementRhs.Z, 4));
                }
                else
                {
                    return Math.Round(centerPointElementLhs.Y, 4).CompareTo(Math.Round(centerPointElementRhs.Y, 4));
                }
            }
            else
            {
                return Math.Round(centerPointElementLhs.X, 4).CompareTo(Math.Round(centerPointElementRhs.X, 4));
            }
        }
    }



    public class SortElementInView_ByY : IComparer<Element>
    {
        View myView = null;
        public SortElementInView_ByY(View MyView)
        {
            this.myView = MyView;
        }
        public int Compare(Element lhs, Element rhs)
        {

            //Get bouding box of rebarSet lHS
            BoundingBoxXYZ myElementLhs_BB = lhs.get_BoundingBox(this.myView);

            XYZ minPointElementLhs = myElementLhs_BB.Min;
            XYZ maxPointElementLhs = myElementLhs_BB.Max;
            XYZ centerPointElementLhs = (minPointElementLhs + maxPointElementLhs) / 2;

            //Get bouding box of rebarSet lHS
            BoundingBoxXYZ myElementRhs_BB = rhs.get_BoundingBox(this.myView);
            XYZ minPointElementRhs = myElementRhs_BB.Min;
            XYZ maxPointElementRhs = myElementRhs_BB.Max;
            XYZ centerPointElementRhs = (minPointElementRhs + maxPointElementRhs) / 2;


            if (Math.Round(centerPointElementLhs.Y, 4) == Math.Round(centerPointElementRhs.Y, 4))
            {
                if (Math.Round(centerPointElementLhs.X, 4) == Math.Round(centerPointElementRhs.X, 4))
                {
                    return Math.Round(centerPointElementLhs.Z, 4).CompareTo(Math.Round(centerPointElementRhs.Z, 4));
                }
                else
                {
                    return Math.Round(centerPointElementLhs.X, 4).CompareTo(Math.Round(centerPointElementRhs.X, 4));
                }
            }
            else
            {
                return Math.Round(centerPointElementLhs.Y, 4).CompareTo(Math.Round(centerPointElementRhs.Y, 4));
            }
        }
    }



    public class SortElementInView_ByZ : IComparer<Element>
    {
        View myView = null;
        public SortElementInView_ByZ(View MyView)
        {
            this.myView = MyView;
        }
        public int Compare(Element lhs, Element rhs)
        {

            //Get bouding box of rebarSet lHS
            BoundingBoxXYZ myElementLhs_BB = lhs.get_BoundingBox(this.myView);

            XYZ minPointElementLhs = myElementLhs_BB.Min;
            XYZ maxPointElementLhs = myElementLhs_BB.Max;
            XYZ centerPointElementLhs = (minPointElementLhs + maxPointElementLhs) / 2;

            //Get bouding box of rebarSet lHS
            BoundingBoxXYZ myElementRhs_BB = rhs.get_BoundingBox(this.myView);
            XYZ minPointElementRhs = myElementRhs_BB.Min;
            XYZ maxPointElementRhs = myElementRhs_BB.Max;
            XYZ centerPointElementRhs = (minPointElementRhs + maxPointElementRhs) / 2;


            if (Math.Round(centerPointElementLhs.Z, 4) == Math.Round(centerPointElementRhs.Z, 4))
            {
                if (Math.Round(centerPointElementLhs.X, 4) == Math.Round(centerPointElementRhs.X, 4))
                {
                    return Math.Round(centerPointElementLhs.Y, 4).CompareTo(Math.Round(centerPointElementRhs.Y, 4));
                }
                else
                {
                    return Math.Round(centerPointElementLhs.X, 4).CompareTo(Math.Round(centerPointElementRhs.X, 4));
                }
            }
            else
            {
                return Math.Round(centerPointElementLhs.Z, 4).CompareTo(Math.Round(centerPointElementRhs.Z, 4));
            }
        }
    }




    public class SortElementInView_ByUp : IComparer<Element>
    {
        View myView = null;
        public SortElementInView_ByUp(View MyView)
        {
            this.myView = MyView;
        }
        public int Compare(Element lhs, Element rhs)
        {

            //Get bouding box of rebarSet lHS
            BoundingBoxXYZ myElementLhs_BB = lhs.get_BoundingBox(this.myView);

            XYZ minPointElementLhs = myElementLhs_BB.Min;
            XYZ maxPointElementLhs = myElementLhs_BB.Max;
            XYZ centerPointElementLhs = (minPointElementLhs + maxPointElementLhs) / 2;

            //Get bouding box of rebarSet lHS
            BoundingBoxXYZ myElementRhs_BB = rhs.get_BoundingBox(this.myView);
            XYZ minPointElementRhs = myElementRhs_BB.Min;
            XYZ maxPointElementRhs = myElementRhs_BB.Max;
            XYZ centerPointElementRhs = (minPointElementRhs + maxPointElementRhs) / 2;



            //Change transform to View
            XYZ centerPointElementInViewLhs = centerPointElementLhs.asPoint3DInView(myView);
            XYZ centerPointElementInViewRhs = centerPointElementRhs.asPoint3DInView(myView);


            if (Math.Round(centerPointElementInViewLhs.Y, 4) == Math.Round(centerPointElementInViewRhs.Y, 4))
            {
                if (Math.Round(centerPointElementInViewLhs.X, 4) == Math.Round(centerPointElementInViewRhs.X, 4))
                {
                    return Math.Round(centerPointElementInViewLhs.Z, 4).CompareTo(Math.Round(centerPointElementInViewRhs.Z, 4));
                }
                else
                {
                    return Math.Round(centerPointElementInViewLhs.X, 4).CompareTo(Math.Round(centerPointElementInViewRhs.X, 4));
                }
            }
            else
            {
                return Math.Round(centerPointElementInViewLhs.Y, 4).CompareTo(Math.Round(centerPointElementInViewRhs.Y, 4));
            }
        }
    }



    public class SortElementInView_ByRight : IComparer<Element>
    {
        View myView = null;
        public SortElementInView_ByRight(View MyView)
        {
            this.myView = MyView;
        }
        public int Compare(Element lhs, Element rhs)
        {

            //Get bouding box of rebarSet lHS
            BoundingBoxXYZ myElementLhs_BB = lhs.get_BoundingBox(this.myView);

            XYZ minPointElementLhs = myElementLhs_BB.Min;
            XYZ maxPointElementLhs = myElementLhs_BB.Max;
            XYZ centerPointElementLhs = (minPointElementLhs + maxPointElementLhs) / 2;

            //Get bouding box of rebarSet lHS
            BoundingBoxXYZ myElementRhs_BB = rhs.get_BoundingBox(this.myView);
            XYZ minPointElementRhs = myElementRhs_BB.Min;
            XYZ maxPointElementRhs = myElementRhs_BB.Max;
            XYZ centerPointElementRhs = (minPointElementRhs + maxPointElementRhs) / 2;



            //Change transform to View
            XYZ centerPointElementInViewLhs = centerPointElementLhs.asPoint3DInView(myView);
            XYZ centerPointElementInViewRhs = centerPointElementRhs.asPoint3DInView(myView);


            if (Math.Round(centerPointElementInViewLhs.X, 4) == Math.Round(centerPointElementInViewRhs.X, 4))
            {
                if (Math.Round(centerPointElementInViewLhs.Y, 4) == Math.Round(centerPointElementInViewRhs.Y, 4))
                {
                    return Math.Round(centerPointElementInViewLhs.Z, 4).CompareTo(Math.Round(centerPointElementInViewRhs.Z, 4));
                }
                else
                {
                    return Math.Round(centerPointElementInViewLhs.Y, 4).CompareTo(Math.Round(centerPointElementInViewRhs.Y, 4));
                }
            }
            else
            {
                return Math.Round(centerPointElementInViewLhs.X, 4).CompareTo(Math.Round(centerPointElementInViewRhs.X, 4));
            }
        }
    }




}
