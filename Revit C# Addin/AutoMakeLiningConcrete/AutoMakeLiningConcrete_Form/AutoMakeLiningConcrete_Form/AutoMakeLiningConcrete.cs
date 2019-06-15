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

            mySelFamilyForm.offsetDisTb.Text = "100";


            FilteredElementCollector fec = new FilteredElementCollector(doc)
                .OfClass(typeof(FloorType))
                .OfCategory(BuiltInCategory.OST_Floors);


            IEnumerable<FloorType> iterFloorTypes = fec.Cast<FloorType>();

            // Lay danh sach vat lieu
            foreach (FloorType myItemType in iterFloorTypes)
            {
                mySelFamilyForm.FamilysCb.Items.Add(myItemType.Name.ToString());

            }


            double offsetVal = Convert.ToDouble(valueOfSetting(@"C:\Revit Setting\RevitSetting.set", "LiningConcreteOffset")) / 304.8;

            int indexSel = mySelFamilyForm.FamilysCb.Items.IndexOf(valueOfSetting(@"C:\Revit Setting\RevitSetting.set", "LiningConcreteFamily"));

            mySelFamilyForm.FamilysCb.SelectedIndex = indexSel;
            mySelFamilyForm.ShowDialog();

            string nameSelectedFamily = mySelFamilyForm.FamilysCb.SelectedItem.ToString();


            //filter element
            List<int> myListCatoId = new List<int>();
            myListCatoId.Add((int)BuiltInCategory.OST_StructuralFoundation);
            myListCatoId.Add((int)BuiltInCategory.OST_StructuralFraming);

            List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, new FilterByIdCategory(myListCatoId), "Select Beam or Slab") as List<Reference>;

            if (myListRef.Count() < 1) return;


            foreach (Reference myRef in myListRef)
            {
                //Get ElementId from ref
                ElementId myFoundationId = doc.GetElement(myRef).Id;

                createLiningConcreteAsFloor2(doc, myFoundationId, nameSelectedFamily, offsetVal);

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



        private void createLiningConcreteAsFloor2( Document doc, ElementId myFoundtionId, string nameFamily, double offsetValue)

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

                    myListLining.Add(myLining);
                }
                trans.Commit();
            }

            // Switch Joint
            if (myListLining.Count() < 1)
            {
                return;
            }
            else
            {
                foreach (Floor myLining in myListLining)
                {
                    switchJoinOrder(doc, myLining);
                }

            }

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
