/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 6/8/2019
 * Time: 3:32 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

using BuildingCoder;


namespace WorkingWithDimension_2
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("73FE242B-A902-4758-BE82-2A04175F802C")]
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
		
				Line line2 = lineDim.CreateOffset(1, new XYZ(1,1,1)) as Line;
					
				
				
				
				Dimension myDim = doc.Create.NewDimension(doc.ActiveView, line2, ra);

				trans.Commit();
			
		}
	}
		

		

		public void createALinearDimensionProject_Pickpoint()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			


				
				Options geomOp = Application.Create.NewGeometryOptions();
				geomOp.ComputeReferences = true;
				
				
				// S2: Get Reference of face parallel
				// refArray to create dimension
				ReferenceArray ra = new ReferenceArray();
				
				List<Face> myListFaces = new List<Face>();
				List<Reference> myListRef = new List<Reference>();
				
				// S1: Pick an element
				Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a foundation");
				Element myE = doc.GetElement(myRef);
				
				GeometryElement geometryElement = myE.get_Geometry(geomOp);
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
				

				setCurrentViewAsWorkPlan();
				
				XYZ myDimPoint_1 = uiDoc.Selection.PickPoint("Pick Point To Place Dimension....");
				
				//TaskDialog.Show("check", XYZtoString(doc.ActiveView.ViewDirection));
				
				XYZ viewDirect = doc.ActiveView.ViewDirection;
				XYZ rightDirect = doc.ActiveView.RightDirection;
				
				XYZ plusXYZ = new XYZ();
				
				if (Math.Round(viewDirect.Z, 6) == 0) 
				{
					plusXYZ = new XYZ(-viewDirect.Y+1, viewDirect.X, viewDirect.Z+1);
					//plusXYZ = new XYZ(- rightDirect.X+1, rightDirect.Y, rightDirect.Z+1);
					
				}
				
				//XYZ plusXYZ = new XYZ();
				
				else{
					
					Grid firstGrid = doc.GetElement(myListRef[0]) as Grid;
					Line gridLine = firstGrid.Curve as Line;
					XYZ girdLineDirect = gridLine.Direction;
					plusXYZ = new XYZ( - girdLineDirect.Y, girdLineDirect.X, girdLineDirect.Z);
					//plusXYZ = new XYZ(0,1,0);
				}
				XYZ myDimPoint_2 = myDimPoint_1 + plusXYZ;
				Line l = Line.CreateBound(myDimPoint_1, myDimPoint_2);	
				
				using(Transaction trans = new Transaction(doc, "Create linear Dimension"))
				{
				trans.Start();
				Dimension myDim = doc.Create.NewDimension(doc.ActiveView, l, ra);
				trans.Commit();	
		}
	}
		

		

		public void createALinearDimensionProject_Pickpoint2()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
		
			Options geomOp = Application.Create.NewGeometryOptions();
			geomOp.ComputeReferences = true;
			
			

			
			// S2: Get Reference of face parallel
			// refArray to create dimension
			ReferenceArray ra = new ReferenceArray();
			
			List<Face> myListFaces = new List<Face>();
			//List<Reference> myListRef = new List<Reference>();
			
			// S1: Pick an element
			List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, "Pick element") as List<Reference>;
			
			foreach (Reference myRef in myListRef) {

				Element myE = doc.GetElement(myRef);
				GeometryElement geometryElement = myE.get_Geometry(geomOp);				

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
								
								//myListRef.Add(myFace.Reference);
								ra.Append(myFace.Reference);
							}						
						}
					}
				}
			
			}
			setCurrentViewAsWorkPlan();
			
			XYZ myDimPoint_1 = uiDoc.Selection.PickPoint("Pick Point To Place Dimension....");
			
			//TaskDialog.Show("check", XYZtoString(doc.ActiveView.ViewDirection));
			
			XYZ viewDirect = doc.ActiveView.ViewDirection;
			XYZ rightDirect = doc.ActiveView.RightDirection;
			
			XYZ plusXYZ = new XYZ();
			
			if (Math.Round(viewDirect.Z, 6) == 0) 
			{
				//plusXYZ = new XYZ(-viewDirect.Y+1, viewDirect.X, viewDirect.Z+1);
				//plusXYZ = new XYZ(- rightDirect.X+1, rightDirect.Y, rightDirect.Z+1);
				
			}
			
			//XYZ plusXYZ = new XYZ();
			
			else{
				
				Grid firstGrid = doc.GetElement(myListRef[0]) as Grid;
				Line gridLine = firstGrid.Curve as Line;
				XYZ girdLineDirect = gridLine.Direction;
				plusXYZ = new XYZ( - girdLineDirect.Y, girdLineDirect.X, girdLineDirect.Z);
				//plusXYZ = new XYZ(0,1,0);
			}
			plusXYZ = new XYZ(0,0,1);
			XYZ myDimPoint_2 = myDimPoint_1 + plusXYZ;

			
			Line l = Line.CreateBound(myDimPoint_1, myDimPoint_2);	
			
			ElementId myDimId;
			
			using(Transaction trans = new Transaction(doc, "Create linear Dimension"))
			{
				trans.Start();
				Dimension myDim = doc.Create.NewDimension(doc.ActiveView, l, ra);

				myDimId = myDim.Id;
				trans.Commit();	
			}
			
			Line l2 = Line.CreateBound(myDimPoint_1, myDimPoint_2);
			
			using(Transaction trans = new Transaction(doc, "Recreate linear Dimension"))
			{
				trans.Start();
				Dimension myOldDim = doc.GetElement(myDimId) as Dimension;
				
				List<Reference> myListRefDim = new List<Reference>();
				
				foreach (Reference myRefDim in myOldDim.References) 
				{
					myListRefDim.Add(myRefDim);
				}
				
				ReferenceArray myRaDim = new ReferenceArray();
				myRaDim.Append(myListRefDim[0]);
				myRaDim.Append(myListRefDim[myListRefDim.Count-1]);
				
				Dimension myDim = doc.Create.NewDimension(doc.ActiveView, l, myRaDim);

				myDimId = myDim.Id;
				trans.Commit();	
			}				
				
	}
		

		

		public void dimLevels()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			
			// Filter by name 
			
			FilterByNameElementType myFilter = new FilterByNameElementType(new List<string>(){"Level"});
			
				
			// Select Grids		
			// S1: Pick an element
			List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, myFilter,
			                                                        "Pick Levels...") as List<Reference>;
			ReferenceArray ra = new ReferenceArray();
			
			//List<Reference> myListRef_2 = new List<Reference>();
			
			foreach (var myRef in myListRef) {
				Level myLevel = doc.GetElement(myRef) as Level;
				Reference myLevelRef = new Reference(myLevel);
				ra.Append(myLevelRef);
				//myListRef_2.Add(myLevelRef);
				
			}
			
			setCurrentViewAsWorkPlan();
			
			XYZ myDimPoint_1 = uiDoc.Selection.PickPoint("Pick Point To Place Dimension....");
			XYZ myDimPoint_2 = new XYZ(myDimPoint_1.X, myDimPoint_1.Y, myDimPoint_1.Z + 5);
			Line dimLine = Line.CreateBound(myDimPoint_1, myDimPoint_2);

				
			using(Transaction trans = new Transaction(doc, "Create linear Dimension"))
			{
				trans.Start();
				Dimension myDim = doc.Create.NewDimension(doc.ActiveView, dimLine, ra);
				trans.Commit();
			}
		
		}


		public void dimGrids()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			// Filter by name 
			
			FilterByNameElementType myFilter = new FilterByNameElementType(new List<string>(){"Grid"});
			
			// Select Grids		
			// S1: Pick an element
			List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, myFilter,
			                                                        "Pick Grids....") as List<Reference>;
			
			ReferenceArray ra = new ReferenceArray();
			
			List<XYZ> myListDirectLine= new List<XYZ>();
			
			foreach (var myRef in myListRef) {
				Grid myGrid = doc.GetElement(myRef) as Grid;
				Reference myGridRef = new Reference(myGrid);
				Line gridAsLine = myGrid.Curve as Line;
				if(gridAsLine == null)
				{
					TaskDialog.Show("Error", "Chỉ hỗ trợ Grid dạng Line song song với nhau, hãy chọn cẩn thận...");
					return;
				}
				myListDirectLine.Add(gridAsLine.Direction);
				
				ra.Append(myGridRef);
				//myListRef_2.Add(myGridRef);	
			}
			
			if(! isListLineParallel(myListDirectLine))
			{
				TaskDialog.Show("Error", "Chỉ hỗ trợ Grid dạng Line song song với nhau, hãy chọn cẩn thận...");
				return;
			}
			
			setCurrentViewAsWorkPlan();
			
			XYZ myDimPoint_1 = uiDoc.Selection.PickPoint("Pick Point To Place Dimension....");
			
			//TaskDialog.Show("check", XYZtoString(doc.ActiveView.ViewDirection));
			
			XYZ viewDirect = doc.ActiveView.ViewDirection;
			
			XYZ plusXYZ = new XYZ();
			
			if (Math.Round(viewDirect.Z, 6) == 0) 
			{
				plusXYZ = new XYZ( - viewDirect.Y, viewDirect.X, viewDirect.Z);
				
			}
			
			else{
				
				Grid firstGrid = doc.GetElement(myListRef[0]) as Grid;
				Line gridLine = firstGrid.Curve as Line;
				XYZ girdLineDirect = gridLine.Direction;
				plusXYZ = new XYZ( - girdLineDirect.Y, girdLineDirect.X, girdLineDirect.Z);
				//plusXYZ = new XYZ(0,1,0);
			}
			XYZ myDimPoint_2 = myDimPoint_1 + plusXYZ;
			Line l = Line.CreateBound(myDimPoint_1, myDimPoint_2);	
			using(Transaction trans = new Transaction(doc, "Create linear Dimension"))
			{
				trans.Start();
				Dimension myDim = doc.Create.NewDimension(doc.ActiveView, l, ra);
				trans.Commit();
			}
		
		}


		public void dimGrids_Para(UIDocument uiDoc, List<Reference> myListRef)
		{
			Document doc = uiDoc.Document;
			
			ReferenceArray ra = new ReferenceArray();
			
			List<XYZ> myListDirectLine= new List<XYZ>();
			
			foreach (var myRef in myListRef) {
				Grid myGrid = doc.GetElement(myRef) as Grid;
				Reference myGridRef = new Reference(myGrid);
				Line gridAsLine = myGrid.Curve as Line;
				if(gridAsLine == null)
				{
					TaskDialog.Show("Error", "Chỉ hỗ trợ Grid dạng Line song song với nhau, hãy chọn cẩn thận...");
					return;
				}
				myListDirectLine.Add(gridAsLine.Direction);
				
				ra.Append(myGridRef);
				//myListRef_2.Add(myGridRef);	
			}
			
			if(! isListLineParallel(myListDirectLine))
			{
				TaskDialog.Show("Error", "Chỉ hỗ trợ Grid dạng Line song song với nhau, hãy chọn cẩn thận...");
				return;
			}
			
			setCurrentViewAsWorkPlan();
			
			XYZ myDimPoint_1 = uiDoc.Selection.PickPoint("Pick Point To Place Dimension....");
			
			//TaskDialog.Show("check", XYZtoString(doc.ActiveView.ViewDirection));
			
			XYZ viewDirect = doc.ActiveView.ViewDirection;
			
			XYZ plusXYZ = new XYZ();
			
			if (Math.Round(viewDirect.Z, 6) == 0) 
			{
				plusXYZ = new XYZ( - viewDirect.Y, viewDirect.X, viewDirect.Z);
				
			}
			
			else{
				
				Grid firstGrid = doc.GetElement(myListRef[0]) as Grid;
				Line gridLine = firstGrid.Curve as Line;
				XYZ girdLineDirect = gridLine.Direction;
				plusXYZ = new XYZ( - girdLineDirect.Y, girdLineDirect.X, girdLineDirect.Z);
				//plusXYZ = new XYZ(0,1,0);
			}
			XYZ myDimPoint_2 = myDimPoint_1 + plusXYZ;
			Line l = Line.CreateBound(myDimPoint_1, myDimPoint_2);	
			using(Transaction trans = new Transaction(doc, "Create linear Dimension"))
			{
				trans.Start();
				Dimension myDim = doc.Create.NewDimension(doc.ActiveView, l, ra);
				trans.Commit();
			}
		
		
		
		}
		
	

		public void dimLevels_Para(UIDocument uiDoc, List<Reference> myListRef)
		{
			
			Document doc = uiDoc.Document;
			
			ReferenceArray ra = new ReferenceArray();
			
			//List<Reference> myListRef_2 = new List<Reference>();
			
			foreach (var myRef in myListRef) {
				Level myLevel = doc.GetElement(myRef) as Level;
				Reference myLevelRef = new Reference(myLevel);
				ra.Append(myLevelRef);
				//myListRef_2.Add(myLevelRef);
				
			}
			
			setCurrentViewAsWorkPlan();
			
			XYZ myDimPoint_1 = uiDoc.Selection.PickPoint("Pick Point To Place Dimension....");
			XYZ myDimPoint_2 = new XYZ(myDimPoint_1.X, myDimPoint_1.Y, myDimPoint_1.Z + 5);
			Line dimLine = Line.CreateBound(myDimPoint_1, myDimPoint_2);

				
			using(Transaction trans = new Transaction(doc, "Create linear Dimension"))
			{
				trans.Start();
				Dimension myDim = doc.Create.NewDimension(doc.ActiveView, dimLine, ra);
				trans.Commit();
			}
		
		}


		
		public void dimGridsOrLevels()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			// Filter by name 
			
			FilterByNameElementType myFilter = new FilterByNameElementType(new List<string>(){"Grid", "Level"});
			
			// Select Grids		
			// S1: Pick an element
			List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, myFilter,
			                                                        "Pick Grids or Levels....") as List<Reference>;
		
			// Group Type Referent
			List<Reference> listRef_Level = new List<Reference>();
			List<Reference> listRef_Grid = new List<Reference>();			
			
			foreach (Reference myRefChecking in myListRef) {
				if(doc.GetElement(myRefChecking) is Level)
				{
					listRef_Level.Add(myRefChecking);
				}
				else
				{
					listRef_Grid.Add(myRefChecking);
				}
			}
			
			
			if(listRef_Grid.Count>=2)
			{
				dimGrids_Para(uiDoc, listRef_Grid);
			}
			if(listRef_Level.Count>=2)
			{
				dimLevels_Para(uiDoc, listRef_Level);
			}
	
		}

		
		public void mergeDims()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			// Filter by name 
			FilterByNameElementType myFilter = new FilterByNameElementType(new List<string>(){"Dimension"});
			
			// Select Grids		
			// S1: Pick an element
			List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, myFilter,
			                                                        "Pick Dimensions....") as List<Reference>;
			
			ReferenceArray ra = new ReferenceArray();
			
			List<XYZ> myListDirectLine = new List<XYZ>();
			
			
			// Getline of first Dimension
			
			Dimension firstDim = doc.GetElement(myListRef[0]) as Dimension;
			Line firstDimAsLine = firstDim.Curve as Line; 
			//Line firstDimAsLine = firstDimAsLine_2.CreateOffset(4,new XYZ(1,1,1)) as Line;
			
