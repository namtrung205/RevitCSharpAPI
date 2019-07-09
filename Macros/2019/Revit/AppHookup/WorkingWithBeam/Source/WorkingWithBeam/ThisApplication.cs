/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 3/25/2019
 * Time: 10:15 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace WorkingWithBeam
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("AD51D46E-7993-46B8-BBD9-0441E0B69DA0")]
	public partial class ThisApplication
	{
		private void Module_Startup(object sender, EventArgs e)
		{

		}

		private void Module_Shutdown(object sender, EventArgs e)
		{

		}

		#region Revit Macros generated code
		private void InternalStartup()
		{
			this.Startup += new System.EventHandler(Module_Startup);
			this.Shutdown += new System.EventHandler(Module_Shutdown);
		}
		#endregion
		
						
		private void sectionViewBeamX_BK(Element myElem)
		{
			UIDocument UiDoc = this.ActiveUIDocument;
			Document doc = UiDoc.Document;
			
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
			


			XYZ min = new XYZ( -l/2-h,0- offset-h, 0 );
			XYZ max = new XYZ( l/2+h, offset, offset );
			
			if(myElem is Wall)
			{
				min = new XYZ( -l/2-3,-3, 0 );
				max = new XYZ( l/2+3, offset+3, offset );
			
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
			
            using(Transaction myTrans = new Transaction(doc, "Create view section X of Beam")) {
            	myTrans.Start();
				ViewSection vSX =  ViewSection.CreateSection( doc, vft.Id, sectionBoxX);
//				vSX.Name = nameOfBeam + "_X";
				//vSX.ViewName = "ABC";
            	myTrans.Commit();       	
            }
			
		}
		

						
		private void sectionViewBeamX(Element myElem)
		{
			UIDocument UiDoc = this.ActiveUIDocument;
			Document doc = UiDoc.Document;
			
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
                ViewFamily.Detail == x.ViewFamily);
            
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
			


			XYZ min = new XYZ( -l/2-h,0- offset-h, 0 );
			XYZ max = new XYZ( l/2+h, offset, offset );
			
			if(myElem is Wall)
			{
				min = new XYZ( -l/2-3,-3, 0 );
				max = new XYZ( l/2+3, offset+3, offset );
			
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
			
            using(Transaction myTrans = new Transaction(doc, "Create view section X of Beam")) {
            	myTrans.Start();
            	
				ViewSection vSX =  ViewSection.CreateDetail( doc, vft.Id, sectionBoxX);
//				vSX.Name = nameOfBeam + "_X";
				//vSX.ViewName = "ABC";
            	myTrans.Commit();       	
            }
			
		}
		
		

		
		private void sectionViewBeamZ()
		{
			UIDocument UiDoc = this.ActiveUIDocument;
			Document doc = UiDoc.Document;
			
			//Get element by filler by Id integer of category
			
			List<int> myListIdCategory= new List<int>();
			myListIdCategory.Add((int)BuiltInCategory.OST_StructuralFraming);
			
			Reference myRef = UiDoc.Selection.PickObject(ObjectType.Element, new FilterByIdCategory(myListIdCategory),"Pick a Beam...");
			
			Element elem = doc.GetElement(myRef);
			ElementId elemTypeId = elem.GetTypeId();  
            Element elemType = doc.GetElement(elemTypeId);  

            
		    // Ensure wall is straight
		 
		    LocationCurve lc = elem.Location as LocationCurve;
		    
		    
            // Type view Family
            ViewFamilyType vft = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType))
            	.Cast<ViewFamilyType>()
            	.FirstOrDefault<ViewFamilyType>(x =>
                ViewFamily.Section == x.ViewFamily);
		   
			#region Create View 2
			
			//Section theo phuong doc dam
			
			// Using 0.5 and "true" to specify that the 
			// parameter is normalized places the transform
			// origin at the center of the location curve
			Transform curveTransform = lc.Curve.ComputeDerivatives(0.5, true);
		 
			// The transform contains the location curve
			// mid-point and tangent, and we can obtain
			// its normal in the XY plane:
			
			XYZ origin_Y = curveTransform.Origin; // Set tam cua section View la origin cua curve Transform
			XYZ viewdir_Y = curveTransform.BasisX.Normalize(); // view dir da bi xoay di 90 do
			XYZ up_Y = XYZ.BasisZ;// Huong len trung voi truc z
			XYZ right_Y = up_Y.CrossProduct(viewdir_Y); // right theo tam dien thuan
			
			// Create a transform from vectors above.
			
			Transform transform_Y = Transform.Identity;
			transform_Y.Origin = origin_Y;
			transform_Y.BasisX = viewdir_Y;
			transform_Y.BasisY = right_Y;
			transform_Y.BasisZ = up_Y;
			
			
		    
			BoundingBoxXYZ sectionBoxY = new BoundingBoxXYZ();
			sectionBoxY.Transform = transform_Y;


			// Xác định điểm offset

			      
            using(Transaction myTrans = new Transaction(doc, "Create view section of Beam")) {
            	myTrans.Start();
				ViewSection.CreateSection( doc, vft.Id, sectionBoxY );
            	myTrans.Commit();       	
            }
			#endregion
		}
		
				
		private void sectionViewBeamY(Element myElemt, double atX, string nameSection, int scaleFactor)
		{
			UIDocument UiDoc = this.ActiveUIDocument;
			Document doc = UiDoc.Document;
			
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
			
			double b2 = 4 * Math.Abs((q.X - p.X)*(point.Y - p.Y) - (point.X - p.X)*(q.Y - p.Y))/
                    Math.Sqrt(Math.Pow(q.X - p.X, 2) + Math.Pow(q.Y - p.Y, 2));
			
			double wv = Math.Min(b2/2, h/2);//width of view
			
			double offset = 1;// khoang cach offset
			
            
            XYZ max = new XYZ( wv + offset, offset, 1);
    		XYZ min = new XYZ( -wv -offset, -offset - h, 0 );
			
			if(myElemt is Wall)
			{
	            max = new XYZ( wv + offset, offset + h, 1);
	    		min = new XYZ( -wv -offset, -offset, 0 );				
			
			}
			

            // Type view Family
            ViewFamilyType vft = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType))
            	.Cast<ViewFamilyType>()
            	.FirstOrDefault<ViewFamilyType>(x =>
                ViewFamily.Detail == x.ViewFamily);
		   
			// origin at the center of the location curve
			Transform curveTransform = lc.Curve.ComputeDerivatives(0.5, true);
		 
			// The transform contains the location curve
			
			XYZ origin = curveTransform.Origin - v*(0.5-atX); // Set tam cua section View la origin cua curve Transform
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

			      
            using(Transaction myTrans = new Transaction(doc, "Create view section of Beam")) {
            	myTrans.Start();
//				ViewSection.CreateSection(doc, vft.Id, sectionBoxY);
				ViewSection new_SectionY = ViewSection.CreateDetail(doc, vft.Id, sectionBoxY);
				new_SectionY.Name = nameSection;
				new_SectionY.Scale = scaleFactor;
				
            	myTrans.Commit();       	
            }
		}
		
		
		public void sectionYFirst()
		
		{
			UIDocument UiDoc = this.ActiveUIDocument;
			Document doc = UiDoc.Document;
			
			List<int> myListIdCategory= new List<int>();
			myListIdCategory.Add((int)BuiltInCategory.OST_StructuralFraming);
			
			List<Reference> myRefList = new List<Reference>();
			
			Reference mybeamRef = UiDoc.Selection.PickObject(ObjectType.Element, new FilterByIdCategory(myListIdCategory),"Pick a Beam...");
			
			myRefList.Add(mybeamRef);
			// Curve of beam
			Line beamLineCur = (doc.GetElement(mybeamRef).Location as LocationCurve).Curve as Line;
			

			
			
			// Select Dim 
			Reference myDimRef = UiDoc.Selection.PickObject(ObjectType.Element, 
			                                             new FilterByNameElementType(new List<string>(){"Dimension"}),"Pick aLinear Dimension...");
			
			Dimension myDim = doc.GetElement(myDimRef) as Dimension;
			
			double dimValue = (double)myDim.Value;
			
			// Xac dinh ti so giua doan dau va doan cuoi
			
			double myFactor = (dimValue + 1/6)/beamLineCur.Length;
			
			foreach (Reference myRef in myRefList) {
						Element myEle = doc.GetElement(myRef);
//						this.sectionViewBeamY(myEle, myFactor);
					}
		}
		
		
		public void makeSectionBeamAuto()
		{
			
			UIDocument UiDoc = this.ActiveUIDocument;
			Document doc = UiDoc.Document;
			
			List<int> myListIdCategory= new List<int>();
			myListIdCategory.Add((int)BuiltInCategory.OST_StructuralFraming);
			myListIdCategory.Add((int)BuiltInCategory.OST_Walls);
			myListIdCategory.Add((int)BuiltInCategory.OST_WallsStructure);

			List<string> myFilter = new List<string>(){"Wall", "Beam"};
			
			List<Reference> myRefList = (List<Reference>)UiDoc.Selection.PickObjects(ObjectType.Element, new FilterByIdCategory(myListIdCategory),"Pick a Beam...");

			foreach (Reference myRef in myRefList) 
			{

				Element myEle = doc.GetElement(myRef);
				this.sectionViewBeamX(myEle);
//				this.sectionViewBeamY(myEle, 0.1);
			}	
		}


				
		public void makeSectionBeamAuto_pickFace()
		{
			
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			//Calculate Geometry Parameter
			
			//Pick Beam
			
			Reference myRefBeam = uiDoc.Selection.PickObject(ObjectType.Element,"Pick a Beam....");

			List<double> myListYFactor = new List<double>(){0.125, 0.5, 0.875};
			
			//TODO: Các bước tính toán hình học của dầm
			
			Element myBeam = doc.GetElement(myRefBeam);
			
			//Get location curve of beam
		    LocationCurve lc = myBeam.Location as LocationCurve;
		    Line line = lc.Curve as Line;
			
		    //Get vector of location cuver beam
            XYZ p = line.GetEndPoint(0);
		    XYZ q = line.GetEndPoint(1);
		    XYZ v = q - p; // Vector equation of line
		    
		    double beamLength = v.GetLength();
			
			
			# region Pick Faces
						
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
			if(myListFacePicked.Count !=2 && myListFacePicked.Count != 4)
			{
				TaskDialog.Show("Error!", "Chua ho tro lua chon: " + myListFacePicked.Count() + " mat, Chon 2 hoac 4 mat");
				return;
				
			}
			#endregion
			
			//Lay danh sach khoang cach tu diem dau toi cac mat da pick
			List<double> myListDistance = getAndSortDisOfEndFaces(myListFacePicked, p);
			
			//Chieu dai tinh toan cua dam
			double lengCalBeamSegment = myListDistance[myListDistance.Count-1]-myListDistance[0];
			
			//Create view Y axis, multiple view
			
			int i = 1;
			
			foreach (double myFac in myListYFactor) 
			{
				string sectionName = "Section " + i.ToString();
				
				double myFactorToDrawView = (myListDistance[0] + lengCalBeamSegment*myFac)/beamLength;
				this.sectionViewBeamY(myBeam, myFactorToDrawView, sectionName, 25);
				i++;
			}
			
			//Create view Y axis, only one View
//			this.sectionViewBeamX(myBeam);

		}
	

		// Ham nay can kiem tra lai
		private double getDisFromPointToPlaneFace(XYZ myPoint, Face myFace) // Both Nagative and Positive
		{
			Plane myPlaneFace = myFace.GetSurface() as Plane;
			
			XYZ v = myPoint - myPlaneFace.Origin;
				
			double myDis = myPlaneFace.Normal.DotProduct(v);
		
			return Math.Abs(myDis); // return only positive
		}
		
		
		
		private List<double> getAndSortDisOfEndFaces(List<Face> myListFace, XYZ myRefpoint)
		{
			List<double> myListDisOfEndFaces = new List<double>();

			
			double myDisFace;
			foreach (Face myFace in myListFace) 
			{
				myDisFace = Math.Round( getDisFromPointToPlaneFace(myRefpoint, myFace), 6);
				 if(!myListDisOfEndFaces.Contains(myDisFace))
			    {
    				 myListDisOfEndFaces.Add(myDisFace);
			    }

			}
			myListDisOfEndFaces.Sort();
			return myListDisOfEndFaces;
		}
	
		
		
		
		
		public void getBoundingBoxOfElement()
		{
			UIDocument UiDoc = this.ActiveUIDocument;
			Document doc = UiDoc.Document;
			
			//Get element by filler by Id integer of category
			
			List<int> myListIdCategory= new List<int>();
			myListIdCategory.Add((int)BuiltInCategory.OST_StructuralFraming);

			
			
			Reference myRef = UiDoc.Selection.PickObject(ObjectType.Element, new FilterByIdCategory(myListIdCategory), "Pick a Beam...");
			// 1 sau tra ve 1 element thuoc beam (structurakFraming)
			Element elem = doc.GetElement(myRef);
			
            
		    // assume curve line is straight 
		    LocationCurve lc = elem.Location as LocationCurve;		 
		    Line line = lc.Curve as Line;
            
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
            
            XYZ p = line.GetEndPoint(0);// toa do diem đầu
		    XYZ q = line.GetEndPoint(1);// tọa độ điểm cuối
		    XYZ v = q - p;// Vector chỉ phương của điểm đầu điểm cuối
		    
		    
		 	//Set boundingBox
		 	BoundingBoxXYZ bb_X = elem.get_BoundingBox(null);// Lấy BoundingBox ("hộp biên" của đối tượng dầm)
		    double minZ_X = bb_X.Min.Z;//
		    double maxZ_X = bb_X.Max.Z;
		    
		    TaskDialog.Show("Min, Max", "MinXYZ: " + bb_X.Min.ToString() + "," + bb_X.Max.ToString());
		    
		    double minY_X = bb_X.Min.Y;
		    double maxY_X = bb_X.Max.Y;
		    
		    
		    //lay cac thong so kich thuoc dam
		    	
			double w = v.GetLength(); // length of beam (endpoint - start Point)
			double h = maxZ_X - minZ_X; // h of beam
			double b = maxY_X - minY_X; // b of beam
			double offset = 2*h;// khoang cach offset
			
			string outString = "Chieu dai: " + w +"\nChieu cao: " + h + "\nBe rong: " + b;
			
			TaskDialog.Show("thong so", outString);
			
			//Phan code tren da xong tra ve 1 boudingbox, cacs thong so cua dam: w, h, b
			
			XYZ min = new XYZ( -w, minZ_X - offset, 0 );
			XYZ max = new XYZ( w, maxZ_X + offset, offset );

		}

			
		public void rebarBeam()
		{
			UIDocument UiDoc = this.ActiveUIDocument;
			Document doc = UiDoc.Document;
			
			//Get element by filler by Id integer of category
			
			List<int> myListIdCategory= new List<int>();
			myListIdCategory.Add((int)BuiltInCategory.OST_StructuralFraming);
			
			Reference myRef = UiDoc.Selection.PickObject(ObjectType.Element, new FilterByIdCategory(myListIdCategory),"Pick An Angular Dimension...");
			
			Element elem = doc.GetElement(myRef);
			ElementId elemTypeId = elem.GetTypeId();  
            Element elemType = doc.GetElement(elemTypeId);  
			
			Parameter bParam = elemType.LookupParameter("b");  
            double width = bParam.AsDouble();  
            Parameter hParam = elemType.LookupParameter("h");  
            double heigth = hParam.AsDouble();  
            Parameter lParam = elem.LookupParameter("Length");  
            double length = lParam.AsDouble();  
            double feet2millimeter = 304.8;  
            width = width * feet2millimeter;  
            heigth = heigth * feet2millimeter;  
            length = length * feet2millimeter;  
            TaskDialog.Show("Revit", "Kích thước cầu kiện:\nRộng: "+ width +"mm\nCao: "+ heigth +"mm\nDài: "+ length +"mm");  
            
            Location loc = elem.Location;
            LocationCurve locCur = loc as LocationCurve;
            Curve curve = locCur.Curve;
                        
            Line line = curve as Line;
            
            XYZ vectorX = line.Direction;
            XYZ vectorZ = XYZ.BasisZ;
            XYZ vectorY = vectorZ.CrossProduct(vectorX);
            
            TaskDialog.Show("Revit", vectorX.ToString() + vectorY.ToString() + vectorZ.ToString());
            
            using (Transaction myTrans = new Transaction(doc, "RotateElement") ){
				
				myTrans.Start();
				// Code here
				XYZ aa = line.GetEndPoint(0);
				 XYZ cc = new XYZ(aa.X, aa.Y, aa.Z + 10);
				 Line axis = Line.CreateBound(aa, cc);
				 locCur.Rotate(axis, Math.PI / 2.0);

				myTrans.Commit();
			}
            return;
		}
		
	
		// Unjoint all solid 
		public void getAllSoildIntersect()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			// Find intersections between family instances and a selected element
			Reference reference = uiDoc.Selection.PickObject(ObjectType.Element, "Select element that will be checked for intersection with all family instances");
			Element element = doc.GetElement(reference);
			GeometryElement geomElement = element.get_Geometry(new Options());
			Solid solid = null;
			IEnumerable<Element> myColumns;
			foreach (GeometryObject geomObj in geomElement)
			{
				if(geomObj is Solid)
				{
					solid = geomObj as Solid;
					break;
				}
			}
			
			myColumns = new FilteredElementCollector(doc).
				OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_StructuralColumns).
				WherePasses(new ElementIntersectsSolidFilter(solid));
			
			TaskDialog.Show("abc", myColumns.Count().ToString());
			
			foreach (Element col in myColumns) {
				TaskDialog.Show("abc", "id of element: " + col.Id);
				
			}
	
		}
		
	
		// Unjoint all solid 
		public void getAllElementIntersect()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			// Find intersections between family instances and a selected element
			Reference reference = uiDoc.Selection.PickObject(ObjectType.Element, "Select element that will be checked for intersection with all family instances");
			Element element = doc.GetElement(reference);
			GeometryElement geomElement = element.get_Geometry(new Options());
			Solid solid = null;
			IEnumerable<Element> myColumns;
			foreach (GeometryObject geomObj in geomElement)
			{
				if(geomObj is Solid)
				{
					solid = geomObj as Solid;
					break;
				}
			}
			
			myColumns = new FilteredElementCollector(doc).
				OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_StructuralColumns).
				WherePasses(new ElementIntersectsElementFilter(element));
			
			TaskDialog.Show("abc", myColumns.Count().ToString());
			
			foreach (Element col in myColumns) {
				TaskDialog.Show("abc", "id of element: " + col.Id);
				
			}
	
		}
			
	
		
		public void getAllColumnIntersectBeam()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			// Create a list to storge
			IList<ElementId> joinedElements = new List<ElementId>();
			
			// Create a listID to storge faceID
			IList<Face> facesSameDirection = new List<Face>();
			
			// Create a dic element id and 
			
			//Select an element beam:
			Reference myRefBeam = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a Beam...");
			Element myBeam = doc.GetElement(myRefBeam);
			
			//Get location curve of beam
		    LocationCurve lc = myBeam.Location as LocationCurve;
		    Line line = lc.Curve as Line;
			
		    //Get vector of location cuver beam
            XYZ p = line.GetEndPoint(0);
		    XYZ q = line.GetEndPoint(1);
		    XYZ v = q - p; // Vector equation of line
		    
		    
		    TaskDialog.Show("info", "Vector equation of beam locCurve: " + XYZToString(v));
		    
			//Get Geometry of the element(beam)
			
			GeometryElement myGeoBeams = myBeam.get_Geometry(new Options());
			
			//
			
			// Get solid from geometry of beam just has selected
			foreach(GeometryObject myGeoBeam in myGeoBeams) 
			{
				if(myGeoBeam is Solid)
				{
					Solid mySolidBeam = myGeoBeam as Solid;
					
					// Get all face of solid of geometry
					foreach (Face myFaceBeam in mySolidBeam.Faces) 
					{
						// for each face, find the other elements that generated the geometry of that face
                        ICollection<ElementId> generatingElementIds = myBeam.GetGeneratingElementIds(myFaceBeam);
                        
                        // remove the originally selected wall, leaving only other elements joined to it
                        generatingElementIds.Remove(myBeam.Id); 
                        foreach (ElementId id in generatingElementIds)
                    	{
	                        if (!(joinedElements.Contains(id)))
	                        {
                            	// add each wall joined to this face to the overall collection 
                            	joinedElements.Add(id);
                            	
                            	// get lement from id
                            	Element myE = doc.GetElement(id);
                            	
                            	// get Face from element check face //
                            	GeometryElement myGeoJoinBeams = myE.get_Geometry(new Options());
                            	
                				foreach(GeometryObject myGeoJoinBeam in myGeoJoinBeams) 
								{
	                            	if(myGeoJoinBeam is Solid)
									{
	                            		Solid mySolidJoinBeam = myGeoJoinBeam as Solid;
            							// Get all face of solid of geometry
										foreach (Face myFaceJoinBeam in mySolidJoinBeam.Faces) 
										{
											XYZ myNormVec = myFaceJoinBeam.ComputeNormal(new UV(0,0));
											if(isSameDirection(v, myNormVec)) // myE.Category.Name == "Structural Columns")
											{
												facesSameDirection.Add(myFaceJoinBeam);
												TaskDialog.Show("test", "the face has: " + myFaceJoinBeam.Area.ToString());
											}					
										}
	                            	}
                            	}	
	                        }
                        }
					}
				
				}
				
			}
			//uiDoc.Selection.SetElementIds(joinedElements);
			TaskDialog.Show("test", "number of face : " + facesSameDirection.Count().ToString());
		}
	


		public void switchJoinOrder()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			List<int> myListIdCategory= new List<int>();
			myListIdCategory.Add((int)BuiltInCategory.OST_StructuralFraming);
			
			// Select first Element (ex beam)
			Reference myRef1 = uiDoc.Selection.PickObject(ObjectType.Element, new FilterByIdCategory(myListIdCategory), "Pick a Beam...");
			//Get element1 from ref
			Element myBeam = doc.GetElement(myRef1);
			
			/// <summary>
			/// Test paint
			/// </summary>
			/// <param name="myXYZ"></param>
			/// <returns></returns>
			
			
			//Get Material Element					
			FilteredElementCollector fec = new FilteredElementCollector(doc).OfClass(typeof(Material));
			IEnumerable<Material> test =  fec.Cast<Material>();
			
    		Material materialElem = fec.
    			Cast<Material>().
    			First<Material>( myMaterial => myMaterial.Name == "Default Mass Zone" );
			
			ElementId myMaterialEleId = materialElem.Id;
			
			
