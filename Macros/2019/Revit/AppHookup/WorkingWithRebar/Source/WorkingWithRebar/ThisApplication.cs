/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 3/29/2019
 * Time: 10:21 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;

//using System.Drawing;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace WorkingWithRebar
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("53FDD446-6589-4E63-93B1-DF332732138D")]
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
		
		
		
		public void getInfo()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			// Pick Rebar
			
			List<int> myListIdCategoryRebar = new List<int>();
			myListIdCategoryRebar.Add((int)BuiltInCategory.OST_Rebar);
			
			// Select first Element (ex beam)
			Reference myRefRebar = uiDoc.Selection.PickObject(ObjectType.Element, new FilterByIdCategory(myListIdCategoryRebar), "Pick a Beam...");
			//Get element1 from ref
			Rebar myRebar = doc.GetElement(myRefRebar) as Rebar;

//			TaskDialog.Show("abc", "xyz" + ": " + myLocCur.Point.ToString());
//			XYZ firstPoint = myLineLocRebar.GetEndPoint(0);
//			
//			TaskDialog.Show("abc", "xyz" + ": " + firstPoint.ToString());

		}

		
				
		public void testRebar_FromCurve()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
//			// prompt select face 
//			
//			Reference myRef = uiDoc.Selection.PickObject(ObjectType.Face);
//			
//			// Get element by pickface
//			Element e = doc.GetElement(myRef) as Element;
//			
//			
//			//Get GeoObject from element;
//			GeometryObject myGeoObj =  e.GetGeometryObjectFromReference(myRef) as Face;
//			
//			//Get face from element Object:
//			Face myPickedFace = myGeoObj as Face;
//			
//			XYZ myNorVecFace = myPickedFace.ComputeNormal(new UV(0,0));
			
			

			
			List<int> myListIdCategoryRebar = new List<int>();
			myListIdCategoryRebar.Add((int)BuiltInCategory.OST_StructuralFraming);
			
			// Select first Element (ex beam)
			Reference myRefBeam = uiDoc.Selection.PickObject(ObjectType.Element, new FilterByIdCategory(myListIdCategoryRebar), "Pick a Beam...");
			//Get element1 from ref
			Element myBeam = doc.GetElement(myRefBeam);
			
			setBeJoined(myBeam);
			
			LocationCurve myLocBeam = myBeam.Location as LocationCurve;
			
			List<XYZ> myEndPoints = getPointCenterTopLine(myBeam);
			
			XYZ p1 = myEndPoints[0];
			XYZ p2 = myEndPoints[1];
			
			
			Line centerLineBeam = Line.CreateBound(p1, p2);
			
			
			
			XYZ p = centerLineBeam.GetEndPoint(0);
			XYZ q = centerLineBeam.GetEndPoint(1);
			XYZ v = p-q;
			
			
			XYZ v1 = new XYZ(v.Y, -1*v.X, v.Z);


			XYZ p1_Rb = new XYZ(p1.X, p1.Y, p1.Z - 25/304.8);
			XYZ p2_Rb = new XYZ(p2.X, p2.Y, p2.Z - 25/304.8);
			
			Line curveOfRebar = Line.CreateBound(p1, p2);
			
			
			List<Curve> myShape = new List<Curve>(){curveOfRebar};
			
			
			// Rebartype
			FilteredElementCollector fec1 = new FilteredElementCollector(doc)
			.OfClass(typeof(RebarBarType));
			
			
			IEnumerable<RebarBarType> iterRebarBarTypes =  fec1.Cast<RebarBarType>();
			
			RebarBarType myRebarType = iterRebarBarTypes.First();
			
						
			// Hooktype
			FilteredElementCollector fec2 = new FilteredElementCollector(doc)
			.OfClass(typeof(RebarHookType));
			
			
			IEnumerable<RebarHookType> iterRebarHookTypes =  fec2.Cast<RebarHookType>();
			
			RebarHookType myRebarHookType = iterRebarHookTypes.First();

			using (Transaction trans = new Transaction(doc, "rebar test") )
			
			{
				trans.Start();
				Rebar myRebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard,myRebarType,
			                                       myRebarHookType,
			                                       myRebarHookType, myBeam,
			                                       v1, myShape,
			                                       RebarHookOrientation.Left, RebarHookOrientation.Left,
			                                      true,false);

				trans.Commit();
			}


		}

		
				
		public void testRebar_FromShape()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			

			// Pick Rebar
			
			List<int> myListIdCategoryRebar = new List<int>();
			myListIdCategoryRebar.Add((int)BuiltInCategory.OST_StructuralFraming);
			
			// Select first Element (ex beam)
			Reference myRefBeam = uiDoc.Selection.PickObject(ObjectType.Element, new FilterByIdCategory(myListIdCategoryRebar), "Pick a Beam...");
			//Get element1 from ref
			Element myBeam = doc.GetElement(myRefBeam);
			

			setBeJoined(myBeam);

			LocationCurve myLocBeam = myBeam.Location as LocationCurve;
			
			Line centerLinebeam = myLocBeam.Curve as Line;
			
			XYZ p = centerLinebeam.GetEndPoint(0);
			XYZ q = centerLinebeam.GetEndPoint(1);
			XYZ v = p-q;
			
			XYZ v1 = v.CrossProduct(p);

			
			List<Curve> myShape = new List<Curve>(){centerLinebeam};
			
			
			// Rebartype
			FilteredElementCollector fec1 = new FilteredElementCollector(doc)
			.OfClass(typeof(RebarBarType));
			
			
			IEnumerable<RebarBarType> iterRebarBarTypes =  fec1.Cast<RebarBarType>();
			
			RebarBarType myRebarType = iterRebarBarTypes.First();
			
			
						
			// RebarShape
			FilteredElementCollector fec3 = new FilteredElementCollector(doc)
			.OfClass(typeof(RebarShape));
			
			
			IEnumerable<RebarShape> iterRebarBarShapes =  fec3.Cast<RebarShape>();
			
			RebarShape myRebarShape = iterRebarBarShapes.First();
			
			
			// Hooktype
			FilteredElementCollector fec2 = new FilteredElementCollector(doc)
			.OfClass(typeof(RebarHookType));
			
			
			IEnumerable<RebarHookType> iterRebarHookTypes =  fec2.Cast<RebarHookType>();
			
			RebarHookType myRebarHookType = iterRebarHookTypes.First();

			XYZ ORIGIN = new XYZ(q.X, q.Y, q.Z-25/304.8);
			
			using (Transaction trans = new Transaction(doc, "rebar test") )
			
			{
				trans.Start();
				Rebar bar = Rebar.CreateFromRebarShape(doc, myRebarShape, myRebarType, myBeam, ORIGIN, v, new XYZ(0,0,1));
				doc.Regenerate();
				
				List<Curve> myCenterLineOfRebar = bar.GetCenterlineCurves(false, false, false, MultiplanarOption.IncludeOnlyPlanarCurves,0) as List<Curve>;

				trans.Commit();
			}
			




		}

		
						
		public void testRebar_FromShapeAndCurve_Top()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			

			List<int> myListIdCategoryRebar = new List<int>();
			myListIdCategoryRebar.Add((int)BuiltInCategory.OST_StructuralFraming);
			
			// Select first Element (ex beam)
			Reference myRefBeam = uiDoc.Selection.PickObject(ObjectType.Element, new FilterByIdCategory(myListIdCategoryRebar), "Pick a Beam...");
			//Get element1 from ref
			Element myBeam = doc.GetElement(myRefBeam);
			
			setBeJoined(myBeam);
