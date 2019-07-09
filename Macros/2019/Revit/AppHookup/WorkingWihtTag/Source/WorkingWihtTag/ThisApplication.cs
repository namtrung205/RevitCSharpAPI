/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 7/8/2019
 * Time: 9:20 PM
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

namespace WorkingWihtTag
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("E3AF7CB1-FA5F-4410-8587-C54ACA05880C")]
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
		
		
		//Tag by hand example
		public void tagRebar()
		{
		
			UIDocument uiDoc= this.ActiveUIDocument;
			
			Document doc = uiDoc.Document;
			
			ElementId myViewId =  new ElementId(649704);
			
			View myView = doc.GetElement(myViewId) as View;


			ElementId myRebarId =  new ElementId(813273);
			
			var rebarList = doc.GetElement(myRebarId);
			

			
			IList<ElementId> elementIds
			= new List<ElementId>();
			

			
			MultiReferenceAnnotationType type
				= doc.GetElement(new ElementId(413569)) as MultiReferenceAnnotationType;

			
			MultiReferenceAnnotationOptions options
			= new MultiReferenceAnnotationOptions( type );
			
			options.TagHeadPosition = new XYZ( 0, 100, 0 );
			options.DimensionLineOrigin = new XYZ( 5, 5, 1 );
			options.DimensionLineDirection = new XYZ( 0, 1, 0 );
			options.DimensionPlaneNormal = myView.ViewDirection;
			options.SetElementsToDimension( elementIds );
			
			using( Transaction tran = new Transaction( doc ) )
			{
			tran.Start( "Create_Rebar_Vertical" );
			
			var mra = MultiReferenceAnnotation.Create(
			  doc, myView.Id, options );
			
			tran.Commit();
			}
			
			
			
		}
	


				//Tag by hand example
		public void tagRebarGet()
		{
		
			UIDocument uiDoc= this.ActiveUIDocument;
			
			Document doc = uiDoc.Document;
			
			Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element, "Pick mutilDim...");
			
			Element myElem = doc.GetElement(myRef);
			
			MultiReferenceAnnotation myMRA = myElem as MultiReferenceAnnotation;
			
			
			
			
			
		}
	


		
		
		//Tag by hand example
		public void tagMultiRebar_GetSubelement()
		{
		
			UIDocument uiDoc= this.ActiveUIDocument;
			
			Document doc = uiDoc.Document;
			
			
			View myView = doc.ActiveView;
			

			
			Reference myRefMRA = uiDoc.Selection.PickObject(ObjectType.Element, "Pick MRA...");
			MultiReferenceAnnotation myMRA= doc.GetElement(myRefMRA) as MultiReferenceAnnotation;

			
			TaskDialog.Show("abc", "TagId: " + myMRA.TagId.ToString());
			
			IndependentTag myTag_2 = doc.GetElement(myMRA.TagId) as IndependentTag;
			
			
//			Reference myRefTag = uiDoc.Selection.PickObject(ObjectType.Element, "Pick MRA...");
//			IndependentTag myTag= doc.GetElement(myRefTag) as IndependentTag;
			setCurrentViewAsWorkPlan();
			XYZ pointPicked = uiDoc.Selection.PickPoint("Pick a Point...");
			
							
			using( Transaction tran = new Transaction( doc ) )
			{
			tran.Start( "Nove_tag" );
			

			
			
			
//			ElementTransformUtils.MoveElement(doc, myTag.Id, new XYZ(1,1,0));
			myTag_2.TagHeadPosition = new XYZ(pointPicked.X, pointPicked.Y, myTag_2.TagHeadPosition.Z);
			
			
			tran.Commit();
			}
			
		}
	

		
		
		
		//Tag by hand example
		public void tagMultiRebar()
		{
		
			UIDocument uiDoc= this.ActiveUIDocument;
			
			Document doc = uiDoc.Document;
			
			
			View myView = doc.ActiveView;
			

			ElementId myRebarId =  new ElementId(813273);
			
			
			var rebarSet = doc.GetElement(myRebarId);
			
			//Get bouding box of rebarSet
			BoundingBoxXYZ myRebar_BB = rebarSet.get_BoundingBox(myView);
			
			XYZ minPointRebar = myRebar_BB.Min;
			XYZ maxPointRebar = myRebar_BB.Max;
			XYZ centerPointRebar = (minPointRebar + maxPointRebar)/2;
			
			
			IList<ElementId> elementIds
			= new List<ElementId>();
			
			elementIds.Add(rebarSet.Id);
			
			MultiReferenceAnnotationType type
				= doc.GetElement(new ElementId(413569)) as MultiReferenceAnnotationType;

			
			MultiReferenceAnnotationOptions options
			= new MultiReferenceAnnotationOptions( type );
			
			
			setCurrentViewAsWorkPlan();
			
			//Pick beam to get min max bound
			
			Reference myRefBeam = uiDoc.Selection.PickObject(ObjectType.Element, "Pick host beam...");
			Element myBeam = doc.GetElement(myRefBeam);
			BoundingBoxXYZ myBeam_BB = myBeam.get_BoundingBox(myView);
			
			XYZ minPointBeam = myBeam_BB.Min;
			XYZ maxPointBeam = myBeam_BB.Max;
			XYZ centerPointBeam = (minPointBeam + maxPointBeam)/2;
			
			double deltaZ = 0.1;
			int factorDelta = 1;
			XYZ myPickPoint = new XYZ();
			
			if(centerPointRebar.Z<centerPointBeam.Z)
			{
				myPickPoint = new XYZ(minPointBeam.X, minPointBeam.Y, minPointBeam.Z + (-1*deltaZ*factorDelta));
			}
			else
			{
				myPickPoint = new XYZ(maxPointBeam.X, maxPointBeam.Y, maxPointBeam.Z + (deltaZ*factorDelta));
			}
			

			
			TaskDialog.Show("abc", "point picked: "+ myPickPoint.X + "; " + myPickPoint.Y + "; " +myPickPoint.Z);
			
//			myPickPoint = new XYZ(-10, 57, -7.54);
			options.TagHeadPosition = new XYZ( myPickPoint.X+5, myPickPoint.Y-1, myPickPoint.Z );
			options.DimensionLineOrigin = new XYZ( myPickPoint.X, myPickPoint.Y, myPickPoint.Z );
			options.DimensionLineDirection = new XYZ( 0, 1, 0 );
			options.DimensionPlaneNormal = myView.ViewDirection;
			options.SetElementsToDimension( elementIds );
			
			
			using( Transaction tran = new Transaction( doc ) )
			{
			tran.Start( "Create_Rebar_Vertical" );
			
			MultiReferenceAnnotation myMRA = MultiReferenceAnnotation.Create(doc, myView.Id, options);
			
			
			tran.Commit();
			}
			
			
			
		}
	


				
		//Tag by hand example
		public void tagMultiRebar_PickBeam()
		{
		
			UIDocument uiDoc= this.ActiveUIDocument;
			
			Document doc = uiDoc.Document;
			
			
			View myView = doc.ActiveView;
			//Pick beam to get min max bound
			
			Reference myRefBeam = uiDoc.Selection.PickObject(ObjectType.Element, "Pick host beam...");
			Element myBeam = doc.GetElement(myRefBeam);
			
						

			BoundingBoxXYZ myBeam_BB = myBeam.get_BoundingBox(myView);
			
			XYZ minPointBeam = myBeam_BB.Min;
			XYZ maxPointBeam = myBeam_BB.Max;
			XYZ centerPointBeam = (minPointBeam + maxPointBeam)/2;
			
			
			
			//Get all Rebar Set In Section of Beam
			
			List<List<Rebar>> myRebarListList = getListRebarSorted_2(myView, myBeam);
			
			
			int factorDelta = 5;
			foreach (Rebar myRebarSetBottom in myRebarListList[0]) 
			{
				
//				//Get bouding box of rebarSet
//				BoundingBoxXYZ myRebar_BB = myRebarSetBottom.get_BoundingBox(myView);
//				
//				XYZ minPointRebar = myRebar_BB.Min;
//				XYZ maxPointRebar = myRebar_BB.Max;
//				XYZ centerPointRebar = (minPointRebar + maxPointRebar)/2;
				
				IList<ElementId> elementIds
				= new List<ElementId>();
				
				elementIds.Add(myRebarSetBottom.Id);
				
				MultiReferenceAnnotationType type
					= doc.GetElement(new ElementId(413569)) as MultiReferenceAnnotationType;
	
				
				MultiReferenceAnnotationOptions options
				= new MultiReferenceAnnotationOptions( type );
					
				setCurrentViewAsWorkPlan();
	
				
				double deltaZ = 0.1;

				XYZ myPickPoint = new XYZ();
				myPickPoint = new XYZ(minPointBeam.X, minPointBeam.Y, minPointBeam.Z + (-1*deltaZ*factorDelta));
//				if(centerPointRebar.Z<centerPointBeam.Z)
//				{
//					myPickPoint = new XYZ(minPointBeam.X, minPointBeam.Y, minPointBeam.Z + (-1*deltaZ*factorDelta));
//				}
//				else
//				{
//					myPickPoint = new XYZ(maxPointBeam.X, maxPointBeam.Y, maxPointBeam.Z + (deltaZ*factorDelta));
//				}
				
	
				
				//TaskDialog.Show("abc", "point picked: "+ myPickPoint.X + "; " + myPickPoint.Y + "; " +myPickPoint.Z);
				
	//			myPickPoint = new XYZ(-10, 57, -7.54);
				options.TagHeadPosition = new XYZ( myPickPoint.X-2, myPickPoint.Y-2, myPickPoint.Z );
				options.DimensionLineOrigin = new XYZ( myPickPoint.X, myPickPoint.Y, myPickPoint.Z );
				options.DimensionLineDirection =  myView.RightDirection;
				options.DimensionPlaneNormal = myView.ViewDirection;
				options.SetElementsToDimension( elementIds );
				
				
				using( Transaction tran = new Transaction( doc ) )
				{
				tran.Start( "Create_Rebar_Vertical" );
				
				MultiReferenceAnnotation myMRA = MultiReferenceAnnotation.Create(doc, myView.Id, options);
				
				
				tran.Commit();
				}
				
				factorDelta = factorDelta+5;
			
			}
			
			
			
			factorDelta = 5;
			foreach (Rebar myRebarSetBottom in myRebarListList[1]) 
			{
				
//				//Get bouding box of rebarSet
//				BoundingBoxXYZ myRebar_BB = myRebarSetBottom.get_BoundingBox(myView);
//				
//				XYZ minPointRebar = myRebar_BB.Min;
//				XYZ maxPointRebar = myRebar_BB.Max;
//				XYZ centerPointRebar = (minPointRebar + maxPointRebar)/2;
				
				IList<ElementId> elementIds
				= new List<ElementId>();
				
				elementIds.Add(myRebarSetBottom.Id);
				
				MultiReferenceAnnotationType type
					= doc.GetElement(new ElementId(413569)) as MultiReferenceAnnotationType;
	
				
				MultiReferenceAnnotationOptions options
				= new MultiReferenceAnnotationOptions( type );
					
				setCurrentViewAsWorkPlan();
	
				
				double deltaZ = 0.1;

				XYZ myPickPoint = new XYZ();
				myPickPoint = new XYZ(minPointBeam.X, minPointBeam.Y, maxPointBeam.Z + (1*deltaZ*factorDelta));
//				if(centerPointRebar.Z<centerPointBeam.Z)
//				{
//					myPickPoint = new XYZ(minPointBeam.X, minPointBeam.Y, minPointBeam.Z + (-1*deltaZ*factorDelta));
//				}
//				else
//				{
//					myPickPoint = new XYZ(maxPointBeam.X, maxPointBeam.Y, maxPointBeam.Z + (deltaZ*factorDelta));
//				}
				
	
				
				//TaskDialog.Show("abc", "point picked: "+ myPickPoint.X + "; " + myPickPoint.Y + "; " +myPickPoint.Z);
				
	//			myPickPoint = new XYZ(-10, 57, -7.54);
				options.TagHeadPosition = new XYZ( myPickPoint.X-2, myPickPoint.Y-2, myPickPoint.Z );
				options.DimensionLineOrigin = new XYZ( myPickPoint.X, myPickPoint.Y, myPickPoint.Z );
				options.DimensionLineDirection = myView.RightDirection;
				options.DimensionPlaneNormal = myView.ViewDirection;
				options.SetElementsToDimension( elementIds );
				
				
				using( Transaction tran = new Transaction( doc ) )
				{
				tran.Start( "Create_Rebar_Vertical" );
				
				MultiReferenceAnnotation myMRA = MultiReferenceAnnotation.Create(doc, myView.Id, options);
				
				
				tran.Commit();
				}
				
				factorDelta = factorDelta+5;
			
			}		
			
			
			
		}
	

				
		//Get all rebar in section
		//Trả về 1 danh sách gồm 2 danh sách, danh sách đầu tiên chứa Id của cac rebar section trên dầm (thép chủ trên),
		// danh sách thứ 2 trả về Id của các rebar phía dưới dầm(thép chủ dưới);
		
		public List<List<Rebar>> getListRebarSorted_2(View myView, Element myBeam)
		{
			UIDocument uiDoc= this.ActiveUIDocument;
			
			Document doc = uiDoc.Document;
			
			
			//Boundary of beam
			BoundingBoxXYZ myBeam_BB = myBeam.get_BoundingBox(myView);
			
			XYZ minPointBeam = myBeam_BB.Min;
			XYZ maxPointBeam = myBeam_BB.Max;
			XYZ centerPointBeam = (minPointBeam + maxPointBeam)/2;
			
			//Retrive all rebar of beam
				
            RebarHostData myRbHostData = RebarHostData.GetRebarHostData(myBeam);

            List<Rebar> myListRebar = myRbHostData.GetRebarsInHost() as List<Rebar>;
			
            //Inter every rebar in ListRebar make section rebar in view
            
            List<Rebar> myListRebarInSection = new List<Rebar>();
            
            foreach (Rebar myRebarOfBeam in myListRebar) 
            {
            	if(myRebarOfBeam.IsRebarInSection(myView) && myRebarOfBeam.NumberOfBarPositions > 1)
            	{
            		myListRebarInSection.Add(myRebarOfBeam);
            	}
            }
            
                        
            List<Rebar> listBottomRebar = new List<Rebar>();
            List<Rebar> listTopRebar = new List<Rebar>();
            
            if(myListRebarInSection.Count <1)
            {
            	TaskDialog.Show("abc", "Has no rebarinsection in view belong to Host picked");
            	return null;
            }

            else 
            {
	            //Trong mỗi rebar in section, so sánh vị trí tương đối của rebar với trục giữa dầm(Z)
	            foreach (Rebar rebarSet in myListRebarInSection) 
	            {
	            	// Lấy bounding box của mỗi rebar
					//Get bouding box of rebarSet
					BoundingBoxXYZ myRebar_BB = rebarSet.get_BoundingBox(myView);
					
					XYZ minPointRebar = myRebar_BB.Min;
					XYZ maxPointRebar = myRebar_BB.Max;
					XYZ centerPointRebar = (minPointRebar + maxPointRebar)/2;
					
					if(centerPointRebar.Z <= centerPointBeam.Z)
					{
						//myPickPoint = new XYZ(minPointBeam.X, minPointBeam.Y, minPointBeam.Z + (-1*deltaZ*factorDelta));
						listBottomRebar.Add(rebarSet);
						
					}
					else
					{
						//myPickPoint = new XYZ(maxPointBeam.X, maxPointBeam.Y, maxPointBeam.Z + (deltaZ*factorDelta));
						listTopRebar.Add(rebarSet);
					}
		            	
	            }
            }
            List<List<Rebar>> rebarListList = new List<List<Rebar>>(){listBottomRebar, listTopRebar};
            TaskDialog.Show("abc", "has: " + rebarListList[0].Count() + "; " + rebarListList[1].Count);
            
            return rebarListList;
		
		}
		
	
		
		//Get all rebar in section
		//Trả về 1 danh sách gồm 2 danh sách, danh sách đầu tiên chứa Id của cac rebar section trên dầm (thép chủ trên),
		// danh sách thứ 2 trả về Id của các rebar phía dưới dầm(thép chủ dưới);
		
		public List<List<Rebar>> myListRebarIdSorted()
		{
			UIDocument uiDoc= this.ActiveUIDocument;
			
			Document doc = uiDoc.Document;
			
			View myView = doc.ActiveView;
			
			
			//Pick beam to get min max bound
			
			Reference myRefBeam = uiDoc.Selection.PickObject(ObjectType.Element, "Pick host beam...");
			Element myBeam = doc.GetElement(myRefBeam);
			
			//Boundary of beam
			BoundingBoxXYZ myBeam_BB = myBeam.get_BoundingBox(myView);
			
			XYZ minPointBeam = myBeam_BB.Min;
			XYZ maxPointBeam = myBeam_BB.Max;
			XYZ centerPointBeam = (minPointBeam + maxPointBeam)/2;
			
			//Retrive all rebar of beam
				
            RebarHostData myRbHostData = RebarHostData.GetRebarHostData(myBeam);

            List<Rebar> myListRebar = myRbHostData.GetRebarsInHost() as List<Rebar>;
			
            //Inter every rebar in ListRebar make section rebar in view
            
            List<Rebar> myListRebarInSection = new List<Rebar>();
            
            foreach (Rebar myRebarOfBeam in myListRebar) 
            {
            	if(myRebarOfBeam.IsRebarInSection(myView) && myRebarOfBeam.NumberOfBarPositions > 1)
            	{
            		myListRebarInSection.Add(myRebarOfBeam);
            	}
            }
            
                        
            List<Rebar> listBottomRebar = new List<Rebar>();
            List<Rebar> listTopRebar = new List<Rebar>();
            
            if(myListRebarInSection.Count <1)
            {
            	TaskDialog.Show("abc", "Has no rebarinsection in view belong to Host picked");
            	return null;
            }

            else 
            {
	            //Trong mỗi rebar in section, so sánh vị trí tương đối của rebar với trục giữa dầm(Z)
	            foreach (Rebar rebarSet in myListRebarInSection) 
	            {
	            	// Lấy bounding box của mỗi rebar
					//Get bouding box of rebarSet
					BoundingBoxXYZ myRebar_BB = rebarSet.get_BoundingBox(myView);
					
					XYZ minPointRebar = myRebar_BB.Min;
					XYZ maxPointRebar = myRebar_BB.Max;
					XYZ centerPointRebar = (minPointRebar + maxPointRebar)/2;
					
					if(centerPointRebar.Z <= centerPointBeam.Z)
					{
						//myPickPoint = new XYZ(minPointBeam.X, minPointBeam.Y, minPointBeam.Z + (-1*deltaZ*factorDelta));
						listBottomRebar.Add(rebarSet);
						
					}
					else
					{
						//myPickPoint = new XYZ(maxPointBeam.X, maxPointBeam.Y, maxPointBeam.Z + (deltaZ*factorDelta));
						listTopRebar.Add(rebarSet);
					}
		            	
	            }
            }
            List<List<Rebar>> rebarListList = new List<List<Rebar>>(){listBottomRebar, listTopRebar};
            TaskDialog.Show("abc", "has: " + rebarListList[0].Count() + "; " + rebarListList[1].Count);
            
            return rebarListList;
		
		}
		
	
		public void getSubElement()
		{
			UIDocument uiDoc= this.ActiveUIDocument;
			
			Document doc = uiDoc.Document;
			
			View myView = doc.ActiveView;
			
			Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a rebar...");
			
			Rebar myRebar = doc.GetElement(myRef) as Rebar;
			
			TaskDialog.Show("abc", "Has: " + myRebar.NumberOfBarPositions.ToString());
			
			ReferenceArray myRefAr = new ReferenceArray();
			
			int myNumberRebar = (int)myRebar.NumberOfBarPositions;
			if (myNumberRebar >1)
			{
				for (int i = 0; i < myNumberRebar; i++) {
					List<Curve> centerLines = myRebar.GetCenterlineCurves(false, false, false,
			                                                         MultiplanarOption.IncludeOnlyPlanarCurves,0)
			    	as List<Curve>;
			    
				    foreach (Curve myCurBar in centerLines) 
				    {
				    	if (myCurBar is Line)
				    	{
				    		Line myLine = myCurBar as Line;
				    		Reference myRefLine = myLine.Reference;
				    		myRefAr.Append(myRefLine);
				    		break;
				    	}
				    }
				}
			}
			
			
			TaskDialog.Show("abc", myRefAr.Size.ToString());
			
			
			setCurrentViewAsWorkPlan();
			
			XYZ myDimPoint_1 = uiDoc.Selection.PickPoint("Pick Point To Place Dimension....");
			XYZ myDimPoint_2 = new XYZ(myDimPoint_1.X+5, myDimPoint_1.Y, myDimPoint_1.Z);
			Line dimLine = Line.CreateBound(myDimPoint_1, myDimPoint_2);
			
							
			using(Transaction trans = new Transaction(doc, "Create linear Dimension"))
			{
				trans.Start();
				Dimension myDim = doc.Create.NewDimension(doc.ActiveView, dimLine, myRefAr);
				trans.Commit();
			}
		}
	

		
	
		public void getSubElement_geo()
		{
			UIDocument uiDoc= this.ActiveUIDocument;
			
			Document doc = uiDoc.Document;
			
			View myView = doc.ActiveView;
			
			Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a rebar...");
			
			Rebar myRebar = doc.GetElement(myRef) as Rebar;
			
			TaskDialog.Show("abc", "Has: " + myRebar.NumberOfBarPositions.ToString());
			
			ReferenceArray myRefAr = new ReferenceArray();
			
			
			
			
			TaskDialog.Show("abc", myRebar.IsRebarInSection(myView).ToString());
			
//			
//			setCurrentViewAsWorkPlan();
//			
//			XYZ myDimPoint_1 = uiDoc.Selection.PickPoint("Pick Point To Place Dimension....");
//			XYZ myDimPoint_2 = new XYZ(myDimPoint_1.X+5, myDimPoint_1.Y, myDimPoint_1.Z);
//			Line dimLine = Line.CreateBound(myDimPoint_1, myDimPoint_2);
//			
//							
//			using(Transaction trans = new Transaction(doc, "Create linear Dimension"))
//			{
//				trans.Start();
//				Dimension myDim = doc.Create.NewDimension(doc.ActiveView, dimLine, myRefAr);
//				trans.Commit();
//			}
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

		
	}
}