//			// Select multiple element (column or beam)
//			IList<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, "Pick column or beam");
//			
			ICollection<ElementId> myListElemIdsJoined =  JoinGeometryUtils.GetJoinedElements(doc, myBeam);
//			
			using (Transaction trans = new Transaction(doc, "Switch Join")) 
			{			
				trans.Start();
				foreach (ElementId myElemId in myListElemIdsJoined) 
				{
					
					if(!JoinGeometryUtils.IsCuttingElementInJoin(doc, doc.GetElement(myElemId), myBeam))
					{
						JoinGeometryUtils.SwitchJoinOrder(doc, doc.GetElement(myElemId), myBeam);
					}
//					TaskDialog.Show("abc", "has: " + myElemId.ToString());
				}
			

				trans.Commit();
			}
			
			// List Distance
			
			List<double> myDistFaceFromEndPoint = new List<double>();
			
			
			using (Transaction trans2 = new Transaction(doc, "Paint Face" )) 
			{
				trans2.Start();
				
				// paint face
				IList<Face> facesSameDirection = new List<Face>();

				//Get location curve of beam
			    LocationCurve lc = myBeam.Location as LocationCurve;
			    Line line = lc.Curve as Line;
				
			    //Get vector of location cuver beam
	            XYZ p = line.GetEndPoint(0);
			    XYZ q = line.GetEndPoint(1);
			    XYZ v = q - p; // Vector equation of line
			    
			    //Get geo Element from myBeam
    			GeometryElement myGeoBeams = myBeam.get_Geometry(new Options());
			    
    			// Get solid from geometry of beam just has selected
				foreach(GeometryObject myGeoBeam in myGeoBeams) 
				{
					if(myGeoBeam is Solid)
					{
						Solid mySolidBeam = myGeoBeam as Solid;
						
						// Get all face of solid of geometry
						foreach (Face myFaceBeam in mySolidBeam.Faces) 
						{
							XYZ myNormVec = myFaceBeam.ComputeNormal(new UV(0,0));
							if(isSameDirection(v, myNormVec)) // myE.Category.Name == "Structural Columns")
							{
								facesSameDirection.Add(myFaceBeam);
								
								//IntersectionResult myInterSect =  myFaceBeam.Project(p);
								double myDisPointToFace = Math.Round(getDisFromPointToPlaneFace(p, myFaceBeam), 6);
								
								myDistFaceFromEndPoint.Add(myDisPointToFace);
								
								doc.Paint(myBeam.Id, myFaceBeam, myMaterialEleId);
								TaskDialog.Show("test", Environment.NewLine + "V: " + XYZToString(v) +
								                Environment.NewLine + "nor: " + XYZToString(myNormVec) +
								                "Dis: " + myDisPointToFace.ToString());
							}							
						}	
					}
				}
				trans2.Commit();
			}

			
		}
		
		

		public List<double> testgetLengthSegBeamAA()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			List<int> myListIdCategory= new List<int>();
			myListIdCategory.Add((int)BuiltInCategory.OST_StructuralFraming);
			
			// Select first Element (ex beam)
			Reference myRef1 = uiDoc.Selection.PickObject(ObjectType.Element, new FilterByIdCategory(myListIdCategory), "Pick a Beam...");
			//Get element1 from ref
			Element myBeam = doc.GetElement(myRef1);
			
			/// <summary>
			/// Test paint
			/// </summary>
			/// <param name="myXYZ"></param>
			/// <returns></returns>
			
			
			//Get Material Element					
			FilteredElementCollector fec = new FilteredElementCollector(doc).OfClass(typeof(Material));
			IEnumerable<Material> test =  fec.Cast<Material>();
			
    		Material materialElem = fec.
    			Cast<Material>().
    			First<Material>( myMaterial => myMaterial.Name == "Default Mass Zone" );
			
			ElementId myMaterialEleId = materialElem.Id;
			
			
