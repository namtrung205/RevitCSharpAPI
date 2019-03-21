/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 3/17/2019
 * Time: 1:24 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace WorkingWithFace
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("DE5D6781-2A6E-4DB8-B433-0B73DE790F9E")]
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
			// set active document
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			// prompt select face 
			
			Reference myRef = uiDoc.Selection.PickObject(ObjectType.Face);
			
			// Get element by pickface
			Element e = doc.GetElement(myRef) as Element;
			

			//Get GeoObject from element;
			GeometryObject myGeoObj =  e.GetGeometryObjectFromReference(myRef) as Face;
			
			//Get face from element Object:
			Face myPickedFace = myGeoObj as Face;
			
			// show Info of Face
			UV myPoint = new UV(0,0);
			
			BoundingBoxUV myBoudingBox = myPickedFace.GetBoundingBox();
			
			TaskDialog.Show("Abc", "\nMax boundingbox : " + myBoudingBox.Max.U + ", " + myBoudingBox.Max.V);

			
			// Get normal vector
			XYZ myXYZpoint = myPickedFace.ComputeNormal(myPoint);
			TaskDialog.Show("Normal Point: ", XYZtoString(myXYZpoint));
			
			// Get BoundingBox
			BoundingBoxUV faceUVBound = myPickedFace.GetBoundingBox();
			TaskDialog.Show("abc","max: " + faceUVBound.Max.ToString() + Environment.NewLine 
			                + "min: " + faceUVBound.Min.ToString());
			
			TaskDialog.Show("abc","the face was picked has area: "+ myPickedFace.Area.ToString());
			
		}
		
		public void paintSurface()
	{
		// set active document
		UIDocument uiDoc = this.ActiveUIDocument;
		Document doc = uiDoc.Document;
		
		// prompt select face 
		
		List<Reference> myLisrRef = uiDoc.Selection.PickObjects(ObjectType.Face) as List<Reference>;
		
		
		FilteredElementCollector fec = new FilteredElementCollector(doc).OfClass(typeof(Material));
		
		Material materialElem = fec.
			Cast<Material>().
			First<Material>( myMaterial => myMaterial.Name == "Default Form" );
		
		ElementId myMaterialEleId = materialElem.Id;
		
		using (Transaction trans = new Transaction(doc, "Paint surface" )) 
		{
			trans.Start();
			foreach (Reference myRef in myLisrRef) 
			{
				Element E = doc.GetElement(myRef);
				
				GeometryObject myGeoObj = E.GetGeometryObjectFromReference(myRef);
				
				Face myPickedFace = myGeoObj as Face;
				
				doc.Paint(E.Id, myPickedFace, myMaterialEleId);
			}
			trans.Commit();
		}	
	
	}
	
		
	
		public void unPaintSurface()
	{
		// set active document
		UIDocument uiDoc = this.ActiveUIDocument;
		Document doc = uiDoc.Document;
		
		// prompt select face 
		
		List<Reference> myLisrRef = uiDoc.Selection.PickObjects(ObjectType.Face) as List<Reference>;
		
		
		FilteredElementCollector fec = new FilteredElementCollector(doc).OfClass(typeof(Material));
		
		Material materialElem = fec.
			Cast<Material>().
			First<Material>( myMaterial => myMaterial.Name == "Default Form" );
		
		ElementId myMaterialEleId = materialElem.Id;
		
		using (Transaction trans = new Transaction(doc, "Paint surface" )) 
		{
			trans.Start();
			foreach (Reference myRef in myLisrRef) 
			{
				Element E = doc.GetElement(myRef);
				
				GeometryObject myGeoObj = E.GetGeometryObjectFromReference(myRef);
				
				Face myPickedFace = myGeoObj as Face;
				
				doc.RemovePaint(E.Id, myPickedFace);

			}
			trans.Commit();
		}
			
		
	}
	
		

		public void paintWalls()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			
			//filtered
			
			List<string> myFil = new List<string>(){"Wall"};
			
			// Select multiple Wall
			List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, 
			                 new FilterByNameElementType(myFil), "Select Walls") as List<Reference>;
			
			
			//Get Material Element
			
						
			FilteredElementCollector fec = new FilteredElementCollector(doc).OfClass(typeof(Material));
    		
    		Material materialElem = fec.
    			Cast<Material>().
    			First<Material>( myMaterial => myMaterial.Name == "Default Form" );
			
			ElementId myMaterialEleId = materialElem.Id;
			
			
			
			foreach (Reference myRef in myListRef) 
			{
				Element myElem = doc.GetElement(myRef);
				Wall myWall = myElem as Wall;
				
				paintFaceOfWall(myWall, myMaterialEleId);
			}

		}
		
		private void paintFaceOfWall(Wall myWall, ElementId myMatId)
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			GeometryElement geometryElement = myWall.get_Geometry(new Options());
			
			using (Transaction myTrans = new Transaction(doc, "fil face of wall and Paint")) 
			{
			
				myTrans.Start();
				UV myPoint = new UV(0,0);

				foreach (GeometryObject geometryObject in geometryElement)
			    {
			        if (geometryObject is Solid)
			        {
			            Solid solid = geometryObject as Solid;
			            XYZ myNormVec = new XYZ();
			            foreach (Face myFace in solid.Faces)
			            {
			            	myNormVec = myFace.ComputeNormal(myPoint);
			            	if (Math.Round(Math.Abs(myNormVec.Z), 1) != 1.0)
							{
			            		doc.Paint(myWall.Id, myFace, myMatId);
							}
			            }
			        }
			    }
				myTrans.Commit();
			}
		
		}
		
		
		private string XYZtoString(XYZ myXYZ)
		{
			return string.Format("X: {0}, Y: {1}, Z: {2}", Math.Round(myXYZ.X,2), Math.Round(myXYZ.Y, 2), Math.Round(myXYZ.Z,2));
		
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
	
}