//			ICollection<ElementId> myDeleteElemIds = null;
			
			foreach (Reference myRef in myListRef) 
			{
				Dimension myDim = doc.GetElement(myRef) as Dimension;
				
				foreach (Reference refOfDim in myDim.References) 
				{
					ra.Append(refOfDim);
				}
				
				Line dimAsLine = myDim.Curve as Line; 
				if(dimAsLine == null)
				{
					TaskDialog.Show("Error", "Chỉ hỗ trợ Linear Dimensions, hãy chọn cẩn thận...");
					return;
				}
				//myListDirectLine.Add(dimAsLine.Direction);
				
//				myDeleteElemIds.Add(myRef.ElementId);
			}
			
//			if(! isListLineParallel(myListDirectLine))
//			{
//				TaskDialog.Show("Error", "Các dimensions bạn chọn không cùng phương, hãy chọn cẩn thận...");
//				return;
//			}
			
			//setCurrentViewAsWorkPlan();
			
			Autodesk.Revit.DB.View view = firstDim.View;

			
//			
//			XYZ myDimPoint_1 = uiDoc.Selection.PickPoint("Pick Point To Place Dimension....");
//			
//			//TaskDialog.Show("check", XYZtoString(doc.ActiveView.ViewDirection));
//			
//			XYZ viewDirect = doc.ActiveView.ViewDirection;
//			
//			XYZ plusXYZ = new XYZ();
//			
//			if (Math.Round(viewDirect.Z, 6) == 0) 
//			{
//				plusXYZ = new XYZ( - viewDirect.Y, viewDirect.X, viewDirect.Z);
//				
//			}
//			
//			else{
//				
//				Grid firstGrid = doc.GetElement(myListRef[0]) as Grid;
//				Line gridLine = firstGrid.Curve as Line;
//				XYZ girdLineDirect = gridLine.Direction;
//				plusXYZ = new XYZ( - girdLineDirect.Y, girdLineDirect.X, girdLineDirect.Z);
//				//plusXYZ = new XYZ(0,1,0);
//			}
//			XYZ myDimPoint_2 = myDimPoint_1 + plusXYZ;
//			Line l = Line.CreateBound(myDimPoint_1, myDimPoint_2);
			using(Transaction trans = new Transaction(doc, "Merge Dimensions"))
			{
				trans.Start();
				Dimension myMergeDim = doc.Create.NewDimension(view, firstDimAsLine, ra);
				
				// Delete dimensions
				
				//doc.Delete(myDeleteElemIds);
				
				trans.Commit();
			}
		
		}


	
		public void getAllRefOfDims()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			// Filter by name 
			FilterByNameElementType myFilter = new FilterByNameElementType(new List<string>(){"Dimension"});
			
			// Select Grids		
			// S1: Pick an element
			List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, myFilter,
			                                                        "Pick Dimensions....") as List<Reference>;
			
			ReferenceArray ra = new ReferenceArray();
			
			List<XYZ> myListDirectLine = new List<XYZ>();
			
			
			Dimension firstDim = doc.GetElement(myListRef[0]) as Dimension;
			Line firstDimAsLine = firstDim.Curve as Line; 
			
			List<ElementId> myDeleteElemIds = new List<ElementId>();
			
			List<Reference> test = new List<Reference>();
			
			foreach (Reference myRef in myListRef) 
			{
				Dimension myDim = doc.GetElement(myRef) as Dimension;
				
				foreach (Reference refOfDim in myDim.References) 
				{
					string refStable = refOfDim.ConvertToStableRepresentation(doc);
					
					if (ra.Size < 2) 
					{
						ra.Append(refOfDim);
					}
					else
					{
						foreach (Reference refInRefArray in ra) 
						{
							if(refInRefArray.ConvertToStableRepresentation(doc) != refStable)
							{
								ra.Append(refOfDim);
							}
						}				
					}
				}
				
				Line dimAsLine = myDim.Curve as Line; 
				if(dimAsLine == null)
				{
					TaskDialog.Show("Error", "Chỉ hỗ trợ Linear Dimensions, hãy chọn cẩn thận...");
					return;
				}
				//myListDirectLine.Add(dimAsLine.Direction);
				
				myDeleteElemIds.Add(myRef.ElementId);
				
			}
			
			Autodesk.Revit.DB.View view = firstDim.View;
			
			TaskDialog.Show("Break", "Point" + ra.Size.ToString());
			
			using(Transaction trans = new Transaction(doc, "Merge Dimensions"))
			{
				trans.Start();
				Dimension myMergeDim = doc.Create.NewDimension(view, firstDimAsLine, ra);
				
				// Delete dimensions
				
				doc.Delete(myDeleteElemIds);
				
				trans.Commit();
			}
			
			
		
		}
		
		
		
		public void getAllRefOfDims_2()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			// Filter by name 
			FilterByNameElementType myFilter = new FilterByNameElementType(new List<string>(){"Dimension"});
			
			// Select Grids		
			// S1: Pick an element
			List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, myFilter,
			                                                        "Pick Dimensions....") as List<Reference>;
			
			ReferenceArray ra = new ReferenceArray();
			
			List<XYZ> myListDirectLine = new List<XYZ>();
			
			
			Dimension firstDim = doc.GetElement(myListRef[0]) as Dimension;
			Line firstDimAsLine = firstDim.Curve as Line; 
			
			List<ElementId> myDeleteElemIds = new List<ElementId>();
			
			List<Reference> test = new List<Reference>();
			
			List<string> stableRepRefList = new List<string>();
			
			foreach (Reference myRef in myListRef) 
			{
				Dimension myDim = doc.GetElement(myRef) as Dimension;
				
				foreach (Reference refOfDim in myDim.References) 
				{
					string refStable = refOfDim.ConvertToStableRepresentation(doc);
					
					if(!stableRepRefList.Contains(refStable))
				   {
						stableRepRefList.Add(refStable);
				   }
				}
				
				Line dimAsLine = myDim.Curve as Line; 
				if(dimAsLine == null)
				{
					TaskDialog.Show("Error", "Chỉ hỗ trợ Linear Dimensions, hãy chọn cẩn thận...");
					return;
				}
				//myListDirectLine.Add(dimAsLine.Direction);
				
				myDeleteElemIds.Add(myRef.ElementId);
				
			}
			
			foreach (string stableRepRef in stableRepRefList) 
			{
				ra.Append(Reference.ParseFromStableRepresentation(doc, stableRepRef));
			}
			
			
			Autodesk.Revit.DB.View view = firstDim.View;
			
			TaskDialog.Show("Break", "Point" + ra.Size.ToString());
			
			using(Transaction trans = new Transaction(doc, "Merge Dimensions"))
			{
				trans.Start();
				Dimension myMergeDim = doc.Create.NewDimension(view, firstDimAsLine, ra);
				
				// Delete dimensions
				
				doc.Delete(myDeleteElemIds);
				
				trans.Commit();
			}
			
			
		
		}
		
			
		
		
		
		public bool isListLineParallel(List<XYZ> myListXYZ)
		{
			for (int i = 0; i < myListXYZ.Count; i++) {
				List<XYZ> subList = myListXYZ.GetRange(i+1, myListXYZ.Count - (i + 1));
				foreach (XYZ nextXYZ in subList)
				{
					double lengthCross = Math.Round(nextXYZ.CrossProduct(myListXYZ[i]).GetLength(), 7);
					
					if (lengthCross >= 0.0000001)
					{
						return false;
					}
				}
			}
			
			return true;
		}
		
		
		
		
		
		public void testListParallel()
		{
			List<XYZ> myListXYZ = new List<XYZ>(){new XYZ(1, 1, 0),
												new XYZ(9, 9, 0),
												new XYZ(5.05, 5.05, 0),
												new XYZ(7, 7, 0),
												new XYZ(5, 5, 0),			
			
			};
			
			
			TaskDialog.Show("abc", isListLineParallel(myListXYZ).ToString());
		
		}
		
		
		
		
		public void setCurrentViewAsWorkPlan()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			using(Transaction trans = new Transaction(doc, "WorkPlane"))
			{
				trans.Start();
				
//				Plane plane = new Plane( uiDoc.Document.ActiveView.ViewDirection, uiDoc.Document.ActiveView.Origin);

				Plane plane = Plane.CreateByNormalAndOrigin(uiDoc.Document.ActiveView.ViewDirection, uiDoc.Document.ActiveView.Origin);
				
				
				SketchPlane sp = SketchPlane.Create(doc, plane);
  				uiDoc.Document.ActiveView.SketchPlane = sp;
				trans.Commit();
			}
		}
		
		

		private static Face GetCloseFace(Element e,
		                         XYZ p,
		                         XYZ normal,
		                         Options opt)
		{
			Face face  = null;
			double min_distance = double.MaxValue;
			GeometryElement geo = e.get_Geometry(opt);
			
			foreach (GeometryObject obj in geo) 
			{
				Solid solid = obj as Solid;
				if(solid != null)
				{
					FaceArray fa = solid.Faces;
					foreach (Face f in fa) 
					{
						PlanarFace pf = f as PlanarFace;
						if (null != pf && Util.IsParallel(normal, pf.FaceNormal))
						{
							XYZ v = p - pf.Origin;
							double d = v.DotProduct(-pf.FaceNormal);
							if(d< min_distance)
							{
								face = f;
								min_distance = d;
							}
						}						
					}
				}
			}
			
			return face;

		}
		
		private static void CreateDimensionElement(
									      View view,
									      XYZ p1,
									      Reference r1,
									      XYZ p2,
									      Reference r2 )
	    {
			Document doc = view.Document;
			
			ReferenceArray ra = new ReferenceArray();
			
			ra.Append( r1 );
			ra.Append( r2 );
			
			Line line = Line.CreateBound( p1, p2 );
			
			using( Transaction t = new Transaction( doc ) )
			{
				t.Start( "Create New Dimension" );
				
				Dimension dim = doc.Create.NewDimension( view, line, ra );
				
				t.Commit();
			}
		}	


		public void dimWall()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
				
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


			Options opt = this.Application.Create.NewGeometryOptions();
			opt.ComputeReferences = true;
			
			List<Face> faces = new List<Face>(2);
			
			faces.Add(GetCloseFace(walls[0], midpoints[1], normal, opt));
			faces.Add(GetCloseFace(walls[1], midpoints[0], normal, opt));

			CreateDimensionElement(doc.ActiveView,
                                   midpoints[0], faces[0].Reference,
                                  midpoints[1], faces[1].Reference);
		}
	
	
	
		private string XYZtoString(XYZ myXYZ)
		{
			return string.Format("X: {0}, Y: {1}, Z: {2}", Math.Round(myXYZ.X,2), Math.Round(myXYZ.Y, 2), Math.Round(myXYZ.Z,2));
		
		}
	
		
		
		// Select all Grid in a view
		
		public void selectAllGridsInView()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			// Element FIlter
			
			
			
			ElementFilter myElementFilter = new ElementClassFilter(typeof(Grid));
			
			FilteredElementCollector myCollector = new FilteredElementCollector(doc, doc.ActiveView.Id).WherePasses(myElementFilter);
			
			TaskDialog.Show("abc", "number of grids in view: " + myCollector.Count());

			List<Grid> myListRefGridCollector = new List<Grid>();
			
			List<Level> myListLevelCollector = new List<Level>();

			foreach (var elem in myCollector) 
			{
				if(elem is Grid)
				{	
					Grid myGrid = elem as Grid;
					myListRefGridCollector.Add(myGrid);
				}
				
				if(elem is Level)
				{	
					Level myLevel = elem as Level;
					myListLevelCollector.Add(myLevel);
				}
			}
		
		}
		
		
		/// <summary>
		/// Auto dimsection view
		/// 
		/// </summary>
	
		public void autoDimSection()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			Reference selectedBeamRef  = uiDoc.Selection.PickObject(ObjectType.Element, "pick a beam...");
			
			FamilyInstance selectedBeam = doc.GetElement(selectedBeamRef) as FamilyInstance;
			
			if(selectedBeam == null) return;
			
			var beamFaces = getSolids(selectedBeam).SelectMany(x => x.Faces.OfType<PlanarFace>()).ToList();
			
			View view = doc.ActiveView;
			
			
			PlanarFace leftFace = beamFaces.FirstOrDefault(x => x.ComputeNormal(UV.Zero).
			                                       IsAlmostEqualTo(-1*view.RightDirection));
			
			PlanarFace rightFace = beamFaces.FirstOrDefault(x => x.ComputeNormal(UV.Zero).
			                                       IsAlmostEqualTo(view.RightDirection));			                                       
			                                       
			if(leftFace == null || rightFace == null)
			{
				TaskDialog.Show("abc", "Can't create dimension");
				return;
			}
			
			double shift  = UnitUtils.ConvertToInternalUnits(500, DisplayUnitType.DUT_MILLIMETERS);
			
			XYZ dimensionOrigin = selectedBeam.GetTotalTransform().Origin + shift * view.UpDirection;
			
			Line dimensionLine = Line.CreateBound(dimensionOrigin, view.RightDirection);
			
			ReferenceArray dimensionReferences = new ReferenceArray();
			
			dimensionReferences.Append(leftFace.Reference);
			dimensionReferences.Append(rightFace.Reference);
			
			using (Transaction trans = new Transaction(doc, "Create dimension...")) {
				trans.Start();
				
				Dimension myDim = doc.Create.NewDimension(view, dimensionLine, dimensionReferences);
				
				
				trans.Commit();
			}
			
			
		}


	
		public void autoDimSection_2()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			Reference selectedBeamRef  = uiDoc.Selection.PickObject(ObjectType.Element, "pick a beam...");
			
			//Pick Element as Beam
			Element selectedBeam = doc.GetElement(selectedBeamRef) as Element;
			
			if(selectedBeam == null) return;
			
			Options myOptions = new Options();
			myOptions.ComputeReferences = true;
			
			GeometryElement geometryElement = selectedBeam.get_Geometry(myOptions);
			List<PlanarFace> myListFace = new List<PlanarFace>();
			
			foreach (GeometryInstance myGeoInstance in geometryElement) {
				
				GeometryElement myGeoElement_2 = myGeoInstance.GetInstanceGeometry();
				foreach (GeometryObject myGeo_2 in myGeoElement_2) {
					if(myGeo_2 is Solid)
					{
						Solid mySolid = myGeo_2 as Solid;
						if(mySolid.Volume !=0)
						{
							foreach (PlanarFace myPlanarFace in mySolid.Faces) {
								myListFace.Add(myPlanarFace);
							}
						
						}				
					}
				}
			}
			
			TaskDialog.Show("abc", "number of face " + myListFace.Count);
			
			// Get all faces of Solids
			
