/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 3/3/2019
 * Time: 8:17 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;

using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace ShowDialog
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("E334D8B6-9F38-4D35-83D0-9DA8A39FE750")]
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
		
		
		public void selectElelment()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element);
			ElementId MyElementId = uiDoc.Selection.PickObject(ObjectType.Element).ElementId;
			
			Element e = doc.GetElement(myRef);
			
			TaskDialog.Show("My Info", "Id1: " + MyElementId.ToString() + "\n" + "Id2: " + e.Id.ToString() + 
			               "Name: " + e.Name + 
			              "Current Document: " + doc.PathName);
			
		}

		
		public void editDimText()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			
			ElementId eId = uiDoc.Selection.PickObject(ObjectType.Element).ElementId;
			
			Element e = doc.GetElement(eId);
			
			List<XYZ> myCoors = new List<XYZ>();
			
//read textFile coordinate
			
			string textFile = @"C:\Users\NAMTRUNG205\Desktop\myFile.txt";
			
			if(File.Exists(textFile)){
//Split each line in
				
				
				
				string[] lines = File.ReadAllLines(textFile);
				foreach (string line in lines) {
					string[] myItem = line.Split('|');
					if (myItem.Length-1 == 2) {
						myCoors.Add(new XYZ(Convert.ToInt32(myItem[0]), Convert.ToInt32(myItem[1]), Convert.ToInt32(myItem[2])));
					}
				}
			}
			
// using transcation (edit DB)
			using (Transaction myTrans = new Transaction(doc,"CopyElementByCoordinate"))
			
			{
				myTrans.Start();		
				foreach(XYZ myXYX in myCoors){
				ElementTransformUtils.CopyElement(doc,eId, myXYX);				
				}
				myTrans.Commit();
			}
			
			
		}
		
/// <summary>
/// Các nhóm hàm liên quan modify (copy, move, rotate ...Element)
/// </summary>
		public void CopyElementByVectorTextFile()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			
			ElementId eId = uiDoc.Selection.PickObject(ObjectType.Element).ElementId;
			
			Element e = doc.GetElement(eId);
			
			List<XYZ> myCoors = new List<XYZ>();
			
			
			var filePath = string.Empty;
			using(System.Windows.Forms.OpenFileDialog myOpenTxtPath = new System.Windows.Forms.OpenFileDialog())
			{
				myOpenTxtPath.InitialDirectory = "c:\\";
        		myOpenTxtPath.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
        		myOpenTxtPath.FilterIndex = 2;
        		myOpenTxtPath.RestoreDirectory = true;
	
        		if (myOpenTxtPath.ShowDialog() == DialogResult.OK)
        		{
        	    //Get the path of specified file
        	    filePath = myOpenTxtPath.FileName;
        		}
			
			//read textFile coordinate
			
			//	string textFile = @"C:\Users\NAMTRUNG205\Desktop\myFile.txt";
			
			string textFile = @filePath;
			
			if(File.Exists(textFile)){
				
			//Split each line in
				string[] lines = File.ReadAllLines(textFile);
				foreach (string line in lines) {
					string[] myItem = line.Split('|');
					if (myItem.Length-1 == 2) {
						
						if(myItem[0]!="0" || myItem[1] != "0"){
						
							myCoors.Add(new XYZ(Convert.ToInt32(myItem[0])/304.8, Convert.ToInt32(myItem[1])/304.8, Convert.ToInt32(myItem[2])/304.8));	
						}
						
						
					}
				}
			}
			
// using transcation (edit DB)
			using (Transaction myTrans = new Transaction(doc,"CopyElementByCoordinate"))
			
			{
				myTrans.Start();		
				foreach(XYZ myXYZ in myCoors){
				ElementTransformUtils.CopyElement(doc,eId, myXYZ);				
				}
				myTrans.Commit();
			}
			
			
		}
	}
		
		
		public void CopyElementByVectorClipBoard()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			
			ElementId eId = uiDoc.Selection.PickObject(ObjectType.Element).ElementId;
			
			Element e = doc.GetElement(eId);
			
			List<XYZ> myCoors = new List<XYZ>();
			
			string myTextClipBoard = Clipboard.GetText();

			//Split each line in
			string[] lines = myTextClipBoard.Split(';');
			foreach (string line in lines) 
			{
				string[] myItem = line.Split('|');
				if (myItem.Length-1 == 2) 
				{
					if(myItem[0]!="0" || myItem[1] != "0")
					{
						myCoors.Add(new XYZ(Convert.ToDouble(myItem[0])/304.8, Convert.ToDouble(myItem[1])/304.8, Convert.ToDouble(myItem[2])/304.8));	
					}	
				}
			}
			
			if( myCoors.Count < 1)
			{
				TaskDialog.Show("Empty Data", "ClipBoard không có dữ liệu phù hợp...");
				return;			
			}
			
