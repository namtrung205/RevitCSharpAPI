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
            using (Transaction trans = new Transaction(doc, "add elevation"))
            {
                // Start Transaction
                trans.Start();


                // Get parameter of room
                double myLevel = myRoom.Level.Elevation;

                // Get BoundingBoxXYZ of the room.
                BoundingBoxXYZ myRoomBB = myRoom.get_BoundingBox(null);

                XYZ maxPointBB = myRoomBB.Max;
                XYZ minPointBB = myRoomBB.Min;

                //Set Origin View Point as rooms Location Point
                LocationPoint lcPointRoom = myRoom.Location as LocationPoint;
                XYZ originPoint = lcPointRoom.Point;

                // Get Center Point of room to make reference Point of views
                XYZ OriginPoint2 = new XYZ((maxPointBB.X + minPointBB.X) / 2, (maxPointBB.Y + minPointBB.Y) / 2, originPoint.Z);

                // second parameter in contructor ElevationMarker
                ViewFamilyType vft = new FilteredElementCollector(doc)
                            .OfClass(typeof(ViewFamilyType))
                            .Cast<ViewFamilyType>()
                            .FirstOrDefault<ViewFamilyType>(x =>
                             ViewFamily.Elevation == x.ViewFamily);

                ElementId myEleId = vft.Id;


                // Create 1 ElevationMaker with scale = 1:40, type View = Elevation
                ElevationMarker myELM = ElevationMarker.CreateElevationMarker(doc, myEleId, originPoint, 40);

                // Set max and Min Elevation of View (axis Y of Section Views), offset = 1 feet;

                double minY = myRoom.Level.Elevation - 1;// offset - 1
                double maxY = myRoom.UpperLimit.Elevation + myRoom.LimitOffset + 1;  //offset + 1

                for (int i = 0; i < 4; i++)

                {
                    ViewSection elevationView = myELM.CreateElevation(doc, doc.ActiveView.Id, i);


                    string roomsName = myRoom.Name.Substring(0, myRoom.Name.Length - 1 - myRoom.Number.ToString().Length);

                    elevationView.Name = roomsName.ToUpper() + ", Section ".ToUpper() + i.ToString().ToUpper();

                    elevationView.CropBoxActive = true;

                    elevationView.DisplayStyle = DisplayStyle.Realistic;

                    BoundingBoxXYZ myCrop = elevationView.get_BoundingBox(null);
                    if (i == 0)
                    {
                        // Case 1: LeftView
                        myCrop.Min = new XYZ(myRoomBB.Min.Y - 1, minY, myCrop.Min.Z);
                        myCrop.Max = new XYZ(myRoomBB.Max.Y + 1, maxY, myCrop.Max.Z);
                        elevationView.CropBox = myCrop;

                        // Set visual style						
                        elevationView.DisplayStyle = DisplayStyle.HLR;

                    }
                    else if (i == 1)
                    {
                        //Case 2: Above View
                        myCrop.Min = new XYZ(myRoomBB.Min.X - 1, minY, myCrop.Min.Z);
                        myCrop.Max = new XYZ(myRoomBB.Max.X + 1, maxY, myCrop.Max.Z);

                        elevationView.CropBox = myCrop;

                        // Set visual style	for View		
                        elevationView.DisplayStyle = DisplayStyle.HLR;

                    }

                    // Case 3, 4 make OriginPoint2 as reference Point
                    else if (i == 2)
                    {
                        //
                        double XMin = -(Math.Abs(maxPointBB.Y - minPointBB.Y) / 2) - 1;
                        double XMax = +(Math.Abs(maxPointBB.Y - minPointBB.Y) / 2) + 1;

                        myCrop.Min = new XYZ(XMin - OriginPoint2.Y, minY, myCrop.Min.Z);
                        myCrop.Max = new XYZ(myCrop.Min.X + myRoomBB.Max.Y - myRoomBB.Min.Y + 2, maxY, myCrop.Max.Z);

                        elevationView.CropBox = myCrop;

                        // Set visual style						
                        elevationView.DisplayStyle = DisplayStyle.HLR;

                    }

                    else
                    {
                        myCrop.Min = new XYZ(myRoomBB.Min.X - 1 - 2 * (OriginPoint2.X), minY, myCrop.Min.Z);
                        myCrop.Max = new XYZ(myCrop.Min.X + myRoomBB.Max.X - myRoomBB.Min.X + 2, maxY, myCrop.Max.Z);
                        elevationView.CropBox = myCrop;

                        // Set visual style						
                        elevationView.DisplayStyle = DisplayStyle.HLR;
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

}
