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

            //Stirrup
            bool isSt1Yes = true;
            bool isOpposite = false;

            double factor = 4;
            double delta_1 = 50 / 304.8;
            double pitch_1 = 150 / 304.8;
            double pitch_2 = 300 / 304.8;

            double delta_3 = 50 / 304.8;
            double pitch_3 = 100 / 304.8;
            int N3 = 4;


            //Stirrup
            bool isYes_C1 = true;
            bool isOpposite_C1 = false;

            double factor_C1 = 4;
            double delta_1_C1 = 50 / 304.8;
            double pitch_1_C1 = 150 / 304.8;
            double pitch_2_C1 = 300 / 304.8;

            double delta_3_C1 = 50 / 304.8;
            double pitch_3_C1 = 100 / 304.8;
            int N3_C1 = 4;

            double myConvertFactor = 304.8;


            //Load rebarShape
            // RebarShape
            FilteredElementCollector fecRebarShap = new FilteredElementCollector(doc)
            .OfClass(typeof(RebarShape));

            IEnumerable<RebarShape> iterRebarBarShapes = fecRebarShap.Cast<RebarShape>();


            // Load Rebar Type
            // Rebartype
            FilteredElementCollector fecBarType = new FilteredElementCollector(doc)
            .OfClass(typeof(RebarBarType));

            IEnumerable<RebarBarType> iterRebarBarTypes = fecBarType.Cast<RebarBarType>();


            // Bottom Layer
            // Layer1
            bool isLB1Yes = true;
            double FB1 = 6;
            double CB1 = 80;
            string rebaNameShape_L1 = string.Empty;
            string rebaNameType_L1 = string.Empty;


            // Bottom Layer
            // Layer2
            bool isLB2Yes = true;
            double FB2 = 3;
            double CB2 = 150;

            string rebaNameShape_L2 = string.Empty;
            string rebaNameType_L2 = string.Empty;

            bool inputSuccess = false;
            while (!inputSuccess)
            {
                using (var myInputFormSetting = new SettingDialog())
                {
                    //add rebar Shape list
                    foreach (RebarShape myRebarShape in iterRebarBarShapes)
                    {
                        myInputFormSetting.RebarShap1_Cb.Items.Add(myRebarShape.Name);
                        myInputFormSetting.RebarShap2_Cb.Items.Add(myRebarShape.Name);

                    }

                    //					myInputFormSetting.RebarShap1_Cb.SelectedIndex = 0;
                    //					myInputFormSetting.RebarShap2_Cb.SelectedIndex = 0;

                    //add rebar type list	
                    foreach (RebarBarType myRebarType in iterRebarBarTypes)
                    {
                        myInputFormSetting.RebarType1_Cb.Items.Add(myRebarType.Name);
                        myInputFormSetting.RebarType2_Cb.Items.Add(myRebarType.Name);
                    }

                    //					myInputFormSetting.RebarType1_Cb.SelectedIndex = 1;
                    //					myInputFormSetting.RebarType2_Cb.SelectedIndex = 1;


                    myInputFormSetting.ShowDialog();

                    //stirrup
                    factor = Convert.ToDouble(myInputFormSetting.factorTb.Text);
                    delta_1 = Convert.ToDouble(myInputFormSetting.delta_1Tb.Text) / myConvertFactor;
                    pitch_1 = Convert.ToDouble(myInputFormSetting.pitch_1Tb.Text) / myConvertFactor;
                    pitch_2 = Convert.ToDouble(myInputFormSetting.pitch_2Tb.Text) / myConvertFactor;

                    delta_3 = Convert.ToDouble(myInputFormSetting.delta_3Tb.Text) / myConvertFactor;
                    pitch_3 = Convert.ToDouble(myInputFormSetting.pitch_3Tb.Text) / myConvertFactor;
                    N3 = Convert.ToInt32(myInputFormSetting.n3Tb.Text);
                    isOpposite = myInputFormSetting.oppDirChecked;
                    isSt1Yes = myInputFormSetting.yesST_Chb.Checked;


                    //C1
                    factor_C1 = Convert.ToDouble(myInputFormSetting.factor_C1Tb.Text);
                    delta_1_C1 = Convert.ToDouble(myInputFormSetting.delta_1_C1Tb.Text) / myConvertFactor;
                    pitch_1_C1 = Convert.ToDouble(myInputFormSetting.pitch_1_C1Tb.Text) / myConvertFactor;
                    pitch_2_C1 = Convert.ToDouble(myInputFormSetting.pitch_2_C1Tb.Text) / myConvertFactor;

                    delta_3_C1 = Convert.ToDouble(myInputFormSetting.delta_3_C1Tb.Text) / myConvertFactor;
                    pitch_3_C1 = Convert.ToDouble(myInputFormSetting.pitch_3_C1Tb.Text) / myConvertFactor;
                    N3_C1 = Convert.ToInt32(myInputFormSetting.n3_C1Tb.Text);
                    isOpposite_C1 = myInputFormSetting.oppDirChecked;
                    isYes_C1 = myInputFormSetting.yesC1_Chb.Checked;


                    //Shape
                    rebaNameShape_L1 = myInputFormSetting.RebarShap1_Cb.Text;
                    rebaNameShape_L2 = myInputFormSetting.RebarShap2_Cb.Text;

                    //type
                    rebaNameType_L1 = myInputFormSetting.RebarType1_Cb.Text;
                    rebaNameType_L2 = myInputFormSetting.RebarType2_Cb.Text;


                    //Yes/ No bottom rebar
                    if (!myInputFormSetting.yes1_Chb.Checked) { isLB1Yes = false; }
                    if (!myInputFormSetting.yes2_Chb.Checked) { isLB2Yes = false; }

                    // Layer1

                    FB1 = Convert.ToDouble(myInputFormSetting.FB1_Tb.Text);
                    CB1 = Convert.ToDouble(myInputFormSetting.CB1_Tb.Text) / myConvertFactor;


                    // Bottom Layer
                    // Layer2

                    FB2 = Convert.ToDouble(myInputFormSetting.FB2_Tb.Text);
                    CB2 = Convert.ToDouble(myInputFormSetting.CB2_Tb.Text) / myConvertFactor;



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

            //Check mode
            //			TaskDialog.Show("opposite", isOpposite.ToString());
            //			TaskDialog.Show("draw stirrup", isSt1Yes.ToString());
            //
            //			TaskDialog.Show("isL1", isLB1Yes.ToString());
            //			TaskDialog.Show("IsL2", isLB1Yes.ToString());
            //
            //
            //			
            //			TaskDialog.Show("Rebar Shape1", rebaNameShape_L1);
            //			TaskDialog.Show("Rebar Shape2", rebaNameShape_L2);		
            //
            //						
            //			TaskDialog.Show("Rebar Shape1", rebaNameType_L1);
            //			TaskDialog.Show("Rebar Shape2", rebaNameType_L2);	


            //Set mybeam
            Element myBeam = null;

            // Kiem tra ve stirrup

            List<Rebar> myListRebar = new List<Rebar>();
            List<Rebar> myListRebar_C1 = new List<Rebar>();

            if (isSt1Yes)
            {
                // Pick Rebar
                List<int> myListIdCategoryRebar = new List<int>();
                myListIdCategoryRebar.Add((int)BuiltInCategory.OST_Rebar);

                // Select first Element (ex beam)
                List<Reference> myListRefRebar = uiDoc.Selection.PickObjects(ObjectType.Element,
                                                                  new FilterByIdCategory(myListIdCategoryRebar),
                                                                  "Pick a stirrup Rebar...") as List<Reference>;


                foreach (Reference myRefRebar in myListRefRebar)
                {
                    Rebar myRebarpicked = doc.GetElement(myRefRebar) as Rebar;
                    myListRebar.Add(myRebarpicked);
                }
                myBeam = doc.GetElement(myListRebar[0].GetHostId());
            }

            if (isYes_C1)
            {
                // Pick Rebar
                List<int> myListIdCategoryRebar_C1 = new List<int>();
                myListIdCategoryRebar_C1.Add((int)BuiltInCategory.OST_Rebar);

                // Select first Element (ex beam)
                List<Reference> myListRefRebar_C1 = uiDoc.Selection.PickObjects(ObjectType.Element,
                                                                  new FilterByIdCategory(myListIdCategoryRebar_C1),
                                                                  "Pick a C Rebar ...") as List<Reference>;


                foreach (Reference myRefRebar_C1 in myListRefRebar_C1)
                {
                    Rebar myRebarpicked_C1 = doc.GetElement(myRefRebar_C1) as Rebar;
                    myListRebar_C1.Add(myRebarpicked_C1);
                }
                myBeam = doc.GetElement(myListRebar_C1[0].GetHostId());
            }

            else
            {
                List<int> myListIdCategoryRebar = new List<int>();
                myListIdCategoryRebar.Add((int)BuiltInCategory.OST_StructuralFraming);
                Reference myRefmyBeam = uiDoc.Selection.PickObject(ObjectType.Element,
                                                                  new FilterByIdCategory(myListIdCategoryRebar),
                                                                  "Pick a Beam...");

                myBeam = doc.GetElement(myRefmyBeam);
            }


            //Get location curve of beam
            LocationCurve lc = myBeam.Location as LocationCurve;
            Line line = lc.Curve as Line;

            //Get vector of location cuver beam
            XYZ p = line.GetEndPoint(0);
            XYZ q = line.GetEndPoint(1);
            XYZ v = q - p; // Vector equation of line

            XYZ pE = p - 0.5 * v;
            XYZ vE = q - p;

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
                    string caseDistributionRebar = "Case 2: 4 Faces";

                    List<double> myListSpace = new List<double>() { pitch_1, pitch_2, pitch_3, pitch_3, pitch_2, pitch_1 };
                    List<double> myListSpace_C1 = new List<double>() { pitch_1_C1, pitch_2_C1, pitch_3_C1, pitch_3_C1, pitch_2_C1, pitch_1_C1 };
                    if (myListFacePicked.Count == 2)
                    {
                        myListSpace = new List<double>() { pitch_1, pitch_2, pitch_1, pitch_2, pitch_2, pitch_1 };
                        myListSpace_C1 = new List<double>() { pitch_1_C1, pitch_2_C1, pitch_1_C1, pitch_2_C1, pitch_2_C1, pitch_1_C1 };
                        caseDistributionRebar = "Case 1: 2 Faces";

                    }
                    TaskDialog.Show("Info", caseDistributionRebar);

                    if (isSt1Yes)
                    {

                        #region Rebar stirrup

                        // List of boundaries faces
                        List<double> myListEndPointDis = getAndSortDisOfEndFaces(myListFacePicked, pE);
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
                        #endregion
                    }


                    if (isYes_C1)
                    {

                        #region Rebar C stirrup

                        // List of boundaries faces
                        List<double> myListEndPointDis_C1 = getAndSortDisOfEndFaces(myListFacePicked, pE);
                        myListEndPointDis_C1.Sort();

                        Dictionary<double, double> myDicDisNumDetail_C1 = detailListDistance_Update_MaxSpace(myListEndPointDis_C1,
                                                                                        factor_C1,
                                                                                        delta_1_C1, pitch_1_C1,
                                                                                        pitch_2_C1,
                                                                                        delta_3_C1, pitch_3_C1, N3_C1);

                        foreach (Rebar myRebar in myListRebar_C1)
                        {
                            List<ElementId> myListRebarCopyId_C1 = copyRebarByDistance_Update_MaxSpace(doc, myRebar, myDicDisNumDetail_C1);


                            List<double> myDistances_C1 = myDicDisNumDetail_C1.Keys.ToList();
                            myDistances_C1.Sort();

                            List<double> myListNum_C1 = new List<double>();

                            foreach (double key in myDistances_C1)
                            {
                                myListNum_C1.Add(myDicDisNumDetail_C1[key]);
                            }


                            // using transcation (edit DB)
                            for (int i = 0; i < myListRebarCopyId_C1.Count(); i++)
                            {
                                using (Transaction myTrans = new Transaction(doc, "CopyElementByCoordinate"))

                                {
                                    myTrans.Start();
                                    ElementId rebarId = myListRebarCopyId_C1[i];
                                    Rebar myRebarI = doc.GetElement(rebarId) as Rebar;

                                    if (i == 1 || i == 4)
                                    {
                                        if (myListNum_C1[i] / myListSpace_C1[i] < 1)
                                        {
                                            myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
                                        }

                                        else
                                        {
                                            myRebarI.GetShapeDrivenAccessor().SetLayoutAsMaximumSpacing(myListSpace_C1[i], (myListNum_C1[i]), isOpposite, false, false);

                                        }
                                    }

                                    else
                                    {
                                        if (myListNum_C1[i] / myListSpace_C1[i] < 1)
                                        {
                                            myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
                                        }

                                        else
                                        {
                                            myRebarI.GetShapeDrivenAccessor().SetLayoutAsMaximumSpacing(myListSpace_C1[i], (myListNum_C1[i]), isOpposite, true, true);
                                        }
                                    }

                                    myTrans.Commit();
                                }

                            }

                        }
                        #endregion
                    }


                    if (isLB1Yes)
                    {
                        testRebar_FromCurve_Bot_Func(doc, myBeam, myListFacePicked, FB1, CB1,
                                                     rebaNameShape_L1, rebaNameType_L1);
                    }
                    if (isLB2Yes)
                    {
                        testRebar_FromCurve_Bot_Func(doc, myBeam, myListFacePicked, FB2, CB2,
                                                     rebaNameShape_L2, rebaNameType_L2);
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





        public void testRebar_FromCurve_Bot_Func(Document doc, Element myBeam, List<Face> myListFacePicked, double divideFac, double coverBar,
                                                string myRebarShapeName, string myRebarTypeName)
        {


            LocationCurve myLocBeam = myBeam.Location as LocationCurve;

            List<XYZ> myEndPoints = getPointCenterBottomLine(doc, myBeam);

            XYZ p1 = myEndPoints[0];
            XYZ p2 = myEndPoints[1];

            XYZ Vp1p2 = p2 - p1;
            double p1p2Length = Vp1p2.GetLength();

            // Cover bar thickness = 25
            //			double coverBar = 25/304.8;

            XYZ p1_Rb = p1 + coverBar / p1p2Length * Vp1p2;

            XYZ p2_Rb = p1 + (1 - coverBar / p1p2Length) * Vp1p2;

            p1_Rb = new XYZ(p1_Rb.X, p1_Rb.Y, p1_Rb.Z + coverBar);
            p2_Rb = new XYZ(p2_Rb.X, p2_Rb.Y, p2_Rb.Z + coverBar);




            Line centerLineBeam = Line.CreateBound(p1_Rb, p2_Rb);

            XYZ v = p2_Rb - p1_Rb;
            XYZ p = p1_Rb - 0.1 * v;
            XYZ v1 = new XYZ(v.Y, -1 * v.X, v.Z);
            XYZ v2 = p2_Rb - p;

            List<double> myListDisSortFace = getAndSortDisOfEndFaces(myListFacePicked, p);

            double lengSeg = myListDisSortFace[myListDisSortFace.Count() - 1] - myListDisSortFace[0];
            //		 	double divideFac = 6;

            XYZ ePR1 = p + ((myListDisSortFace[0] + lengSeg / divideFac) / v2.GetLength()) * v2;
            XYZ ePR2 = p + ((myListDisSortFace[myListDisSortFace.Count() - 1] - lengSeg / divideFac) / v2.GetLength()) * v2;

            Line curveOfRebar = Line.CreateBound(ePR1, ePR2);


            List<Curve> myShape = new List<Curve>() { curveOfRebar };

            // RebarShape
            FilteredElementCollector fecRebarShape = new FilteredElementCollector(doc)
            .OfClass(typeof(RebarShape));

            RebarShape myRebarShape = fecRebarShape.Cast<RebarShape>().
                    First<RebarShape>(myRebarShape2 => myRebarShape2.Name == myRebarShapeName);


            // Rebartype
            FilteredElementCollector fecRebarType = new FilteredElementCollector(doc)
            .OfClass(typeof(RebarBarType));
            RebarBarType myRebarType = fecRebarType.Cast<RebarBarType>().
                    First<RebarBarType>(myRebarType2 => myRebarType2.Name == myRebarTypeName);



            // Hooktype
            FilteredElementCollector fec2 = new FilteredElementCollector(doc)
            .OfClass(typeof(RebarHookType));

            IEnumerable<RebarHookType> iterRebarHookTypes = fec2.Cast<RebarHookType>();

            RebarHookType myRebarHookType = iterRebarHookTypes.First();

            using (Transaction trans = new Transaction(doc, "rebar test"))

            {
                trans.Start();
                //				Rebar myRebar = Rebar.CreateFromCurvesAndShape(doc, myRebarShape, myRebarType,
                //				                                               null, null,
                //				                                               myBeam,v1,
                //				                                               myShape,
                //				                                               RebarHookOrientation.Left,
                //				                                               RebarHookOrientation.Left);
                Rebar myRebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, myRebarType,
                                                       null, null,
                                                       myBeam, v1,
                                                       myShape,
                                                       RebarHookOrientation.Left, RebarHookOrientation.Right,
                                                       true, false);


                trans.Commit();
            }
        }





        public List<XYZ> getPointCenterBottomLine(Document doc, Element myBeam)
        {

            //Get all face of beam
            GeometryElement geometryElement = myBeam.get_Geometry(new Options());


            UV myPoint = new UV(0, 0);
            List<Face> topFaces = new List<Face>();
            List<XYZ> topPoints = new List<XYZ>();

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
                        if (Math.Round(myNormVec.Z, 1) == -1.0 && isBottomFace(doc, myFace, myBeam))
                        {
                            topFaces.Add(myFace);
                            List<XYZ> myListPointOnFace = getAllPointOfFace(myFace);
                            foreach (XYZ myXYZ in myListPointOnFace)
                            {
                                if (!topPoints.Contains(myXYZ))
                                {
                                    topPoints.Add(myXYZ);
                                }
                            }
                        }
                    }
                }
            }

            //get BoundingBox of beam
            BoundingBoxXYZ myBoundBeam = myBeam.get_BoundingBox(null);
            double hBeam = myBoundBeam.Max.Z - myBoundBeam.Min.Z;


            // get startPoint of location line
            LocationCurve myLocBeam = myBeam.Location as LocationCurve;

            Line centerLinebeam = myLocBeam.Curve as Line;

            XYZ p = centerLinebeam.GetEndPoint(0);
            XYZ q = centerLinebeam.GetEndPoint(1);
            XYZ v = q - p;

            // lay tat ca cac diem thuoc face
            Dictionary<double, XYZ> myDic = new Dictionary<double, XYZ>();
            foreach (XYZ myXYZ in topPoints)
            {
                if (!myDic.Keys.ToList().Contains(p.DistanceTo(myXYZ)))
                {
                    myDic.Add(p.DistanceTo(myXYZ), myXYZ);
                }

            }

            List<double> myListDisFromEndPoint = myDic.Keys.ToList();

            myListDisFromEndPoint.Sort();


            XYZ P1 = myDic[myListDisFromEndPoint[0]];
            XYZ P2 = myDic[myListDisFromEndPoint[1]];

            XYZ P3 = myDic[myListDisFromEndPoint[myListDisFromEndPoint.Count - 1]];
            XYZ P4 = myDic[myListDisFromEndPoint[myListDisFromEndPoint.Count - 2]];

            XYZ eP1 = (P1 + P2) / 2;
            XYZ eP2 = (P3 + P4) / 2;

            XYZ vP = eP2 - eP1;


            if (Math.Abs(Math.Round(vP.X * v.Y - vP.Y * v.X, 2)) > 0.001)
            {
                List<XYZ> myListTopPointCenter = getPointCenterTopLine(doc, myBeam);
                XYZ eP1Top = myListTopPointCenter[0];
                XYZ eP2Top = myListTopPointCenter[1];

                XYZ vPTop = eP2Top - eP1Top;

                if (Math.Abs(Math.Round(vPTop.X * v.Y - vPTop.Y * v.X, 2)) > 0.01)
                {
                    TaskDialog.Show("abc", "Mat tren dam di dang, hay ve duong tham chieu cua dam center top.");
                    eP1 = new XYZ(p.X, p.Y, p.Z - hBeam);
                    eP2 = new XYZ(q.X, q.Y, q.Z - hBeam);
                }
                else
                {
                    eP1 = new XYZ(eP1Top.X, eP1Top.Y, P1.Z);
                    eP2 = new XYZ(eP2Top.X, eP2Top.Y, P2.Z);

                }
            }



            List<XYZ> myEndPointOfCenterLine = new List<XYZ>() { eP1, eP2 };



            return myEndPointOfCenterLine;

        }



        private bool isBottomFace(Document doc, Face myFace, Element myBeam)

        {

            BoundingBoxXYZ myBoundBeam = myBeam.get_BoundingBox(null);

            double zMinValue = myBoundBeam.Min.Z;

            if (Math.Abs(zMinValue - getElevetionOfFace(myFace)) < 0.001)
            {
                return true;
            }

            return false;

        }



        private List<XYZ> getAllPointOfFace(Face myFace)
        {
            List<XYZ> myListPointOnFace = new List<XYZ>();

            EdgeArrayArray myEdgeArAr = myFace.EdgeLoops;
            foreach (EdgeArray myEdgeAr in myEdgeArAr)
            {
                foreach (Edge myEdge in myEdgeAr)
                {
                    Curve myCurve = myEdge.AsCurve();
                    XYZ eP1 = new XYZ(Math.Round(myCurve.GetEndPoint(0).X, 6),
                                       Math.Round(myCurve.GetEndPoint(0).Y, 6),
                                       Math.Round(myCurve.GetEndPoint(0).Z, 6));

                    XYZ eP2 = new XYZ(Math.Round(myCurve.GetEndPoint(1).X, 6),
                                                  Math.Round(myCurve.GetEndPoint(1).Y, 6),
                                                  Math.Round(myCurve.GetEndPoint(1).Z, 6));

                    myListPointOnFace.Add(eP1);
                    myListPointOnFace.Add(eP2);
                }
            }
            return myListPointOnFace;
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



        public List<XYZ> getPointCenterTopLine(Document doc, Element myBeam)
        {

            //Get all face of beam
            GeometryElement geometryElement = myBeam.get_Geometry(new Options());


            UV myPoint = new UV(0, 0);
            List<Face> topFaces = new List<Face>();
            List<XYZ> topPoints = new List<XYZ>();

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
                        if (Math.Round(myNormVec.Z, 1) == 1.0 && isTopFace(doc, myFace, myBeam))
                        {
                            topFaces.Add(myFace);
                            List<XYZ> myListPointOnFace = getAllPointOfFace(myFace);
                            foreach (XYZ myXYZ in myListPointOnFace)
                            {
                                if (!topPoints.Contains(myXYZ))
                                {
                                    topPoints.Add(myXYZ);
                                }
                            }
                        }
                    }
                }
            }


            //get BoundingBox of beam
            BoundingBoxXYZ myBoundBeam = myBeam.get_BoundingBox(null);
            double hBeam = myBoundBeam.Max.Z - myBoundBeam.Min.Z;


            // get startPoint of location line
            LocationCurve myLocBeam = myBeam.Location as LocationCurve;

            Line centerLinebeam = myLocBeam.Curve as Line;

            XYZ p = centerLinebeam.GetEndPoint(0);
            XYZ q = centerLinebeam.GetEndPoint(1);
            XYZ v = q - p;

            // lay tat ca cac diem thuoc face
            Dictionary<double, XYZ> myDic = new Dictionary<double, XYZ>();
            foreach (XYZ myXYZ in topPoints)
            {
                if (!myDic.Keys.ToList().Contains(p.DistanceTo(myXYZ)))
                {
                    myDic.Add(p.DistanceTo(myXYZ), myXYZ);
                }

            }

            List<double> myListDisFromEndPoint = myDic.Keys.ToList();

            myListDisFromEndPoint.Sort();


            XYZ P1 = myDic[myListDisFromEndPoint[0]];
            XYZ P2 = myDic[myListDisFromEndPoint[1]];

            XYZ P3 = myDic[myListDisFromEndPoint[myListDisFromEndPoint.Count - 1]];
            XYZ P4 = myDic[myListDisFromEndPoint[myListDisFromEndPoint.Count - 2]];

            XYZ eP1 = (P1 + P2) / 2;
            XYZ eP2 = (P3 + P4) / 2;

            XYZ vP = eP2 - eP1;


            if (Math.Abs(Math.Round(vP.X * v.Y - vP.Y * v.X, 2)) > 0.001)
            {
                TaskDialog.Show("abc", "Mat tren dam di dang, hay ve duong tham chieu cua dam center top.");
                eP1 = p;
                eP2 = q;

            }



            List<XYZ> myEndPointOfCenterLine = new List<XYZ>() { eP1, eP2 };



            return myEndPointOfCenterLine;

        }



        private bool isTopFace(Document doc, Face myFace, Element myBeam)

        {


            BoundingBoxXYZ myBoundBeam = myBeam.get_BoundingBox(null);

            double zMaxValue = myBoundBeam.Max.Z;

            if (Math.Abs(zMaxValue - getElevetionOfFace(myFace)) < 0.001)
            {
                return true;
            }

            return false;

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