// using transcation (edit DB)
			using (Transaction myTrans = new Transaction(doc,"CopyElementByCoordinate"))
			
			{
				myTrans.Start();		
				foreach(XYZ myXYZ in myCoors){
				ElementTransformUtils.CopyElement(doc,eId, myXYZ);				
				}
				myTrans.Commit();
			}

		}

	
		
		public void CopyElementByVectorClipBoard2()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			
			ElementId eIdHost = uiDoc.Selection.PickObject(ObjectType.Element,"pick Host").ElementId;
			
			Element eHost = doc.GetElement(eIdHost);
			
			LocationCurve cur = eHost.Location as LocationCurve;
			Line lineCur = cur.Curve as Line;
			
			XYZ q = lineCur.GetEndPoint(0);
			XYZ p = lineCur.GetEndPoint(1);
			XYZ v = p - q;
			double lengCurLine = v.GetLength();
			
			
			ElementId eIdRebar = uiDoc.Selection.PickObject(ObjectType.Element,"Pick rebar").ElementId;
			
			Element eRebar = doc.GetElement(eIdRebar);

			
			List<XYZ> myCoors = new List<XYZ>();
			
			string myTextClipBoard = Clipboard.GetText();

			//Split each line in
			string[] distances = myTextClipBoard.Split(';');
			foreach (string distance in distances) 
			{

				XYZ myPointPlace = (Convert.ToDouble(distance)/lengCurLine)*v;
				myCoors.Add(myPointPlace);
			}
			
			if( myCoors.Count < 1)
			{
				TaskDialog.Show("Empty Data", "ClipBoard không có dữ liệu phù hợp...");
				return;			
			}
			
			// using transcation (edit DB)
			using (Transaction myTrans = new Transaction(doc,"CopyElementByCoordinate"))
			
			{
				myTrans.Start();		
				foreach(XYZ myXYZ in myCoors)
				{
					ElementTransformUtils.CopyElement(doc, eIdRebar, myXYZ);				
				}
				myTrans.Commit();
			}

		}

		
			
		public void RotateElements()
		{
			UIDocument UiDoc = this.ActiveUIDocument;
			Document doc = UiDoc.Document;
			
			try {	
				List<Reference> myRefList = (List<Reference>)UiDoc.Selection.PickObjects(ObjectType.Element);

				// Get infor from InputBox
				double myAngle = 0;
				
			   //Access a new instance of the Form1 created earlier and call it 'form'
			   
			   bool inputSuccess = false;
			   while (!inputSuccess) 
			   {
				   using(var myInputForm = new InputNumber())
				   {
					    //use ShowDialog to show the form as a modal dialog box. 
					    myInputForm.ShowDialog();
					    
					    //if the user hits cancel just drop out of macro
					    if(myInputForm.DialogResult == System.Windows.Forms.DialogResult.Cancel) return;
					    {
					    	//else do all this :)    
					    	myInputForm.Close();
					    }
					    
					    if(myInputForm.DialogResult == System.Windows.Forms.DialogResult.Yes)
					    {
						    if(myInputForm.Mode == "3P") {
						    	
						    	//Get Angle from Pickpoint
						    	myAngle = AngleFromPick3Point();
						    	inputSuccess = true;	
						    }
						    
	 					    if(myInputForm.Mode == "PickAngular") {
						    	
						    	//Get Angle from Pickpoint
						    	myAngle = getDimValueAngular();
						    	inputSuccess = true;	
						    }  
				   		}
						    
					    else
					    {
						    try {
						    	
						    	myAngle = Convert.ToDouble(myInputForm.TextString);
						    	inputSuccess = true;
					    	} 
						    catch (Exception) {
						    	TaskDialog.Show("Input Error", "So nhap vao khong dung");
						    	inputSuccess = false;
						    	continue;
				    		}
					    }
				   }
			   }
					
				foreach (Reference myRef in myRefList) {
					Element myEle = doc.GetElement(myRef);
					this.RotateElementCustom(myAngle, myEle);
				}	
			}
			catch (Exception) {				
				return;
			}			
		}


		
		private void RotateElementCustom(double degrees, Element myEle)
		{
			UIDocument UiDoc = this.ActiveUIDocument;
			Document doc = UiDoc.Document;
			
			LocationCurve locCurve = myEle.Location as LocationCurve;
			if (null == locCurve)
			{
				XYZ point = ((LocationPoint)myEle.Location).Point;
				XYZ point2 = point.Add(XYZ.BasisZ);
				
				Line axis = Line.CreateBound(point, point2);
				//Line lineAsAxis = Line.CreateBound(new XYZ(0, 0, 0), new XYZ(0, 0, 1));
				
				using (Transaction myTrans = new Transaction(doc, "RotateElement Location Point") ){	
					myTrans.Start();
					// Code here
					ElementTransformUtils.RotateElement(doc, myEle.Id, axis, DegreesToRadians(degrees));
					myTrans.Commit();
				}
			}
			
			else 
			{	            
	            using (Transaction myTrans = new Transaction(doc, "RotateElement Location Curve") ){	
					myTrans.Start();
					// Code here
					Curve line = locCurve.Curve;
					XYZ sP = line.GetEndPoint(0);
					XYZ eP = line.GetEndPoint(1);
					XYZ mP = new XYZ((sP.X+eP.X)/2, (sP.Y+eP.Y)/2, (sP.Z+eP.Z)/2);
	
					XYZ mzP = new XYZ(mP.X, mP.Y, mP.Z + 10);
					Line axis = Line.CreateBound(mP, mzP);
					locCurve.Rotate(axis, DegreesToRadians(degrees));
	
					myTrans.Commit();
				}
			}
		}
	

		
		//Get angle (degree) by PickPoint Revit
		private double AngleFromPick3Point()
		{
			UIDocument Uidoc = this.ActiveUIDocument;
			Document doc = Uidoc.Document;

		    //ObjectSnapTypes snapTypes = ObjectSnapTypes.Endpoints | ObjectSnapTypes.Intersections;
		    //XYZ point = Uidoc.Selection.PickPoint(snapTypes, "Select an end point or intersection");
		    
		    XYZ point1 = Uidoc.Selection.PickPoint("Pick first Point ");
		    XYZ point2 = Uidoc.Selection.PickPoint("Pick 2nd Point ");
		    XYZ point3 = Uidoc.Selection.PickPoint("Pick 3nd Point ");
    	
		    return getAngleFrom3P(point1, point2, point3)*180/Math.PI;
		}


		
		//Get Angle from 3 Point Coordinate(radian)
		private double getAngleFrom3P(XYZ p1, XYZ p2, XYZ p3){
			// Return radian angle
			double a = Math.Atan2(p3.Y - p1.Y, p3.X - p1.X) - Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
			return a;
		}
		
	
		
		//Convert Angle from Degree to radians
		private double DegreesToRadians(double degrees)
		{
			return degrees * Math.PI/180;
		}
		
		
		
		private double getDimValueAngular(){
		
			UIDocument UiDoc = this.ActiveUIDocument;
			Document doc = UiDoc.Document;

			Reference myRef = UiDoc.Selection.PickObject(ObjectType.Element, new AngularDimFilter(), "Pick An Angular Dimension...");

			AngularDimension angleDim = doc.GetElement(myRef) as AngularDimension;

			// Get infor from InputBox
			return Math.Round((double)(angleDim.Value * 180/Math.PI), 4);
		}
	
	
		
		private void getTypeFromElemt(){
		
			UIDocument UiDoc = this.ActiveUIDocument;
			Document doc = UiDoc.Document;
	
				Reference myRef = UiDoc.Selection.PickObject(ObjectType.Element);
				
				Element e = doc.GetElement(myRef);
				
				TaskDialog.Show("abc", e.GetType().Name);
		}
		
		
		/// <summary>
		/// Createsection View for elements
		/// </summary>
				
		public void createSectionViewWall(){
		
		    UIDocument uidoc = this.ActiveUIDocument;
		    Document doc = uidoc.Document;
		 
			Reference myRef = uidoc.Selection.PickObject(ObjectType.Element);
					
			Wall wall = doc.GetElement(myRef) as Wall;
			
			

		    // Ensure wall is straight
		 
		    LocationCurve lc = wall.Location as LocationCurve;
		 
		    Line line = lc.Curve as Line;
		 
		    if( null == line )
		    {
		    	TaskDialog.Show("Error","Unable to retrieve wall location line.");
		      	return;
		    }
		 
		    // Determine view family type to use
		    ViewFamilyType vft
		      = new FilteredElementCollector( doc )
		        .OfClass( typeof( ViewFamilyType ) )
		        .Cast<ViewFamilyType>()
		        .FirstOrDefault<ViewFamilyType>( x =>
		          ViewFamily.Section == x.ViewFamily );
		 
		    // Determine section box
		    XYZ p = line.GetEndPoint(0);
		    XYZ q = line.GetEndPoint(1);
		    XYZ v = q - p;
		 
		    BoundingBoxXYZ bb = wall.get_BoundingBox( null );
		    double minZ = bb.Min.Z;
		    double maxZ = bb.Max.Z;
		 
		    double w = v.GetLength();
		    double h = maxZ - minZ;
		    double d = wall.WallType.Width;
		    double offset = 0.5;
		 
		    XYZ min = new XYZ( -w, - offset, -offset);
		    XYZ max = new XYZ( w, h + offset, offset );
		 
		    XYZ midpoint = p + 0.5 * v;
		    XYZ walldir = v.Normalize();
		    XYZ up = XYZ.BasisZ;
		    XYZ viewdir = walldir.CrossProduct( up );
		 
		    Transform t = Transform.Identity;
		    t.Origin = midpoint;
		    t.BasisX = walldir;
		    t.BasisY = up;
		    t.BasisZ = viewdir;
		 
		    BoundingBoxXYZ sectionBox = new BoundingBoxXYZ();
		    sectionBox.Transform = t;
		    sectionBox.Min = min;
		    sectionBox.Max = max;
		 
		    // Create wall section view
		 
		    using( Transaction tx = new Transaction( doc ) )
		    {
		      tx.Start( "Create Wall Section View" );
		 
		      ViewSection.CreateSection( doc, vft.Id, sectionBox );
		 
		      tx.Commit();
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
			double offset = 0.1*l;// khoang cach offset
			


			XYZ min = new XYZ( -l/2-h,0- offset-h, 0 );
			XYZ max = new XYZ( l/2+h, offset, offset );
			
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
		
				
		private void sectionViewBeamY(Element myElemt, double atX)
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
		    XYZ v = q - p; // Vector equation of line
		    v = v.Normalize();
		    
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
				ViewSection.CreateDetail(doc, vft.Id, sectionBoxY);
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
						this.sectionViewBeamY(myEle, myFactor);
					}
			
		
		
		
		}
		
		
		public void makeSectionBeamAuto()
		{
			
			UIDocument UiDoc = this.ActiveUIDocument;
			Document doc = UiDoc.Document;
			
			List<int> myListIdCategory= new List<int>();
			myListIdCategory.Add((int)BuiltInCategory.OST_StructuralFraming);
			
			List<Reference> myRefList = (List<Reference>)UiDoc.Selection.PickObjects(ObjectType.Element, new FilterByIdCategory(myListIdCategory),"Pick a Beam...");
			foreach (Reference myRef in myRefList) {
						Element myEle = doc.GetElement(myRef);
						this.sectionViewBeamX(myEle);
						this.sectionViewBeamY(myEle, 0.01);
					}	
		}
	
		
		public void getBoundingBoxOfElement()
		{
			UIDocument UiDoc = this.ActiveUIDocument;
			Document doc = UiDoc.Document;
			
			//Get element by filler by Id integer of category
			
			List<int> myListIdCategory= new List<int>();
			myListIdCategory.Add((int)BuiltInCategory.OST_StructuralFraming);
			
			Reference myRef = UiDoc.Selection.PickObject(ObjectType.Element, new FilterByIdCategory(myListIdCategory),"Pick a Beam...");
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
			double offset = 0.1*w;// khoang cach offset
			
			string outString = "Chieu dai: " + w +"\nChieu cao: " + h + "\nBe rong: " + b;
			
			TaskDialog.Show("thong so", outString);
			
			//Phan code tren da xong tra ve 1 boudingbox, cacs thong so cua dam: w, h, b
			
			XYZ min = new XYZ( -w, minZ_X - offset, 0 );
			XYZ max = new XYZ( w, maxZ_X + offset, offset );
			
			

			
		}

			
		public void RebarBeam()
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