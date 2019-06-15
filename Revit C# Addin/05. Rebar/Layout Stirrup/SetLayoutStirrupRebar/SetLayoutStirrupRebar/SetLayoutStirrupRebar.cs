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



        // Select Faces boundaries

        public void rebarBeam_Form_Update()
        {
            UIDocument uiDoc = this.ActiveUIDocument;
            Document doc = uiDoc.Document;

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
                using (var myInputFormSetting = new InputDialog())
                {
                    myInputFormSetting.ShowDialog();

                    factor = Convert.ToDouble(myInputFormSetting.factorTb.Text);
                    delta_1 = Convert.ToDouble(myInputFormSetting.delta_1Tb.Text) / myConvertFactor;
                    pitch_1 = Convert.ToDouble(myInputFormSetting.pitch_1Tb.Text) / myConvertFactor;
                    pitch_2 = Convert.ToDouble(myInputFormSetting.pitch_2Tb.Text) / myConvertFactor;

                    delta_3 = Convert.ToDouble(myInputFormSetting.delta_3Tb.Text) / myConvertFactor;
                    pitch_3 = Convert.ToDouble(myInputFormSetting.pitch_3Tb.Text) / myConvertFactor;
                    N3 = Convert.ToInt32(myInputFormSetting.n3Tb.Text);


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
            Reference myRefRebar = uiDoc.Selection.PickObject(ObjectType.Element,
                                                              new FilterByIdCategory(myListIdCategoryRebar),
                                                              "Pick a Rebar...");
            //Get rebar from ref
            Rebar myRebar = doc.GetElement(myRefRebar) as Rebar;

            //Set rebar single
            //			using (Transaction myTrans = new Transaction(doc,"SET FIRST REBAR AS SINGLE"))
            //			
            //			{
            //				myTrans.Start();
            //				myRebar.GetShapeDrivenAccessor().SetLayoutAsSingle();
            //				myTrans.Commit();
            //			}


            Element myBeam = doc.GetElement(myRebar.GetHostId());

            //Get location curve of beam
            LocationCurve lc = myBeam.Location as LocationCurve;
            Line line = lc.Curve as Line;

            //Get vector of location cuver beam
            XYZ p1 = line.GetEndPoint(0);
            XYZ q = line.GetEndPoint(1);
            XYZ v = q - p1; // Vector equation of line

            XYZ p = p1 - 0.1 * v;

            //Set current Beam be Joined

            setBeJoined(myBeam);

            while (true)
            {


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

                if (myListFacePicked.Count != 2 && myListFacePicked.Count != 4)
                {
                    TaskDialog.Show("Error!", "Chua ho tro lua chon: " + myListFacePicked.Count() + " mat, Chon 2 hoac 4 mat");
                    continue;

                }

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



                    Dictionary<double, int> myDicDisNumDetail = detailListDistance_Update(myListEndPointDis,
                                                                                    factor,
                                                                                    delta_1, pitch_1,
                                                                                    pitch_2,
                                                                                    delta_3, pitch_3, N3);


                    List<ElementId> myListRebarCopyId = copyRebarByDistance2_Update(myRebar, myDicDisNumDetail);


                    List<double> myDistances = myDicDisNumDetail.Keys.ToList();
                    myDistances.Sort();



                    List<int> myListNum = new List<int>();

                    foreach (double key in myDistances)
                    {
                        myListNum.Add(myDicDisNumDetail[key]);
                    }

                    //Layout

                    // using transcation (edit DB)
                    for (int i = 0; i < myListRebarCopyId.Count(); i++)
                    {
                        using (Transaction myTrans = new Transaction(doc, "CopyElementByCoordinate"))

                        {
                            myTrans.Start();
                            ElementId rebarId = myListRebarCopyId[i];
                            Rebar myRebarI = doc.GetElement(rebarId) as Rebar;

                            if (myListNum[i] < -1)
                            {
                                myRebarI.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing((myListNum[i]) * -1, myListSpace[i], true, true, true);
                            }

                            if (myListNum[i] == -1)
                            {
                                myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();

                            }

                            if (myListNum[i] == 0)
                            {
                                myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();

                            }

                            if (myListNum[i] == 1)
                            {
                                myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();

                            }

                            if (myListNum[i] > 1)
                            {
                                myRebarI.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing(myListNum[i], myListSpace[i], false, true, true);

                            }
                            myTrans.Commit();
                        }
                    }

                    //delete element
                    using (Transaction myTrans = new Transaction(doc, "Delete First ReBar"))

                    {
                        myTrans.Start();
                        //doc.Delete(myRebar.Id);
                        myTrans.Commit();
                    }
                }

            }
        }




        public void rebarStirrupLayout_Form(UIDocument uiDoc)
        {
            Document doc = uiDoc.Document;

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
            Reference myRefRebar = uiDoc.Selection.PickObject(ObjectType.Element,
                                                              new FilterByIdCategory(myListIdCategoryRebar),
                                                              "Pick a Rebar...");
            //Get rebar from ref
            Rebar myRebar = doc.GetElement(myRefRebar) as Rebar;

            //Set rebar single

            using (Transaction myTrans = new Transaction(doc, "SET FIRST REBAR AS SINGLE"))

            {
                myTrans.Start();
                myRebar.GetShapeDrivenAccessor().SetLayoutAsSingle();
                myTrans.Commit();
            }


            Element myBeam = doc.GetElement(myRebar.GetHostId());

            //Set current Beam be Joined

            setBeJoined(doc, myBeam);

            while (true)
            {

                List<int> myListBeamCol = new List<int>();
                myListBeamCol.Add((int)BuiltInCategory.OST_StructuralFraming);
                myListBeamCol.Add((int)BuiltInCategory.OST_StructuralColumns);
                // Select first Element (ex beam)
                List<Reference> myListInterRef = uiDoc.Selection.PickObjects(ObjectType.Element,
                                                                  new FilterByIdCategory(myListBeamCol),
                                                                  "Pick a Beam and Col...") as List<Reference>;

                List<Element> myInterSec = new List<Element>();


                foreach (Reference myRef in myListInterRef)
                {
                    myInterSec.Add(doc.GetElement(myRef));
                }

                // Kiem tra co 2 cot ko?
                int numCol = 0;

                foreach (Element myE in myInterSec)
                {
                    if (myE.Category.Name == "Structural Columns")
                    {
                        numCol += 1;
                    }
                }

                if (numCol != 2 || myInterSec.Count > 3)
                {
                    TaskDialog.Show("Erorr!!!", "Các đối tượng được chọn phải có đủ 2 cột và tối đa 1 dầm");
                    //return;
                }

                // Kiem tra truong hop tinh
                if (myInterSec.Count() == 2)
                {
                    rebarBeam2Col(doc, factor, delta_1, pitch_1, pitch_2, myRebar, myInterSec);

                }

                else
                {
                    List<double> myListEndPointDis = getEndsSegBeam2(doc, myBeam.Id, myInterSec);
                    myListEndPointDis.Sort();


                    Dictionary<double, int> myDicDisNumDetail = detailListDistance2(myListEndPointDis,
                                                                                    factor,
                                                                                    delta_1, pitch_1,
                                                                                    pitch_2,
                                                                                    delta_3, pitch_3, N3);


                    List<double> myListSpace = new List<double>() { pitch_1, pitch_2, pitch_3, pitch_3, pitch_2, pitch_1 };

                    List<ElementId> myListRebarCopyId = copyRebarByDistance2(doc, myRebar, myDicDisNumDetail);


                    List<double> myDistances = myDicDisNumDetail.Keys.ToList();
                    myDistances.Sort();



                    List<int> myListNum = new List<int>();

                    foreach (double key in myDistances)
                    {
                        myListNum.Add(myDicDisNumDetail[key]);
                    }

                    //Layout

                    // using transcation (edit DB)
                    for (int i = 0; i < myListRebarCopyId.Count(); i++)
                    {
                        using (Transaction myTrans = new Transaction(doc, "CopyElementByCoordinate"))

                        {
                            myTrans.Start();
                            ElementId rebarId = myListRebarCopyId[i];
                            Rebar myRebarI = doc.GetElement(rebarId) as Rebar;

                            if (myListNum[i] < -1)
                            {
                                myRebarI.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing((myListNum[i]) * -1, myListSpace[i], true, true, true);
                            }

                            if (myListNum[i] == -1)
                            {
                                myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();

                            }

                            if (myListNum[i] == 0)
                            {
                                myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();

                            }

                            if (myListNum[i] == 1)
                            {
                                myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();

                            }

                            if (myListNum[i] > 1)
                            {
                                myRebarI.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing(myListNum[i], myListSpace[i], false, true, true);

                            }
                            myTrans.Commit();
                        }
                    }

                    //delete element
                    using (Transaction myTrans = new Transaction(doc, "Delete First ReBar"))

                    {
                        myTrans.Start();
                        //doc.Delete(myRebar.Id);
                        myTrans.Commit();
                    }
                }

            }
        }


        public void rebarBeam2Col(Document doc, double factor, double delta_1, double pitch_1, double pitch_2, Rebar myRebar, List<Element> myInterSec)
        {
            //UIDocument uiDoc = this.ActiveUIDocument;
            //Document doc = uiDoc.Document;


            // Get baem Id
            ElementId myBeamId = myRebar.GetHostId();

            double lenSegment;
            List<double> myListDis = new List<double>();


            ElementId myCol1Id = myInterSec[0].Id;
            ElementId myCol2Id = myInterSec[1].Id;


            myListDis = getEndsSegBeam(doc, myBeamId, myCol1Id, myCol2Id);
            lenSegment = myListDis[1] - myListDis[0];



            List<double> myListDisDetail = detailListDistance(myListDis, factor, delta_1, pitch_1);


            List<ElementId> myListRebarCopyId = copyRebarByDistance(doc, myRebar, myListDisDetail);

            // RebarSet Layout


            using (Transaction trans = new Transaction(doc, "Change rebar set"))
            {
                trans.Start();

                for (int i = 0; i < myListRebarCopyId.Count; i++)
                {
                    ElementId rebarId = myListRebarCopyId[i];

                    Rebar myRebarI = doc.GetElement(rebarId) as Rebar;

                    int numberRebar = 2;

                    if (i == myListRebarCopyId.Count - 1)
                    {
                        numberRebar = (int)((lenSegment / factor) / pitch_1);

                        myRebarI.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing(numberRebar, pitch_1, true, true, true);
                    }

                    else if (i == 0)
                    {
                        numberRebar = (int)((lenSegment / factor) / pitch_1);
                        myRebarI.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing(numberRebar, pitch_1, false, true, true);
                    }

                    else if (i == 1)
                    {

                        double delta_2 = (lenSegment / factor - delta_1) % pitch_1;
                        double len2 = (lenSegment - 2 * (lenSegment / factor) + delta_2);

                        numberRebar = (int)((lenSegment - 2 * (lenSegment / factor) + delta_2) / pitch_2) + 1;
                        myRebarI.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing(numberRebar, pitch_2, false, true, true);
                    }

                }

                trans.Commit();
            }


        }

        //Tra ve 2 dic gom khoang cach offset va so thanh layout tuong ung 
        private Dictionary<double, int> detailListDistance2(List<double> endPoiList, double factorDivide,
                                                 double delta_1, double pitch_1,
                                                  double pitch_2,
                                                 double delta_3, double pitch_3, int N3)
        {
            Dictionary<double, int> myReturnDic = new Dictionary<double, int>();
            if (endPoiList.Count != 4)
            {
                return myReturnDic;
            }

            else
            {
                // Always positive
                double lengthSeg = endPoiList[endPoiList.Count() - 1] - endPoiList[0];

                // Diem dau doan 1
                double X1 = endPoiList[0] + delta_1;
                int N1 = (int)((lengthSeg / factorDivide) / pitch_1);
                myReturnDic.Add(X1, N1);

                // Diem dau doan 2
                double X2 = endPoiList[0] + lengthSeg / factorDivide;
                int N2 = (int)((endPoiList[1] - delta_3 - (N3 - 1) * pitch_3 - (lengthSeg / factorDivide) - endPoiList[0]) / pitch_2) + 1;
                myReturnDic.Add(X2, N2);

                // Diem dau doan 3-dau dam // number nguoc
                double X3 = endPoiList[1] - delta_3;
                myReturnDic.Add(X3, -1 * N3);

                // Diem dau doan 4-dau dam // number xuoi
                double X4 = endPoiList[2] + delta_3;
                myReturnDic.Add(X4, 1 * N3);


                // Diem dau doan 5 dau dam // number nghich dao
                double X5 = endPoiList[3] - lengthSeg / factorDivide;
                int N5 = (int)((endPoiList[3] - (lengthSeg / factorDivide) - (N3 - 1) * pitch_3 - delta_3 - endPoiList[2]) / pitch_2) + 1;
                myReturnDic.Add(X5, -1 * N5);


                // Diem dau doan 6 dau dam // number nghich dao
                double X6 = endPoiList[3] - delta_1;
                int N6 = (int)((lengthSeg / factorDivide) / pitch_1);
                myReturnDic.Add(X6, -1 * N6);


                return myReturnDic;

                //				return new Dictionary<double, int>(){{1.5,5}};
            }

        }


        private List<ElementId> copyRebarByDistance2(Document doc, Rebar myRebar, Dictionary<double, int> myDicDisNum)
        {

            List<double> myDistances = myDicDisNum.Keys.ToList();
            myDistances.Sort();

            List<int> myListNum = new List<int>();

            foreach (double key in myDistances)
            {
                myListNum.Add(myDicDisNum[key]);
            }

            //UIDocument uiDoc = this.ActiveUIDocument;
            //Document doc = uiDoc.Document;

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

                XYZ q = lineCur.GetEndPoint(0);
                XYZ p = lineCur.GetEndPoint(1);
                XYZ v = p - q;
                double lengCurLine = v.GetLength();


                List<XYZ> myCoors = new List<XYZ>();


                double delta0 = getDelta_0(doc, myRebar);

                if (delta0 == 10000000) return null;

                foreach (double distance in myDistances)
                {

                    XYZ myPointPlace = ((distance - delta0) / lengCurLine) * v;
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


        /// <summary>
        /// Pick 1 dam, 2 cot tra ve khoang cach giua 2 mat gan nhat cua cot
        /// </summary>
        /// <returns></returns>
        private List<double> getEndsSegBeam(Document doc, ElementId myBeamId, ElementId myCol1Id, ElementId myCol2Id)
        {
            //UIDocument uiDoc = this.ActiveUIDocument;
            //Document doc = uiDoc.Document;

            Element myBeam = doc.GetElement(myBeamId);

            Element myCol1 = doc.GetElement(myCol1Id);


            //Get element1 from ref
            Element myCol2 = doc.GetElement(myCol2Id);


            // List Distance
            List<double> myDistFaceFromEndPoint = new List<double>();


            using (Transaction trans2 = new Transaction(doc, "Get Distance From face to face"))
            {
                trans2.Start();

                //  Faces same direction with lc of beam
                IList<Face> facesSameDirection = new List<Face>();

                //Get location curve of beam
                LocationCurve lc = myBeam.Location as LocationCurve;
                Line line = lc.Curve as Line;

                //Get vector of location cuver beam
                XYZ p = line.GetEndPoint(0);
                XYZ q = line.GetEndPoint(1);
                XYZ v = q - p; // Vector equation of line

                //get Bounding Box

                XYZ checkPoint = new XYZ(p.X, p.Y, myBeam.get_BoundingBox(null).Min.Z - 0.01);


                // List Column Element
                List<Element> myColumns = new List<Element>() { myCol1, myCol2 };

                // For each col, get all face of col has same direction
                foreach (Element myCol in myColumns)
                {

                    GeometryElement myColGeos = myCol.get_Geometry(new Options());

                    foreach (GeometryObject myColGeo in myColGeos)
                    {
                        if (myColGeo is Solid)
                        {
                            Solid myColSolid = myColGeo as Solid;

                            // Get all face of solid of geometry
                            foreach (Face myColFace in myColSolid.Faces)
                            {
                                XYZ myNormVec = myColFace.ComputeNormal(new UV(0, 0));
                                if (isSameDirection(v, myNormVec)) // myE.Category.Name == "Structural Columns")
                                {
                                    facesSameDirection.Add(myColFace);

                                    IntersectionResult myInterSect = myColFace.Project(checkPoint);

                                    // NOTE: ROUND DISTANCE
                                    double myDisPointToFace = Math.Round(myInterSect.Distance, 6);

                                    myDistFaceFromEndPoint.Add(myDisPointToFace);
                                }
                            }
                        }
                    }
                }

                trans2.Commit();
            }

            if (myDistFaceFromEndPoint.Count() < 1)

            {
                TaskDialog.Show("abc", "Point face out");

            }

            myDistFaceFromEndPoint.Sort();

            //List<double> myReturnList = myDistFaceFromEndPoint.GetRange(1,2);
            List<double> myReturnList = new List<double>() { myDistFaceFromEndPoint[1], myDistFaceFromEndPoint[2] };

            //TaskDialog.Show("RESULT", "Dis1: " + myReturnList[0] + Environment.NewLine + "Dis2: " + myReturnList[1]);

            return myReturnList;
        }



        /// <summary>
        /// Pick 1 dam, 2 cot 
        /// </summary>
        /// <returns></returns>
        private List<double> getEndsSegBeam2(Document doc, ElementId myBeamId, List<Element> myIntersectWithBeam)
        {
            //UIDocument uiDoc = this.ActiveUIDocument;
            //Document doc = uiDoc.Document;


            Element myBeam = doc.GetElement(myBeamId);


            // List Distance
            List<double> myDistFaceFromEndPoint = new List<double>();


            using (Transaction trans2 = new Transaction(doc, "Get Distance From face to face"))
            {
                trans2.Start();

                //  Faces same direction with lc of beam
                IList<Face> facesSameDirection = new List<Face>();

                //Get location curve of beam
                LocationCurve lc = myBeam.Location as LocationCurve;
                Line line = lc.Curve as Line;

                //Get vector of location cuver beam
                XYZ p = line.GetEndPoint(0);
                XYZ q = line.GetEndPoint(1);
                XYZ v = q - p; // Vector equation of line

                //get Bounding Box
                XYZ checkPointBeam = new XYZ(p.X, p.Y, myBeam.get_BoundingBox(null).Max.Z - 0.01);
                XYZ checkPointCol = new XYZ(p.X, p.Y, myBeam.get_BoundingBox(null).Min.Z - 0.01);


                // List Column Element
                List<Element> myColumns = myIntersectWithBeam;

                // For each col, get all face of col has same direction
                foreach (Element myInterElem in myIntersectWithBeam)
                {

                    GeometryElement myInterElemGeos = myInterElem.get_Geometry(new Options());

                    foreach (GeometryObject myInterElemGeo in myInterElemGeos)
                    {
                        if (myInterElemGeo is Solid)
                        {
                            Solid myInterElemSolid = myInterElemGeo as Solid;

                            // Get all face of solid of geometry
                            foreach (Face myInterElemFace in myInterElemSolid.Faces)
                            {
                                XYZ myNormVec = myInterElemFace.ComputeNormal(new UV(0, 0));
                                if (isSameDirection(v, myNormVec)) // myE.Category.Name == "Structural Columns")
                                {
                                    facesSameDirection.Add(myInterElemFace);
                                    IntersectionResult myInterSect;
                                    if (myInterElem.Category.Name == "Structural Framing")
                                    {
                                        myInterSect = myInterElemFace.Project(checkPointBeam);
                                    }

                                    else
                                    {
                                        myInterSect = myInterElemFace.Project(checkPointCol);
                                    }


                                    // NOTE: ROUND DISTANCE
                                    double myDisPointToFace = Math.Round(myInterSect.Distance, 6);

                                    //if(!myDistFaceFromEndPoint.Contains(myDisPointToFace))
                                    //{
                                    myDistFaceFromEndPoint.Add(myDisPointToFace);
                                    //}


                                }
                            }
                        }
                    }
                }

                trans2.Commit();
            }

            if (myDistFaceFromEndPoint.Count() < 1)

            {
                TaskDialog.Show("abc", "Point face out");

            }

            myDistFaceFromEndPoint.Sort();


            //List<double> myReturnList = myDistFaceFromEndPoint.GetRange(1,2);
            List<double> myReturnList = myDistFaceFromEndPoint.GetRange(1, myDistFaceFromEndPoint.Count() - 2);


            string outPut = "Dis: ";
            foreach (double myDist in myReturnList)
            {
                outPut += Environment.NewLine + myDist.ToString() + ",";

            }

            //TaskDialog.Show("RESULT", outPut);
            //TaskDialog.Show("Result", "Length SegmentBeam" + (myReturnList[myReturnList.Count() - 1] - myReturnList[0]));

            return myReturnList;
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


        private List<double> detailListDistance(List<double> endPoiList, double factorDivide, double delta_1, double pitch_1)
        {


            if (endPoiList.Count != 2)
            {
                return null;
            }

            else
            {
                // Always positive
                double lengthSeg = endPoiList[1] - endPoiList[0];

                // Diem dau doan 1
                double X1 = endPoiList[0] + delta_1;

                // Diem dau doan 2
                double X2 = endPoiList[0] + lengthSeg / factorDivide;

                // Diem dau doan 3
                double deltaSeg3 = (lengthSeg / factorDivide - delta_1) % pitch_1;

                double X3 = endPoiList[0] + lengthSeg - delta_1;

                return new List<double>() { X1, X2, X3 };
            }

        }


        private List<ElementId> copyRebarByDistance(Document doc, Rebar myRebar, List<double> myDistances)
        {
            //UIDocument uiDoc = this.ActiveUIDocument;
            //Document doc = uiDoc.Document;

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

                XYZ q = lineCur.GetEndPoint(0);
                XYZ p = lineCur.GetEndPoint(1);
                XYZ v = p - q;
                double lengCurLine = v.GetLength();


                List<XYZ> myCoors = new List<XYZ>();


                double delta0 = getDelta_0(doc, myRebar);

                if (delta0 == 10000000) return null;

                foreach (double distance in myDistances)
                {

                    XYZ myPointPlace = ((distance - delta0) / lengCurLine) * v;
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


        private double getDelta_0(Document doc, Rebar myRebar)
        {
            //UIDocument uiDoc = this.ActiveUIDocument;
            //Document doc = uiDoc.Document;

            //Get diameter of rebar

            RebarBarType myRbType = doc.GetElement(myRebar.GetTypeId()) as RebarBarType;
            double myRebarDiameter = myRbType.BarDiameter;

            // Get host of rebar

            ElementId myBeamId = myRebar.GetHostId();

            Element myBeam = doc.GetElement(myBeamId);

            if (myBeam.Category.Name != "Structural Framing")
            {
                TaskDialog.Show("Loi!", "Hay chon 1 rebar co host la 1 Structural Framing");
                return 10000000;
            }

            else
            {

                //Get location curve of beam
                LocationCurve lc = myBeam.Location as LocationCurve;
                Line line = lc.Curve as Line;

                //Get vector of location cuver beam
                XYZ p = line.GetEndPoint(0);
                XYZ q = line.GetEndPoint(1);
                XYZ v = q - p; // Vector equation of line

                XYZ middlePoint = myRebar.get_BoundingBox(null).Max.Add(myRebar.get_BoundingBox(null).Min) / 2;

                // NOTE LAM TRON SO
                //			    middlePoint = new XYZ(middlePoint.X,middlePoint.Y), 
                //			                          Math.Round(middlePoint.Z,1));
                double delta_0 = Math.Sqrt(Math.Pow(middlePoint.X - p.X, 2) + Math.Pow(middlePoint.Y - p.Y, 2)) - myRebarDiameter / 2;

                return delta_0;
            }

        }

        // return bool 2 vector (x,y) same direction, Only in Oxy, ignore Z axis
        private bool isSameDirection(XYZ vec1, XYZ vec2)
        {
            double X1 = Math.Round(vec1.X, 9);
            double Y1 = Math.Round(vec1.Y, 9);

            double X2 = Math.Round(vec2.X, 9);
            double Y2 = Math.Round(vec2.Y, 9);

            if ((X1 == Y1 && X1 == 0) || (X2 == Y2 && X2 == 0))
            {
                return false;
            }

            if (Math.Round(X1 * Y2, 4) == Math.Round(X2 * Y1, 4))
            {
                return true;
            }
            else
            {
                return false;
            }

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
