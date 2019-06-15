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

namespace AutoMakeSectionViewBeamWall
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class AutoMakeSectionViewBeamWall : IExternalCommand
    {
        // Set GUID [Guid("79226C29-8E4F-4B07-8EB4-622C17EDB5D3")]
        static AddInId appId = new AddInId(new Guid("79226C29-8E4F-4B07-8EB4-622C17EDB5D3"));

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
            makeSectionBeamAuto(commandData.Application.ActiveUIDocument);
            return Autodesk.Revit.UI.Result.Succeeded;
        }

        public void makeSectionBeamAuto(UIDocument uiDoc)
        {
            Document doc = uiDoc.Document;

            List<int> myListIdCategory = new List<int>();
            myListIdCategory.Add((int)BuiltInCategory.OST_StructuralFraming);
            myListIdCategory.Add((int)BuiltInCategory.OST_Walls);
            myListIdCategory.Add((int)BuiltInCategory.OST_WallsStructure);

            List<string> myFilter = new List<string>() { "Wall", "Beam" };

            List<Reference> myRefList = (List<Reference>)uiDoc.Selection.PickObjects(ObjectType.Element, new FilterByIdCategory(myListIdCategory), "Pick a Beam...");
            foreach (Reference myRef in myRefList)
            {
                Element myEle = doc.GetElement(myRef);
                this.sectionViewBeamX(uiDoc, myEle);
                this.sectionViewBeamY(uiDoc, myEle, 0.05);
            }
        }


        private void sectionViewBeamX(UIDocument uiDoc, Element myElem)
        {

            Document doc = uiDoc.Document;

            Element elem = myElem;
            ElementId elemTypeId = elem.GetTypeId();
            Element elemType = doc.GetElement(elemTypeId);

            // Element type Name

            string nameOfBeam = elem.Name;
            //TaskDialog.Show("test2", nameOfBeam);

            // Ensure wall is straight

            LocationCurve lc = elem.Location as LocationCurve;

            Line line = lc.Curve as Line;

            // Create section View
            // Các tham số để tạo view section

            /// <summary>
            /// public static ViewSection CreateSection(Document document, ElementId viewFamilyTypeId, BoundingBoxXYZ sectionBox)
            /// </summary>
            /// 
            //Tao viewFamilytypeId (tham so thu 2)

            ViewFamilyType vft = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType))
                .Cast<ViewFamilyType>()
                .FirstOrDefault<ViewFamilyType>(x =>
                ViewFamily.Section == x.ViewFamily);

            // tao BoundingBox (tham so thu 3)

            XYZ p = line.GetEndPoint(0);
            XYZ q = line.GetEndPoint(1);
            XYZ v = q - p;


            //Set boundingBox
            BoundingBoxXYZ bb_X = elem.get_BoundingBox(null);
            double minZ_X = bb_X.Min.Z;
            double maxZ_X = bb_X.Max.Z;

            double minY_X = bb_X.Min.Y;
            double maxY_X = bb_X.Max.Y;


            //lay cac thong so kich thuoc dam

            double l = v.GetLength(); // length of beam (endpoint - start Point)
            double h = maxZ_X - minZ_X; // h of beam
            double b = maxY_X - minY_X; // b of beam
            double offset = h;// khoang cach offset



            XYZ min = new XYZ(-l / 2 - 3.3, 0 - offset - h, 0);
            XYZ max = new XYZ(l / 2 + 3.3, offset, offset);

            if (myElem is Wall)
            {
                min = new XYZ(-l / 2 - 3.3, -3, 0);
                max = new XYZ(l / 2 + 3.3, offset + 3, offset);

            }



            XYZ midpoint = p + 0.5 * v; // Toa do trung diem
            XYZ beamdir = v.Normalize(); // Truc dam
            XYZ upX = XYZ.BasisZ; // Huong z thang dung
            XYZ viewdirX = beamdir.CrossProduct(upX); // Huong nhin cua View

            Transform tX = Transform.Identity;
            tX.Origin = midpoint;// Goc view
            tX.BasisX = beamdir;// Huong X la trung voi huong beam
            tX.BasisY = upX; // Huong y trung voi truc z
            tX.BasisZ = viewdirX; // Huong z trung voi huong nhin view (y)


            BoundingBoxXYZ sectionBoxX = new BoundingBoxXYZ();
            sectionBoxX.Transform = tX;
            sectionBoxX.Min = min;
            sectionBoxX.Max = max;

            using (Transaction myTrans = new Transaction(doc, "Create view section X of Beam"))
            {
                myTrans.Start();
                ViewSection vSX = ViewSection.CreateSection(doc, vft.Id, sectionBoxX);
                //				vSX.Name = nameOfBeam + "_X";
                //vSX.ViewName = "ABC";
                myTrans.Commit();
            }

        }



        private void sectionViewBeamY(UIDocument uiDoc, Element myElemt, double atX)
        {
            Document doc = uiDoc.Document;

            Element elem = myElemt;

            ElementId elemTypeId = elem.GetTypeId();
            Element elemType = doc.GetElement(elemTypeId);


            LocationCurve lc = elem.Location as LocationCurve;

            Line line = lc.Curve as Line;

            // tao BoundingBox (tham so thu 3)

            XYZ p = line.GetEndPoint(0);
            XYZ q = line.GetEndPoint(1);
            XYZ v = q - p;

            //Set boundingBox
            BoundingBoxXYZ bb = elem.get_BoundingBox(null);
            double minZ = bb.Min.Z;
            double maxZ = bb.Max.Z;

            double minY = bb.Min.Y;
            double maxY = bb.Max.Y;

            //lay cac thong so kich thuoc dam, get parameter of beam

            double l = v.GetLength(); // length of beam (endpoint - start Point)
            double h = maxZ - minZ; // h of beam
            double b = maxY - minY; // b of beam

            //Distance from point to vector		
            XYZ point = bb.Max;

            // Tính toán dien tich tam giac

            double b2 = 4 * Math.Abs((q.X - p.X) * (point.Y - p.Y) - (point.X - p.X) * (q.Y - p.Y)) /
                    Math.Sqrt(Math.Pow(q.X - p.X, 2) + Math.Pow(q.Y - p.Y, 2));

            double wv = Math.Min(b2 / 2, h / 2);//width of view

            double offset = 1;// khoang cach offset


            XYZ max = new XYZ(wv + offset, offset, 1);
            XYZ min = new XYZ(-wv - offset, -offset - h, 0);

            if (myElemt is Wall)
            {
                max = new XYZ(wv + offset, offset + h, 1);
                min = new XYZ(-wv - offset, -offset, 0);

            }


            // Type view Family
            ViewFamilyType vft = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType))
                .Cast<ViewFamilyType>()
                .FirstOrDefault<ViewFamilyType>(x =>
                ViewFamily.Detail == x.ViewFamily);

            // origin at the center of the location curve
            Transform curveTransform = lc.Curve.ComputeDerivatives(0.5, true);

            // The transform contains the location curve

            XYZ origin = curveTransform.Origin - v * (0.5 - atX); // Set tam cua section View la origin cua curve Transform
            XYZ viewdir = curveTransform.BasisX.Normalize(); // view dir da bi xoay di 90 do
            XYZ up = XYZ.BasisZ;// Huong len trung voi truc z
            XYZ right = up.CrossProduct(viewdir); // right theo tam dien thuan

            // Create a transform from vectors above.

            Transform transform = Transform.Identity;
            transform.Origin = origin;
            transform.BasisX = right;
            transform.BasisY = up;
            transform.BasisZ = viewdir;


            BoundingBoxXYZ sectionBoxY = new BoundingBoxXYZ();
            sectionBoxY.Transform = transform;
            sectionBoxY.Min = min;
            sectionBoxY.Max = max;


            using (Transaction myTrans = new Transaction(doc, "Create view section of Beam"))
            {
                myTrans.Start();
                //				ViewSection.CreateSection(doc, vft.Id, sectionBoxY);
                ViewSection.CreateDetail(doc, vft.Id, sectionBoxY);
                myTrans.Commit();
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