//			doc.Regenerate();
			
			LocationCurve myLocBeam = myBeam.Location as LocationCurve;
			
			List<XYZ> myEndPoints = getPointCenterTopLine(myBeam);
			
			XYZ p1 = myEndPoints[0];
			XYZ p2 = myEndPoints[1];
			
			XYZ Vp1p2 = p2-p1;
			double p1p2Length = Vp1p2.GetLength();
			
			// Cover bar thickness = 25
			double coverBar = 25/304.8;
			
			XYZ p1_Rb = p1 + coverBar/p1p2Length*Vp1p2;
			
			XYZ p2_Rb = p1 + (1 - coverBar/p1p2Length)*Vp1p2;
			
			p1_Rb = new XYZ(p1_Rb.X, p1_Rb.Y, p1_Rb.Z - coverBar);
			p2_Rb = new XYZ(p2_Rb.X, p2_Rb.Y, p2_Rb.Z - coverBar);
			
			Line curveOfRebar = Line.CreateBound(p1_Rb, p2_Rb);
			
			
			List<Curve> myShape = new List<Curve>(){curveOfRebar};
			
			
			
			Line centerLineBeam = Line.CreateBound(p1, p2);
			
			
			
			XYZ p = centerLineBeam.GetEndPoint(0);
			XYZ q = centerLineBeam.GetEndPoint(1);
			XYZ v = p-q;
			
			
			XYZ v1 = new XYZ(v.Y, -1*v.X, v.Z);

			

			

			
			
			// RebarShape
			FilteredElementCollector fec3 = new FilteredElementCollector(doc)
			.OfClass(typeof(RebarShape));
			
			
			IEnumerable<RebarShape> iterRebarBarShapes =  fec3.Cast<RebarShape>();
			
			RebarShape myRebarShape = iterRebarBarShapes.First();
			
			
			// Rebartype
			FilteredElementCollector fec1 = new FilteredElementCollector(doc)
			.OfClass(typeof(RebarBarType));
			
			
			IEnumerable<RebarBarType> iterRebarBarTypes =  fec1.Cast<RebarBarType>();
			
			RebarBarType myRebarType = iterRebarBarTypes.First();
			
						
			// Hooktype
			FilteredElementCollector fec2 = new FilteredElementCollector(doc)
			.OfClass(typeof(RebarHookType));
			
			
			IEnumerable<RebarHookType> iterRebarHookTypes =  fec2.Cast<RebarHookType>();
			
			RebarHookType myRebarHookType = iterRebarHookTypes.First();

			using (Transaction trans = new Transaction(doc, "rebar test") )
			
			{
				trans.Start();
				Rebar myRebar = Rebar.CreateFromCurvesAndShape(doc, myRebarShape, myRebarType,
				                                               null, null,
				                                               myBeam,v1,
				                                               myShape,
				                                               RebarHookOrientation.Left,
				                                               RebarHookOrientation.Left);
				//myRebar.GetShapeDrivenAccessor().SetLayoutAsFixedNumber(4,1,true, true, true);

				trans.Commit();
			}




		}


						
		public void testRebar_FromShapeAndCurve_Bot()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			

			List<int> myListIdCategoryRebar = new List<int>();
			myListIdCategoryRebar.Add((int)BuiltInCategory.OST_StructuralFraming);
			
			// Select first Element (ex beam)
			Reference myRefBeam = uiDoc.Selection.PickObject(ObjectType.Element, new FilterByIdCategory(myListIdCategoryRebar), "Pick a Beam...");
			//Get element1 from ref
			Element myBeam = doc.GetElement(myRefBeam);
			
			setBeJoined(myBeam);
