/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 3/18/2019
 * Time: 4:26 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;


using BuildingCoder;

namespace WorkingWithDimension
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("A8AE82F0-6703-4D76-96FF-E30DB07648D0")]
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

		
		
		public void duplicateDimension()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			// Pick a dimension to dupplicate it
			Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a dimension....");
			Dimension myDim = doc.GetElement(myRef) as Dimension;
			
			Line line = myDim.Curve as Line;
	
			Line line2 = line.CreateOffset(1, new XYZ(1,1,0)) as Line;
			
			
			
			if( null != line )
			{
				Autodesk.Revit.DB.View view = myDim.View;
				
				ReferenceArray references = myDim.References;
				
				using (Transaction trans = new Transaction(doc, "duplicate dimension") )
				{
					trans.Start();			
					Dimension newDimension = doc.Create.NewDimension(view, line2, references );
					trans.Commit();   	
				}

			}
	    }
	


		public void createALinearDimensionFamilyEditor()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			using(Transaction trans = new Transaction(doc, "Create linear Dimension"))
			{
				trans.Start();


			
			XYZ pt1 = new XYZ( 5, 5, 0);
			XYZ pt2 = new XYZ( 5, 10, 0);
			
			
			Line line = Line.CreateBound(pt1, pt2);
			
			Plane plane = Plane.CreateByNormalAndOrigin(pt1.CrossProduct(pt2), pt2);
			
			SketchPlane skPlane = SketchPlane.Create(doc, plane);
			
			ModelCurve modelCurve1 = doc.FamilyCreate.NewModelCurve( line, skPlane);
			
			pt1 = new XYZ(10, 5, 0);
			pt2 = new XYZ(10, 10, 0);
			
			line = Line.CreateBound( pt1, pt2);
			
			plane = Plane.CreateByNormalAndOrigin(pt1.CrossProduct(pt2), pt2);
			
			
			skPlane = SketchPlane.Create(doc, plane);
			
			ModelCurve modelCurve2 = doc.FamilyCreate.NewModelCurve(line, skPlane);
			
			// Create a linear dimension between them
			
			ReferenceArray ra = new ReferenceArray();
			ra.Append(modelCurve1.GeometryCurve.Reference);
			ra.Append( modelCurve2.GeometryCurve.Reference);
			
			pt1 = new XYZ( 5, 10, 0);
			pt2 = new XYZ(10, 10, 0);
			line = Line.CreateBound(pt1, pt2);
			
			Dimension dim = doc.FamilyCreate.NewLinearDimension(doc.ActiveView, line, ra);
			
			// Create a lable for the dimension called "width"
			
			FamilyParameter param = doc.FamilyManager.AddParameter("width",
			                                                       BuiltInParameterGroup.PG_CONSTRAINTS,
			                                                       ParameterType.Length, false);
			
			dim.FamilyLabel = param;
			
						
			trans.Commit();
			
		}
	}
	
		
		


		public void createALinearDimensionProject()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			using(Transaction trans = new Transaction(doc, "Create linear Dimension"))
			{
				trans.Start();

				
				// S1: Pick an element
				Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a foundation");
				Element myE = doc.GetElement(myRef);
				
				Options geomOp = Application.Create.NewGeometryOptions();
				geomOp.ComputeReferences = true;
				
				
				GeometryElement geometryElement = myE.get_Geometry(geomOp);
				
				
				// S2: Get Reference of face parallel
				// refArray to create dimension
				ReferenceArray ra = new ReferenceArray();
				
				List<Face> myListFaces = new List<Face>();

				
				List<Reference> myListRef = new List<Reference>();
				foreach (GeometryObject geometryObject in geometryElement) 
				{
					UV myPoint = new UV();
					
					if(geometryObject is Solid)
					{
						Solid solid = geometryObject as Solid;
						
						XYZ myNormFace = new XYZ();
						foreach (Face myFace in solid.Faces ) 
						{
							myNormFace = myFace.ComputeNormal(myPoint);
							
							if (Math.Abs(Math.Round(myNormFace.Z, 1)) == 1.0)
							{
								myListFaces.Add(myFace);
								
								myListRef.Add(myFace.Reference);
								ra.Append(myFace.Reference);
							}						
						}
					}
				}
				

				// Pick a dimension to get line from it
		
				Reference myRefDim = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a dimension....");
				Dimension myDimExits = doc.GetElement(myRefDim) as Dimension;
				
				Line lineDim = myDimExits.Curve as Line;
		
				Line line2 = lineDim.CreateOffset(1, new XYZ(1,1,0)) as Line;
					
				
				Dimension myDim = doc.Create.NewDimension(doc.ActiveView, line2, ra);

				trans.Commit();
			
		}
	}
	
	

		public void getReferenceOfGrid()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			using(Transaction trans = new Transaction(doc, "Create linear Dimension"))
			{
				trans.Start();
				
				// Select Grids
				
								
				// S1: Pick an element
				List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element,
				                                                        "Pick a Grids") as List<Reference>;
				
				ReferenceArray ra = new ReferenceArray();
				
				List<Reference> myListRef_2 = new List<Reference>();
				
				foreach (var myRef in myListRef) {
					Grid myGrid = doc.GetElement(myRef) as Grid;
					Reference myGridRef = new Reference(myGrid);
					
					ra.Append(myGridRef);
					myListRef_2.Add(myGridRef);
					
				}
				
				// Pick a dimension to get line from it
		
				Reference myRefDim = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a dimension....");
				Dimension myDimExits = doc.GetElement(myRefDim) as Dimension;
				
				Line lineDim = myDimExits.Curve as Line;
		
				Line line2 = lineDim.CreateOffset(5, new XYZ(5,5,5)) as Line;
					
				
				Dimension myDim = doc.Create.NewDimension(doc.ActiveView, line2, ra);
				
				
				trans.Commit();
				
		
		}

		
		
	}

		
			

		public void getReferenceOfLevel()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			using(Transaction trans = new Transaction(doc, "Create linear Dimension"))
			{
				trans.Start();
				
				// Select Grids
				
								
				// S1: Pick an element
				List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element,
				                                                        "Pick a Grids") as List<Reference>;
				
				ReferenceArray ra = new ReferenceArray();
				
				List<Reference> myListRef_2 = new List<Reference>();
				
				foreach (var myRef in myListRef) {
					Level myLevel = doc.GetElement(myRef) as Level;
					Reference myLevelRef = new Reference(myLevel);
					
					ra.Append(myLevelRef);
					myListRef_2.Add(myLevelRef);
					
				}
				
				// Pick a dimension to get line from it
		
				Reference myRefDim = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a dimension....");
				Dimension myDimExits = doc.GetElement(myRefDim) as Dimension;
				
				Line lineDim = myDimExits.Curve as Line;
		
				Line line2 = lineDim.CreateOffset(5, new XYZ(5,5,5)) as Line;
					
				
				Dimension myDim = doc.Create.NewDimension(doc.ActiveView, line2, ra);
				
				
				trans.Commit();
				
		
		}

		
		
	}





		public void dimWall()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			using(Transaction trans = new Transaction(doc, "Create linear Dimension"))
			{
				trans.Start();
				
				ICollection<ElementId> ids = uiDoc.Selection.GetElementIds();
				
				List<Wall> walls = new List<Wall>(2);
				
				foreach (ElementId id in ids) {
					Element e = doc.GetElement(id);
					
					if(e is Wall)
					{
						walls.Add(e as Wall);	
					}
					
				}
				
				if( 2 != walls.Count)
				{
					TaskDialog.Show("abc", "Please select two parallel opposing straight walls.");
					return;
				}
				
				List<Line> lines = new List<Line>(2);
				List<XYZ> midpoints = new List<XYZ>(2);
				XYZ normal = null;
				
				
				foreach (Wall wall in walls) {
					LocationCurve lc = wall.Location as LocationCurve;
					Curve curve = lc.Curve;
					
					if( !(curve is Line))
					{
						TaskDialog.Show("abc", "Please select two parallel opposing straight walls.");
						return;
					}
					
					Line l = curve as Line;
					lines.Add(l);
					midpoints.Add( Util.Midpoint(l));
					if(null == normal)
					{
						normal = Util.Normal(l);
					}
					
					else
					{
						if(!Util.IsParallel(normal, Util.Normal(l)))
						{
							TaskDialog.Show("abc", "Please select two parallel opposing straight walls.");
							return;
						}
					}	
				}	
				}
				

				trans.Commit();
		
		
		}


		
		
}
}