//			// Select multiple element (column or beam)
//			IList<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, "Pick column or beam");
//			
			ICollection<ElementId> myListElemIdsJoined =  JoinGeometryUtils.GetJoinedElements(doc, myBeam);
//			
			using (Transaction trans = new Transaction(doc, "Switch Join")) 
			{			
				trans.Start();
				foreach (ElementId myElemId in myListElemIdsJoined) 
				{
					
					if(!JoinGeometryUtils.IsCuttingElementInJoin(doc, doc.GetElement(myElemId), myBeam))
					{
						JoinGeometryUtils.SwitchJoinOrder(doc, doc.GetElement(myElemId), myBeam);
					}
//					TaskDialog.Show("abc", "has: " + myElemId.ToString());
				}
			

				trans.Commit();
			}
			
			// List Distance
			
			List<double> myDistFaceFromEndPoint = new List<double>();
			
			
			using (Transaction trans2 = new Transaction(doc, "Paint Face" )) 
			{
				trans2.Start();
				
				// paint face
				IList<Face> facesSameDirection = new List<Face>();

				//Get location curve of beam
			    LocationCurve lc = myBeam.Location as LocationCurve;
			    Line line = lc.Curve as Line;
				
			    //Get vector of location cuver beam
	            XYZ p = line.GetEndPoint(0);
			    XYZ q = line.GetEndPoint(1);
			    XYZ v = q - p; // Vector equation of line
			    
			    //Get geo Element from myBeam
    			GeometryElement myGeoBeams = myBeam.get_Geometry(new Options());
			    
    			// Get solid from geometry of beam just has selected
				foreach(GeometryObject myGeoBeam in myGeoBeams) 
				{
					if(myGeoBeam is Solid)
					{
						Solid mySolidBeam = myGeoBeam as Solid;
						
						// Get all face of solid of geometry
						foreach (Face myFaceBeam in mySolidBeam.Faces) 
						{
							XYZ myNormVec = myFaceBeam.ComputeNormal(new UV(0,0));
							if(isSameDirection(v, myNormVec)) // myE.Category.Name == "Structural Columns")
							{
								facesSameDirection.Add(myFaceBeam);
								
								//IntersectionResult myInterSect =  myFaceBeam.Project(p);
								double myDisPointToFace = Math.Round(getDisFromPointToPlaneFace(p, myFaceBeam), 6);
								
								myDistFaceFromEndPoint.Add(myDisPointToFace);
								
								doc.Paint(myBeam.Id, myFaceBeam, myMaterialEleId);
								TaskDialog.Show("test", Environment.NewLine + "V: " + XYZToString(v) +
								                Environment.NewLine + "nor: " + XYZToString(myNormVec) +
								                "Dis: " + myDisPointToFace.ToString());
							}							
						}	
					}
				}
				trans2.Commit();
			}
			
			List<double> myReturnList = myDistFaceFromEndPoint.GetRange(1,2);
			
			return myReturnList;
			
		}
		
		

		/// <summary>
		/// Pick 1 dam, 2 cot tra ve khoang cach giua 2 mat gan nhat cua cot
		/// </summary>
		/// <returns></returns>
		private void testgetLengthSegBeam2()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			
			//Pick Beam
			
			List<int> myListIdCategoryBeam = new List<int>();
			myListIdCategoryBeam.Add((int)BuiltInCategory.OST_StructuralFraming);
			
			// Select first Element (ex beam)
			Reference myRef1 = uiDoc.Selection.PickObject(ObjectType.Element, new FilterByIdCategory(myListIdCategoryBeam), "Pick a Beam...");
			//Get element1 from ref
			Element myBeam = doc.GetElement(myRef1);
			
			
			// Pick Rebar
			
			List<int> myListIdCategoryRebar = new List<int>();
			myListIdCategoryRebar.Add((int)BuiltInCategory.OST_Rebar);
			
			// Select first Element (ex beam)
			Reference myRefRebar = uiDoc.Selection.PickObject(ObjectType.Element, new FilterByIdCategory(myListIdCategoryRebar), "Pick a Beam...");
			//Get element1 from ref
			Element myRebar = doc.GetElement(myRefRebar);
			
			
			// Pick Column 1
			List<int> myListIdCategoryFirstCol = new List<int>();
			myListIdCategoryFirstCol.Add((int)BuiltInCategory.OST_StructuralColumns);
			
			// Select first Element (ex beam)
			Reference myRefCol1 = uiDoc.Selection.PickObject(ObjectType.Element, new FilterByIdCategory(myListIdCategoryFirstCol), "Pick first Column...");
			//Get element1 from ref
			Element myCol1 = doc.GetElement(myRefCol1);
			

			// Pick Column 1
			List<int> myListIdCategorySecondCol = new List<int>();
			myListIdCategorySecondCol.Add((int)BuiltInCategory.OST_StructuralColumns);
			
			// Select first Element (ex beam)
			Reference myRefCol2 = uiDoc.Selection.PickObject(ObjectType.Element, new FilterByIdCategory(myListIdCategorySecondCol), "Pick second Column...");
			//Get element1 from ref
			Element myCol2 = doc.GetElement(myRefCol2);
			
			#region Switch Join (beam cutting)
