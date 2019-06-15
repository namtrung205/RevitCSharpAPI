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

namespace CreateElevationAuto
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CreateViewAuto : IExternalCommand
    {
        // Set GUID
        static AddInId appId = new AddInId(new Guid("A3D97034-4AAC-48E8-B662-95A095903C04"));

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
            createElevationsByRooms(commandData.Application.ActiveUIDocument);
            return Autodesk.Revit.UI.Result.Succeeded;
        }


        /// <summary>
        /// Duyệt qua từng room trong rooms đã chọn (multiple)
        /// </summary>
        /// <param name="uiDoc">Active UIDocument</param> 
        private void createElevationsByRooms(UIDocument uiDoc)
        {
            // Set active document
            Document doc = uiDoc.Document;

            // Tạ danh sách lọc như 1 tham số đầu vào cho lớp "FilterByNameElementType", type Room
            List<string> myListNameFilter = new List<string>() { "Room" };
            // Thực hiện chọn các đối tượng theo bộ lọc room...
            List<Reference> myListRefRoom = uiDoc.Selection.PickObjects(ObjectType.Element, new FilterByNameElementType(myListNameFilter),
                                                               "Select rooms to auto create Elevations...") as List<Reference>;
            // Nếu danh sách reference được chọn không đúng, trả về null, dừng hàm
            if (myListRefRoom.Count() < 1 || myListRefRoom == null) return;

            // Nếu danh sách reference hợp lí (1 danh sách ref chứa rooms), thực thi hàm createElevationByRoom()
            foreach (Reference myRefRoom in myListRefRoom)
            {
                Room myRoom = doc.GetElement(myRefRoom) as Room;
                createElevationByRoom(uiDoc, myRoom);
            }
        }


        /// <summary>
        /// Nhận vào 2 tham số, trả về null khi tạo 4 mặt đứng bằng instance thuộc lớp ElevationMarker
        /// </summary>
        /// <param name="uiDoc"></param>
        /// <param name="myRoom"></param>
        private void createElevationByRoom(UIDocument uiDoc, Room myRoom)
        {
            // Set active document
            Document doc = uiDoc.Document;

            List<ViewSection> myListViewSecton = new List<ViewSection>();

            ElevationMarker myELM = null;

            BoundingBoxXYZ myRoomBB = null;

            XYZ minPointBB = null;
            XYZ maxPointBB = null;

            XYZ minPointBb = null;

            XYZ OriginPoint2 = null;

            double minY = myRoom.Level.Elevation - 1;// offset - 1
            double maxY = myRoom.UpperLimit.Elevation + myRoom.LimitOffset + 1;  //offset + 1

            using (Transaction trans0 = new Transaction(doc, "add elevation"))
            {
                // Start Transaction
                trans0.Start();

                // Get parameter of room
                double myLevel = myRoom.Level.Elevation;

                // Get BoundingBoxXYZ of the room.
                myRoomBB = myRoom.get_BoundingBox(null);

                maxPointBB = myRoomBB.Max;
                minPointBB = myRoomBB.Min;

                //Set Origin View Point as rooms Location Point
                LocationPoint lcPointRoom = myRoom.Location as LocationPoint;
                XYZ originPoint = lcPointRoom.Point;

                // Get Center Point of room to make reference Point of views
                OriginPoint2 = new XYZ((maxPointBB.X + minPointBB.X) / 2, (maxPointBB.Y + minPointBB.Y) / 2, originPoint.Z);

                // second parameter in contructor ElevationMarker
                ViewFamilyType vft = new FilteredElementCollector(doc)
                            .OfClass(typeof(ViewFamilyType))
                            .Cast<ViewFamilyType>()
                            .FirstOrDefault<ViewFamilyType>(x =>
                             ViewFamily.Elevation == x.ViewFamily);

                ElementId myEleId = vft.Id;


                // Create 1 ElevationMaker with scale = 1:40, type View = Elevation
                myELM = ElevationMarker.CreateElevationMarker(doc, myEleId, originPoint, 40);

                // Set max and Min Elevation of View (axis Y of Section Views), offset = 1 feet;


                // Commit 1
                trans0.Commit();
            }

            string roomsName = myRoom.Name.ToString();

            for (int i = 0; i < 4; i++)

            {
                #region section0

                if (i == 0)
                {

                    using (Transaction trans1 = new Transaction(doc, "create sec 0"))
                    {
                        trans1.Start();


                        ViewSection elevationView = myELM.CreateElevation(doc, doc.ActiveView.Id, i);


                        //string roomsName = myRoom.Name.Substring(0, myRoom.Name.Length - 1 - myRoom.Number.ToString().Length);

                        elevationView.Name = roomsName.ToUpper() + ", Section ".ToUpper() + i.ToString().ToUpper();

                        elevationView.CropBoxActive = true;

                        elevationView.DisplayStyle = DisplayStyle.Realistic;

                        BoundingBoxXYZ myCrop = elevationView.get_BoundingBox(null);


                        // Case 1: LeftView
                        myCrop.Min = new XYZ(myRoomBB.Min.Y - 1, minY, myCrop.Min.Z);
                        myCrop.Max = new XYZ(myRoomBB.Max.Y + 1, maxY, myCrop.Max.Z);
                        elevationView.CropBox = myCrop;

                        // Set visual style						
                        elevationView.DisplayStyle = DisplayStyle.HLR;

                        trans1.Commit();
                        //add View to List View
                        myListViewSecton.Add(elevationView);
                    }

                }

                #endregion


                #region section1
                else if (i == 1)
                {

                    using (Transaction trans2 = new Transaction(doc, "create sec 1"))
                    {

                        trans2.Start();

                        ViewSection elevationView = myELM.CreateElevation(doc, doc.ActiveView.Id, i);


                        //string roomsName = myRoom.Name.Substring(0, myRoom.Name.Length - 1 - myRoom.Number.ToString().Length);

                        elevationView.Name = roomsName.ToUpper() + ", Section ".ToUpper() + i.ToString().ToUpper();

                        elevationView.CropBoxActive = true;

                        elevationView.DisplayStyle = DisplayStyle.Realistic;

                        BoundingBoxXYZ myCrop = elevationView.get_BoundingBox(null);

                        //Case 2: Above View
                        myCrop.Min = new XYZ(myRoomBB.Min.X - 1, minY, myCrop.Min.Z);
                        myCrop.Max = new XYZ(myRoomBB.Max.X + 1, maxY, myCrop.Max.Z);

                        elevationView.CropBox = myCrop;

                        // Set visual style	for View		
                        elevationView.DisplayStyle = DisplayStyle.HLR;

                        trans2.Commit();
                        //add View to List View
                        myListViewSecton.Add(elevationView);
                    }
                }

                #endregion


                #region section2
                else if (i == 2)
                {
                    using (Transaction trans3 = new Transaction(doc, "create sec 2"))
                    {

                        trans3.Start();


                        ViewSection elevationView = myELM.CreateElevation(doc, doc.ActiveView.Id, i);


                        //string roomsName = myRoom.Name.Substring(0, myRoom.Name.Length - 1 - myRoom.Number.ToString().Length);

                        elevationView.Name = roomsName.ToUpper() + ", Section ".ToUpper() + i.ToString().ToUpper();

                        elevationView.CropBoxActive = true;

                        elevationView.DisplayStyle = DisplayStyle.Realistic;

                        BoundingBoxXYZ myCrop = elevationView.get_BoundingBox(null);


                        //
                        double XMin = -(Math.Abs(maxPointBB.Y - minPointBB.Y) / 2) - 1;
                        double XMax = +(Math.Abs(maxPointBB.Y - minPointBB.Y) / 2) + 1;

                        myCrop.Min = new XYZ(XMin - OriginPoint2.Y, minY, myCrop.Min.Z);
                        myCrop.Max = new XYZ(myCrop.Min.X + myRoomBB.Max.Y - myRoomBB.Min.Y + 2, maxY, myCrop.Max.Z);

                        elevationView.CropBox = myCrop;

                        // Set visual style						
                        elevationView.DisplayStyle = DisplayStyle.HLR;

                        trans3.Commit();


                        //add View to List View
                        myListViewSecton.Add(elevationView);
                    }
                }

                #endregion


                #region section3


                else
                {
                    using (Transaction trans4 = new Transaction(doc, "create sec 3"))
                    {

                        trans4.Start();

                        ViewSection elevationView = myELM.CreateElevation(doc, doc.ActiveView.Id, i);


                        //string roomsName = myRoom.Name.Substring(0, myRoom.Name.Length - 1 - myRoom.Number.ToString().Length);

                        elevationView.Name = roomsName.ToUpper() + ", Section ".ToUpper() + i.ToString().ToUpper();

                        elevationView.CropBoxActive = true;

                        elevationView.DisplayStyle = DisplayStyle.Realistic;

                        BoundingBoxXYZ myCrop = elevationView.get_BoundingBox(null);



                        myCrop.Min = new XYZ(myRoomBB.Min.X - 1 - 2 * (OriginPoint2.X), minY, myCrop.Min.Z);
                        myCrop.Max = new XYZ(myCrop.Min.X + myRoomBB.Max.X - myRoomBB.Min.X + 2, maxY, myCrop.Max.Z);
                        elevationView.CropBox = myCrop;

                        // Set visual style						
                        elevationView.DisplayStyle = DisplayStyle.HLR;

                        trans4.Commit();

                        //add View to List View
                        myListViewSecton.Add(elevationView);
                    }
                }
                #endregion

            }

            #region change setting graphic
            foreach (ViewSection mySec in myListViewSecton)
            {
                setStyleCropBoxFromView(uiDoc, mySec);
            }
            #endregion
        }



        private void setStyleCropBoxFromView(UIDocument uiDoc, View myView)

        {
            //Set current Document
            Document doc = uiDoc.Document;

            // Create A Collection of ElementID (get all ElementId on a View)
            ICollection<ElementId> myListElemId1 = null;

            //Start a transaction to hide cropbox of view

            using (Transaction trans1 = new Transaction(doc, "Hide the cropbox of a myView"))
            {
                trans1.Start();
                myView.CropBoxVisible = false;
                trans1.Commit();
            }

            // Set myListElemId1( Without cropBox's elementId)
            myListElemId1 = new FilteredElementCollector(doc, myView.Id).ToElementIds();

            // Start another transaction to unhide (active) cropBox

            using (Transaction trans2 = new Transaction(doc, "unHide CropBox"))
            {
                trans2.Start();
                myView.CropBoxVisible = true;
                trans2.Commit();
            }

            // the CropBox ElementId is firts item in list ElementId convert from FilteredElementCollector

            ElementId myCropBoxId = new FilteredElementCollector(doc, myView.Id).
                Excluding(myListElemId1).
                ToElementIds().First();

            //			TaskDialog.Show("Abc", "cropbOx Id: " + myCropBoxId.ToString());

            //Create a graphic Overide for Element (OverrideGraphicSettings)

            OverrideGraphicSettings ogsCropBox = new OverrideGraphicSettings();

            // Color
            ogsCropBox.SetProjectionLineColor(new Color(255, 0, 0));
            //Line Weight
            ogsCropBox.SetProjectionLineWeight(5);
            //Pattern

            FilteredElementCollector fec = new FilteredElementCollector(doc).OfClass(typeof(LinePatternElement));

            LinePatternElement linePatternElem = fec.
                Cast<LinePatternElement>().
                First<LinePatternElement>(linePattern => linePattern.Name == "Dash");

            ElementId myLinePatId = linePatternElem.Id;

            ogsCropBox.SetProjectionLinePatternId(myLinePatId);

            // Set ElementOverride (Style Line)

            using (Transaction trans3 = new Transaction(doc, "Change Style of CropBox"))
            {
                trans3.Start();
                myView.CropBoxVisible = true;
                myView.SetElementOverrides(myCropBoxId, ogsCropBox);
                trans3.Commit();
            }
        }


    }


    public class PaintWall : IExternalCommand
    {
        // Set GUID
        static AddInId appId = new AddInId(new Guid("7E2BD841-B712-41AE-A640-A43377D953F9"));

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
            createElevationsByRooms(commandData.Application.ActiveUIDocument);
            return Autodesk.Revit.UI.Result.Succeeded;
        }


        /// <summary>
        /// Duyệt qua từng room trong rooms đã chọn (multiple)
        /// </summary>
        /// <param name="uiDoc">Active UIDocument</param> 
        private void createElevationsByRooms(UIDocument uiDoc)
        {
            // Set active document
            Document doc = uiDoc.Document;

            // Tạ danh sách lọc như 1 tham số đầu vào cho lớp "FilterByNameElementType", type Room
            List<string> myListNameFilter = new List<string>() { "Wall" };
            // Thực hiện chọn các đối tượng theo bộ lọc room...
            List<Reference> myListRefRoom = uiDoc.Selection.PickObjects(ObjectType.Element, new FilterByNameElementType(myListNameFilter),
                                                               "Select rooms to auto create Elevations...") as List<Reference>;
            // Nếu danh sách reference được chọn không đúng, trả về null, dừng hàm
            if (myListRefRoom.Count() < 1 || myListRefRoom == null) return;

            // Nếu danh sách reference hợp lí (1 danh sách ref chứa rooms), thực thi hàm createElevationByRoom()
            foreach (Reference myRefRoom in myListRefRoom)
            {
                Room myRoom = doc.GetElement(myRefRoom) as Room;
                createElevationByRoom(uiDoc, myRoom);
            }
        }


        /// <summary>
        /// Nhận vào 2 tham số, trả về null khi tạo 4 mặt đứng bằng instance thuộc lớp ElevationMarker
        /// </summary>
        /// <param name="uiDoc"></param>
        /// <param name="myRoom"></param>
        private void createElevationByRoom(UIDocument uiDoc, Room myRoom)
        {
            // Set active document
            Document doc = uiDoc.Document;

            List<ViewSection> myListViewSecton = new List<ViewSection>();

            ElevationMarker myELM = null;

            BoundingBoxXYZ myRoomBB = null;

            XYZ minPointBB = null;
            XYZ maxPointBB = null;

            XYZ minPointBb = null;

            XYZ OriginPoint2 = null;

            double minY = myRoom.Level.Elevation - 1;// offset - 1
            double maxY = myRoom.UpperLimit.Elevation + myRoom.LimitOffset + 1;  //offset + 1

            using (Transaction trans0 = new Transaction(doc, "add elevation"))
            {
                // Start Transaction
                trans0.Start();

                // Get parameter of room
                double myLevel = myRoom.Level.Elevation;

                // Get BoundingBoxXYZ of the room.
                myRoomBB = myRoom.get_BoundingBox(null);

                maxPointBB = myRoomBB.Max;
                minPointBB = myRoomBB.Min;

                //Set Origin View Point as rooms Location Point
                LocationPoint lcPointRoom = myRoom.Location as LocationPoint;
                XYZ originPoint = lcPointRoom.Point;

                // Get Center Point of room to make reference Point of views
                OriginPoint2 = new XYZ((maxPointBB.X + minPointBB.X) / 2, (maxPointBB.Y + minPointBB.Y) / 2, originPoint.Z);

                // second parameter in contructor ElevationMarker
                ViewFamilyType vft = new FilteredElementCollector(doc)
                            .OfClass(typeof(ViewFamilyType))
                            .Cast<ViewFamilyType>()
                            .FirstOrDefault<ViewFamilyType>(x =>
                             ViewFamily.Elevation == x.ViewFamily);

                ElementId myEleId = vft.Id;


                // Create 1 ElevationMaker with scale = 1:40, type View = Elevation
                myELM = ElevationMarker.CreateElevationMarker(doc, myEleId, originPoint, 40);

                // Set max and Min Elevation of View (axis Y of Section Views), offset = 1 feet;


                // Commit 1
                trans0.Commit();
            }

            string roomsName = myRoom.Name.ToString();

            for (int i = 0; i < 4; i++)

            {
                #region section0

                if (i == 0)
                {

                    using (Transaction trans1 = new Transaction(doc, "create sec 0"))
                    {
                        trans1.Start();


                        ViewSection elevationView = myELM.CreateElevation(doc, doc.ActiveView.Id, i);

                        elevationView.Name = roomsName.ToUpper() + ", Section ".ToUpper() + i.ToString().ToUpper();

                        elevationView.CropBoxActive = true;

                        elevationView.DisplayStyle = DisplayStyle.Realistic;

                        BoundingBoxXYZ myCrop = elevationView.get_BoundingBox(null);


                        // Case 1: LeftView
                        myCrop.Min = new XYZ(myRoomBB.Min.Y - 1, minY, myCrop.Min.Z);
                        myCrop.Max = new XYZ(myRoomBB.Max.Y + 1, maxY, myCrop.Max.Z);
                        elevationView.CropBox = myCrop;

                        // Set visual style						
                        elevationView.DisplayStyle = DisplayStyle.HLR;

                        trans1.Commit();
                        //add View to List View
                        myListViewSecton.Add(elevationView);
                    }

                }

                #endregion


                #region section1
                else if (i == 1)
                {

                    using (Transaction trans2 = new Transaction(doc, "create sec 1"))
                    {

                        trans2.Start();

                        ViewSection elevationView = myELM.CreateElevation(doc, doc.ActiveView.Id, i);


                        //string roomsName = myRoom.Name.Substring(0, myRoom.Name.Length - 1 - myRoom.Number.ToString().Length);

                        elevationView.Name = roomsName.ToUpper() + ", Section ".ToUpper() + i.ToString().ToUpper();

                        elevationView.CropBoxActive = true;

                        elevationView.DisplayStyle = DisplayStyle.Realistic;

                        BoundingBoxXYZ myCrop = elevationView.get_BoundingBox(null);

                        //Case 2: Above View
                        myCrop.Min = new XYZ(myRoomBB.Min.X - 1, minY, myCrop.Min.Z);
                        myCrop.Max = new XYZ(myRoomBB.Max.X + 1, maxY, myCrop.Max.Z);

                        elevationView.CropBox = myCrop;

                        // Set visual style	for View		
                        elevationView.DisplayStyle = DisplayStyle.HLR;

                        trans2.Commit();
                        //add View to List View
                        myListViewSecton.Add(elevationView);
                    }
                }

                #endregion


                #region section2
                else if (i == 2)
                {
                    using (Transaction trans3 = new Transaction(doc, "create sec 2"))
                    {

                        trans3.Start();


                        ViewSection elevationView = myELM.CreateElevation(doc, doc.ActiveView.Id, i);


                        //string roomsName = myRoom.Name.Substring(0, myRoom.Name.Length - 1 - myRoom.Number.ToString().Length);

                        elevationView.Name = roomsName.ToUpper() + ", Section ".ToUpper() + i.ToString().ToUpper();

                        elevationView.CropBoxActive = true;

                        elevationView.DisplayStyle = DisplayStyle.Realistic;

                        BoundingBoxXYZ myCrop = elevationView.get_BoundingBox(null);


                        //
                        double XMin = -(Math.Abs(maxPointBB.Y - minPointBB.Y) / 2) - 1;
                        double XMax = +(Math.Abs(maxPointBB.Y - minPointBB.Y) / 2) + 1;

                        myCrop.Min = new XYZ(XMin - OriginPoint2.Y, minY, myCrop.Min.Z);
                        myCrop.Max = new XYZ(myCrop.Min.X + myRoomBB.Max.Y - myRoomBB.Min.Y + 2, maxY, myCrop.Max.Z);

                        elevationView.CropBox = myCrop;

                        // Set visual style						
                        elevationView.DisplayStyle = DisplayStyle.HLR;

                        trans3.Commit();


                        //add View to List View
                        myListViewSecton.Add(elevationView);
                    }
                }

                #endregion


                #region section3


                else
                {
                    using (Transaction trans4 = new Transaction(doc, "create sec 3"))
                    {

                        trans4.Start();

                        ViewSection elevationView = myELM.CreateElevation(doc, doc.ActiveView.Id, i);


                        //string roomsName = myRoom.Name.Substring(0, myRoom.Name.Length - 1 - myRoom.Number.ToString().Length);

                        elevationView.Name = roomsName.ToUpper() + ", Section ".ToUpper() + i.ToString().ToUpper();

                        elevationView.CropBoxActive = true;

                        elevationView.DisplayStyle = DisplayStyle.Realistic;

                        BoundingBoxXYZ myCrop = elevationView.get_BoundingBox(null);



                        myCrop.Min = new XYZ(myRoomBB.Min.X - 1 - 2 * (OriginPoint2.X), minY, myCrop.Min.Z);
                        myCrop.Max = new XYZ(myCrop.Min.X + myRoomBB.Max.X - myRoomBB.Min.X + 2, maxY, myCrop.Max.Z);
                        elevationView.CropBox = myCrop;

                        // Set visual style						
                        elevationView.DisplayStyle = DisplayStyle.HLR;

                        trans4.Commit();

                        //add View to List View
                        myListViewSecton.Add(elevationView);
                    }
                }
                #endregion

            }

            #region change setting graphic
            foreach (ViewSection mySec in myListViewSecton)
            {
                setStyleCropBoxFromView(uiDoc, mySec);
            }
            #endregion
        }



        private void setStyleCropBoxFromView(UIDocument uiDoc, View myView)

        {
            //Set current Document
            Document doc = uiDoc.Document;

            // Create A Collection of ElementID (get all ElementId on a View)
            ICollection<ElementId> myListElemId1 = null;

            //Start a transaction to hide cropbox of view

            using (Transaction trans1 = new Transaction(doc, "Hide the cropbox of a myView"))
            {
                trans1.Start();
                myView.CropBoxVisible = false;
                trans1.Commit();
            }

            // Set myListElemId1( Without cropBox's elementId)
            myListElemId1 = new FilteredElementCollector(doc, myView.Id).ToElementIds();

            // Start another transaction to unhide (active) cropBox

            using (Transaction trans2 = new Transaction(doc, "unHide CropBox"))
            {
                trans2.Start();
                myView.CropBoxVisible = true;
                trans2.Commit();
            }

            // the CropBox ElementId is firts item in list ElementId convert from FilteredElementCollector

            ElementId myCropBoxId = new FilteredElementCollector(doc, myView.Id).
                Excluding(myListElemId1).
                ToElementIds().First();

            //			TaskDialog.Show("Abc", "cropbOx Id: " + myCropBoxId.ToString());

            //Create a graphic Overide for Element (OverrideGraphicSettings)

            OverrideGraphicSettings ogsCropBox = new OverrideGraphicSettings();

            // Color
            ogsCropBox.SetProjectionLineColor(new Color(255, 0, 0));
            //Line Weight
            ogsCropBox.SetProjectionLineWeight(5);
            //Pattern

            FilteredElementCollector fec = new FilteredElementCollector(doc).OfClass(typeof(LinePatternElement));

            LinePatternElement linePatternElem = fec.
                Cast<LinePatternElement>().
                First<LinePatternElement>(linePattern => linePattern.Name == "Dash");

            ElementId myLinePatId = linePatternElem.Id;

            ogsCropBox.SetProjectionLinePatternId(myLinePatId);

            // Set ElementOverride (Style Line)

            using (Transaction trans3 = new Transaction(doc, "Change Style of CropBox"))
            {
                trans3.Start();
                myView.CropBoxVisible = true;
                myView.SetElementOverrides(myCropBoxId, ogsCropBox);
                trans3.Commit();
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

}
