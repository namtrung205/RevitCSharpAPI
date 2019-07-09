/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 3/17/2019
 * Time: 1:24 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

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
			Material myMaterial = doc.GetElement(myPickedFace.MaterialElementId) as Material;
			TaskDialog.Show("get Material", "The Face has: " + myMaterial.Name);
			
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

		
		
		public void getInfo2()
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
			
			// Lay danh sach edge cua face
			
			EdgeArrayArray myEdgeArs = myPickedFace.EdgeLoops;
			
			// Lay danh sach Array

			
			foreach (EdgeArray EA in myEdgeArs) 
			{
				foreach (Edge myEdge in EA) 
				{
					// Get one test point
                        XYZ testPoint = myEdge.Evaluate(0.5);
                        string edgeInfo = string.Format("Point on edge: ({0},{1},{2})", testPoint.X, testPoint.Y, testPoint.Z);
                        TaskDialog.Show("Revit",edgeInfo);
                        
                        break;
				}
				break;
				
			}
			
			
			TaskDialog.Show("abc","the face was picked has area: "+ myPickedFace.Area.ToString());
			
		}

	
		
		public void getInfo3() // Get Edge of face
		{
			// set active document
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			// prompt select face 
			
			Reference myRef = uiDoc.Selection.PickObject(ObjectType.Face);
			
			// Get element by pickface
			Element e = doc.GetElement(myRef) as Element;
			
			Level myLevel = doc.GetElement(e.LevelId) as Level;
			
			
			//Get GeoObject from element;
			GeometryObject myGeoObj =  e.GetGeometryObjectFromReference(myRef) as Face;
			
			//Get face from element Object:
			Face myPickedFace = myGeoObj as Face;
			
			XYZ myNorVecFace = myPickedFace.ComputeNormal(new UV(0,0));
			
			TaskDialog.Show("curve loop", "abc: " + myPickedFace.GetEdgesAsCurveLoops().Count());
			
			List<CurveLoop> myListCurvefromFace = myPickedFace.GetEdgesAsCurveLoops() as List<CurveLoop>;
			
			using (Transaction trans = new Transaction(doc, "abc")) 
			{
				trans.Start();
				
				CurveArray myBoundaFloor = new CurveArray();
				
				
				
				foreach (CurveLoop myCurLoop in myListCurvefromFace) 
				{
					CurveLoop curOffset =  CurveLoop.CreateViaOffset(myCurLoop, 1, myNorVecFace);
					TaskDialog.Show("abc", "xyz: " +curOffset.GetPlane().Normal.ToString());

					foreach (Curve myCur in curOffset) 
					{
//						TaskDialog.Show("abc", "anc has: " + myCur.Length); 
						myBoundaFloor.Append(myCur);
					}
				}
				
				FloorType floorType
			      = new FilteredElementCollector( doc )
					.OfClass( typeof( FloorType ) ).OfCategory(BuiltInCategory.OST_StructuralFoundation)
					.First<Element>() as FloorType;
							
				doc.Create.NewFoundationSlab(myBoundaFloor, floorType, myLevel, true, new XYZ(0,0,1));
				
					trans.Commit();				
			}

		}

		
		public void getInfo4() // Get Edge of face
		{
			// set active document
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

			
			//Get location curve of beam
		    LocationCurve lc = myBeam.Location as LocationCurve;
		    Line line = lc.Curve as Line;
			
		    //Get vector of location cuver beam
            XYZ p = line.GetEndPoint(0);
		    XYZ q = line.GetEndPoint(1);
		    //XYZ v = q - p; // Vector equation of line

			
			
			
			// prompt select face 
			Reference myRef = uiDoc.Selection.PickObject(ObjectType.Face);
			
			// Get element by pickface
			Element e = doc.GetElement(myRef) as Element;
			
			Level myLevel = doc.GetElement(e.LevelId) as Level;
			
			
			//Get GeoObject from element;
			GeometryObject myGeoObj =  e.GetGeometryObjectFromReference(myRef) as Face;
			
			//Get face from element Object:
			Face myPickedFace = myGeoObj as Face;
			
			Plane myPlaneFace = myPickedFace.GetSurface() as Plane;
			
			
			XYZ v = p - myPlaneFace.Origin;
			
			double myDis = myPlaneFace.Normal.DotProduct(v);
			
			TaskDialog.Show("Dis" , "from first P: " + myDis);
			
			
			
			
			
		}


		
		public void  createLiningConcrete()
		{
			// set active document
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			//Show Form

			SelectFamilySlab mySelFamilyForm = new SelectFamilySlab();
			
			mySelFamilyForm.thickTb.Text = "100";
			
			
			FilteredElementCollector fec = new FilteredElementCollector(doc)
				.OfClass(typeof(FloorType))
				.OfCategory(BuiltInCategory.OST_StructuralFoundation);
			
			
			IEnumerable<FloorType> iterFloorTypes =  fec.Cast<FloorType>();
			
			// Lay danh sach vat lieu
			foreach (FloorType myItemType in iterFloorTypes) 
			{
				mySelFamilyForm.FamilysCb.Items.Add(myItemType.Name.ToString());
				
				//mySelMatForm.listMatCb.Items.Add(myItemType.Name.ToString());
			}
			
			
			
			double offsetVal = Convert.ToDouble(valueOfSetting(@"C:\Revit Setting\RevitSetting.set","LiningConcreteOffset")) / 304.8;
			
			int indexSel = mySelFamilyForm.FamilysCb.Items.IndexOf(valueOfSetting(@"C:\Revit Setting\RevitSetting.set","LiningConcreteFamily"));
			
			mySelFamilyForm.FamilysCb.SelectedIndex = indexSel;
			mySelFamilyForm.ShowDialog();
			
			string nameSelectedFamily = mySelFamilyForm.FamilysCb.SelectedItem.ToString();
			
			
			
			//filter element
			List<int> myListCatoId = new List<int>();
			myListCatoId.Add((int)BuiltInCategory.OST_StructuralFoundation);
			myListCatoId.Add((int)BuiltInCategory.OST_StructuralFraming);
			
			List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element) as List<Reference>;

			foreach (Reference myRef in myListRef) 
			{
				//Get ElementId from ref
				ElementId myFoundationId = doc.GetElement(myRef).Id;
				
				createLiningConcreteFoundation(myFoundationId, nameSelectedFamily, offsetVal);
				
			}
			
			mySelFamilyForm.Close();
		
		}


		
		private void  createLiningConcreteFoundation(ElementId myFoundtionId, string nameFamily, double offsetValue) 
		
		{
			// set active document
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			

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
					XYZ myNorVecFace = myPickedFace.ComputeNormal(new UV(0,0));
					List<CurveLoop> myListCurvefromFace = myPickedFace.GetEdgesAsCurveLoops() as List<CurveLoop>;
					
	
						CurveArray myBoundaFloor = new CurveArray();
						
						foreach (CurveLoop myCurLoop in myListCurvefromFace) 
						{
							CurveLoop curOffset =  CurveLoop.CreateViaOffset(myCurLoop, offsetValue, myNorVecFace);
							//TaskDialog.Show("abc", "xyz: " +curOffset.GetPlane().Normal.ToString());
		
							foreach (Curve myCur in curOffset) 
							{
								myBoundaFloor.Append(myCur);
							}
						}
						
						FloorType floorType
					      = new FilteredElementCollector(doc)
							.OfClass(typeof(FloorType))
							.OfCategory(BuiltInCategory.OST_StructuralFoundation)
							.First<Element>(f => f.Name.Equals(nameFamily)) as FloorType;
									
						Floor myLining =  doc.Create.NewFoundationSlab(myBoundaFloor, floorType, myLevel, true, new XYZ(0,0,1));
						myListLining.Add(myLining);
					}
					trans.Commit();		
			}

				
			if(myListLining.Count() < 1) 
			{
				return;
			}
			else
			{
				foreach (Floor myLining in myListLining) 
				{
					switchJoinOrder(myLining);
				}
			
			}
				

	
		}
		
	

		
		public void  createLiningConcrete2()
		{
			// set active document
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			//Show Form

			SelectFamilySlab mySelFamilyForm = new SelectFamilySlab();
			
			//mySelFamilyForm.thickTb.Text = "100";
			
			
			FilteredElementCollector fec = new FilteredElementCollector(doc)
				.OfClass(typeof(FloorType))
				.OfCategory(BuiltInCategory.OST_Floors);
			
			
			IEnumerable<FloorType> iterFloorTypes =  fec.Cast<FloorType>();
			
			// Lay danh sach vat lieu
			foreach (FloorType myItemType in iterFloorTypes) 
			{
				mySelFamilyForm.FamilysCb.Items.Add(myItemType.Name.ToString());
				
				//mySelMatForm.listMatCb.Items.Add(myItemType.Name.ToString());
			}
			
            double offsetVal = 100;
            try
            {
                offsetVal = Convert.ToDouble(valueOfSetting(@"C:\Revit Setting\RevitSetting.set", "LiningConcreteOffset"));
            }

            catch
            {
                

            }
            
            
            mySelFamilyForm.thickTb.Text = offsetVal.ToString();
			

			
			//double offsetVal = Convert.ToDouble(valueOfSetting(@"C:\Revit Setting\RevitSetting.set","LiningConcreteOffset")) / 304.8;
			
			int indexSel = mySelFamilyForm.FamilysCb.Items.IndexOf(valueOfSetting(@"C:\Revit Setting\RevitSetting.set","LiningConcreteFamily"));
			
			mySelFamilyForm.FamilysCb.SelectedIndex = indexSel;
			mySelFamilyForm.ShowDialog();
			
			string nameSelectedFamily = mySelFamilyForm.FamilysCb.SelectedItem.ToString();
            offsetVal = Convert.ToDouble(mySelFamilyForm.thickTb.Text);
			
			// Get the element selection of current document.
                Selection selection = uiDoc.Selection;
                ICollection<ElementId> selectedIds = uiDoc.Selection.GetElementIds();
			
                if (selectedIds.Count < 1) 
                {
    				//filter element
					List<int> myListCatoId = new List<int>();
					myListCatoId.Add((int)BuiltInCategory.OST_StructuralFoundation);
					myListCatoId.Add((int)BuiltInCategory.OST_StructuralFraming);
					
					List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, new FilterByIdCategory(myListCatoId), "Select Beam or Slab") as List<Reference>;

					if(myListRef.Count() < 1)
					{
						return;
					}	
					else
					{
						foreach (Reference myRef in myListRef) 
						{
							selectedIds.Add(doc.GetElement(myRef).Id);
						}
					}
                }

			bool isLiningBottomBeam = false;
			foreach (ElementId myFoundationId in selectedIds) 
			{
//				//Get ElementId from ref
//				ElementId myFoundationId = doc.GetElement(myRef).Id;
				Element myCurrentFoundationAs = doc.GetElement(myFoundationId);
				
				if(myCurrentFoundationAs.Category.Name == "Structural Framing")
				{
					isLiningBottomBeam = true;
				}
				
				else
				{
					isLiningBottomBeam = false;
				}
				
			 	List<List<Floor>> myListListFloor = createLiningConcreteAsFloor2(myFoundationId,
				                                                             nameSelectedFamily,
				                                                             offsetVal/304.8, isLiningBottomBeam);

				foreach (Floor myFloorLining in myListListFloor[0])
				{
					switchJoinOrder(myFloorLining);
				}

				foreach (Floor myFloorLiningCutting in myListListFloor[1])
				{
					joiningLining(myFloorLiningCutting);
				}
				
			}
			
			mySelFamilyForm.Close();
		
		}


		
		
		private void  createLiningConcreteAsFloor(ElementId myFoundtionId, string nameFamily, double offsetValue) 
		
		{
			// set active document
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			

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
					XYZ myNorVecFace = myPickedFace.ComputeNormal(new UV(0,0));
					List<CurveLoop> myListCurvefromFace = myPickedFace.GetEdgesAsCurveLoops() as List<CurveLoop>;
					
	
						CurveArray myBoundaFloor = new CurveArray();
						
						foreach (CurveLoop myCurLoop in myListCurvefromFace) 
						{
							CurveLoop curOffset =  CurveLoop.CreateViaOffset(myCurLoop, offsetValue, myNorVecFace);
							//TaskDialog.Show("abc", "xyz: " +curOffset.GetPlane().Normal.ToString());
		
							foreach (Curve myCur in curOffset) 
							{
								myBoundaFloor.Append(myCur);
							}
						}
						
						FloorType floorType
					      = new FilteredElementCollector(doc)
							.OfClass(typeof(FloorType))
							.OfCategory(BuiltInCategory.OST_Floors)
							.First<Element>(f => f.Name.Equals(nameFamily)) as FloorType;
									
						//Floor myLining =  doc.Create.NewFoundationSlab(myBoundaFloor, floorType, myLevel, true, new XYZ(0,0,1));
						
						//Floor myLining =  doc.Create.NewFloor(myBoundaFloor, floorType, myLevel, true, new XYZ(0,0,1));
						
						Floor myLining =  doc.Create.NewFloor(myBoundaFloor,floorType, myLevel,true, new XYZ(0,0,1));
						
						myListLining.Add(myLining);
					}
					trans.Commit();		
			}

			// Switch Joint
			if(myListLining.Count() < 1) 
			{
				return;
			}
			else
			{
				foreach (Floor myLining in myListLining) 
				{
					switchJoinOrder(myLining);
				}
			
			}
				

	
		}
		

		
		private void  createLiningConcreteAsFloor1(ElementId myFoundtionId, string nameFamily, double offsetValue, bool isCutting) 
		
		{
			// set active document
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			

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
			
			// List Floor cutting
			List<Floor> myListLiningCutting = new List<Floor>();
			
			
			using (Transaction trans = new Transaction(doc, "abc")) 
			{
				trans.Start();
				foreach (Face myPickedFace in myListBottomFace) 
				{
					//Get Nomarl vector
					XYZ myNorVecFace = myPickedFace.ComputeNormal(new UV(0,0));
					List<CurveLoop> myListCurvefromFace = myPickedFace.GetEdgesAsCurveLoops() as List<CurveLoop>;
					
	
						CurveArray myBoundaFloor = new CurveArray();
						
						foreach (CurveLoop myCurLoop in myListCurvefromFace) 
						{
							
							if(myFoundation.Category.Name != "Structural Framing")
							{
								// Offset For Slab
								CurveLoop curOffset =  CurveLoop.CreateViaOffset(myCurLoop, offsetValue, myNorVecFace);
								//TaskDialog.Show("abc", "xyz: " +curOffset.GetPlane().Normal.ToString());
							
								foreach (Curve myCur in curOffset) 
								{
									myBoundaFloor.Append(myCur);
								}
							}
							
							else
							{
								List<double> myOffsetDist = getOffsetDis(myCurLoop, offsetValue);
								

								CurveLoop curOffset =  CurveLoop.CreateViaOffset(myCurLoop, myOffsetDist, myNorVecFace);
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
						
						Floor myLining =  doc.Create.NewFloor(myBoundaFloor,floorType, myLevel,true, new XYZ(0,0,1));
						
						
						// Cutting if foundation is beam
						
						if(isCutting)
						{
							myListLiningCutting.Add(myLining);
						
						}
						
						myListLining.Add(myLining);
					}
					trans.Commit();		
			}

			// Switch Joint
			if(myListLining.Count() < 1) 
			{
				return;
			}
			else
			{
				foreach (Floor myLining in myListLining) 
				{
					switchJoinOrder(myLining);
				}
			
			}
			
			
			// Cutting
			if(myListLiningCutting.Count() < 1)
			{
				return;
				
			}
			else
			{
				foreach (Floor myLining in myListLiningCutting) 
				{
					// Cutting (joining)
				}
			
			}
				

	
		}
		
	
		
		private List<List<Floor>>  createLiningConcreteAsFloor2(ElementId myFoundtionId, string nameFamily, double offsetValue, bool isCutting)
		
		{
			// set active document
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			

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
			
			// List Floor cutting
			List<Floor> myListLiningCutting = new List<Floor>();
			
			
			using (Transaction trans = new Transaction(doc, "abc")) 
			{
				trans.Start();
				foreach (Face myPickedFace in myListBottomFace) 
				{
					//Get Nomarl vector
					XYZ myNorVecFace = myPickedFace.ComputeNormal(new UV(0,0));
					List<CurveLoop> myListCurvefromFace = myPickedFace.GetEdgesAsCurveLoops() as List<CurveLoop>;
					
	
						CurveArray myBoundaFloor = new CurveArray();
						
						foreach (CurveLoop myCurLoop in myListCurvefromFace) 
						{
							
							if(myFoundation.Category.Name != "Structural Framing")
							{
								// Offset For Slab
								CurveLoop curOffset =  CurveLoop.CreateViaOffset(myCurLoop, offsetValue, myNorVecFace);
								//TaskDialog.Show("abc", "xyz: " +curOffset.GetPlane().Normal.ToString());
							
								foreach (Curve myCur in curOffset) 
								{
									myBoundaFloor.Append(myCur);
								}
							}
							
							else
							{
								List<double> myOffsetDist = getOffsetDis(myCurLoop, offsetValue);
								

								CurveLoop curOffset =  CurveLoop.CreateViaOffset(myCurLoop, myOffsetDist, myNorVecFace);
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
						
						Floor myLining =  doc.Create.NewFloor(myBoundaFloor,floorType, myLevel,true, new XYZ(0,0,1));
						
						
						// Cutting if foundation is beam
						
						if(isCutting)
						{
							myListLiningCutting.Add(myLining);
						
						}
						
						myListLining.Add(myLining);
					}
					trans.Commit();		
			}

			// Switch Joint
			List<List<Floor>> myListListFloor = new List<List<Floor>>(){myListLining, myListLiningCutting};
			return myListListFloor;

		}
		


				
		public void  createFloorFromPolylines()
		{
			// set active document
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			List<XYZ> myListPoint = new List<XYZ>(){new XYZ(0,0,0),
				new XYZ(100, 0, 0),
				new XYZ(100, 100, 0),
				new XYZ(0, 100, 0)};

			
			
			
			
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
				if(currentCur.Length == myListLength[myListLength.Count() - 1] || 
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

		
		
		private void joiningLining( Floor myCuttingFloor)
		{
			
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;

			Element element = myCuttingFloor as Element;
			GeometryElement geomElement = element.get_Geometry(new Options());
			Solid solid = null;
			foreach (GeometryObject geomObj in geomElement)
			{
			    solid = geomObj as Solid;
			    if (solid != null) break;
			}
			
			FilteredElementCollector collector = new FilteredElementCollector(doc);
			collector.WherePasses(new ElementIntersectsSolidFilter(solid)); // Apply intersection filter to find matches
			
			//TaskDialog.Show("Revit", collector.Count() + " family instances intersect with the selected element (" + element.Category.Name + " id:" + element.Id.ToString() + ")");

			List<ElementId> myListInter = new List<ElementId>();
			foreach (Element myE in collector) {
				if(myE.Id != element.Id)
				{
					myListInter.Add(myE.Id);
				}
			}
			
			// Joint
			
			try {
				
				using (Transaction trans = new Transaction(doc, "Join Floor")) 
				{
					trans.Start();
					foreach (ElementId myEId in myListInter) 
					{
						if(!JoinGeometryUtils.AreElementsJoined(doc, doc.GetElement(myEId), element))
						{
							JoinGeometryUtils.JoinGeometry(doc, doc.GetElement(myEId), element);
							//JoinGeometryUtils.UnjoinGeometry(doc, doc.GetElement(myEId), element);
						}
					
					}
					trans.Commit();
				}
			} catch (Exception e) {
				
				TaskDialog.Show("Error", e.Message);
			}
			
			
			
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
			First<Material>( myMaterial => myMaterial.Name == "Default Mass Roof" );
		
		ElementId myMaterialEleId = materialElem.Id;
		
		using (Transaction trans = new Transaction(doc, "AABC" )) 
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
	


		public void paintWalls2()
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
    		
			
			IEnumerable<Material> test =  fec.Cast<Material>();
			
    		Material materialElem = fec.
    			Cast<Material>().
    			First<Material>( myMaterial => myMaterial.Name == "Default Mass Zone" );
			
			ElementId myMaterialEleId = materialElem.Id;
			
			
			
			foreach (Reference myRef in myListRef) 
			{
				Element myElem = doc.GetElement(myRef);
				Wall myWall = myElem as Wall;
				
				paintFaceOfWall2(myWall, myMaterialEleId);
			}

		}
	

		
		public void paintAuto()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			SelectMaterialForms mySelMatForm = new SelectMaterialForms();
			
			FilteredElementCollector fec = new FilteredElementCollector(doc).OfClass(typeof(Material));
			
			
    		
			
			IEnumerable<Material> iterMaterials =  fec.Cast<Material>();
			
			// Lay danh sach vat lieu
			foreach (Material myItemMat in iterMaterials) 
			{
				mySelMatForm.listMatCb.Items.Add(myItemMat.Name.ToString());
			}
			
//			mySelMatForm.listMatCb.SelectedIndex = 2;

			int indexSel = mySelMatForm.listMatCb.Items.IndexOf(valueOfSetting(@"C:\Revit Setting\RevitSetting.set","PaintMaterial"));
			
			mySelMatForm.listMatCb.SelectedIndex = indexSel;
			mySelMatForm.ShowDialog();
			
			string nameSelectedMaterial = mySelMatForm.listMatCb.SelectedItem.ToString();
			
			//filtered
			
			List<string> myFil = new List<string>(){"Wall", "Floor"};
			
			// Select multiple Wall
			List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, 
			                 new FilterByNameElementType(myFil), "Select Walls, Floor") as List<Reference>;
			
			
			//Get Material Element
			
    		Material materialElem = fec.
    			Cast<Material>().
    			First<Material>(myMaterial => myMaterial.Name == nameSelectedMaterial);
			
			ElementId myMaterialEleId = materialElem.Id;
			
			
			
			foreach (Reference myRef in myListRef) 
			{
	
				Element myElem = doc.GetElement(myRef);
				if(myElem.GetType().Name == "Wall"){
					
					Wall myWall = myElem as Wall;
					paintFaceOfWall2(myWall, myMaterialEleId);
				}
				else if(myElem.GetType().Name == "Floor")
				{
					if(myElem.Category.Name == "Structural Foundations")
					{
						//TaskDialog.Show("abc", "Paint Foundation");
						Floor mySlab = myElem as Floor;
						paintFaceOfFoundation(mySlab, myMaterialEleId);
					}
					else 
					{
						Floor myFloor = myElem as Floor;
						paintFaceOfFloor(myFloor, myMaterialEleId);
//						TaskDialog.Show("abc", "Paint Floor");
					}	
				}
			}

		}


		
		public void paintAuto2()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			//Pick to get material from face(get MaterialId)			
			Reference myRef1 = uiDoc.Selection.PickObject(ObjectType.Face,"Select Face to get material...");
			
			// Get element by pickface
			Element e = doc.GetElement(myRef1) as Element;
			
			//Get GeoObject from element;
			GeometryObject myGeoObj =  e.GetGeometryObjectFromReference(myRef1) as Face;
			
			//Get face from element Object:
			Face myPickedFace = myGeoObj as Face;
			ElementId myMaterialEleId  = myPickedFace.MaterialElementId;
			
			//filtered
			List<string> myFil = new List<string>(){"Wall", "Floor"};
			
			// Select multiple Wall
			List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, 
			                 new FilterByNameElementType(myFil), "Select Walls, Floor...") as List<Reference>;

			
			
			foreach (Reference myRef in myListRef) 
			{
	
				Element myElem = doc.GetElement(myRef);
				if(myElem.GetType().Name == "Wall"){
					
					Wall myWall = myElem as Wall;
					paintFaceOfWall2(myWall, myMaterialEleId);
				}
				else if(myElem.GetType().Name == "Floor")
				{
					if(myElem.Category.Name == "Structural Foundations")
					{
						//TaskDialog.Show("abc", "Paint Foundation");
						Floor mySlab = myElem as Floor;
						paintFaceOfFoundation(mySlab, myMaterialEleId);
					}
					else 
					{
						Floor myFloor = myElem as Floor;
						paintFaceOfFloor(myFloor, myMaterialEleId);
//						TaskDialog.Show("abc", "Paint Floor");
					}	
				}
			}

		}



		private string valueOfSetting(string pathSettingFile, string settingName)
		
		{
		
			// Open file setting
			try 
			{
				if(File.Exists(pathSettingFile))
				{
					//Read all Line in file
					string[] myFullSetting = File.ReadAllLines(pathSettingFile);
					//Create a Dictionay wiht key and 
					string[] mySettingList;
					foreach (string pairSetting in myFullSetting) 
					{
						// if  satisfy conditions, add to Dic{List[0]: List[1],...}
	
						if(pairSetting.Count(f => f == '|') == 1)
						{
							// Split line to list
							mySettingList = pairSetting.Split('|');
							// Add to Dic
							if(mySettingList[0] == settingName)
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
			catch (Exception e) {

				TaskDialog.Show("Error", e.Message);
				return "";
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

		
		
		private void paintFaceOfFoundation(Floor myFoundation, ElementId myMatId)
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			GeometryElement geometryElement = myFoundation.get_Geometry(new Options());
			
			using (Transaction myTrans = new Transaction(doc, "filter face of foundation and Paint")) 
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
			            		doc.Paint(myFoundation.Id, myFace, myMatId);
							}
			            }
			        }
			    }
				myTrans.Commit();
			}
		
		}

		
		
		private void paintFaceOfFloor(Floor myFloor, ElementId myMatId)
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			GeometryElement geometryElement = myFloor.get_Geometry(new Options());
			
			using (Transaction myTrans = new Transaction(doc, "filter face of Floor and Paint")) 
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
			            	if (Math.Round(myNormVec.Z, 1) != 1.0)
							{
			            		doc.Paint(myFloor.Id, myFace, myMatId);
							}
			            }
			        }
			    }
				myTrans.Commit();
			}
		
		}


		
		private void paintFaceOfWall2(Wall myWall, ElementId myMatId)
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			GeometryElement geometryElement = myWall.get_Geometry(new Options());
			
			double widthWall = myWall.Width;
			// Lay cuver of location cuver
			
			LocationCurve myWallLc = myWall.Location as LocationCurve;
			
			Curve myWallCurve = myWallLc.Curve as Curve;
			
			double myAreaOfWall = myWallCurve.Length * widthWall;
			
			
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
			            	
			            	// If normal vector of face has Z value != -1 paint
			            	if (Math.Abs(Math.Round(myNormVec.Z, 1)) != 1.0)
							{
			            		doc.Paint(myWall.Id, myFace, myMatId);
							}
			            	
			            	// else 
			            	else
			            	{
				            	if(Math.Round(myNormVec.Z, 1) == -1.0 && !isBottomFace(myFace, myWall))
				            	{
				            		doc.Paint(myWall.Id, myFace, myMatId);
				            	}
			            	}

			            }
			        }
			    }
				myTrans.Commit();
			}
		
		}
		
			

		private bool isBottomFace(Face myFace, Wall myWall) 
		
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			BoundingBoxXYZ myBoundWall = myWall.get_BoundingBox(null);
			
			double zMinValue = myBoundWall.Min.Z;
			
			if (Math.Abs(zMinValue - getElevetionOfFace(myFace)) < 0.001)
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

		
		
		private string XYZtoString(XYZ myXYZ)
		{
			return string.Format("X: {0}, Y: {1}, Z: {2}", Math.Round(myXYZ.X,2), Math.Round(myXYZ.Y, 2), Math.Round(myXYZ.Z,2));
		
		}
	
	
		public void pickToSwitchJoint() 
		
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			//Pick object
			// prompt select face 
		
			List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element) as List<Reference>;
			
			foreach (Reference myRef in myListRef) 
			{
				Element myE = doc.GetElement(myRef);
				switchJoinOrder(myE);
			}
		}
		
		

		private void switchJoinOrder(Element myLining)
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			
			ICollection<ElementId> myListElemIdsJoined = JoinGeometryUtils.GetJoinedElements(doc, myLining);

			using (Transaction trans = new Transaction(doc, "Switch Join")) 
			{			
				trans.Start();
				foreach (ElementId myElemId in myListElemIdsJoined) 
				{
					
					if(!JoinGeometryUtils.IsCuttingElementInJoin(doc, doc.GetElement(myElemId), myLining))
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