//			// Switch Join (Joint all other element to beam element (beam cutting))
//
//			ICollection<ElementId> myListElemIdsJoined =  JoinGeometryUtils.GetJoinedElements(doc, myBeam);		
//			using (Transaction trans = new Transaction(doc, "Switch Join")) 
//			{			
//				trans.Start();
//				foreach (ElementId myElemId in myListElemIdsJoined) 
//				{
//					
//					if(!JoinGeometryUtils.IsCuttingElementInJoin(doc, doc.GetElement(myElemId), myBeam))
//					{
//						JoinGeometryUtils.SwitchJoinOrder(doc, doc.GetElement(myElemId), myBeam);
//					}
//				}
			
//
//				trans.Commit();
//			}
			#endregion
			
			// List Distance
			List<double> myDistFaceFromEndPoint = new List<double>();
			
			
			using (Transaction trans2 = new Transaction(doc, "Get Distance From face to face" )) 
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
			    
			    
			    // List Column Element
			    List<Element> myColumns = new List<Element>(){myCol1, myCol2};
			    
			    // For each col, get all face of col has same direction
			    foreach (Element myCol in myColumns) 
			    {
			    	
			    	GeometryElement myColGeos = myCol.get_Geometry(new Options());
			    	
					foreach(GeometryObject myColGeo in myColGeos) 
					{
						if(myColGeo is Solid)
						{
							Solid myColSolid = myColGeo as Solid;
							
							// Get all face of solid of geometry
							foreach (Face myColFace in myColSolid.Faces) 
							{
								XYZ myNormVec = myColFace.ComputeNormal(new UV(0,0));
								if(isSameDirection(v, myNormVec)) // myE.Category.Name == "Structural Columns")
								{
									facesSameDirection.Add(myColFace);
									
									//IntersectionResult myInterSect =  myColFace.Project(p);
									
									// NOTE: ROUND DISTANCE
									double myDisPointToFace = Math.Round(getDisFromPointToPlaneFace(p, myColFace), 6);
									
									myDistFaceFromEndPoint.Add(myDisPointToFace);
								}							
							}	
						}
					}
			    }

				trans2.Commit();
			}
			
			myDistFaceFromEndPoint.Sort();
			
			List<double> myReturnList = myDistFaceFromEndPoint.GetRange(1,2);
			
			TaskDialog.Show("RESULT", "Dis1: " + myReturnList[0] + Environment.NewLine + "Dis2: " + myReturnList[1]);

		}
		
		

		
		/// <summary>
		/// Pick 1 dam, 2 cot tra ve khoang cach giua 2 mat gan nhat cua cot
		/// </summary>
		/// <returns></returns>
		private List<double> getEndsSegBeam(ElementId myBeamId, ElementId myCol1Id,  ElementId myCol2Id)
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			
			Element myBeam = doc.GetElement(myBeamId);
			
			//Get element1 from ref
			Element myCol1 = doc.GetElement(myCol1Id);
			
			
			//Get element1 from ref
			Element myCol2 = doc.GetElement(myCol2Id);
			
			
			// List Distance
			List<double> myDistFaceFromEndPoint = new List<double>();
			
			
			using (Transaction trans2 = new Transaction(doc, "Get Distance From face to face" )) 
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
			    
			    XYZ checkPoint = new XYZ(p.X, p.Y, myBeam.get_BoundingBox(null).Min.Z-0.01);

			    
			    // List Column Element
			    List<Element> myColumns = new List<Element>(){myCol1, myCol2};
			    
			    // For each col, get all face of col has same direction
			    foreach (Element myCol in myColumns) 
			    {
			    	
			    	GeometryElement myColGeos = myCol.get_Geometry(new Options());
			    	
					foreach(GeometryObject myColGeo in myColGeos) 
					{
						if(myColGeo is Solid)
						{
							Solid myColSolid = myColGeo as Solid;
							
							// Get all face of solid of geometry
							foreach (Face myColFace in myColSolid.Faces) 
							{
								XYZ myNormVec = myColFace.ComputeNormal(new UV(0,0));
								if(isSameDirection(v, myNormVec)) 
								{
									facesSameDirection.Add(myColFace);
									
									//IntersectionResult myInterSect =  myColFace.Project(checkPoint);
									
									// NOTE: ROUND DISTANCE
									double myDisPointToFace = Math.Round(getDisFromPointToPlaneFace(p, myColFace), 6);
									
									if(!myDistFaceFromEndPoint.Contains(myDisPointToFace))
								   {
									   myDistFaceFromEndPoint.Add(myDisPointToFace);
									   
								   }
								}							
							}	
						}
					}
			    }

				trans2.Commit();
			}
			
			if(myDistFaceFromEndPoint.Count() < 1)
			
			{
				TaskDialog.Show("abc", "Point face out");
			
			}
			
			myDistFaceFromEndPoint.Sort();
			
			//List<double> myReturnList = myDistFaceFromEndPoint.GetRange(1,2);
			List<double> myReturnList = new List<double>(){myDistFaceFromEndPoint[1], myDistFaceFromEndPoint[2]};
			
			TaskDialog.Show("RESULT", "Dis1: " + myReturnList[0] + Environment.NewLine + "Dis2: " + myReturnList[1]);
			
			return myReturnList;
		}
		

		
				
		/// <summary>
		/// Pick 1 dam, 2 cot 
		/// </summary>
		/// <returns></returns>
		private List<double> getEndsSegBeam2(ElementId myBeamId, List<Element> myIntersectWithBeam)
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			

			Element myBeam = doc.GetElement(myBeamId);
			

			// List Distance
			List<double> myDistFaceFromEndPoint = new List<double>();
			
			
			using (Transaction trans2 = new Transaction(doc, "Get Distance From face to face" )) 
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
			    XYZ checkPointBeam = new XYZ(p.X, p.Y, myBeam.get_BoundingBox(null).Max.Z-0.01);
			    XYZ checkPointCol = new XYZ(p.X, p.Y, myBeam.get_BoundingBox(null).Min.Z-0.01);

			    
			    // List Column Element
			    List<Element> myColumns = myIntersectWithBeam;
			    
			    // For each col, get all face of col has same direction
			    foreach (Element myInterElem in myIntersectWithBeam) 
			    {
			    	
			    	GeometryElement myInterElemGeos = myInterElem.get_Geometry(new Options());
			    	
					foreach(GeometryObject myInterElemGeo in myInterElemGeos) 
					{
						if(myInterElemGeo is Solid)
						{
							Solid myInterElemSolid = myInterElemGeo as Solid;
							
							// Get all face of solid of geometry
							foreach (Face myInterElemFace in myInterElemSolid.Faces) 
							{
								XYZ myNormVec = myInterElemFace.ComputeNormal(new UV(0,0));
								if(isSameDirection(v, myNormVec)) // myE.Category.Name == "Structural Columns")
								{
									facesSameDirection.Add(myInterElemFace);
//									IntersectionResult myInterSect;
//									if(myInterElem.Category.Name == "Structural Framing")
//									{
//										myInterSect =  myInterElemFace.Project(checkPointBeam);
//									}
//									
//									else
//									{
//										myInterSect =  myInterElemFace.Project(checkPointCol);
//									}

									
									// NOTE: ROUND DISTANCE
									double myDisPointToFace = Math.Round(getDisFromPointToPlaneFace(p, myInterElemFace), 6);
									
									if(!myDistFaceFromEndPoint.Contains(myDisPointToFace))
								   {
						   				myDistFaceFromEndPoint.Add(myDisPointToFace);
								   }
									

								}							
							}	
						}
					}
			    }

				trans2.Commit();
			}
			
			if(myDistFaceFromEndPoint.Count() < 1)
			
			{
				TaskDialog.Show("abc", "Point face out");
			
			}
			
			myDistFaceFromEndPoint.Sort();
			
			
			//List<double> myReturnList = myDistFaceFromEndPoint.GetRange(1,2);
			List<double> myReturnList = myDistFaceFromEndPoint.GetRange(1, myDistFaceFromEndPoint.Count() - 2);
			
			
			string outPut = "Dis: ";
			foreach (double myDist in myReturnList) {
				outPut += Environment.NewLine + myDist.ToString() + ",";
				
			}
			
			TaskDialog.Show("RESULT", outPut);
			TaskDialog.Show("Result", "Length SegmentBeam" + (myReturnList[myReturnList.Count()-1] - myReturnList[0]));
			
			return myReturnList;
		}


		
		public List<double> getDisFromPick_BK()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
		
			// Pick Rebar
			
			List<int> myListIdCategoryRebar = new List<int>();
			myListIdCategoryRebar.Add((int)BuiltInCategory.OST_Rebar);
			
			// Select first Element (ex beam)
			Reference myRefRebar = uiDoc.Selection.PickObject(ObjectType.Element, 
			                                                  new FilterByIdCategory(myListIdCategoryRebar),
			                                                  "Pick a Rebar...");
			//Get rebar from ref
			Rebar myRebar = doc.GetElement(myRefRebar) as Rebar;
			
			
			Element myBeam = doc.GetElement(myRebar.GetHostId());

			//Set current Beam be Joined
			
			setBeJoined(myBeam);
			
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
			
			
			List<double> myDists = getEndsSegBeam2(myBeam.Id, myInterSec);
			return myDists;
		}


		

		public List<double> getDisFromPick()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
		
			// Pick Rebar
			
			List<int> myListIdCategoryRebar = new List<int>();
			myListIdCategoryRebar.Add((int)BuiltInCategory.OST_Rebar);
			
			// Select first Element (ex beam)
			Reference myRefRebar = uiDoc.Selection.PickObject(ObjectType.Element, 
			                                                  new FilterByIdCategory(myListIdCategoryRebar),
			                                                  "Pick a Rebar...");
			//Get rebar from ref
			Rebar myRebar = doc.GetElement(myRefRebar) as Rebar;
			
			
			Element myBeam = doc.GetElement(myRebar.GetHostId());

			//Set current Beam be Joined
			
			setBeJoined(myBeam);
			
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
			
			
			List<double> myDists = getEndsSegBeam2(myBeam.Id, myInterSec);
			return myDists;
		}



		public void getDisFromPick_Test()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
		
			// Pick Rebar
			
			List<int> myListIdCategoryRebar = new List<int>();
			myListIdCategoryRebar.Add((int)BuiltInCategory.OST_Rebar);
			
			// Select first Element (ex beam)
			Reference myRefRebar = uiDoc.Selection.PickObject(ObjectType.Element, 
			                                                  new FilterByIdCategory(myListIdCategoryRebar),
			                                                  "Pick a Rebar...");
			//Get rebar from ref
			Rebar myRebar = doc.GetElement(myRefRebar) as Rebar;
			
			
			Element myBeam = doc.GetElement(myRebar.GetHostId());

			//Set current Beam be Joined
			
			setBeJoined(myBeam);
			
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
			
			
			List<double> myDists = getEndsSegBeam2(myBeam.Id, myInterSec);
		}
				
	
		
		private double getDelta_0(Rebar myRebar)
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			//Get diameter of rebar
			
			RebarBarType myRbType = doc.GetElement(myRebar.GetTypeId()) as RebarBarType;
			double myRebarDiameter = myRbType.BarDiameter;
			
			// Get host of rebar
			
			ElementId myBeamId = myRebar.GetHostId();
			
			Element myBeam = doc.GetElement(myBeamId);
			
			if(myBeam.Category.Name != "Structural Framing")
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

			    XYZ middlePoint =   myRebar.get_BoundingBox(null).Max.Add(myRebar.get_BoundingBox(null).Min)/2;
			    
			    // NOTE LAM TRON SO
			    
			    
			    double delta_0 = Math.Sqrt(Math.Pow(middlePoint.X - p.X, 2) + Math.Pow(middlePoint.Y - p.Y, 2))- myRebarDiameter/2;
			    
			    return delta_0;
			}
			
		}


		
		private double getDelta_02(Rebar myRebar)
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			//Get diameter of rebar
			
			RebarBarType myRbType = doc.GetElement(myRebar.GetTypeId()) as RebarBarType;
			double myRebarDiameter = myRbType.BarDiameter;
			
			// Get host of rebar
			
			ElementId myBeamId = myRebar.GetHostId();
			
			Element myBeam = doc.GetElement(myBeamId);
			
			if(myBeam.Category.Name != "Structural Framing")
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

			    XYZ middlePoint =   myRebar.get_BoundingBox(null).Max.Add(myRebar.get_BoundingBox(null).Min)/2;
			    
			    Plane myReBarPlane =  Plane.CreateByNormalAndOrigin(v, middlePoint);
			    
			    // NOTE LAM TRON SO
			    
    			XYZ v1 = p - myReBarPlane.Origin;
			
    			double delta_0 = Math.Abs(myReBarPlane.Normal.DotProduct(v1)) - myRebarDiameter/2;
			    
			    
			    
			    //double delta_0 = Math.Sqrt(Math.Pow(middlePoint.X - p.X, 2) + Math.Pow(middlePoint.Y - p.Y, 2))- myRebarDiameter/2;
			    
			    return delta_0;
			}
			
		}
	
		
		
		private double getDelta_0_Update(Rebar myRebar, Face myFace)
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			//Get diameter of rebar
			
			RebarBarType myRbType = doc.GetElement(myRebar.GetTypeId()) as RebarBarType;
			double myRebarDiameter = myRbType.BarDiameter;
			
			// Get host of rebar
			
			ElementId myBeamId = myRebar.GetHostId();
			
			Element myBeam = doc.GetElement(myBeamId);
			
			if(myBeam.Category.Name != "Structural Framing")
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

			    XYZ middlePoint =   myRebar.get_BoundingBox(null).Max.Add(myRebar.get_BoundingBox(null).Min)/2;
			    
			    // NOTE LAM TRON SO
			    
			    double delta_0 = getDisFromPointToPointByPlaneFace(middlePoint, p, myFace);
			    
			    //double delta_0 = Math.Sqrt(Math.Pow(middlePoint.X - p.X, 2) + Math.Pow(middlePoint.Y - p.Y, 2))- myRebarDiameter/2;
			    
			    return delta_0;
			}
			
		}
	

		
		private List<ElementId> copyRebarByDistance(Rebar myRebar, List<double> myDistances)
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			//Get Id rebar
			ElementId myIdRebar = myRebar.Id;
			
			
			//Get Id Beam
			ElementId myIdBeam = myRebar.GetHostId();
			
			Element myBeam = doc.GetElement(myIdBeam);
			
			
			if(myBeam.Category.Name != "Structural Framing")
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

				
				double delta0 = getDelta_02(myRebar);
				
				if(delta0 == 10000000) return null;
				
				foreach (double distance in myDistances) 
				{
	
					XYZ myPointPlace = ((distance - delta0)/lengCurLine)*v;
					myCoors.Add(myPointPlace);
				}
				
				if( myCoors.Count < 1)
				{
					TaskDialog.Show("Loi!", "Khong the copy...");
					return null;			
				}
				
				ICollection<ElementId> myRebarIdCol;
				List<ElementId> myListIdRebar = new List<ElementId>();
				
				// using transcation (edit DB)
				using (Transaction myTrans = new Transaction(doc,"CopyElementByCoordinate"))
				
				{
					myTrans.Start();		
					foreach(XYZ myXYZ in myCoors)
					{
						myRebarIdCol =  ElementTransformUtils.CopyElement(doc, myIdRebar, myXYZ);
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
		

		
		private List<ElementId> copyRebarByDistance_Update(Rebar myRebar, Face myFace, List<double> myDistances)
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			//Get Id rebar
			ElementId myIdRebar = myRebar.Id;
			
			
			//Get Id Beam
			ElementId myIdBeam = myRebar.GetHostId();
			
			Element myBeam = doc.GetElement(myIdBeam);
			
			
			if(myBeam.Category.Name != "Structural Framing")
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

				
				double delta0 = getDelta_0_Update(myRebar, myFace);
				
				if(delta0 == 10000000) return null;
				
				foreach (double distance in myDistances) 
				{
	
					XYZ myPointPlace = ((distance - delta0)/lengCurLine)*v;
					myCoors.Add(myPointPlace);
				}
				
				if( myCoors.Count < 1)
				{
					TaskDialog.Show("Loi!", "Khong the copy...");
					return null;			
				}
				
				ICollection<ElementId> myRebarIdCol;
				List<ElementId> myListIdRebar = new List<ElementId>();
				
				// using transcation (edit DB)
				using (Transaction myTrans = new Transaction(doc,"CopyElementByCoordinate"))
				
				{
					myTrans.Start();		
					foreach(XYZ myXYZ in myCoors)
					{
						myRebarIdCol =  ElementTransformUtils.CopyElement(doc, myIdRebar, myXYZ);
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
		

		
		private List<ElementId> copyRebarByDistance2(Rebar myRebar, Dictionary<double, int> myDicDisNum)
		{
			
			List<double> myDistances = myDicDisNum.Keys.ToList();
			myDistances.Sort();
			
			List<int> myListNum = new List<int>();
			
			foreach (double key in myDistances) {
				myListNum.Add(myDicDisNum[key]);
			}
			
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			//Get Id rebar
			ElementId myIdRebar = myRebar.Id;
			
			
			//Get Id Beam
			ElementId myIdBeam = myRebar.GetHostId();
			
			Element myBeam = doc.GetElement(myIdBeam);
			
			
			if(myBeam.Category.Name != "Structural Framing")
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

				
				double delta0 = getDelta_02(myRebar);
				
				if(delta0 == 10000000) return null;
				
				foreach (double distance in myDistances) 
				{
	
					XYZ myPointPlace = ((distance - delta0)/lengCurLine)*v;
					myCoors.Add(myPointPlace);
				}
				
				if( myCoors.Count < 1)
				{
					TaskDialog.Show("Loi!", "Khong the copy...");
					return null;			
				}
				
				ICollection<ElementId> myRebarIdCol;
				List<ElementId> myListIdRebar = new List<ElementId>();
				
				// using transcation (edit DB)
				using (Transaction myTrans = new Transaction(doc,"CopyElementByCoordinate"))
				
				{
					myTrans.Start();		
					foreach(XYZ myXYZ in myCoors)
					{
						myRebarIdCol =  ElementTransformUtils.CopyElement(doc, myIdRebar, myXYZ);
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
		
		
		private List<ElementId> copyRebarByDistance2_Update(Rebar myRebar, Dictionary<double, int> myDicDisNum)
		{
			
			List<double> myDistances = myDicDisNum.Keys.ToList();
			myDistances.Sort();
			
			List<int> myListNum = new List<int>();
			
			foreach (double key in myDistances) {
				myListNum.Add(myDicDisNum[key]);
			}
			
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			//Get Id rebar
			ElementId myIdRebar = myRebar.Id;
			
			
			//Get Id Beam
			ElementId myIdBeam = myRebar.GetHostId();
			
			Element myBeam = doc.GetElement(myIdBeam);
			
			
			if(myBeam.Category.Name != "Structural Framing")
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

				XYZ p = p1 - 0.1*v;
				
				List<XYZ> myCoors = new List<XYZ>();
				
			    // NOTE LAM TRON SO
			    
    			//Get diameter of rebar
			
			    XYZ middlePoint =   myRebar.get_BoundingBox(null).Max.Add(myRebar.get_BoundingBox(null).Min)/2;
			    
			    List<Curve> centerLines = myRebar.GetCenterlineCurves(false, false, false,
			                                                         MultiplanarOption.IncludeOnlyPlanarCurves,0)
			    	as List<Curve>;
			    
			    
			    foreach (Curve myCurBar in centerLines) 
			    {
			    	middlePoint = myCurBar.GetEndPoint(0);
			    	break;
			    }
			    
			    Plane myReBarPlane =  Plane.CreateByNormalAndOrigin(v, middlePoint);
    			
    			
    			// Distance from first rebar to
				RebarBarType myRbType = doc.GetElement(myRebar.GetTypeId()) as RebarBarType;
				double myRebarDiameter = myRbType.BarDiameter;
			    
			    
    			XYZ v1 = p - myReBarPlane.Origin;
			
    			double delta_0 = Math.Abs(myReBarPlane.Normal.DotProduct(v1));
				
				if(delta_0 == 10000000) return null;
				
				foreach (double distance in myDistances) 
				{
	
					XYZ myPointPlace = ((distance - delta_0)/lengCurLine)*v;
					myCoors.Add(myPointPlace);
				}
				
				if( myCoors.Count < 1)
				{
					TaskDialog.Show("Loi!", "Khong the copy...");
					return null;			
				}
				
				ICollection<ElementId> myRebarIdCol;
				List<ElementId> myListIdRebar = new List<ElementId>();
				
				// using transcation (edit DB)
				using (Transaction myTrans = new Transaction(doc,"CopyElementByCoordinate"))
				
				{
					myTrans.Start();		
					foreach(XYZ myXYZ in myCoors)
					{
						myRebarIdCol =  ElementTransformUtils.CopyElement(doc, myIdRebar, myXYZ);
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
					
		
		private List<ElementId> copyRebarByDistance2_Update2(Rebar myRebar, Dictionary<double, int> myDicDisNum)
		{
			
			List<double> myDistances = myDicDisNum.Keys.ToList();
			myDistances.Sort();
			
			List<int> myListNum = new List<int>();
			
			foreach (double key in myDistances) {
				myListNum.Add(myDicDisNum[key]);
			}
			
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			//Get Id rebar
			ElementId myIdRebar = myRebar.Id;
			
			
			//Get Id Beam
			ElementId myIdBeam = myRebar.GetHostId();
			
			Element myBeam = doc.GetElement(myIdBeam);
			
			
			if(myBeam.Category.Name != "Structural Framing")
			{
				TaskDialog.Show("Loi!", "Hay chon 1 rebar co host la 1 Structural Framing");
				return null;
			}
			
			else 
			{
				LocationCurve cur = myBeam.Location as LocationCurve;
				Line lineCur = cur.Curve as Line;
				
				XYZ p1 = getPointCenterTopLine(myBeam)[0];
				XYZ p2 = getPointCenterTopLine(myBeam)[1];
			
			
				Line centerLineBeam = Line.CreateBound(p1, p2);
				
				
				XYZ p = p1;
				XYZ q = p2;
				XYZ v = q - p;
				double lengCurLine = v.GetLength();

				
				List<XYZ> myCoors = new List<XYZ>();
				
			    // NOTE LAM TRON SO
			    
    			//Get diameter of rebar
			
			    XYZ middlePoint =   myRebar.get_BoundingBox(null).Max.Add(myRebar.get_BoundingBox(null).Min)/2;
			    
			    List<Curve> centerLines = myRebar.GetCenterlineCurves(false, false, false,
			                                                         MultiplanarOption.IncludeOnlyPlanarCurves,0)
			    	as List<Curve>;
			    
			    
			    foreach (Curve myCurBar in centerLines) 
			    {
			    	middlePoint = myCurBar.GetEndPoint(0);
			    	break;
			    }
			    
			    Plane myReBarPlane =  Plane.CreateByNormalAndOrigin(v, middlePoint);
    			
    			
    			// Distance from first rebar to
				RebarBarType myRbType = doc.GetElement(myRebar.GetTypeId()) as RebarBarType;
				double myRebarDiameter = myRbType.BarDiameter;
			    
			    
    			XYZ v1 = p - myReBarPlane.Origin;
			
    			double delta_0 = Math.Abs(myReBarPlane.Normal.DotProduct(v1));
				
				if(delta_0 == 10000000) return null;
				
				foreach (double distance in myDistances) 
				{
	
					XYZ myPointPlace = ((distance - delta_0)/lengCurLine)*v;
					myCoors.Add(myPointPlace);
				}
				
				if( myCoors.Count < 1)
				{
					TaskDialog.Show("Loi!", "Khong the copy...");
					return null;			
				}
				
				ICollection<ElementId> myRebarIdCol;
				List<ElementId> myListIdRebar = new List<ElementId>();
				
				// using transcation (edit DB)
				using (Transaction myTrans = new Transaction(doc,"CopyElementByCoordinate"))
				
				{
					myTrans.Start();		
					foreach(XYZ myXYZ in myCoors)
					{
						myRebarIdCol =  ElementTransformUtils.CopyElement(doc, myIdRebar, myXYZ);
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
					

				
		public void rebarBeam2Col(double factor, double delta_1, double pitch_1, double pitch_2, Rebar myRebar,List<Element> myInterSec )
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			
			// Get baem Id
			ElementId myBeamId = myRebar.GetHostId();
			
			double lenSegment;
			List<double> myListDis = new List<double>();
		
			
			ElementId myCol1Id = myInterSec[0].Id;
			ElementId myCol2Id = myInterSec[1].Id;

			
			myListDis = getEndsSegBeam(myBeamId, myCol1Id, myCol2Id);
			lenSegment = myListDis[1] - myListDis[0];
			
			
			
			List<double> myListDisDetail =  detailListDistance(myListDis, factor, delta_1, pitch_1);
			
			
			List<ElementId> myListRebarCopyId =  copyRebarByDistance(myRebar, myListDisDetail);
			
			// RebarSet Layout
			

			using (Transaction trans = new Transaction(doc, "Change rebar set")) 
			{
				trans.Start();
				
				for (int i = 0; i < myListRebarCopyId.Count; i++) 
				{
					ElementId rebarId = myListRebarCopyId[i];
					
					Rebar myRebarI = doc.GetElement(rebarId) as Rebar;
					
					int numberRebar = 2;
					
					if(i == myListRebarCopyId.Count-1)
					{
						numberRebar = (int)((lenSegment/factor)/pitch_1);
						
						myRebarI.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing(numberRebar, pitch_1, true, true, true);
					}

					else if(i == 0)
					{
						numberRebar = (int)((lenSegment/factor)/pitch_1);
						myRebarI.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing(numberRebar, pitch_1, false, true, true);
					}
					
					else if(i == 1)
					{
						
						double delta_2 = (lenSegment/factor - delta_1)%pitch_1 ;
						double len2 = (lenSegment - 2*(lenSegment/factor) + delta_2);
						
						numberRebar = (int)((lenSegment - 2*(lenSegment/factor) + delta_2)/pitch_2) + 1;
						myRebarI.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing(numberRebar, pitch_2, false, true, true);
					}
					
				}	

				trans.Commit();
			}

		
		}
	
	
				
		public void rebarBeam2Col_Update(double factor, double delta_1, double pitch_1, double pitch_2, Rebar myRebar, Face myFace, List<Element> myInterSec )
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			
			// Get baem Id
			ElementId myBeamId = myRebar.GetHostId();
			
			double lenSegment;
			List<double> myListDis = new List<double>();
		
			
			ElementId myCol1Id = myInterSec[0].Id;
			ElementId myCol2Id = myInterSec[1].Id;

			
			myListDis = getEndsSegBeam(myBeamId, myCol1Id, myCol2Id);
			lenSegment = myListDis[1] - myListDis[0];
			
			
			
			List<double> myListDisDetail =  detailListDistance(myListDis, factor, delta_1, pitch_1);
			
			
			List<ElementId> myListRebarCopyId =  copyRebarByDistance_Update(myRebar, myFace, myListDisDetail);
			
			// RebarSet Layout
			

			using (Transaction trans = new Transaction(doc, "Change rebar set")) 
			{
				trans.Start();
				
				for (int i = 0; i < myListRebarCopyId.Count; i++) 
				{
					ElementId rebarId = myListRebarCopyId[i];
					
					Rebar myRebarI = doc.GetElement(rebarId) as Rebar;
					
					int numberRebar = 2;
					
					if(i == myListRebarCopyId.Count-1)
					{
						numberRebar = (int)((lenSegment/factor)/pitch_1);
						
						myRebarI.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing(numberRebar, pitch_1, true, true, true);
					}

					else if(i == 0)
					{
						numberRebar = (int)((lenSegment/factor)/pitch_1);
						myRebarI.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing(numberRebar, pitch_1, false, true, true);
					}
					
					else if(i == 1)
					{
						
						double delta_2 = (lenSegment/factor - delta_1)%pitch_1 ;
						double len2 = (lenSegment - 2*(lenSegment/factor) + delta_2);
						
						numberRebar = (int)((lenSegment - 2*(lenSegment/factor) + delta_2)/pitch_2) + 1;
						myRebarI.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing(numberRebar, pitch_2, false, true, true);
					}
					
				}	

				trans.Commit();
			}

		
		}
	
			
						
		public void rebarBeam_Form()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			double factor = 4;
			double delta_1 = 50/304.8;
			double pitch_1 = 150/304.8 ;
			double pitch_2 = 300/304.8;
			
			double delta_3 = 50/304.8;
			double pitch_3 = 100/304.8;
			int N3 = 5;
			
			double myConvertFactor = 304.8;

			
			bool inputSuccess = false;
			while (!inputSuccess) 
			{
				using(var myInputFormSetting = new InputDialog())
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
				    if(myInputFormSetting.DialogResult == System.Windows.Forms.DialogResult.Cancel) return;
				    {
				    	//else do all this :)    
				    	myInputFormSetting.Close();
				    }
				    
				    if(myInputFormSetting.DialogResult == System.Windows.Forms.DialogResult.OK)
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

			using (Transaction myTrans = new Transaction(doc,"sET FIRST REBAR AS SINGLE"))
			
			{
				myTrans.Start();
				myRebar.GetShapeDrivenAccessor().SetLayoutAsSingle();
				myTrans.Commit();
			}
			

			Element myBeam = doc.GetElement(myRebar.GetHostId());

			//Set current Beam be Joined
			
			//setBeJoined(myBeam);
			
			while (true) 
			{
			
			List<int> myListBeamCol = new List<int>();
			myListBeamCol.Add((int)BuiltInCategory.OST_StructuralFraming);
			myListBeamCol.Add((int)BuiltInCategory.OST_StructuralColumns);
			myListBeamCol.Add((int)BuiltInCategory.OST_Walls);
			// Select first Element (ex beam)
			List<Reference> myListInterRef = uiDoc.Selection.PickObjects(ObjectType.Element, 
			                                                  new FilterByIdCategory(myListBeamCol),
			                                                  "Pick a Beam and Col...") as List<Reference>;
			
			List<Element> myInterSec = new List<Element>();
			
			
			foreach (Reference myRef in myListInterRef) 
			{
				myInterSec.Add(doc.GetElement(myRef));
			}
			

			if( myInterSec.Count > 3)
			{
				TaskDialog.Show("Erorr!!!", "Các đối tượng được chọn phải có đủ 2 cột và tối đa 1 dầm");
				//return;
			}
			
			// Kiem tra truong hop tinh
			if(myInterSec.Count() == 2)
			{
				rebarBeam2Col(factor, delta_1, pitch_1, pitch_2, myRebar, myInterSec);
			
			}
			
			else
			{
				List<double> myListEndPointDis = getEndsSegBeam2(myBeam.Id, myInterSec);
				myListEndPointDis.Sort();
					
	
				Dictionary<double, int> myDicDisNumDetail = detailListDistance2(myListEndPointDis,
				                                                                factor,
				                                                                delta_1, pitch_1,
				                                                                pitch_2,
				                                                                delta_3, pitch_3, N3);
				
				
				List<double> myListSpace = new List<double>(){pitch_1, pitch_2, pitch_3, pitch_3, pitch_2, pitch_1};
				
				List<ElementId> myListRebarCopyId = copyRebarByDistance2(myRebar, myDicDisNumDetail);
				
				
				List<double> myDistances = myDicDisNumDetail.Keys.ToList();
				myDistances.Sort();
				
	
				
				List<int> myListNum = new List<int>();
				
				foreach (double key in myDistances) 
				{
					myListNum.Add(myDicDisNumDetail[key]);
				}
				
				//Layout
				
				// using transcation (edit DB)
				for(int i = 0; i < myListRebarCopyId.Count(); i++)
				{
					using (Transaction myTrans = new Transaction(doc,"CopyElementByCoordinate"))
			
					{
						myTrans.Start();
						ElementId rebarId = myListRebarCopyId[i];
						Rebar myRebarI = doc.GetElement(rebarId) as Rebar;
						
						if(myListNum[i] < -1)
						{
							myRebarI.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing((myListNum[i]) * -1, myListSpace[i], true, true, true);
						}
						
						if(myListNum[i] == -1)
						{
							myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
						
						}
	
						if(myListNum[i] == 0)
						{
							myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
						
						}
						
						if(myListNum[i] == 1)
						{
							myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
						
						}
						
						if(myListNum[i] > 1)
						{
							myRebarI.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing(myListNum[i], myListSpace[i], false, true, true);
							
						}
						myTrans.Commit();
					}
					}
				
					//delete element
					using (Transaction myTrans = new Transaction(doc,"Delete First ReBar"))
					
					{
						myTrans.Start();
						//doc.Delete(myRebar.Id);
						myTrans.Commit();
					}
			}
	
			}
		}
	
		
		// Select Faces boundaries
						
		public void rebarBeam_Form_Update()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			double factor = 4;
			double delta_1 = 50/304.8;
			double pitch_1 = 150/304.8 ;
			double pitch_2 = 300/304.8;
			
			double delta_3 = 50/304.8;
			double pitch_3 = 100/304.8;
			int N3 = 5;
			
			double myConvertFactor = 304.8;

			
			bool inputSuccess = false;
			while (!inputSuccess) 
			{
				using(var myInputFormSetting = new InputDialog())
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
				    if(myInputFormSetting.DialogResult == System.Windows.Forms.DialogResult.Cancel) return;
				    {
				    	//else do all this :)    
				    	myInputFormSetting.Close();
				    }
				    
				    if(myInputFormSetting.DialogResult == System.Windows.Forms.DialogResult.OK)
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
		    
		    XYZ p = p1 - 0.1*v;

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
			
			if(myListFacePicked.Count !=2 && myListFacePicked.Count != 4)
			{
				TaskDialog.Show("Error!", "Chua ho tro lua chon: " + myListFacePicked.Count() + " mat, Chon 2 hoac 4 mat");
				continue;
				
			}
			
			else
			{
				string 	caseDistributionRebar = "TH2: Co dam o giua";
				
				List<double> myListSpace = new List<double>(){pitch_1, pitch_2, pitch_3, pitch_3, pitch_2, pitch_1};
				if(myListFacePicked.Count ==2)
				{
					myListSpace = new List<double>(){pitch_1, pitch_2, pitch_1, pitch_2, pitch_2, pitch_1};
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
				for(int i = 0; i < myListRebarCopyId.Count(); i++)
				{
					using (Transaction myTrans = new Transaction(doc,"CopyElementByCoordinate"))
			
					{
						myTrans.Start();
						ElementId rebarId = myListRebarCopyId[i];
						Rebar myRebarI = doc.GetElement(rebarId) as Rebar;
						
						if(myListNum[i] < -1)
						{
							myRebarI.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing((myListNum[i]) * -1, myListSpace[i], true, true, true);
						}
						
						if(myListNum[i] == -1)
						{
							myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
						
						}
	
						if(myListNum[i] == 0)
						{
							myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
						
						}
						
						if(myListNum[i] == 1)
						{
							myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
						
						}
						
						if(myListNum[i] > 1)
						{
							myRebarI.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing(myListNum[i], myListSpace[i], false, true, true);
							
						}
						myTrans.Commit();
					}
				}
				
				//delete element
				using (Transaction myTrans = new Transaction(doc,"Delete First ReBar"))
				
				{
					myTrans.Start();
					//doc.Delete(myRebar.Id);
					myTrans.Commit();
				}
			}

		}
	}
	


		
		// Select Faces boundaries
						
		public void rebarBeam_Form_Update2()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			double factor = 4;
			double delta_1 = 50/304.8;
			double pitch_1 = 150/304.8 ;
			double pitch_2 = 300/304.8;
			
			double delta_3 = 50/304.8;
			double pitch_3 = 100/304.8;
			int N3 = 5;
			
			double myConvertFactor = 304.8;

			
			bool inputSuccess = false;
			while (!inputSuccess) 
			{
				using(var myInputFormSetting = new InputDialog())
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
				    if(myInputFormSetting.DialogResult == System.Windows.Forms.DialogResult.Cancel) return;
				    {
				    	//else do all this :)    
				    	myInputFormSetting.Close();
				    }
				    
				    if(myInputFormSetting.DialogResult == System.Windows.Forms.DialogResult.OK)
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
			
			XYZ p1 = getPointCenterTopLine(myBeam)[0];
			XYZ p2 = getPointCenterTopLine(myBeam)[1];
			
			
			Line centerLineBeam = Line.CreateBound(p1, p2);
		    
		    
//		    //Get vector of location cuver beam
//            XYZ p = line.GetEndPoint(0);
//		    XYZ q = line.GetEndPoint(1);
//		    XYZ v = q - p; // Vector equation of line
		    
            XYZ p = p1;
		    XYZ q = p2;
		    XYZ v = q - p; // Vector equation of line

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
			
			if(myListFacePicked.Count !=2 && myListFacePicked.Count != 4)
			{
				TaskDialog.Show("Error!", "Chua ho tro lua chon: " + myListFacePicked.Count() + " mat, Chon 2 hoac 4 mat");
				continue;
				
			}
			
			else
			{
				string 	caseDistributionRebar = "TH2: Co dam o giua";
				
				List<double> myListSpace = new List<double>(){pitch_1, pitch_2, pitch_3, pitch_3, pitch_2, pitch_1};
				if(myListFacePicked.Count ==2)
				{
					myListSpace = new List<double>(){pitch_1, pitch_2, pitch_1, pitch_2, pitch_2, pitch_1};
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
				
				
				List<ElementId> myListRebarCopyId = copyRebarByDistance2_Update2(myRebar, myDicDisNumDetail);
				
				
				List<double> myDistances = myDicDisNumDetail.Keys.ToList();
				myDistances.Sort();
				
	
				
				List<int> myListNum = new List<int>();
				
				foreach (double key in myDistances) 
				{
					myListNum.Add(myDicDisNumDetail[key]);
				}
				
				//Layout
				
				// using transcation (edit DB)
				for(int i = 0; i < myListRebarCopyId.Count(); i++)
				{
					using (Transaction myTrans = new Transaction(doc,"CopyElementByCoordinate"))
			
					{
						myTrans.Start();
						ElementId rebarId = myListRebarCopyId[i];
						Rebar myRebarI = doc.GetElement(rebarId) as Rebar;
						
						if(myListNum[i] < -1)
						{
							myRebarI.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing((myListNum[i]) * -1, myListSpace[i], true, true, true);
						}
						
						if(myListNum[i] == -1)
						{
							myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
						
						}
	
						if(myListNum[i] == 0)
						{
							myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
						
						}
						
						if(myListNum[i] == 1)
						{
							myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
						
						}
						
						if(myListNum[i] > 1)
						{
							myRebarI.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing(myListNum[i], myListSpace[i], false, true, true);
							
						}
						myTrans.Commit();
					}
				}
				
				//delete element
				using (Transaction myTrans = new Transaction(doc,"Delete First ReBar"))
				
				{
					myTrans.Start();
					//doc.Delete(myRebar.Id);
					myTrans.Commit();
				}
			}

		}
	}
	
	
		
		
		public void rebarSetChange()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			// Find intersections between family instances and a selected element
			Reference reference = uiDoc.Selection.PickObject(ObjectType.Element, "Select element that will be checked for intersection with all family instances");
			Element element = doc.GetElement(reference);
			
			using (Transaction trans = new Transaction(doc, "Change set layout rebar")) 
			{
				
				trans.Start();

			
				Rebar myReBar = element as Rebar;
				
				myReBar.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing(5, 3.1, false, true, true);
								myReBar.GetShapeDrivenAccessor().SetLayoutAsNumberWithSpacing(5, 3.1, false, true, true);
				
				trans.Commit();
				
	//			
	//			string mm = myReBar.LayoutRule.ToString();
	//			TaskDialog.Show("abc", "adsa" + mm);
			}
		}


	

		
		private List<double> detailListDistance(List<double> endPoiList, double factorDivide, double delta_1, double pitch_1)
		{
		
			
			if(endPoiList.Count != 2) 
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
				double X2 = endPoiList[0] + lengthSeg/factorDivide;
				
				// Diem dau doan 3
				double deltaSeg3 = (lengthSeg/factorDivide - delta_1)%pitch_1;
				
				double X3 = endPoiList[0] + lengthSeg - delta_1;
			
				return new List<double>(){X1, X2, X3};
			}
		
		}
		
		//Tra ve 2 dic gom khoang cach offset va so thanh layout tuong ung 
		private Dictionary<double, int> detailListDistance2(List<double> endPoiList, double factorDivide,
		                                         double delta_1, double pitch_1,
		                                          double pitch_2,
		                                         double delta_3, double pitch_3, int N3)
		{
			Dictionary<double, int> myReturnDic = new Dictionary<double, int>();
			if(endPoiList.Count !=4) 
			{
				return myReturnDic;
			}
			
			else
			{
				// Always positive
				double lengthSeg = endPoiList[endPoiList.Count() - 1] - endPoiList[0];
				
				// Diem dau doan 1
				double X1 = endPoiList[0] + delta_1;
				int N1 = (int)((lengthSeg/factorDivide)/pitch_1);
				myReturnDic.Add(X1, N1);
				
				// Diem dau doan 2
				double X2 = endPoiList[0] + lengthSeg/factorDivide;
				int N2 = (int)((endPoiList[1] - delta_3 - (N3-1)*pitch_3-(lengthSeg/factorDivide) - endPoiList[0])/pitch_2)+1;
				myReturnDic.Add(X2, N2);
				
				// Diem dau doan 3-dau dam // number nguoc
				double X3 = endPoiList[1] - delta_3;
				myReturnDic.Add(X3, -1*N3);
				
				// Diem dau doan 4-dau dam // number xuoi
				double X4 = endPoiList[2] + delta_3;
				myReturnDic.Add(X4, 1*N3);		
				
				
				// Diem dau doan 5 dau dam // number nghich dao
				double X5 = endPoiList[3] - lengthSeg/factorDivide;
				int N5 = (int)((endPoiList[3] - (lengthSeg/factorDivide) - (N3-1)*pitch_3 - delta_3 - endPoiList[2])/pitch_2)+1;
				myReturnDic.Add(X5, -1*N5);
				
				
				// Diem dau doan 6 dau dam // number nghich dao
				double X6 = endPoiList[3] - delta_1;
				int N6 = (int)((lengthSeg/factorDivide)/pitch_1);
				myReturnDic.Add(X6, -1*N6);
				
				
				return myReturnDic;
				
//				return new Dictionary<double, int>(){{1.5,5}};
			}
		
		}
	


		//Tra ve 2 dic gom khoang cach offset va so thanh layout tuong ung 
		private Dictionary<double, int> detailListDistance_Update(List<double> endPoiList, double factorDivide,
		                                         double delta_1, double pitch_1,
		                                          double pitch_2,
		                                         double delta_3, double pitch_3, int N3)
		{
			Dictionary<double, int> myReturnDic = new Dictionary<double, int>();
			if(endPoiList.Count !=2 && endPoiList.Count !=4)
			{
				return myReturnDic;
			}
			
			if(endPoiList.Count == 2)
			{
				// Always positive
				double lengthSeg = endPoiList[endPoiList.Count() - 1] - endPoiList[0];
				// Diem dau doan 1
				double X1 = endPoiList[0] + delta_1;
				int N1 = (int)((lengthSeg/factorDivide)/pitch_1);
				myReturnDic.Add(X1, N1);
				
				// Diem dau doan 2
				double X2 = endPoiList[0] + lengthSeg/factorDivide;
				int N2 = (int)((lengthSeg - lengthSeg/factorDivide - delta_1 - (N1-1)*pitch_1)/pitch_2)+1;
				myReturnDic.Add(X2, N2);
				
				// Diem dau doan 3
				double X5 = endPoiList[0] + lengthSeg - delta_1;
				int N5 = -1* N1;
				myReturnDic.Add(X5, N5);

				return myReturnDic;
			}
			
			else
			{
				// Always positive
				double lengthSeg = endPoiList[endPoiList.Count() - 1] - endPoiList[0];
				
				// Diem dau doan 1
				double X1 = endPoiList[0] + delta_1;
				int N1 = (int)((lengthSeg/factorDivide)/pitch_1);
				myReturnDic.Add(X1, N1);
				
				// Diem dau doan 2
				double X2 = endPoiList[0] + lengthSeg/factorDivide;
				int N2 = (int)((endPoiList[1] - delta_3 - (N3-1)*pitch_3-(lengthSeg/factorDivide) - endPoiList[0])/pitch_2)+1;
				myReturnDic.Add(X2, N2);
				
				// Diem dau doan 3-dau dam // number nguoc
				double X3 = endPoiList[1] - delta_3;
				myReturnDic.Add(X3, -1*N3);
				
				// Diem dau doan 4-dau dam // number xuoi
				double X4 = endPoiList[2] + delta_3;
				myReturnDic.Add(X4, 1*N3);		
				
				
				// Diem dau doan 5 dau dam // number nghich dao
				double X5 = endPoiList[3] - lengthSeg/factorDivide;
				int N5 = (int)((endPoiList[3] - (lengthSeg/factorDivide) - (N3-1)*pitch_3 - delta_3 - endPoiList[2])/pitch_2)+1;
				myReturnDic.Add(X5, -1*N5);
				
				
				// Diem dau doan 6 dau dam // number nghich dao
				double X6 = endPoiList[3] - delta_1;
				int N6 = (int)((lengthSeg/factorDivide)/pitch_1);
				myReturnDic.Add(X6, -1*N6);
				
				
				return myReturnDic;
			}
		
		}
		
		
		
		// return xyz as string
		private string XYZToString(XYZ myXYZ)
		{
			return string.Format("X: {0}, Y: {1}, Z: {2} ", Math.Round(myXYZ.X,3), Math.Round(myXYZ.Y,3), Math.Round(myXYZ.Z,3));
		}
		
		
		// return bool 2 vector (x,y) same direction, Only in Oxy, ignore Z axis
		private bool isSameDirection(XYZ vec1, XYZ vec2)
		{
			double X1 = Math.Round(vec1.X, 9);
			double Y1 = Math.Round(vec1.Y, 9);
			
			double X2 = Math.Round(vec2.X, 9);
			double Y2 = Math.Round(vec2.Y, 9);
			
			if((X1 == Y1 && X1 == 0) || (X2 == Y2 && X2 == 0))
			{
				return false;
			}
			
			if(Math.Round(X1 * Y2, 4) == Math.Round(X2 * Y1, 4) )
			{
				return true;
			}
			else{
				return false;
			}

		}
		
		
		private void setBeJoined(Element myBeJoined)
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			
			ICollection<ElementId> myListElemIdsJoined = JoinGeometryUtils.GetJoinedElements(doc, myBeJoined);

			using (Transaction trans = new Transaction(doc, "Switch Join")) 
			{			
				trans.Start();
				foreach (ElementId myElemId in myListElemIdsJoined) 
				{
					
					if(!JoinGeometryUtils.IsCuttingElementInJoin(doc, doc.GetElement(myElemId), myBeJoined))
					{
						JoinGeometryUtils.SwitchJoinOrder(doc, doc.GetElement(myElemId), myBeJoined);
					}

				}

				trans.Commit();
			}	
		}
	
		

		
		private double getDisFromPointToPointByPlaneFace(XYZ myPoint1, XYZ myPoint2, Face myFace)
		{
			Plane myPlaneFace = myFace.GetSurface() as Plane;
			
			XYZ v1 = myPoint1 - myPlaneFace.Origin;
			
			double myDis1 = myPlaneFace.Normal.DotProduct(v1);
		
			
			XYZ v2 = myPoint2 - myPlaneFace.Origin;
			
			double myDis2 = myPlaneFace.Normal.DotProduct(v2);
		
			return Math.Abs(myDis1 - myDis2);
		}

		
	
	

		//Get centerLine of beam
				// Xac dinh center line top cua 1 dam
		
		public List<XYZ> getPointCenterTopLine(Element myBeam)
		{
			
			//Get all face of beam
			GeometryElement geometryElement = myBeam.get_Geometry(new Options());
			
			
			UV myPoint = new UV(0,0);
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
		            	if (Math.Round(myNormVec.Z, 1) == 1.0)
						{
		            		topFaces.Add(myFace);
		            		List<XYZ> myListPointOnFace = getAllPointOfFace(myFace);
		            		foreach (XYZ myXYZ in myListPointOnFace)
		            		{
		            			if(!topPoints.Contains(myXYZ))
		            			{
		            				topPoints.Add(myXYZ);
		            			}
		            		}
						}
		            }
		        }
		    }
			
			
			
			// get startPoint of location line
			LocationCurve myLocBeam = myBeam.Location as LocationCurve;
			
			Line centerLinebeam = myLocBeam.Curve as Line;
			
			XYZ p = centerLinebeam.GetEndPoint(0);
			XYZ q = centerLinebeam.GetEndPoint(1);
			XYZ v = p-q;
			
			// lay tat ca cac diem thuoc face
			Dictionary<double, XYZ> myDic = new Dictionary<double, XYZ>();
			foreach (XYZ myXYZ in topPoints) 
			{
				if(!myDic.Keys.ToList().Contains(p.DistanceTo(myXYZ)))
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
			
			List<XYZ> myEndPointOfCenterLine = new List<XYZ>(){(P1+ P2)/2, (P3 + P4)/2};
			
			return myEndPointOfCenterLine;
			
		}
		
					
		private List<XYZ> getAllPointOfFace(Face myFace)
		{
			List<XYZ> myListPointOnFace = new List<XYZ>();
			
			 EdgeArrayArray myEdgeArAr =  myFace.EdgeLoops;
			 foreach (EdgeArray myEdgeAr in myEdgeArAr) 
			 {
			 	foreach (Edge myEdge in myEdgeAr) 
			 	{
			 		Curve myCurve = myEdge.AsCurve();
			 		XYZ eP1 =  new XYZ(Math.Round(myCurve.GetEndPoint(0).X,6),
			 		                   Math.Round(myCurve.GetEndPoint(0).Y,6),
			 		                   Math.Round(myCurve.GetEndPoint(0).Z,6));
			 		
			 		XYZ eP2 =  new XYZ(Math.Round(myCurve.GetEndPoint(1).X,6),
		                   						Math.Round(myCurve.GetEndPoint(1).Y,6),
		                   						Math.Round(myCurve.GetEndPoint(1).Z,6));
			 		
			 		myListPointOnFace.Add(eP1);    
			 		myListPointOnFace.Add(eP2);			 		
			 	}
			 }
			return myListPointOnFace;
		}

		
		
	}
	
		// Classes Filter
	public class AngularDimFilter : ISelectionFilter
	{
		
		public bool AllowElement(Element e)
		{
			
			if(e.GetType().Name.ToString() == "AngularDimension")
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
	
	
	//By name type of class. Ex: "AngularDimension", Wall
	
	public class FilterByNameElementType : ISelectionFilter
	{
		
		//Cac bien thanh vien
		List<string> myListNameFilter = new List<string>();
		
		// Bo khoi dung
		public FilterByNameElementType(List<string> myListName){
		myListNameFilter = myListName;
		}
		
		public bool AllowElement(Element e)
		{
			string typeE = e.GetType().Name.ToString();

			if(this.myListNameFilter.Contains(typeE))
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
		public FilterByIdCategory(List<int> myListIdCategory){
		this.ListIdCategory = myListIdCategory;
		}
		
		public bool AllowElement(Element e)
		{
			int categoryE = e.Category.Id.IntegerValue;

			if(this.ListIdCategory.Contains(categoryE))
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