//			doc.Regenerate();
			
			LocationCurve myLocBeam = myBeam.Location as LocationCurve;
			
			List<XYZ> myEndPoints = getPointCenterBottomLine(myBeam);
			
			XYZ p1 = myEndPoints[0];
			XYZ p2 = myEndPoints[1];
			
			XYZ Vp1p2 = p2-p1;
			double p1p2Length = Vp1p2.GetLength();
			
			// Cover bar thickness = 25
			double coverBar = 25/304.8;
			
			XYZ p1_Rb = p1 + coverBar/p1p2Length*Vp1p2;
			
			XYZ p2_Rb = p1 + (1 - coverBar/p1p2Length)*Vp1p2;
			
			p1_Rb = new XYZ(p1_Rb.X, p1_Rb.Y, p1_Rb.Z + coverBar);
			p2_Rb = new XYZ(p2_Rb.X, p2_Rb.Y, p2_Rb.Z + coverBar);
			
			Line curveOfRebar = Line.CreateBound(p1_Rb, p2_Rb);
			
			
			List<Curve> myShape = new List<Curve>(){curveOfRebar};
			

			Line centerLineBeam = Line.CreateBound(p1, p2);
			
			
			XYZ p = centerLineBeam.GetEndPoint(0);
			XYZ q = centerLineBeam.GetEndPoint(1);
			XYZ v = p-q;
			
			
			XYZ v1 = new XYZ(v.Y, -1*v.X, v.Z);

			
			// RebarShape
			FilteredElementCollector fec3 = new FilteredElementCollector(doc)
			.OfClass(typeof(RebarShape));
			
			
			IEnumerable<RebarShape> iterRebarBarShapes =  fec3.Cast<RebarShape>();
			
			RebarShape myRebarShape = iterRebarBarShapes.First();
			
			
			// Rebartype
			FilteredElementCollector fec1 = new FilteredElementCollector(doc)
			.OfClass(typeof(RebarBarType));
			
		
			
			IEnumerable<RebarBarType> iterRebarBarTypes =  fec1.Cast<RebarBarType>();
			
			RebarBarType myRebarType = iterRebarBarTypes.First();
		
			
						
			// Hooktype
			FilteredElementCollector fec2 = new FilteredElementCollector(doc)
			.OfClass(typeof(RebarHookType));

			IEnumerable<RebarHookType> iterRebarHookTypes =  fec2.Cast<RebarHookType>();
			
			RebarHookType myRebarHookType = iterRebarHookTypes.First();

			using (Transaction trans = new Transaction(doc, "rebar test") )
			
			{
				trans.Start();
				Rebar myRebar = Rebar.CreateFromCurvesAndShape(doc, myRebarShape, myRebarType,
				                                               null, null,
				                                               myBeam,v1,
				                                               myShape,
				                                               RebarHookOrientation.Left,
				                                               RebarHookOrientation.Left);
				//myRebar.GetShapeDrivenAccessor().SetLayoutAsFixedNumber(4,1,true, true, true);
				trans.Commit();
			}
		}

			
		// Xac dinh center line top cua 1 dam
		
		public void getPointCenterTopLineTest()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			// prompt select face 
			
			Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element);
			
			// Get element by pickface
			Element myBeam = doc.GetElement(myRef) as Element;
			
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
			
			XYZ eP1 = (P1+ P2)/2;
			XYZ eP2 = (P3 + P4)/2;
			
			if(Math.Round(centerLinebeam.Distance(eP1)) != Math.Round(centerLinebeam.Distance(eP2)))
			{
				TaskDialog.Show("abc","Deo dung");
			
			}
			else
			{
				TaskDialog.Show("abc","Deo dung NUA DI");
			}
			
			
			
			
			

		}
		
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
		            	if (Math.Round(myNormVec.Z, 1) == 1.0 && isTopFace(myFace, myBeam))
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
			XYZ v = q-p;
			
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
			
			XYZ eP1 = (P1+ P2)/2;
			XYZ eP2 = (P3 + P4)/2;
			
			XYZ vP = eP2 - eP1;
			

			if( Math.Abs(Math.Round(vP.X * v.Y - vP.Y * v.X,2)) > 0.01)
			{
				TaskDialog.Show("abc","Mat tren dam di dang, hay ve duong tham chieu cua dam center top.");
				eP1 = p;
				eP2 = q;
			
			}
			

			
			List<XYZ> myEndPointOfCenterLine = new List<XYZ>(){eP1, eP2};
			
			
			
			return myEndPointOfCenterLine;
			
		}
		
		
		public List<XYZ> getPointCenterBottomLine(Element myBeam)
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
		            	if (Math.Round(myNormVec.Z, 1) == -1.0 && isBottomFace(myFace, myBeam))
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
			XYZ v = q-p;
			
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
			
			XYZ eP1 = (P1+ P2)/2;
			XYZ eP2 = (P3 + P4)/2;
			
			XYZ vP = eP2 - eP1;
			

			if( Math.Abs(Math.Round(vP.X * v.Y - vP.Y * v.X,2)) > 0.01)
			{
				TaskDialog.Show("abc","Mat tren dam di dang, hay ve duong tham chieu cua dam center top.");
				eP1 = p;
				eP2 = q;
			
			}
			

			
			List<XYZ> myEndPointOfCenterLine = new List<XYZ>(){eP1, eP2};
			
			
			
			return myEndPointOfCenterLine;
			
		}
		
		
		
		private bool isBottomFace(Face myFace, Element myBeam) 
		
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			BoundingBoxXYZ myBoundBeam = myBeam.get_BoundingBox(null);
			
			double zMinValue = myBoundBeam.Min.Z;
			
			if (Math.Abs(zMinValue - getElevetionOfFace(myFace)) < 0.001)
			{
				return true;
			}

			return false;

		}
		

		
		private bool isTopFace(Face myFace, Element myBeam) 
		
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			BoundingBoxXYZ myBoundBeam = myBeam.get_BoundingBox(null);
			
			double zMaxValue = myBoundBeam.Max.Z;
			
			if (Math.Abs(zMaxValue - getElevetionOfFace(myFace)) < 0.001)
			{
				return true;
			}

			return false;

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
	

		
		private void setJoined(Element myJoined)
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			
			ICollection<ElementId> myListElemIdsJoined = JoinGeometryUtils.GetJoinedElements(doc, myJoined);

			using (Transaction trans = new Transaction(doc, "Switch Join")) 
			{			
				trans.Start();
				foreach (ElementId myElemId in myListElemIdsJoined) 
				{
					
					if(!JoinGeometryUtils.IsCuttingElementInJoin(doc, myJoined, doc.GetElement(myElemId)))
					{
						JoinGeometryUtils.SwitchJoinOrder(doc, myJoined, doc.GetElement(myElemId));
					}

				}

				trans.Commit();
			}	
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
	
		
		
		public void makeGroupRebarByHost()
		{                
			try {
				UIDocument uiDoc = this.ActiveUIDocument;
	            Document doc = uiDoc.Document;
	            
	            string nameGroup = "";
	            string namePartition = "";
	            int rebarCount = 1;
	            // Show form
	            
	            using (SettingDialog myInputFormSetting = new SettingDialog()) 
	            {
	            	myInputFormSetting.ShowDialog();
	            	
					//if the user hits cancel just drop out of macro
				    if(myInputFormSetting.DialogResult == System.Windows.Forms.DialogResult.Cancel) return;
				    {
				    	//else do all this :)    
				    	myInputFormSetting.Close();
				    }
				    
				    if(myInputFormSetting.DialogResult == System.Windows.Forms.DialogResult.OK)
				    {
						nameGroup = myInputFormSetting.GroupName_Tb1.Text;
						namePartition = myInputFormSetting.Partition1_Tb.Text;
						
						rebarCount = Convert.ToInt32(myInputFormSetting.RebarCount_Tb.Text);
					    	
				    	myInputFormSetting.Close();
				    }
	            } 
	            
	            
	
	            List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, new FilterByNumberRebarHostIn()).ToList();
	
	            foreach (Reference myRef in myListRef)
	            {
	                Element myHost = doc.GetElement(myRef);
	
	                RebarHostData myRbHostData = RebarHostData.GetRebarHostData(myHost);
	
	                List<Rebar> myListRebar = myRbHostData.GetRebarsInHost() as List<Rebar>;
	                
	                
	                using (Transaction trans_1 = new Transaction(doc, "Change parameter Rebar"))
	                {
	                    trans_1.Start();

	                    
	                    
    	                //Set parameter
	                    foreach (Rebar myRebar in myListRebar)
		                {
		                    
		                    //Partition
		                    Parameter partitionPara = myRebar.LookupParameter("Partition");
		
		                    if(partitionPara == null)
		                    {
		                        TaskDialog.Show("Error!!", "Has no parpation");
		                    }
		                    else
		                    {
    		                    partitionPara.Set(namePartition);
		                    }

		                    //Partition
		                    Parameter rebarCountPara = myRebar.LookupParameter("COUNT_REBAR");
		
		                    if (rebarCountPara == null)
		                    {
		                        TaskDialog.Show("Error!!", "Has no parpation");
		                    }
		                    else
		                    {
		                    	rebarCountPara.Set(rebarCount);
		                    }
		                        
		                }
	                    trans_1.Commit();
	                }
	                
	                
	                //Set parameter
	
	                List<ElementId> myListElementId = new List<ElementId>();
	                foreach (Rebar myRebar in myListRebar)
	                {
	                    myListElementId.Add(myRebar.Id);
	                }
	
	
	                using (Transaction trans = new Transaction(doc, "Make group Rebar"))
	                {
	                    trans.Start();

	                    if (myListElementId.Count > 0)
	                    {
	                        Group myGroupRebar = doc.Create.NewGroup(myListElementId);
	                        if(nameGroup !="")
	                        {
	                        	myGroupRebar.GroupType.Name = nameGroup;                        
	                        }

	
	                    }
	                    else
	                    {
	                        TaskDialog.Show("Warning!", "No rebar was hosted by this element, so no any group was created!");
	                    }
	                    trans.Commit();
	                }
	
	        	}
	       		} 
			catch (Exception) 
			{
            	TaskDialog.Show("Error!!!", "Co loi xay ra, co the tat ca rebar da co trong 1 group");
            	throw;
            }
	}
	


		
	//By name type of class. Ex: "AngularDimension", Wall

        public class FilterByNumberRebarHostIn : ISelectionFilter
        {


            public bool AllowElement(Element e)
            {
                RebarHostData myRbHostData = RebarHostData.GetRebarHostData(e);

                if (myRbHostData.GetRebarsInHost().Count > 0)
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
}