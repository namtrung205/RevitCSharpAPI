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



}
