﻿using System;
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

namespace SetLayoutStirrupRebar
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class SetLayoutStirrupRebar : IExternalCommand
    {
        // Set [Guid("B99FEFE1-83B1-48A1-9A43-B3A086E35D09")]
        static AddInId appId = new AddInId(new Guid("B99FEFE1-83B1-48A1-9A43-B3A086E35D09"));

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
                rebarStirrupLayout_Form(commandData.Application.ActiveUIDocument);
                return Result.Succeeded;
            }

            catch
            {
                //TaskDialog.Show("Error", "OK ");
                return Result.Succeeded;

            }
        }




        public void rebarStirrupLayout_Form(UIDocument uiDoc)
        {
            Document doc = uiDoc.Document;

            bool isOpposite = false;

            double factor = 4;
            double delta_1 = 50 / 304.8;
            double pitch_1 = 150 / 304.8;
            double pitch_2 = 300 / 304.8;

            double delta_3 = 50 / 304.8;
            double pitch_3 = 100 / 304.8;
            int N3 = 5;

            double myConvertFactor = 304.8;


            bool inputSuccess = false;
            while (!inputSuccess)
            {
                using (var myInputFormSetting = new SettingDialog())
                {
                    myInputFormSetting.ShowDialog();

                    factor = Convert.ToDouble(myInputFormSetting.factorTb.Text);
                    delta_1 = Convert.ToDouble(myInputFormSetting.delta_1Tb.Text) / myConvertFactor;
                    pitch_1 = Convert.ToDouble(myInputFormSetting.pitch_1Tb.Text) / myConvertFactor;
                    pitch_2 = Convert.ToDouble(myInputFormSetting.pitch_2Tb.Text) / myConvertFactor;

                    delta_3 = Convert.ToDouble(myInputFormSetting.delta_3Tb.Text) / myConvertFactor;
                    pitch_3 = Convert.ToDouble(myInputFormSetting.pitch_3Tb.Text) / myConvertFactor;
                    N3 = Convert.ToInt32(myInputFormSetting.n3Tb.Text);
                    isOpposite = myInputFormSetting.oppDirChecked;

                    //if the user hits cancel just drop out of macro
                    if (myInputFormSetting.DialogResult == System.Windows.Forms.DialogResult.Cancel) return;
                    {
                        //else do all this :)    
                        myInputFormSetting.Close();
                    }

                    if (myInputFormSetting.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        //else do all this :) 
                        inputSuccess = true;
                        myInputFormSetting.Close();
                    }

                }

            }



            // Pick Rebar
            List<int> myListIdCategoryRebar = new List<int>();
            myListIdCategoryRebar.Add((int)BuiltInCategory.OST_Rebar);

            // Select first Element (ex beam)
            List<Reference> myListRefRebar = uiDoc.Selection.PickObjects(ObjectType.Element,
                                                              new FilterByIdCategory(myListIdCategoryRebar),
                                                              "Pick a Rebar...") as List<Reference>;

            List<Rebar> myListRebar = new List<Rebar>();

            foreach (Reference myRefRebar in myListRefRebar)
            {
                Rebar myRebarpicked = doc.GetElement(myRefRebar) as Rebar;
                myListRebar.Add(myRebarpicked);
            }


            Element myBeam = doc.GetElement(myListRebar[0].GetHostId());

            //Get location curve of beam
            LocationCurve lc = myBeam.Location as LocationCurve;
            Line line = lc.Curve as Line;

            //Get vector of location cuver beam
            XYZ p1 = line.GetEndPoint(0);
            XYZ q = line.GetEndPoint(1);
            XYZ v = q - p1; // Vector equation of line

            XYZ p = p1 - 0.1 * v;

            //Set current Beam be Joined

            //setBeJoined(myBeam);

            while (true)
            {

                #region Pick faces
                //Pick Face end

                List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Face) as List<Reference>;

                List<Face> myListFacePicked = new List<Face>();

                foreach (Reference myRef in myListRef)
                {
                    Element E = doc.GetElement(myRef);

                    GeometryObject myGeoObj = E.GetGeometryObjectFromReference(myRef);

                    Face myPickedFace = myGeoObj as Face;

                    myListFacePicked.Add(myPickedFace);
                }
                #endregion


                #region Error picked faces	
                if (myListFacePicked.Count != 2 && myListFacePicked.Count != 4)
                {
                    TaskDialog.Show("Error!", "Chua ho tro lua chon: " + myListFacePicked.Count() + " mat, Chon 2 hoac 4 mat");
                    continue;

                }

                #endregion

                else
                {

                    string caseDistributionRebar = "TH2: Co dam o giua";

                    List<double> myListSpace = new List<double>() { pitch_1, pitch_2, pitch_3, pitch_3, pitch_2, pitch_1 };
                    if (myListFacePicked.Count == 2)
                    {
                        myListSpace = new List<double>() { pitch_1, pitch_2, pitch_1, pitch_2, pitch_2, pitch_1 };
                        caseDistributionRebar = "TH1: Khong co dam o giua";
                    }
                    TaskDialog.Show("Info", caseDistributionRebar);

                    // List of boundaries faces
                    List<double> myListEndPointDis = getAndSortDisOfEndFaces(myListFacePicked, p);
                    myListEndPointDis.Sort();

                    Dictionary<double, double> myDicDisNumDetail = detailListDistance_Update_MaxSpace(myListEndPointDis,
                                                                                    factor,
                                                                                    delta_1, pitch_1,
                                                                                    pitch_2,
                                                                                    delta_3, pitch_3, N3);

                    foreach (Rebar myRebar in myListRebar)
                    {
                        List<ElementId> myListRebarCopyId = copyRebarByDistance_Update_MaxSpace(doc, myRebar, myDicDisNumDetail);


                        List<double> myDistances = myDicDisNumDetail.Keys.ToList();
                        myDistances.Sort();

                        List<double> myListNum = new List<double>();

                        foreach (double key in myDistances)
                        {
                            myListNum.Add(myDicDisNumDetail[key]);
                        }


                        // using transcation (edit DB)
                        for (int i = 0; i < myListRebarCopyId.Count(); i++)
                        {
                            using (Transaction myTrans = new Transaction(doc, "CopyElementByCoordinate"))

                            {
                                myTrans.Start();
                                ElementId rebarId = myListRebarCopyId[i];
                                Rebar myRebarI = doc.GetElement(rebarId) as Rebar;

                                if (i == 1 || i == 4)
                                {
                                    if (myListNum[i] / myListSpace[i] < 1)
                                    {
                                        myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
                                    }


                                    else
                                    {
                                        myRebarI.GetShapeDrivenAccessor().SetLayoutAsMaximumSpacing(myListSpace[i], (myListNum[i]), isOpposite, false, false);

                                    }

                                }

                                else
                                {
                                    if (myListNum[i] / myListSpace[i] < 1)
                                    {
                                        myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
                                    }

                                    else
                                    {
                                        myRebarI.GetShapeDrivenAccessor().SetLayoutAsMaximumSpacing(myListSpace[i], (myListNum[i]), isOpposite, true, true);
                                    }

                                }

                                myTrans.Commit();
                            }

                        }

                    }
                }
            }
        }


        //Tra ve 2 dic gom khoang cach offset va so thanh layout tuong ung 
        private Dictionary<double, double> detailListDistance_Update_MaxSpace(List<double> endPoiList, double factorDivide,
                                                 double delta_1, double pitch_1,
                                                  double pitch_2,
                                                 double delta_3, double pitch_3, int N3)
        {
            Dictionary<double, double> myReturnDic = new Dictionary<double, double>();
            if (endPoiList.Count != 2 && endPoiList.Count != 4)
            {
                return myReturnDic;
            }

            if (endPoiList.Count == 2)
            {
                // Always positive
                double lengthSeg = endPoiList[endPoiList.Count() - 1] - endPoiList[0];
                // Diem dau cac doan
                double X1 = endPoiList[0] + delta_1;
                double X2 = X1 + lengthSeg / factorDivide - delta_1;
                double X5 = X2 + lengthSeg - 2 * lengthSeg / factorDivide;

                //Length Array
                double L1 = X2 - X1;
                double L2 = X5 - X2;
                double L5 = L1;


                myReturnDic.Add(X1, L1);
                myReturnDic.Add(X2, L2);
                myReturnDic.Add(X5, L5);

                return myReturnDic;
            }

            else
            {
                // Always positive
                double lengthSeg = endPoiList[endPoiList.Count() - 1] - endPoiList[0];
                // Diem dau cac doan
                double X1 = endPoiList[0] + delta_1;
                double X2 = X1 + lengthSeg / factorDivide - delta_1;
                double X3 = endPoiList[1] - delta_3 - (N3) * pitch_3;
                double X4 = endPoiList[2] + delta_3;
                double X5 = X4 + (N3) * pitch_3;

                double X6 = X2 + lengthSeg - 2 * lengthSeg / factorDivide;



                //Length Array
                double L1 = X2 - X1;
                double L2 = X3 - X2;
                double L3 = (N3) * pitch_3;
                double L4 = (N3) * pitch_3;
                double L5 = X6 - X5;
                double L6 = L1;


                myReturnDic.Add(X1, L1);
                myReturnDic.Add(X2, L2);
                myReturnDic.Add(X3, L3);
                myReturnDic.Add(X4, L4);
                myReturnDic.Add(X5, L5);
                myReturnDic.Add(X6, L6);
                return myReturnDic;

            }

        }





        private List<ElementId> copyRebarByDistance_Update_MaxSpace(Document doc, Rebar myRebar, Dictionary<double, double> myDicDisNum)
        {

            List<double> myDistances = myDicDisNum.Keys.ToList();
            myDistances.Sort();

            List<double> myListNum = new List<double>();

            foreach (double key in myDistances)
            {
                myListNum.Add(myDicDisNum[key]);
            }


            //Get Id rebar
            ElementId myIdRebar = myRebar.Id;


            //Get Id Beam
            ElementId myIdBeam = myRebar.GetHostId();

            Element myBeam = doc.GetElement(myIdBeam);


            if (myBeam.Category.Name != "Structural Framing")
            {
                TaskDialog.Show("Loi!", "Hay chon 1 rebar co host la 1 Structural Framing");
                return null;
            }

            else
            {
                LocationCurve cur = myBeam.Location as LocationCurve;
                Line lineCur = cur.Curve as Line;


                XYZ p1 = lineCur.GetEndPoint(0);
                XYZ q = lineCur.GetEndPoint(1);
                XYZ v = q - p1;
                double lengCurLine = v.GetLength();

                XYZ p = p1 - 0.1 * v;

                List<XYZ> myCoors = new List<XYZ>();

                // NOTE LAM TRON SO

                //Get diameter of rebar

                XYZ middlePoint = myRebar.get_BoundingBox(null).Max.Add(myRebar.get_BoundingBox(null).Min) / 2;

                List<Curve> centerLines = myRebar.GetCenterlineCurves(false, false, false,
                                                                     MultiplanarOption.IncludeOnlyPlanarCurves, 0)
                    as List<Curve>;


                foreach (Curve myCurBar in centerLines)
                {
                    middlePoint = myCurBar.GetEndPoint(0);
                    break;
                }

                Plane myReBarPlane = Plane.CreateByNormalAndOrigin(v, middlePoint);


                // Distance from first rebar to
                RebarBarType myRbType = doc.GetElement(myRebar.GetTypeId()) as RebarBarType;
                double myRebarDiameter = myRbType.BarDiameter;


                XYZ v1 = p - myReBarPlane.Origin;

                double delta_0 = Math.Abs(myReBarPlane.Normal.DotProduct(v1));

                if (delta_0 == 10000000) return null;

                foreach (double distance in myDistances)
                {

                    XYZ myPointPlace = ((distance - delta_0) / lengCurLine) * v;
                    myCoors.Add(myPointPlace);
                }

                if (myCoors.Count < 1)
                {
                    TaskDialog.Show("Loi!", "Khong the copy...");
                    return null;
                }

                ICollection<ElementId> myRebarIdCol;
                List<ElementId> myListIdRebar = new List<ElementId>();

                // using transcation (edit DB)
                using (Transaction myTrans = new Transaction(doc, "CopyElementByCoordinate"))

                {
                    myTrans.Start();
                    foreach (XYZ myXYZ in myCoors)
                    {
                        myRebarIdCol = ElementTransformUtils.CopyElement(doc, myIdRebar, myXYZ);
                        foreach (ElementId elemRebarId in myRebarIdCol)
                        {
                            myListIdRebar.Add(elemRebarId);
                        }

                    }
                    myTrans.Commit();
                }
                return myListIdRebar;

            }

        }



        private List<double> getAndSortDisOfEndFaces(List<Face> myListFace, XYZ myRefpoint)
        {
            List<double> myListDisOfEndFaces = new List<double>();

            double myDisFace;
            foreach (Face myFace in myListFace)
            {
                myDisFace = getDisFromPointToPlaneFace(myRefpoint, myFace);
                if (!myListDisOfEndFaces.Contains(myDisFace))
                {
                    myListDisOfEndFaces.Add(myDisFace);
                }

            }
            myListDisOfEndFaces.Sort();
            return myListDisOfEndFaces;
        }




        private void setBeJoined(Document doc, Element myBeJoined)
        {
            ICollection<ElementId> myListElemIdsJoined = JoinGeometryUtils.GetJoinedElements(doc, myBeJoined);

            using (Transaction trans = new Transaction(doc, "Switch Join"))
            {
                trans.Start();
                foreach (ElementId myElemId in myListElemIdsJoined)
                {

                    if (!JoinGeometryUtils.IsCuttingElementInJoin(doc, doc.GetElement(myElemId), myBeJoined))
                    {
                        JoinGeometryUtils.SwitchJoinOrder(doc, doc.GetElement(myElemId), myBeJoined);
                    }

                }

                trans.Commit();
            }
        }



        private double getDisFromPointToPlaneFace(XYZ myPoint, Face myFace) // Both Nagative and Positive
        {
            Plane myPlaneFace = myFace.GetSurface() as Plane;

            XYZ v = myPoint - myPlaneFace.Origin;

            double myDis = myPlaneFace.Normal.DotProduct(v);


            return Math.Abs(myDis);
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
