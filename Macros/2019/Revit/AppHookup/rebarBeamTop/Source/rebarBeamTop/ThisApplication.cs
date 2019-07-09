/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 4/11/2019
 * Time: 9:57 AM
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

namespace rebarBeamTop
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("C87413A8-8220-4B3B-A043-D6472F3274AE")]
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
	
	
								
		public void Rebar_FromCurve_Top_Func(Element myBeam, List<Face> myListFacePicked,  double divideFac, double coverBar, 
		                                        string myRebarShapeName, string myRebarTypeName)
		{

			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			//BoundingBox beam
			BoundingBoxXYZ myBoundBeam = myBeam.get_BoundingBox(null);
			double hBeam = myBoundBeam.Max.Z - myBoundBeam.Min.Z;
			
			LocationCurve myLocBeam = myBeam.Location as LocationCurve;
			
			Line myLocLine = myLocBeam.Curve as Line;
			
			XYZ p = myLocLine.GetEndPoint(0);
			XYZ q = myLocLine.GetEndPoint(1);

			XYZ v = q-p;
			
			XYZ pE = p - 0.5*v;
		 	XYZ vE = q - pE;
						
			XYZ vRB = new XYZ(v.Y, -1*v.X, v.Z);
			
		 	List<double> myListDisSortFace =  getAndSortDisOfEndFaces(myListFacePicked, pE);
			 

		 	double lengSeg_1 = myListDisSortFace[1] - myListDisSortFace[0];
		 	double lengSeg_2 = myListDisSortFace[3] - myListDisSortFace[2];		 	
		 	
		 	XYZ ePR1_XY = pE + ((myListDisSortFace[1] - lengSeg_1/divideFac)/vE.GetLength())*vE;
		 	XYZ ePR2_XY = pE + ((myListDisSortFace[2] + lengSeg_2/divideFac)/vE.GetLength())*vE;
			 
		 	
		 	XYZ ePR1 = new XYZ(ePR1_XY.X, ePR1_XY.Y, myBoundBeam.Max.Z - coverBar);
		 	XYZ ePR2 = new XYZ(ePR2_XY.X, ePR2_XY.Y, myBoundBeam.Max.Z - coverBar);
		 	
 			Line curveOfRebar = Line.CreateBound(ePR1, ePR2);
			
			//Tang 
			List<Curve> myShape = new List<Curve>(){curveOfRebar};
			
			// RebarShape
			FilteredElementCollector fecRebarShape = new FilteredElementCollector(doc)
			.OfClass(typeof(RebarShape));
			
    		RebarShape myRebarShape = fecRebarShape.Cast<RebarShape>().
    				First<RebarShape>(myRebarShape2 => myRebarShape2.Name == myRebarShapeName);
			

			// Rebartype
			FilteredElementCollector fecRebarType = new FilteredElementCollector(doc)
			.OfClass(typeof(RebarBarType));
    		RebarBarType myRebarType = fecRebarType.Cast<RebarBarType>().
    				First<RebarBarType>(myRebarType2 => myRebarType2.Name == myRebarTypeName);
		
			
						
			// Hooktype
			FilteredElementCollector fec2 = new FilteredElementCollector(doc)
			.OfClass(typeof(RebarHookType));

			IEnumerable<RebarHookType> iterRebarHookTypes =  fec2.Cast<RebarHookType>();
			
			RebarHookType myRebarHookType = iterRebarHookTypes.First();

			using (Transaction trans = new Transaction(doc, "rebar test") )
			
			{
				trans.Start();
				Rebar myRebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, myRebarType,
				                                       null, null,
				                                       myBeam, vRB,
				                                       myShape,
				                                       RebarHookOrientation.Left, RebarHookOrientation.Right,
				                                       true, false);


				trans.Commit();
			}
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
	
				
				
		private double getDisFromPointToPlaneFace(XYZ myPoint, Face myFace) // Both Nagative and Positive
		{
			Plane myPlaneFace = myFace.GetSurface() as Plane;

			XYZ v = myPoint - myPlaneFace.Origin;
			
			
			double myDis = myPlaneFace.Normal.DotProduct(v);
		
			return Math.Abs(myDis);
		}
		

		
		// Select Faces boundaries
		public void rebarBeam_Top()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			


			double myConvertFactor = 304.8;
			
			
			//Load rebarShape
			// RebarShape
			FilteredElementCollector fecRebarShap = new FilteredElementCollector(doc)
			.OfClass(typeof(RebarShape));
			
			IEnumerable<RebarShape> iterRebarBarShapes =  fecRebarShap.Cast<RebarShape>();
			

			
			// Load Rebar Type
			// Rebartype
			FilteredElementCollector fecBarType = new FilteredElementCollector(doc)
			.OfClass(typeof(RebarBarType));

			IEnumerable<RebarBarType> iterRebarBarTypes =  fecBarType.Cast<RebarBarType>();
			

			// Top Layer
			// Layer1
			bool isLB1Yes = true;
			double FB1 = 6;
			double CB1 = 80;
			string rebaNameShape_L1 = string.Empty;
			string rebaNameType_L1 = string.Empty;			

			
			// Bottom Layer
			// Layer2
			bool isLB2Yes = true;
			double FB2 = 3;
			double CB2 = 150;
			
			string rebaNameShape_L2 = string.Empty;
			string rebaNameType_L2 = string.Empty;	
			
			bool inputSuccess = false;
			while (!inputSuccess) 
			{
				using(var myInputFormSetting = new SettingDialog())
			   {
					//add rebar Shape list
					foreach (RebarShape myRebarShape in iterRebarBarShapes) 
					{
						myInputFormSetting.RebarShap1Top_Cb.Items.Add(myRebarShape.Name);
						myInputFormSetting.RebarShap2Top_Cb.Items.Add(myRebarShape.Name);

					}
					
					
					//add rebar type list	
					foreach (RebarBarType myRebarType in iterRebarBarTypes) 
					{
						myInputFormSetting.RebarType1Top_Cb.Items.Add(myRebarType.Name);
						myInputFormSetting.RebarType2Top_Cb.Items.Add(myRebarType.Name);
					}
					
					
					myInputFormSetting.ShowDialog();
			   	

					//Shape
					rebaNameShape_L1 = myInputFormSetting.RebarShap1Top_Cb.Text;
					rebaNameShape_L2 = myInputFormSetting.RebarShap2Top_Cb.Text;					
					
					//type
					rebaNameType_L1 = myInputFormSetting.RebarType1Top_Cb.Text;
					rebaNameType_L2 = myInputFormSetting.RebarType2Top_Cb.Text;

					
					//Yes/ No bottom rebar
					if(!myInputFormSetting.yes1Top_Chb.Checked) {isLB1Yes = false;}
					if(!myInputFormSetting.yes2Top_Chb.Checked) {isLB2Yes = false;}

					// Layer1

					FB1 = Convert.ToDouble(myInputFormSetting.FT1_Tb.Text);
					CB1 = Convert.ToDouble(myInputFormSetting.CT1_Tb.Text) / myConvertFactor;
					

					// Bottom Layer
					// Layer2

					FB2 = Convert.ToDouble(myInputFormSetting.FT2_Tb.Text);
					CB2 = Convert.ToDouble(myInputFormSetting.CT2_Tb.Text) / myConvertFactor;
									
					
					if(myInputFormSetting.yes1Top_Chb.Checked == false && myInputFormSetting.yes2Top_Chb.Checked == false)
					{
						TaskDialog.Show("Warning!", "Tich vao Yes/No de bo tri it nhat 1 lop");
					
					}
					else
					{
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
			
			}
			
			//Check mode

//			TaskDialog.Show("isL1", isLB1Yes.ToString());
//			TaskDialog.Show("IsL2", isLB1Yes.ToString());
//
//
//			
//			TaskDialog.Show("Rebar Shape1", rebaNameShape_L1);
//			TaskDialog.Show("Rebar Shape2", rebaNameShape_L2);		
//
//						
//			TaskDialog.Show("Rebar Shape1", rebaNameType_L1);
//			TaskDialog.Show("Rebar Shape2", rebaNameType_L2);	
			
			

			List<int> myListIdCategoryRebar = new List<int>();
			myListIdCategoryRebar.Add((int)BuiltInCategory.OST_StructuralFraming);
			Reference myRefmyBeam = uiDoc.Selection.PickObject(ObjectType.Element, 
			                                                  new FilterByIdCategory(myListIdCategoryRebar),
			                                                  "Pick a Beam...");
		
			Element myBeam = doc.GetElement(myRefmyBeam);			


			
			//Get location curve of beam
		    LocationCurve lc = myBeam.Location as LocationCurve;
		    Line line = lc.Curve as Line;
			
		    //Get vector of location cuver beam
            XYZ p = line.GetEndPoint(0);
		    XYZ q = line.GetEndPoint(1);
		    XYZ v = q - p; // Vector equation of line
		    
		    XYZ pE = p - 0.5*v;
		    XYZ vE = q-p;

			//Set current Beam be Joined
			
			setBeJoined(myBeam);
			
			while (true) 
			{

				#region Pick faces
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
				#endregion
			
			
				#region Error picked faces	
				if(myListFacePicked.Count != 4)
				{
					TaskDialog.Show("Error!", "Chua ho tro lua chon: " + myListFacePicked.Count() + " mat, Chon 4 mat");
					continue;
					
				}
				
				#endregion
				
				else
				{
					string 	caseDistributionRebar = "Case 2: 4 Faces";
					
					if(myListFacePicked.Count ==2)
					{
						caseDistributionRebar = "Case 1: 2 Faces";
						
					}
					TaskDialog.Show("Info", caseDistributionRebar);
					

					
					if(isLB1Yes)
					{
						Rebar_FromCurve_Top_Func(myBeam, myListFacePicked, FB1, CB1,
						                             rebaNameShape_L1,rebaNameType_L1);
					}
					if(isLB2Yes)
					{
						Rebar_FromCurve_Top_Func(myBeam, myListFacePicked, FB2, CB2,
						                             rebaNameShape_L2,rebaNameType_L2);
					}
				}
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