//			GeometryElement geometryElement = selectedBeam.get_Geometry(new Options());
			

			
			//TaskDialog.Show("abc", "number geoObject" + geometryElement.Count());
			

			
			
			View view = doc.ActiveView;
			
			
			
			XYZ myRightDirect = view.RightDirection;
			XYZ myLeftDirect = view.RightDirection*-1;
			
			PlanarFace leftFace = null;
			PlanarFace rightFace = null;
			
			foreach (PlanarFace myFace in myListFace) {
				if (myFace.ComputeNormal(new UV()).IsAlmostEqualTo(view.UpDirection))
				{
					leftFace = myFace;
				}
				if (myFace.ComputeNormal(new UV()).IsAlmostEqualTo(-1*view.UpDirection))
				{
					rightFace = myFace;
				}
			}
			
			
			//PlanarFace leftFace = myListFace.FirstOrDefault(x => x.ComputeNormal(UV.Zero).
			                                       //IsAlmostEqualTo(-1*view.RightDirection));

//			PlanarFace rightFace = myListFace.FirstOrDefault(x => x.ComputeNormal(UV.Zero).
//			                                       IsAlmostEqualTo(view.RightDirection));			                                       
			                                       
			if(leftFace == null || rightFace == null)
			{
				TaskDialog.Show("abc", "Can't create dimension");
				return;
			}
			
			double shift  = UnitUtils.ConvertToInternalUnits(500, DisplayUnitType.DUT_MILLIMETERS);
			
			FamilyInstance selectedBeam_2 = selectedBeam as FamilyInstance;
			
			XYZ dimensionOrigin = view.Origin;
			
			XYZ p1 = dimensionOrigin;
			XYZ plusDim = new XYZ(1, 0, 0);
			
			XYZ p2 = p1 + plusDim;
			
			// Pick a dimension to get Line of Dimension
			
			Dimension myPickDim = doc.GetElement(uiDoc.Selection.PickObject(ObjectType.Element,"Pick a Dimension...")) as Dimension;
			
			Line myDimLine = myPickDim.Curve as Line;
			
			Line dimensionLine = Line.CreateBound(p1, p2);
			
			ReferenceArray dimensionReferences = new ReferenceArray();
			
			dimensionReferences.Append(leftFace.Reference);
			dimensionReferences.Append(rightFace.Reference);
			
			using (Transaction trans = new Transaction(doc, "Create dimension...")) {
				trans.Start();
				
				Dimension myDim = doc.Create.NewDimension(view, myDimLine, dimensionReferences);
				
				trans.Commit();
			}
			
			
		}


		public void autoDimSection_3()
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

			
				foreach (GeometryInstance myGeoInstance in geometryElement) {
					
					GeometryElement myGeoElement_2 = myGeoInstance.GetInstanceGeometry();
					foreach (GeometryObject myGeo_2 in myGeoElement_2) 
					{
						if(myGeo_2 is Solid)
						{
							Solid solid = myGeo_2 as Solid;
							
							XYZ myNormFace = new XYZ();
							foreach (Face myFace in solid.Faces ) 
							{
								myNormFace = myFace.ComputeNormal(new UV());
								
								if (Math.Abs(Math.Round(myNormFace.Z, 1)) == 1.0)
								{
									myListFaces.Add(myFace);
									ra.Append(myFace.Reference);
									TaskDialog.Show("abc", myFace.Area.ToString());
								}						
											
							}
					}
				}
				

				TaskDialog.Show("abc", "number of faces: " + myListFaces.Count);
 				// Pick a dimension to get line from it
		
				Reference myRefDim = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a dimension....");
				Dimension myDimExits = doc.GetElement(myRefDim) as Dimension;
				
				Line lineDim = myDimExits.Curve as Line;
		
				Line line2 = lineDim.CreateOffset(1, new XYZ(1,1,1)) as Line;
					
				Dimension myDim = doc.Create.NewDimension(doc.ActiveView, line2, ra);

				trans.Commit();
			
			}
		}
		}
		
		
		private IEnumerable<Solid> getSolids(Element element)
		{
			var geometry = element.get_Geometry(new Options {ComputeReferences = true});
			
			return getSolids(geometry).Where(XYZ => XYZ.Volume > 0);
		}
		
		
		
		private static IEnumerable<Solid> getSolids(IEnumerable<GeometryObject> geometryElement)
		{
			foreach (var geometry in geometryElement) 
			{
				var solid = geometry as Solid;
				if (solid != null) {
					yield return solid;
					
					var instance = geometry as GeometryInstance;
					if (instance != null)
					{
						foreach (var instanceSolid in getSolids(instance.GetSymbolGeometry())) 
						{
							yield return instanceSolid;
							
						}
					}
					
					var element = geometry as GeometryElement;
					if(element != null)
					{
						foreach (var elementSolid in getSolids(element)) 
						{
							yield return elementSolid;
						}
					}
				}
			}
		
		
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