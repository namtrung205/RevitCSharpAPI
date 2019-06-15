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

namespace AutoMakeLiningConcrete_Form
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class AutoMakeLiningConcrete_Form : IExternalCommand
    {
        // Set GUID [Guid("EBD72940-FD47-4082-B53B-1BF13937F4CB")]
        static AddInId appId = new AddInId(new Guid("EBD72940-FD47-4082-B53B-1BF13937F4CB"));

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
            createLiningConcrete(commandData.Application.ActiveUIDocument);
            return Autodesk.Revit.UI.Result.Succeeded;
        }



        public void createLiningConcrete( UIDocument uiDoc)
        {
            // Set doc
            Document doc = uiDoc.Document;

            //Show Form

            SelectFamilyNameForms mySelFamilyForm = new SelectFamilyNameForms();




            FilteredElementCollector fec = new FilteredElementCollector(doc)
                .OfClass(typeof(FloorType))
                .OfCategory(BuiltInCategory.OST_Floors);


            IEnumerable<FloorType> iterFloorTypes = fec.Cast<FloorType>();

            // Lay danh sach vat lieu
            foreach (FloorType myItemType in iterFloorTypes)
            {
                mySelFamilyForm.FamilysCb.Items.Add(myItemType.Name.ToString());

                //mySelMatForm.listMatCb.Items.Add(myItemType.Name.ToString());
            }
            double offsetVal = 100;
            try
            {
                offsetVal = Convert.ToDouble(valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "LiningConcreteOffset"));
            }

            catch
            {
                mySelFamilyForm.offsetDisTb.Text = "100";

            }

            mySelFamilyForm.offsetDisTb.Text = offsetVal.ToString();


            int indexSel = mySelFamilyForm.FamilysCb.Items.IndexOf(valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "LiningConcreteFamily"));

            mySelFamilyForm.FamilysCb.SelectedIndex = indexSel;
            mySelFamilyForm.ShowDialog();

            string nameSelectedFamily = mySelFamilyForm.FamilysCb.SelectedItem.ToString();
            offsetVal = Convert.ToDouble(mySelFamilyForm.offsetDisTb.Text);

            // Get the element selection of current document.
            Selection selection = uiDoc.Selection;
            ICollection<ElementId> selectedIds = uiDoc.Selection.GetElementIds();

            if (selectedIds.Count < 1)
            {
                //filter element
                List<int> myListCatoId = new List<int>();
                myListCatoId.Add((int)BuiltInCategory.OST_StructuralFoundation);
                myListCatoId.Add((int)BuiltInCategory.OST_StructuralFraming);

                List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, new FilterByIdCategory(myListCatoId), "Select Beam or Slab") as List<Reference>;

                if (myListRef.Count() < 1)
                {
                    return;
                }
                else
                {
                    foreach (Reference myRef in myListRef)
                    {
                        selectedIds.Add(doc.GetElement(myRef).Id);
                    }
                }
            }

            bool isLiningBottomBeam = false;
            foreach (ElementId myFoundationId in selectedIds)
            {
                //				//Get ElementId from ref
                //				ElementId myFoundationId = doc.GetElement(myRef).Id;
                Element myCurrentFoundationAs = doc.GetElement(myFoundationId);

                if (myCurrentFoundationAs.Category.Name == "Structural Framing")
                {
                    isLiningBottomBeam = true;
                }

                else
                {
                    isLiningBottomBeam = false;
                }

                List<List<Floor>> myListListFloor = createLiningConcreteAsFloor2(doc, myFoundationId,
                                                                            nameSelectedFamily,
                                                                            offsetVal/304.8, isLiningBottomBeam);

                foreach (Floor myFloorLining in myListListFloor[0])
                {
                    switchJoinOrder(doc, myFloorLining);
                }

                foreach (Floor myFloorLiningCutting in myListListFloor[1])
                {
                    joiningLining(doc, myFloorLiningCutting);
                }

            }

            mySelFamilyForm.Close();

        }



        private string valueOfSetting(string pathSettingFile, string settingName)

        {

            // Open file setting
            try
            {
                if (File.Exists(pathSettingFile))
                {
                    //Read all Line in file
                    string[] myFullSetting = File.ReadAllLines(pathSettingFile);
                    //Create a Dictionay wiht key and 
                    string[] mySettingList;
                    foreach (string pairSetting in myFullSetting)
                    {
                        // if  satisfy conditions, add to Dic{List[0]: List[1],...}

                        if (pairSetting.Count(f => f == '|') == 1)
                        {
                            // Split line to list
                            mySettingList = pairSetting.Split('|');
                            // Add to Dic
                            if (mySettingList[0] == settingName)
                            {
                                return mySettingList[1];
                            }

                            continue;
                        }
                        continue;
                    }
                    return "";
                }
                else
                {
                    return "";
                }

            }
            catch (Exception e)
            {

                TaskDialog.Show("Error", e.Message);
                return "";
            }


        }



        private List<List<Floor>> createLiningConcreteAsFloor2(Document doc, ElementId myFoundtionId, string nameFamily, double offsetValue, bool isCutting)

        {

            Element myFoundation = doc.GetElement(myFoundtionId) as Element;

            //Get level from elemet
            Level myLevel = doc.GetElement(myFoundation.LevelId) as Level;

            //Get geometry from element
            GeometryElement geometryElement = myFoundation.get_Geometry(new Options());

            //Get list Of face (with normal vector = xyz(0,0,-1);

            List<Face> myListBottomFace = new List<Face>();

            using (Transaction myTrans = new Transaction(doc, "fil face of foundation"))
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

                            // If normal vector of face has Z value == -1 add to list
                            if (Math.Round(myNormVec.Z, 1) == -1.0)
                            {
                                myListBottomFace.Add(myFace);
                            }
                        }
                    }
                }
                myTrans.Commit();
            }

            // Now We has a list of face (with normal vector = (0,0,-1)

            //Save floor to a list

            List<Floor> myListLining = new List<Floor>();

            // List Floor cutting
            List<Floor> myListLiningCutting = new List<Floor>();


            using (Transaction trans = new Transaction(doc, "abc"))
            {
                trans.Start();
                foreach (Face myPickedFace in myListBottomFace)
                {
                    //Get Nomarl vector
                    XYZ myNorVecFace = myPickedFace.ComputeNormal(new UV(0, 0));
                    List<CurveLoop> myListCurvefromFace = myPickedFace.GetEdgesAsCurveLoops() as List<CurveLoop>;


                    CurveArray myBoundaFloor = new CurveArray();

                    foreach (CurveLoop myCurLoop in myListCurvefromFace)
                    {

                        if (myFoundation.Category.Name != "Structural Framing")
                        {
                            // Offset For Slab
                            CurveLoop curOffset = CurveLoop.CreateViaOffset(myCurLoop, offsetValue, myNorVecFace);
                            //TaskDialog.Show("abc", "xyz: " +curOffset.GetPlane().Normal.ToString());

                            foreach (Curve myCur in curOffset)
                            {
                                myBoundaFloor.Append(myCur);
                            }
                        }

                        else
                        {
                            List<double> myOffsetDist = getOffsetDis(myCurLoop, offsetValue);


                            CurveLoop curOffset = CurveLoop.CreateViaOffset(myCurLoop, myOffsetDist, myNorVecFace);
                            //TaskDialog.Show("abc", "xyz: " +curOffset.GetPlane().Normal.ToString());

                            foreach (Curve myCur in curOffset)
                            {
                                myBoundaFloor.Append(myCur);
                            }


                        }

                    }

                    FloorType floorType
                      = new FilteredElementCollector(doc)
                        .OfClass(typeof(FloorType))
                        .OfCategory(BuiltInCategory.OST_Floors)
                        .First<Element>(f => f.Name.Equals(nameFamily)) as FloorType;

                    //Floor myLining =  doc.Create.NewFoundationSlab(myBoundaFloor, floorType, myLevel, true, new XYZ(0,0,1));

                    //Floor myLining =  doc.Create.NewFloor(myBoundaFloor, floorType, myLevel, true, new XYZ(0,0,1));

                    Floor myLining = doc.Create.NewFloor(myBoundaFloor, floorType, myLevel, true, new XYZ(0, 0, 1));


                    // Cutting if foundation is beam

                    if (isCutting)
                    {
                        myListLiningCutting.Add(myLining);

                    }

                    myListLining.Add(myLining);
                }
                trans.Commit();
            }

            // Switch Joint
            List<List<Floor>> myListListFloor = new List<List<Floor>>() { myListLining, myListLiningCutting };
            return myListListFloor;

        }



        private List<double> getOffsetDis(CurveLoop myBeamCur, double offsetDist)
        {
            List<double> myListLength = new List<double>();
            // Lay danh sach do dai curve
            foreach (Curve currentCur in myBeamCur)
            {
                myListLength.Add(currentCur.Length);
            }

            myListLength.Sort();

            List<double> myOffsetDist = new List<double>();

            foreach (Curve currentCur in myBeamCur)
            {
                if (currentCur.Length == myListLength[myListLength.Count() - 1] ||
                   currentCur.Length == myListLength[myListLength.Count() - 2])
                {
                    myOffsetDist.Add(offsetDist);

                }
                else
                {
                    myOffsetDist.Add(-0.000001);

                }
            }

            return myOffsetDist;

        }



        private void switchJoinOrder(Document doc, Element myLining)
        {

            ICollection<ElementId> myListElemIdsJoined = JoinGeometryUtils.GetJoinedElements(doc, myLining);

            using (Transaction trans = new Transaction(doc, "Switch Join"))
            {
                trans.Start();
                foreach (ElementId myElemId in myListElemIdsJoined)
                {

                    if (!JoinGeometryUtils.IsCuttingElementInJoin(doc, doc.GetElement(myElemId), myLining))
                    {
                        JoinGeometryUtils.SwitchJoinOrder(doc, doc.GetElement(myElemId), myLining);
                    }

                }

                trans.Commit();
            }
        }



        private void joiningLining(Document doc, Floor myCuttingFloor)
        {

            Element element = myCuttingFloor as Element;
            GeometryElement geomElement = element.get_Geometry(new Options());
            Solid solid = null;
            foreach (GeometryObject geomObj in geomElement)
            {
                solid = geomObj as Solid;
                if (solid != null) break;
            }

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.WherePasses(new ElementIntersectsSolidFilter(solid)); // Apply intersection filter to find matches

            //TaskDialog.Show("Revit", collector.Count() + " family instances intersect with the selected element (" + element.Category.Name + " id:" + element.Id.ToString() + ")");

            List<ElementId> myListInter = new List<ElementId>();
            foreach (Element myE in collector)
            {
                if (myE.Id != element.Id)
                {
                    myListInter.Add(myE.Id);
                }
            }

            // Joint

            try
            {

                using (Transaction trans = new Transaction(doc, "Join Floor"))
                {
                    trans.Start();
                    foreach (ElementId myEId in myListInter)
                    {
                        if (!JoinGeometryUtils.AreElementsJoined(doc, doc.GetElement(myEId), element))
                        {
                            JoinGeometryUtils.JoinGeometry(doc, doc.GetElement(myEId), element);
                            //JoinGeometryUtils.UnjoinGeometry(doc, doc.GetElement(myEId), element);
                        }

                    }
                    trans.Commit();
                }
            }
            catch (Exception e)
            {

                TaskDialog.Show("Error", e.Message);
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
