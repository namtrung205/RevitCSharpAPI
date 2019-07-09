/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 3/12/2019
 * Time: 5:51 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace WorkWithElevation
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("ACFC451F-103D-4583-9227-5FEB8046B3F3")]
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
		
		
		public void createElevationsByRooms()
		{
			// Set active document
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			List<string> myListNameFilter = new List<string>(){"Room"};
			// Pickpoint
			List<Reference> myListRefRoom = uiDoc.Selection.PickObjects(ObjectType.Element, new FilterByNameElementType(myListNameFilter),
			                                                   "Pick Rooms") as List<Reference>;
//				ElementId MyElementId = uiDoc.Selection.PickObject(ObjectType.Element,
//				                                                   .ElementId;
//			TaskDialog.Show("abc", myListRefRoom.Count().ToString());
			if (myListRefRoom.Count() < 1 || myListRefRoom == null) return;

			foreach (Reference myRefRoom in myListRefRoom) {
				Room myRoom = doc.GetElement(myRefRoom) as Room;
				createElevationByRoom2(myRoom);
		}
	}
		
		
		
		private void createElevationByRoom(Room myRoom)
		{
			// Set active document
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			// List View Section
			
//			List<ViewSection> myListViewSecton = null;
			
			using (Transaction trans = new Transaction(doc,"add elevation"))
			{
				trans.Start();

				// Show parameter of picked room
				
				BoundingBoxXYZ roomBounding = myRoom.get_BoundingBox(null);
				
				XYZ minPointBb = roomBounding.Min;
				
				
//				TaskDialog.Show("Min point", Math.Round(minPointBb.X,0).ToString() + "," + Math.Round(minPointBb.Y,0).ToString()+"," + minPointBb.Z.ToString());
//				TaskDialog.Show("Max point", Math.Round(minPointBb.X,0).ToString() + "," + Math.Round(minPointBb.Y,0).ToString()+"," + minPointBb.Z.ToString());
				
				// Get parameter of room
				double myLevel = myRoom.Level.Elevation;
				BoundingBoxXYZ myRoomBB = myRoom.get_BoundingBox(null);
				
				XYZ maxPointBB = myRoomBB.Max;
				XYZ minPointBB = myRoomBB.Min;
				
				
				
//				TaskDialog.Show("abc","Level: " + myLevel.ToString() + "Min: " + minPointBB.ToString());
				
				LocationPoint lcPointRoom = myRoom.Location as LocationPoint;
				
				XYZ originPoint = lcPointRoom.Point;
				
				XYZ OriginPoint2 = new XYZ((maxPointBB.X + minPointBb.X)/2, (maxPointBB.Y + minPointBb.Y)/2, originPoint.Z);
				
				// second parameter in contructor ElevationMarker
				ViewFamilyType vft = new FilteredElementCollector( doc )
					        .OfClass( typeof( ViewFamilyType ) )
					        .Cast<ViewFamilyType>()
					        .FirstOrDefault<ViewFamilyType>( x =>
					          ViewFamily.Elevation == x.ViewFamily );
				
				ElementId myEleId = vft.Id;
				
				
				// Tao 1 ElevationMaker
				ElevationMarker myELM = ElevationMarker.CreateElevationMarker(doc, myEleId, originPoint, 40);
				
				
				myRoomBB.Min = new XYZ(myRoomBB.Min.X, myRoomBB.Min.Y, myRoom.Level.Elevation);
				myRoomBB.Max = new XYZ(myRoomBB.Max.X, myRoomBB.Max.Y, myRoom.Level.Elevation + 1);
				
//				TaskDialog.Show("Abc", myRoom.Level.Elevation.ToString());
				
				for (int i = 0; i< 4; i++)
				
				{
					ViewSection elevationView = myELM.CreateElevation(doc, doc.ActiveView.Id, i);
					
					string roomsName = myRoom.Name.Substring(0, myRoom.Name.Length - 1 - myRoom.Number.ToString().Length);
					
					elevationView.Name = roomsName.ToUpper() + ", Section ".ToUpper() + i.ToString().ToUpper();
//					TaskDialog.Show("abc", myELM.Name.ToString());
					elevationView.CropBoxActive = true;
					
					// Set style
//					setStyleCropBoxFromView(elevationView);

					BoundingBoxXYZ myCrop = elevationView.get_BoundingBox(null);
					if (i == 0)
					{
						myCrop.Min = new XYZ(myRoomBB.Min.Y-1, myRoom.Level.Elevation - 0.5, myCrop.Min.Z);
						myCrop.Max = new XYZ(myRoomBB.Max.Y+1, myRoom.UpperLimit.Elevation + myRoom.LimitOffset + 0.5, myCrop.Max.Z);
						elevationView.CropBox = myCrop;
						
						
//						doc.ActiveView.SetElementOverrides(elevationView.Id,ogs);
						// Set visual style
						elevationView.DisplayStyle = DisplayStyle.HLR;
					}
					else if (i==1) {
						
						
						myCrop.Min = new XYZ(myRoomBB.Min.X-1, myRoom.Level.Elevation - 0.5, myCrop.Min.Z);
						myCrop.Max = new XYZ(myRoomBB.Max.X+1, myRoom.UpperLimit.Elevation + myRoom.LimitOffset + 0.5, myCrop.Max.Z);
						
						elevationView.CropBox = myCrop;
						
						// Set visual style						
						elevationView.DisplayStyle = DisplayStyle.HLR;
					}
					
					// mai xu li
					else if (i==2) {

						double XMin = -(Math.Abs(maxPointBB.Y - minPointBb.Y)/2) - 1;
						double XMax =  +(Math.Abs(maxPointBB.Y - minPointBb.Y)/2) + 1;

						myCrop.Min = new XYZ(XMin - OriginPoint2.Y, myRoom.Level.Elevation - 0.5, myCrop.Min.Z);
						myCrop.Max = new XYZ(myCrop.Min.X + myRoomBB.Max.Y - myRoomBB.Min.Y+2, myRoom.UpperLimit.Elevation + myRoom.LimitOffset + 0.5, myCrop.Max.Z);
						
						// Set visual style
						elevationView.DisplayStyle = DisplayStyle.HLR;
						
						
						elevationView.CropBox = myCrop;
					}
					
					else{
						myCrop.Min = new XYZ(myRoomBB.Min.X- 1- 2*(OriginPoint2.X), myRoom.Level.Elevation - 0.5, myCrop.Min.Z);
						myCrop.Max = new XYZ(myCrop.Min.X + myRoomBB.Max.X - myRoomBB.Min.X + 2, myRoom.UpperLimit.Elevation + myRoom.LimitOffset + 0.5, myCrop.Max.Z);
						elevationView.CropBox = myCrop;
						
						// Set visual style
						elevationView.DisplayStyle = DisplayStyle.HLR;
						
					}
				}
				trans.Commit();

		}

	}



		
		private void createElevationByRoom2(Room myRoom)
		{
			// Set active document
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			// List View Section
			
			List<ViewSection> myListViewSecton = new List<ViewSection>();
			
			ElevationMarker myELM = null;
			
			BoundingBoxXYZ myRoomBB = null;
			
			XYZ minPointBB = null;
			XYZ maxPointBB = null;
			
			XYZ minPointBb = null;
			
			XYZ OriginPoint2 = null;
//			XYZ OriginPoint = null;
			
			
			using (Transaction trans0 = new Transaction(doc, "Create a new Elevation Marker")) 
			{
							
			trans0.Start();
			// Show parameter of picked room
			
			BoundingBoxXYZ roomBounding = myRoom.get_BoundingBox(null);
			
			minPointBb = roomBounding.Min;
				
			// Get parameter of room
			double myLevel = myRoom.Level.Elevation;
			myRoomBB = myRoom.get_BoundingBox(null);
			
			maxPointBB = myRoomBB.Max;
			minPointBB = myRoomBB.Min;
			
			LocationPoint lcPointRoom = myRoom.Location as LocationPoint;
			
			XYZ originPoint = lcPointRoom.Point;
			
			OriginPoint2 = new XYZ((maxPointBB.X + minPointBb.X)/2, (maxPointBB.Y + minPointBb.Y)/2, originPoint.Z);
			
			// second parameter in contructor ElevationMarker
			ViewFamilyType vft = new FilteredElementCollector( doc )
				        .OfClass( typeof( ViewFamilyType ) )
				        .Cast<ViewFamilyType>()
				        .FirstOrDefault<ViewFamilyType>( x =>
				          ViewFamily.Elevation == x.ViewFamily );
			
			ElementId myEleId = vft.Id;
			
			
			
			// Tao 1 ElevationMaker
			myELM = ElevationMarker.CreateElevationMarker(doc, myEleId, originPoint, 40);
			


				myRoomBB.Min = new XYZ(myRoomBB.Min.X, myRoomBB.Min.Y, myRoom.Level.Elevation);
				myRoomBB.Max = new XYZ(myRoomBB.Max.X, myRoomBB.Max.Y, myRoom.Level.Elevation + 1);
				trans0.Commit();
			
			}
			
//			string roomsName = myRoom.Name.Substring(0, myRoom.Name.Length - 1 - myRoom.Number.ToString().Length);
			string roomsName = myRoom.Name.ToString();
			
			for (int i = 0; i< 4; i++)
			
			{

				if (i == 0)
				{
					using (Transaction trans1 = new Transaction(doc, "create sec 0")) 
					{
						trans1.Start();
						
						ViewSection elevationView = myELM.CreateElevation(doc, doc.ActiveView.Id, i);
						elevationView.Name = roomsName.ToUpper() + ", Section ".ToUpper() + i.ToString().ToUpper();
						elevationView.CropBoxActive = true;
						BoundingBoxXYZ myCrop = elevationView.get_BoundingBox(null);
								
						myCrop.Min = new XYZ(myRoomBB.Min.Y-1, myRoom.Level.Elevation - 1, myCrop.Min.Z);
						myCrop.Max = new XYZ(myRoomBB.Max.Y+1, myRoom.UpperLimit.Elevation + myRoom.LimitOffset + 1, myCrop.Max.Z);
						elevationView.CropBox = myCrop;

						// Set visual style
						elevationView.DisplayStyle = DisplayStyle.HLR;
						
						trans1.Commit();
						
						//add View to List View
						myListViewSecton.Add(elevationView);
					}
					
				}
				else if (i==1) {
					using (Transaction trans2 = new Transaction(doc, "create sec 1")) 
					{
						
						trans2.Start();
						ViewSection elevationView = myELM.CreateElevation(doc, doc.ActiveView.Id, i);
						elevationView.Name = roomsName.ToUpper() + ", Section ".ToUpper() + i.ToString().ToUpper();
						elevationView.CropBoxActive = true;
						BoundingBoxXYZ myCrop = elevationView.get_BoundingBox(null);
						
						myCrop.Min = new XYZ(myRoomBB.Min.X-1, myRoom.Level.Elevation - 1, myCrop.Min.Z);
						myCrop.Max = new XYZ(myRoomBB.Max.X+1, myRoom.UpperLimit.Elevation + myRoom.LimitOffset + 1, myCrop.Max.Z);
						
						elevationView.CropBox = myCrop;
						
						// Set visual style						
						elevationView.DisplayStyle = DisplayStyle.HLR;
						
						trans2.Commit();
													
						//add View to List View
						myListViewSecton.Add(elevationView);
					}
				}
				
				// mai xu li
				else if (i==2) {
					using (Transaction trans3 = new Transaction(doc, "create sec 2")) 
					{
						
						trans3.Start();
						ViewSection elevationView = myELM.CreateElevation(doc, doc.ActiveView.Id, i);
						elevationView.Name = roomsName.ToUpper() + ", Section ".ToUpper() + i.ToString().ToUpper();
						elevationView.CropBoxActive = true;
						BoundingBoxXYZ myCrop = elevationView.get_BoundingBox(null);
						

						double XMin = -(Math.Abs(maxPointBB.Y - minPointBb.Y)/2) - 1;
						double XMax =  +(Math.Abs(maxPointBB.Y - minPointBb.Y)/2) + 1;

						myCrop.Min = new XYZ(XMin - OriginPoint2.Y, myRoom.Level.Elevation - 1, myCrop.Min.Z);
						myCrop.Max = new XYZ(myCrop.Min.X + myRoomBB.Max.Y - myRoomBB.Min.Y+2, myRoom.UpperLimit.Elevation + myRoom.LimitOffset + 1, myCrop.Max.Z);
						
						// Set visual style
						elevationView.DisplayStyle = DisplayStyle.HLR;
						
						
						elevationView.CropBox = myCrop;
						
						trans3.Commit();
																				
						//add View to List View
						myListViewSecton.Add(elevationView);
						
					}
						
				}
				
				else{
						using (Transaction trans4 = new Transaction(doc, "create sec 3")) 
						{
							
							trans4.Start();
							ViewSection elevationView = myELM.CreateElevation(doc, doc.ActiveView.Id, i);
							elevationView.Name = roomsName.ToUpper() + ", Section ".ToUpper() + i.ToString().ToUpper();
							elevationView.CropBoxActive = true;
							BoundingBoxXYZ myCrop = elevationView.get_BoundingBox(null);
	
							myCrop.Min = new XYZ(myRoomBB.Min.X- 1- 2*(OriginPoint2.X), myRoom.Level.Elevation - 1, myCrop.Min.Z);
							myCrop.Max = new XYZ(myCrop.Min.X + myRoomBB.Max.X - myRoomBB.Min.X + 2, myRoom.UpperLimit.Elevation + myRoom.LimitOffset + 1, myCrop.Max.Z);
							elevationView.CropBox = myCrop;
							
							// Set visual style
							elevationView.DisplayStyle = DisplayStyle.HLR;
							
							trans4.Commit();
							
							//add View to List View
							myListViewSecton.Add(elevationView);	
						}
					}
				}
//			TaskDialog.Show("abc", "my List view create: " + myListViewSecton.Count().ToString());
			
			foreach (ViewSection mySec in myListViewSecton) 
			{
//				using (Transaction trans11 = new Transaction(doc, "Chang style for cropBox"))
//				{
//					trans11.Start();
					
					setStyleCropBoxFromView(mySec);
					
//					trans11.Commit();
						
//				}	
			}

			}

		
		private void setStyleCropBoxFromView(View myView)
		
		{
			//Set current Document
			UIDocument uiDoc = this.ActiveUIDocument;
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
    		ogsCropBox.SetProjectionLineColor(new Color(255,0,0));
    		//Line Weight
    		ogsCropBox.SetProjectionLineWeight(5);
    		//Pattern
    		
    		FilteredElementCollector fec = new FilteredElementCollector(doc).OfClass(typeof(LinePatternElement));
    		
    		LinePatternElement linePatternElem = fec.
    			Cast<LinePatternElement>().
    			First<LinePatternElement>( linePattern => linePattern.Name == "Default Form" );

    		 ElementId myMaterialPatId = linePatternElem.Id;
    		 
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
		
		
		
		public void test2()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			
			View myView2 = doc.ActiveView;
			setStyleCropBoxFromView(myView2);
			
		
		}
		
		///
		/// test View
		///
		public void cropBoxTest()
		
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			
			View myView = doc.ActiveView;
			ICollection<ElementId> myFilt = null;
			using (Transaction trans1 = new Transaction(doc, "Hide cropBox")) 
			{
				trans1.Start();
				myView.CropBoxVisible = false;
				trans1.Commit();
			}
			myFilt  = new FilteredElementCollector(doc, myView.Id).ToElementIds();

			using (Transaction trans2 = new Transaction(doc, "Hide cropBox")) 
			{
				trans2.Start();
				myView.CropBoxVisible = true;
				trans2.Commit();
			}
			
			ICollection<ElementId> myFilt2  = new FilteredElementCollector(doc, myView.Id).Excluding(myFilt).ToElementIds();
			
			// Get Id
			ElementId myCropBoxId = myFilt2.First() as ElementId;
			
			
			// Line Style - OverrideGraphicSettings
			OverrideGraphicSettings ogs = new OverrideGraphicSettings();
    		ogs.SetProjectionLineColor(new Color(0,255,0));
			
			// Get Element
			Element myCropBoxElement = doc.GetElement(myCropBoxId);
			
			using (Transaction trans3 = new Transaction(doc, "Set Style")) 
			{
			
				trans3.Start();
				doc.ActiveView.SetElementOverrides(myCropBoxId,ogs);
				trans3.Commit();
						
				TaskDialog.Show("Test2", "num e: " + myCropBoxId.ToString());
				
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
	

}