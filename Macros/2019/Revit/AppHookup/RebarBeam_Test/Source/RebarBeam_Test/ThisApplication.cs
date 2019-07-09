/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 4/8/2019
 * Time: 8:01 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace RebarBeam_Test
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("0118554B-BCF2-4FE3-84BB-1486B66C0D3B")]
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

		
		// Ham nay can kiem tra lai
		private double getDisFromPointToPlaneFace(XYZ myPoint, Face myFace) // Both Nagative and Positive
		{
			Plane myPlaneFace = myFace.GetSurface() as Plane;
			
			XYZ v = myPoint - myPlaneFace.Origin;
				
			double myDis = myPlaneFace.Normal.DotProduct(v);
		
			return Math.Abs(myDis); // return only positive
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
		

						
		// Select Faces boundaries
		public void SetLayoutRebarsBeam_Form_MaxSpace_Rebar_Bottom()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			
			//Stirrup
			bool isSt1Yes = true;
			bool isOpposite = false;

			double factor = 4;
			double delta_1 = 50/304.8;
			double pitch_1 = 150/304.8 ;
			double pitch_2 = 300/304.8;
			
			double delta_3 = 50/304.8;
			double pitch_3 = 100/304.8;
			int N3 = 5;

			
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
			

			// Bottom Layer
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
						myInputFormSetting.RebarShap1_Cb.Items.Add(myRebarShape.Name);
						myInputFormSetting.RebarShap2_Cb.Items.Add(myRebarShape.Name);

					}
					
					myInputFormSetting.RebarShap1_Cb.SelectedIndex = 0;
					myInputFormSetting.RebarShap2_Cb.SelectedIndex = 0;
					
					//add rebar type list	
					foreach (RebarBarType myRebarType in iterRebarBarTypes) 
					{
						myInputFormSetting.RebarType1_Cb.Items.Add(myRebarType.Name);
						myInputFormSetting.RebarType2_Cb.Items.Add(myRebarType.Name);
					}
					
					myInputFormSetting.RebarType1_Cb.SelectedIndex = 1;
					myInputFormSetting.RebarType2_Cb.SelectedIndex = 1;
					
					
					myInputFormSetting.ShowDialog();
			   	
					//stirrup
					factor = Convert.ToDouble(myInputFormSetting.factorTb.Text);
					delta_1 = Convert.ToDouble(myInputFormSetting.delta_1Tb.Text) / myConvertFactor;
					pitch_1 = Convert.ToDouble(myInputFormSetting.pitch_1Tb.Text) / myConvertFactor;
					pitch_2 = Convert.ToDouble(myInputFormSetting.pitch_2Tb.Text) / myConvertFactor;
					
					delta_3 = Convert.ToDouble(myInputFormSetting.delta_3Tb.Text) / myConvertFactor;
					pitch_3 = Convert.ToDouble(myInputFormSetting.pitch_3Tb.Text) / myConvertFactor;
					N3 = Convert.ToInt32(myInputFormSetting.n3Tb.Text);
					isOpposite = myInputFormSetting.oppDirChecked;
					isSt1Yes = myInputFormSetting.yesST_Chb.Checked;
					
					
					
					
					//Shape
					rebaNameShape_L1 = myInputFormSetting.RebarShap1_Cb.Text;
					rebaNameShape_L2 = myInputFormSetting.RebarShap2_Cb.Text;					
					
					//type
					rebaNameType_L1 = myInputFormSetting.RebarType1_Cb.Text;
					rebaNameType_L2 = myInputFormSetting.RebarType2_Cb.Text;

					
					//Yes/ No bottom rebar
					if(!myInputFormSetting.yes1_Chb.Checked) {isLB1Yes = false;}
					if(!myInputFormSetting.yes2_Chb.Checked) {isLB2Yes = false;}

					// Layer1

					FB1 = Convert.ToDouble(myInputFormSetting.FB1_Tb.Text);
					CB1 = Convert.ToDouble(myInputFormSetting.CB1_Tb.Text) / myConvertFactor;
					

					// Bottom Layer
					// Layer2

					FB2 = Convert.ToDouble(myInputFormSetting.FB2_Tb.Text);
					CB2 = Convert.ToDouble(myInputFormSetting.CB2_Tb.Text) / myConvertFactor;
									
					
				
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
			
			//Check mode
//			TaskDialog.Show("opposite", isOpposite.ToString());
//			TaskDialog.Show("draw stirrup", isSt1Yes.ToString());
//
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
			
			
			//Set mybeam
			Element myBeam = null;
			
			// Kiem tra ve stirrup
			
			List<Rebar> myListRebar = new List<Rebar>();
			
			if(isSt1Yes)
			{
				// Pick Rebar
				List<int> myListIdCategoryRebar = new List<int>();
				myListIdCategoryRebar.Add((int)BuiltInCategory.OST_Rebar);
				
				// Select first Element (ex beam)
				List<Reference> myListRefRebar = uiDoc.Selection.PickObjects(ObjectType.Element, 
				                                                  new FilterByIdCategory(myListIdCategoryRebar),
				                                                  "Pick a Rebar...") as List<Reference>;
				

				foreach (Reference myRefRebar in myListRefRebar) 
				{
					Rebar myRebarpicked = doc.GetElement(myRefRebar) as Rebar;
					myListRebar.Add(myRebarpicked);
				}
				myBeam = doc.GetElement(myListRebar[0].GetHostId());
			}
			
			else
			{
				List<int> myListIdCategoryRebar = new List<int>();
				myListIdCategoryRebar.Add((int)BuiltInCategory.OST_StructuralFraming);
				Reference myRefmyBeam = uiDoc.Selection.PickObject(ObjectType.Element, 
				                                                  new FilterByIdCategory(myListIdCategoryRebar),
				                                                  "Pick a Beam...");
			
				myBeam = doc.GetElement(myRefmyBeam);			
			}

			
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
				if(myListFacePicked.Count !=2 && myListFacePicked.Count != 4)
				{
					TaskDialog.Show("Error!", "Chua ho tro lua chon: " + myListFacePicked.Count() + " mat, Chon 2 hoac 4 mat");
					continue;
					
				}
				
				#endregion
				
				else
				{
					string 	caseDistributionRebar = "Case 2: 4 Faces";
					
					List<double> myListSpace = new List<double>(){pitch_1, pitch_2, pitch_3, pitch_3, pitch_2, pitch_1};
					if(myListFacePicked.Count ==2)
					{
						myListSpace = new List<double>(){pitch_1, pitch_2, pitch_1, pitch_2, pitch_2, pitch_1};
						caseDistributionRebar = "Case 1: 2 Faces";
						
					}
					TaskDialog.Show("Info", caseDistributionRebar);
					
					if(isSt1Yes)
					{
					
					#region Rebar stirrup
					
					// List of boundaries faces
					List<double> myListEndPointDis = getAndSortDisOfEndFaces(myListFacePicked, pE);
					myListEndPointDis.Sort();
					
					Dictionary<double, double> myDicDisNumDetail = detailListDistance_Update_MaxSpace(myListEndPointDis,
					                                                                factor,
					                                                                delta_1, pitch_1,
					                                                                pitch_2,
					                                                                delta_3, pitch_3, N3);
						
					foreach (Rebar myRebar in myListRebar) 
					{							
						List<ElementId> myListRebarCopyId = copyRebarByDistance_MaxSpace(myRebar, myDicDisNumDetail);
						
						
						List<double> myDistances = myDicDisNumDetail.Keys.ToList();
						myDistances.Sort();
						
						List<double> myListNum = new List<double>();
						
						foreach (double key in myDistances) 
						{
							myListNum.Add(myDicDisNumDetail[key]);
						}
						
						
						// using transcation (edit DB)
						for(int i = 0; i < myListRebarCopyId.Count(); i++)
						{
							using (Transaction myTrans = new Transaction(doc,"CopyElementByCoordinate"))
					
							{
								myTrans.Start();
								ElementId rebarId = myListRebarCopyId[i];
								Rebar myRebarI = doc.GetElement(rebarId) as Rebar;
								
								if(i == 1 || i == 4)
								{
									if(myListNum[i]/myListSpace[i] < 1)
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
									}

									else
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsMaximumSpacing( myListSpace[i],(myListNum[i]), isOpposite, false, false);
										
									}
								}
								
								else
								{
									if(myListNum[i]/myListSpace[i] < 1)
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
									}
		
									else
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsMaximumSpacing( myListSpace[i],(myListNum[i]), isOpposite, true, true);
									}
								}
		
								myTrans.Commit();
							}
		
						}
						
					}
				#endregion
					}
					
					if(isLB1Yes)
					{
						Rebar_FromCurve_Bot_Func(myBeam, myListFacePicked, FB1, CB1,
						                             rebaNameShape_L1,rebaNameType_L1);
					}
					if(isLB2Yes)
					{
						Rebar_FromCurve_Bot_Func(myBeam, myListFacePicked, FB2, CB2,
						                             rebaNameShape_L2,rebaNameType_L2);
					}
				}
		}
	}
		
	
						
		// Select Faces boundaries
		public void SetLayoutRebarsBeam_Form_MaxSpace_Rebar_Bottom_C1()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			
			//Stirrup
			bool isSt1Yes = true;
			bool isOpposite = false;

			double factor = 4;
			double delta_1 = 50/304.8;
			double pitch_1 = 150/304.8 ;
			double pitch_2 = 300/304.8;
			
			double delta_3 = 50/304.8;
			double pitch_3 = 100/304.8;
			int N3 = 4;

						
			//Stirrup
			bool isYes_C1 = true;
			bool isOpposite_C1 = false;

			double factor_C1 = 4;
			double delta_1_C1 = 50/304.8;
			double pitch_1_C1 = 150/304.8 ;
			double pitch_2_C1 = 300/304.8;
			
			double delta_3_C1 = 50/304.8;
			double pitch_3_C1 = 100/304.8;
			int N3_C1 = 4;
			
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
			

			// Bottom Layer
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
						myInputFormSetting.RebarShap1_Cb.Items.Add(myRebarShape.Name);
						myInputFormSetting.RebarShap2_Cb.Items.Add(myRebarShape.Name);

					}
					
//					myInputFormSetting.RebarShap1_Cb.SelectedIndex = 0;
//					myInputFormSetting.RebarShap2_Cb.SelectedIndex = 0;
					
					//add rebar type list	
					foreach (RebarBarType myRebarType in iterRebarBarTypes) 
					{
						myInputFormSetting.RebarType1_Cb.Items.Add(myRebarType.Name);
						myInputFormSetting.RebarType2_Cb.Items.Add(myRebarType.Name);
					}
					
//					myInputFormSetting.RebarType1_Cb.SelectedIndex = 1;
//					myInputFormSetting.RebarType2_Cb.SelectedIndex = 1;
					
					
					myInputFormSetting.ShowDialog();
			   	
					//stirrup
					factor = Convert.ToDouble(myInputFormSetting.factorTb.Text);
					delta_1 = Convert.ToDouble(myInputFormSetting.delta_1Tb.Text) / myConvertFactor;
					pitch_1 = Convert.ToDouble(myInputFormSetting.pitch_1Tb.Text) / myConvertFactor;
					pitch_2 = Convert.ToDouble(myInputFormSetting.pitch_2Tb.Text) / myConvertFactor;
					
					delta_3 = Convert.ToDouble(myInputFormSetting.delta_3Tb.Text) / myConvertFactor;
					pitch_3 = Convert.ToDouble(myInputFormSetting.pitch_3Tb.Text) / myConvertFactor;
					N3 = Convert.ToInt32(myInputFormSetting.n3Tb.Text);
					isOpposite = myInputFormSetting.oppDirChecked;
					isSt1Yes = myInputFormSetting.yesST_Chb.Checked;
					
			   	
					//C1
					factor_C1 = Convert.ToDouble(myInputFormSetting.factor_C1Tb.Text);
					delta_1_C1 = Convert.ToDouble(myInputFormSetting.delta_1_C1Tb.Text) / myConvertFactor;
					pitch_1_C1 = Convert.ToDouble(myInputFormSetting.pitch_1_C1Tb.Text) / myConvertFactor;
					pitch_2_C1  = Convert.ToDouble(myInputFormSetting.pitch_2_C1Tb.Text) / myConvertFactor;
					
					delta_3_C1  = Convert.ToDouble(myInputFormSetting.delta_3_C1Tb.Text) / myConvertFactor;
					pitch_3_C1  = Convert.ToDouble(myInputFormSetting.pitch_3_C1Tb.Text) / myConvertFactor;
					N3_C1  = Convert.ToInt32(myInputFormSetting.n3_C1Tb.Text);
					isOpposite_C1  = myInputFormSetting.oppDirChecked;
					isYes_C1  = myInputFormSetting.yesC1_Chb.Checked;					
					
					
					//Shape
					rebaNameShape_L1 = myInputFormSetting.RebarShap1_Cb.Text;
					rebaNameShape_L2 = myInputFormSetting.RebarShap2_Cb.Text;					
					
					//type
					rebaNameType_L1 = myInputFormSetting.RebarType1_Cb.Text;
					rebaNameType_L2 = myInputFormSetting.RebarType2_Cb.Text;

					
					//Yes/ No bottom rebar
					if(!myInputFormSetting.yes1_Chb.Checked) {isLB1Yes = false;}
					if(!myInputFormSetting.yes2_Chb.Checked) {isLB2Yes = false;}

					// Layer1

					FB1 = Convert.ToDouble(myInputFormSetting.FB1_Tb.Text);
					CB1 = Convert.ToDouble(myInputFormSetting.CB1_Tb.Text) / myConvertFactor;
					

					// Bottom Layer
					// Layer2

					FB2 = Convert.ToDouble(myInputFormSetting.FB2_Tb.Text);
					CB2 = Convert.ToDouble(myInputFormSetting.CB2_Tb.Text) / myConvertFactor;
									
					
				
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
			
			//Check mode
//			TaskDialog.Show("opposite", isOpposite.ToString());
//			TaskDialog.Show("draw stirrup", isSt1Yes.ToString());
//
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
			
			
			//Set mybeam
			Element myBeam = null;
			
			// Kiem tra ve stirrup
			
			List<Rebar> myListRebar = new List<Rebar>();
			List<Rebar> myListRebar_C1 = new List<Rebar>();
			
			if(isSt1Yes)
			{
				// Pick Rebar
				List<int> myListIdCategoryRebar = new List<int>();
				myListIdCategoryRebar.Add((int)BuiltInCategory.OST_Rebar);
				
				// Select first Element (ex beam)
				List<Reference> myListRefRebar = uiDoc.Selection.PickObjects(ObjectType.Element, 
				                                                  new FilterByIdCategory(myListIdCategoryRebar),
				                                                  "Pick a stirrup Rebar...") as List<Reference>;
				

				foreach (Reference myRefRebar in myListRefRebar) 
				{
					Rebar myRebarpicked = doc.GetElement(myRefRebar) as Rebar;
					myListRebar.Add(myRebarpicked);
				}
				myBeam = doc.GetElement(myListRebar[0].GetHostId());
			}
			
			if(isYes_C1)
			{
				// Pick Rebar
				List<int> myListIdCategoryRebar_C1 = new List<int>();
				myListIdCategoryRebar_C1.Add((int)BuiltInCategory.OST_Rebar);
				
				// Select first Element (ex beam)
				List<Reference> myListRefRebar_C1 = uiDoc.Selection.PickObjects(ObjectType.Element, 
				                                                  new FilterByIdCategory(myListIdCategoryRebar_C1),
				                                                  "Pick a C Rebar ...") as List<Reference>;
				

				foreach (Reference myRefRebar_C1 in myListRefRebar_C1) 
				{
					Rebar myRebarpicked_C1 = doc.GetElement(myRefRebar_C1) as Rebar;
					myListRebar_C1.Add(myRebarpicked_C1);
				}
				myBeam = doc.GetElement(myListRebar_C1[0].GetHostId());
			}
			
			else
			{
				List<int> myListIdCategoryRebar = new List<int>();
				myListIdCategoryRebar.Add((int)BuiltInCategory.OST_StructuralFraming);
				Reference myRefmyBeam = uiDoc.Selection.PickObject(ObjectType.Element, 
				                                                  new FilterByIdCategory(myListIdCategoryRebar),
				                                                  "Pick a Beam...");
			
				myBeam = doc.GetElement(myRefmyBeam);			
			}

			
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
			
			//setBeJoined(myBeam);
			
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
				if(myListFacePicked.Count !=2 && myListFacePicked.Count != 4)
				{
					TaskDialog.Show("Error!", "Chua ho tro lua chon: " + myListFacePicked.Count() + " mat, Chon 2 hoac 4 mat");
					continue;
					
				}
				
				#endregion
				
				else
				{
					string 	caseDistributionRebar = "Case 2: 4 Faces";
					
					List<double> myListSpace = new List<double>(){pitch_1, pitch_2, pitch_3, pitch_3, pitch_2, pitch_1};
					List<double> myListSpace_C1 = new List<double>(){pitch_1_C1, pitch_2_C1, pitch_3_C1, pitch_3_C1, pitch_2_C1, pitch_1_C1};
					if(myListFacePicked.Count ==2)
					{
						myListSpace = new List<double>(){pitch_1, pitch_2, pitch_1, pitch_2, pitch_2, pitch_1};
						myListSpace_C1 = new List<double>(){pitch_1_C1, pitch_2_C1, pitch_1_C1, pitch_2_C1, pitch_2_C1, pitch_1_C1};
						caseDistributionRebar = "Case 1: 2 Faces";
						
					}
					TaskDialog.Show("Info", caseDistributionRebar);
					
					if(isSt1Yes)
					{
					
					#region Rebar stirrup
					
					// List of boundaries faces
					List<double> myListEndPointDis = getAndSortDisOfEndFaces(myListFacePicked, pE);
					myListEndPointDis.Sort();
					
					Dictionary<double, double> myDicDisNumDetail = detailListDistance_Update_MaxSpace(myListEndPointDis,
					                                                                factor,
					                                                                delta_1, pitch_1,
					                                                                pitch_2,
					                                                                delta_3, pitch_3, N3);
						
					foreach (Rebar myRebar in myListRebar) 
					{							
						List<ElementId> myListRebarCopyId = copyRebarByDistance_MaxSpace(myRebar, myDicDisNumDetail);
						
						
						List<double> myDistances = myDicDisNumDetail.Keys.ToList();
						myDistances.Sort();
						
						List<double> myListNum = new List<double>();
						
						foreach (double key in myDistances) 
						{
							myListNum.Add(myDicDisNumDetail[key]);
						}
						
						
						// using transcation (edit DB)
						for(int i = 0; i < myListRebarCopyId.Count(); i++)
						{
							using (Transaction myTrans = new Transaction(doc,"CopyElementByCoordinate"))
					
							{
								myTrans.Start();
								ElementId rebarId = myListRebarCopyId[i];
								Rebar myRebarI = doc.GetElement(rebarId) as Rebar;
								
								if(i == 1 || i == 4)
								{
									if(myListNum[i]/myListSpace[i] < 1)
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
									}

									else
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsMaximumSpacing( myListSpace[i],(myListNum[i]), isOpposite, false, false);
										
									}
								}
								
								else
								{
									if(myListNum[i]/myListSpace[i] < 1)
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
									}
		
									else
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsMaximumSpacing( myListSpace[i],(myListNum[i]), isOpposite, true, true);
									}
								}
		
								myTrans.Commit();
							}
		
						}
						
					}
				#endregion
					}
					
					
					if(isYes_C1)
					{
					
					#region Rebar C stirrup
					
					// List of boundaries faces
					List<double> myListEndPointDis_C1 = getAndSortDisOfEndFaces(myListFacePicked, pE);
					myListEndPointDis_C1.Sort();
					
					Dictionary<double, double> myDicDisNumDetail_C1 = detailListDistance_Update_MaxSpace(myListEndPointDis_C1,
					                                                                factor_C1,
					                                                                delta_1_C1, pitch_1_C1,
					                                                                pitch_2_C1,
					                                                                delta_3_C1, pitch_3_C1, N3_C1);
						
					foreach (Rebar myRebar in myListRebar_C1) 
					{							
						List<ElementId> myListRebarCopyId_C1 = copyRebarByDistance_MaxSpace(myRebar, myDicDisNumDetail_C1);
						
						
						List<double> myDistances_C1 = myDicDisNumDetail_C1.Keys.ToList();
						myDistances_C1.Sort();
						
						List<double> myListNum_C1 = new List<double>();
						
						foreach (double key in myDistances_C1) 
						{
							myListNum_C1.Add(myDicDisNumDetail_C1[key]);
						}
						
						
						// using transcation (edit DB)
						for(int i = 0; i < myListRebarCopyId_C1.Count(); i++)
						{
							using (Transaction myTrans = new Transaction(doc,"CopyElementByCoordinate"))
					
							{
								myTrans.Start();
								ElementId rebarId = myListRebarCopyId_C1[i];
								Rebar myRebarI = doc.GetElement(rebarId) as Rebar;
								
								if(i == 1 || i == 4)
								{
									if(myListNum_C1[i]/myListSpace_C1[i] < 1)
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
									}

									else
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsMaximumSpacing( myListSpace_C1[i],(myListNum_C1[i]), isOpposite, false, false);
										
									}
								}
								
								else
								{
									if(myListNum_C1[i]/myListSpace_C1[i] < 1)
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
									}
		
									else
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsMaximumSpacing( myListSpace_C1[i],(myListNum_C1[i]), isOpposite, true, true);
									}
								}
		
								myTrans.Commit();
							}
		
						}
						
					}
				#endregion
					}
					
					
					if(isLB1Yes)
					{
						Rebar_FromCurve_Bot_Func(myBeam, myListFacePicked, FB1, CB1,
						                             rebaNameShape_L1,rebaNameType_L1);
					}
					if(isLB2Yes)
					{
						Rebar_FromCurve_Bot_Func(myBeam, myListFacePicked, FB2, CB2,
						                             rebaNameShape_L2,rebaNameType_L2);
					}
				}
		}
	}
		
	
						
		// Select Faces boundaries
		public void SetLayoutRebarsBeam_Form_MaxSpace_Rebar_Bottom_C1_new()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			
			//Stirrup
			bool isSt1Yes = true;
			bool isOpposite = false;

			double factor = 4;
			double delta_1 = 50/304.8;
			double pitch_1 = 150/304.8 ;
			double pitch_2 = 300/304.8;
			
			double delta_3 = 50/304.8;
			double pitch_3 = 100/304.8;
			int N3 = 4;

						
			//Stirrup
			bool isYes_C1 = true;
			bool isOpposite_C1 = false;

			double factor_C1 = 4;
			double delta_1_C1 = 50/304.8;
			double pitch_1_C1 = 150/304.8 ;
			double pitch_2_C1 = 300/304.8;
			
			double delta_3_C1 = 50/304.8;
			double pitch_3_C1 = 100/304.8;
			int N3_C1 = 4;
			
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
			

			// Bottom Layer
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
						myInputFormSetting.RebarShap1_Cb.Items.Add(myRebarShape.Name);
						myInputFormSetting.RebarShap2_Cb.Items.Add(myRebarShape.Name);

					}
					
//					myInputFormSetting.RebarShap1_Cb.SelectedIndex = 0;
//					myInputFormSetting.RebarShap2_Cb.SelectedIndex = 0;
					
					//add rebar type list	
					foreach (RebarBarType myRebarType in iterRebarBarTypes) 
					{
						myInputFormSetting.RebarType1_Cb.Items.Add(myRebarType.Name);
						myInputFormSetting.RebarType2_Cb.Items.Add(myRebarType.Name);
					}
					
//					myInputFormSetting.RebarType1_Cb.SelectedIndex = 1;
//					myInputFormSetting.RebarType2_Cb.SelectedIndex = 1;
					
					
					myInputFormSetting.ShowDialog();
			   	
					//stirrup
					factor = Convert.ToDouble(myInputFormSetting.factorTb.Text);
					delta_1 = Convert.ToDouble(myInputFormSetting.delta_1Tb.Text) / myConvertFactor;
					pitch_1 = Convert.ToDouble(myInputFormSetting.pitch_1Tb.Text) / myConvertFactor;
					pitch_2 = Convert.ToDouble(myInputFormSetting.pitch_2Tb.Text) / myConvertFactor;
					
					delta_3 = Convert.ToDouble(myInputFormSetting.delta_3Tb.Text) / myConvertFactor;
					pitch_3 = Convert.ToDouble(myInputFormSetting.pitch_3Tb.Text) / myConvertFactor;
					N3 = Convert.ToInt32(myInputFormSetting.n3Tb.Text);
					isOpposite = myInputFormSetting.oppDirChecked;
					isSt1Yes = myInputFormSetting.yesST_Chb.Checked;
					
			   	
					//C1
					factor_C1 = Convert.ToDouble(myInputFormSetting.factor_C1Tb.Text);
					delta_1_C1 = Convert.ToDouble(myInputFormSetting.delta_1_C1Tb.Text) / myConvertFactor;
					pitch_1_C1 = Convert.ToDouble(myInputFormSetting.pitch_1_C1Tb.Text) / myConvertFactor;
					pitch_2_C1  = Convert.ToDouble(myInputFormSetting.pitch_2_C1Tb.Text) / myConvertFactor;
					
					delta_3_C1  = Convert.ToDouble(myInputFormSetting.delta_3_C1Tb.Text) / myConvertFactor;
					pitch_3_C1  = Convert.ToDouble(myInputFormSetting.pitch_3_C1Tb.Text) / myConvertFactor;
					N3_C1  = Convert.ToInt32(myInputFormSetting.n3_C1Tb.Text);
					isOpposite_C1  = myInputFormSetting.oppDirChecked;
					isYes_C1  = myInputFormSetting.yesC1_Chb.Checked;					
					
					
					//Shape
					rebaNameShape_L1 = myInputFormSetting.RebarShap1_Cb.Text;
					rebaNameShape_L2 = myInputFormSetting.RebarShap2_Cb.Text;					
					
					//type
					rebaNameType_L1 = myInputFormSetting.RebarType1_Cb.Text;
					rebaNameType_L2 = myInputFormSetting.RebarType2_Cb.Text;

					
					//Yes/ No bottom rebar
					if(!myInputFormSetting.yes1_Chb.Checked) {isLB1Yes = false;}
					if(!myInputFormSetting.yes2_Chb.Checked) {isLB2Yes = false;}

					// Layer1

					FB1 = Convert.ToDouble(myInputFormSetting.FB1_Tb.Text);
					CB1 = Convert.ToDouble(myInputFormSetting.CB1_Tb.Text) / myConvertFactor;
					

					// Bottom Layer
					// Layer2

					FB2 = Convert.ToDouble(myInputFormSetting.FB2_Tb.Text);
					CB2 = Convert.ToDouble(myInputFormSetting.CB2_Tb.Text) / myConvertFactor;
									
					
				
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
			
			//Check mode
//			TaskDialog.Show("opposite", isOpposite.ToString());
//			TaskDialog.Show("draw stirrup", isSt1Yes.ToString());
//
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
			
			
			//Set mybeam
			Element myBeam = null;
			
			// Kiem tra ve stirrup
			
			List<Rebar> myListRebar = new List<Rebar>();
			List<Rebar> myListRebar_C1 = new List<Rebar>();
			
			if(isSt1Yes)
			{
				// Pick Rebar
				List<int> myListIdCategoryRebar = new List<int>();
				myListIdCategoryRebar.Add((int)BuiltInCategory.OST_Rebar);
				
				// Select first Element (ex beam)
				List<Reference> myListRefRebar = uiDoc.Selection.PickObjects(ObjectType.Element, 
				                                                  new FilterByIdCategory(myListIdCategoryRebar),
				                                                  "Pick a stirrup Rebar...") as List<Reference>;
				

				foreach (Reference myRefRebar in myListRefRebar) 
				{
					Rebar myRebarpicked = doc.GetElement(myRefRebar) as Rebar;
					myListRebar.Add(myRebarpicked);
				}
				myBeam = doc.GetElement(myListRebar[0].GetHostId());
			}
			
			if(isYes_C1)
			{
				// Pick Rebar
				List<int> myListIdCategoryRebar_C1 = new List<int>();
				myListIdCategoryRebar_C1.Add((int)BuiltInCategory.OST_Rebar);
				
				// Select first Element (ex beam)
				List<Reference> myListRefRebar_C1 = uiDoc.Selection.PickObjects(ObjectType.Element, 
				                                                  new FilterByIdCategory(myListIdCategoryRebar_C1),
				                                                  "Pick a C Rebar ...") as List<Reference>;
				

				foreach (Reference myRefRebar_C1 in myListRefRebar_C1) 
				{
					Rebar myRebarpicked_C1 = doc.GetElement(myRefRebar_C1) as Rebar;
					myListRebar_C1.Add(myRebarpicked_C1);
				}
				myBeam = doc.GetElement(myListRebar_C1[0].GetHostId());
			}
			
			//Nếu LB1
			if( myBeam== null)
			{
				List<int> myListIdCategoryRebar = new List<int>();
				myListIdCategoryRebar.Add((int)BuiltInCategory.OST_StructuralFraming);
				Reference myRefmyBeam = uiDoc.Selection.PickObject(ObjectType.Element, 
				                                                  new FilterByIdCategory(myListIdCategoryRebar),
				                                                  "Pick a Beam...");
			
				myBeam = doc.GetElement(myRefmyBeam);			
			}

			//TODO: Các bước tính toán hình học của dầm
			
			
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
			
			//setBeJoined(myBeam);
			
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
				if(myListFacePicked.Count !=2 && myListFacePicked.Count != 4)
				{
					TaskDialog.Show("Error!", "Chua ho tro lua chon: " + myListFacePicked.Count() + " mat, Chon 2 hoac 4 mat");
					continue;
					
				}
				
				#endregion
				
				else
				{
					string 	caseDistributionRebar = "Case 2: 4 Faces";
					
					List<double> myListSpace = new List<double>(){pitch_1, pitch_2, pitch_3, pitch_3, pitch_2, pitch_1};
					List<double> myListSpace_C1 = new List<double>(){pitch_1_C1, pitch_2_C1, pitch_3_C1, pitch_3_C1, pitch_2_C1, pitch_1_C1};
					if(myListFacePicked.Count ==2)
					{
						myListSpace = new List<double>(){pitch_1, pitch_2, pitch_1, pitch_2, pitch_2, pitch_1};
						myListSpace_C1 = new List<double>(){pitch_1_C1, pitch_2_C1, pitch_1_C1, pitch_2_C1, pitch_2_C1, pitch_1_C1};
						caseDistributionRebar = "Case 1: 2 Faces";
						
					}
					TaskDialog.Show("Info", caseDistributionRebar);
					
					if(isSt1Yes)
					{
					
					#region Rebar stirrup
					
					// List of boundaries faces
					List<double> myListEndPointDis = getAndSortDisOfEndFaces(myListFacePicked, pE);
					myListEndPointDis.Sort();
					
					Dictionary<double, double> myDicDisNumDetail = detailListDistance_Update_MaxSpace(myListEndPointDis,
					                                                                factor,
					                                                                delta_1, pitch_1,
					                                                                pitch_2,
					                                                                delta_3, pitch_3, N3);
						
					foreach (Rebar myRebar in myListRebar) 
					{							
						List<ElementId> myListRebarCopyId = copyRebarByDistance_MaxSpace(myRebar, myDicDisNumDetail);
						
						
						List<double> myDistances = myDicDisNumDetail.Keys.ToList();
						myDistances.Sort();
						
						List<double> myListNum = new List<double>();
						
						foreach (double key in myDistances) 
						{
							myListNum.Add(myDicDisNumDetail[key]);
						}
						
						
						// using transcation (edit DB)
						for(int i = 0; i < myListRebarCopyId.Count(); i++)
						{
							using (Transaction myTrans = new Transaction(doc,"CopyElementByCoordinate"))
					
							{
								myTrans.Start();
								ElementId rebarId = myListRebarCopyId[i];
								Rebar myRebarI = doc.GetElement(rebarId) as Rebar;
								
								if(i == 1 || i == 4)
								{
									if(myListNum[i]/myListSpace[i] < 1)
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
									}

									else
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsMaximumSpacing( myListSpace[i],(myListNum[i]), isOpposite, false, false);
										
									}
								}
								
								else
								{
									if(myListNum[i]/myListSpace[i] < 1)
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
									}
		
									else
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsMaximumSpacing( myListSpace[i],(myListNum[i]), isOpposite, true, true);
									}
								}
		
								myTrans.Commit();
							}
		
						}
						
					}
				#endregion
					}
					
					
					if(isYes_C1)
					{
					
					#region Rebar C stirrup
					
					// List of boundaries faces
					List<double> myListEndPointDis_C1 = getAndSortDisOfEndFaces(myListFacePicked, pE);
					myListEndPointDis_C1.Sort();
					
					Dictionary<double, double> myDicDisNumDetail_C1 = detailListDistance_Update_MaxSpace(myListEndPointDis_C1,
					                                                                factor_C1,
					                                                                delta_1_C1, pitch_1_C1,
					                                                                pitch_2_C1,
					                                                                delta_3_C1, pitch_3_C1, N3_C1);
						
					foreach (Rebar myRebar in myListRebar_C1) 
					{							
						List<ElementId> myListRebarCopyId_C1 = copyRebarByDistance_MaxSpace(myRebar, myDicDisNumDetail_C1);
						
						
						List<double> myDistances_C1 = myDicDisNumDetail_C1.Keys.ToList();
						myDistances_C1.Sort();
						
						List<double> myListNum_C1 = new List<double>();
						
						foreach (double key in myDistances_C1) 
						{
							myListNum_C1.Add(myDicDisNumDetail_C1[key]);
						}
						
						
						// using transcation (edit DB)
						for(int i = 0; i < myListRebarCopyId_C1.Count(); i++)
						{
							using (Transaction myTrans = new Transaction(doc,"CopyElementByCoordinate"))
					
							{
								myTrans.Start();
								ElementId rebarId = myListRebarCopyId_C1[i];
								Rebar myRebarI = doc.GetElement(rebarId) as Rebar;
								
								if(i == 1 || i == 4)
								{
									if(myListNum_C1[i]/myListSpace_C1[i] < 1)
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
									}

									else
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsMaximumSpacing( myListSpace_C1[i],(myListNum_C1[i]), isOpposite, false, false);
										
									}
								}
								
								else
								{
									if(myListNum_C1[i]/myListSpace_C1[i] < 1)
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
									}
		
									else
									{
										myRebarI.GetShapeDrivenAccessor().SetLayoutAsMaximumSpacing( myListSpace_C1[i],(myListNum_C1[i]), isOpposite, true, true);
									}
								}
		
								myTrans.Commit();
							}
		
						}
						
					}
				#endregion
					}
					
					
					if(isLB1Yes)
					{
						Rebar_FromCurve_Bot_Func(myBeam, myListFacePicked, FB1, CB1,
						                             rebaNameShape_L1,rebaNameType_L1);
					}
					if(isLB2Yes)
					{
						Rebar_FromCurve_Bot_Func(myBeam, myListFacePicked, FB2, CB2,
						                             rebaNameShape_L2,rebaNameType_L2);
					}
				}
		}
	}
		
		
		//Tra ve 2 dic gom khoang cach offset va so thanh layout tuong ung 
		private Dictionary<double, double> detailListDistance_Update_MaxSpace(List<double> endPoiList, double factorDivide,
		                                         double delta_1, double pitch_1,
		                                          double pitch_2,
		                                         double delta_3, double pitch_3, int N3)
		{
			Dictionary<double, double> myReturnDic = new Dictionary<double, double>();
			if(endPoiList.Count !=2 && endPoiList.Count !=4)
			{
				return myReturnDic;
			}
			
			if(endPoiList.Count == 2)
			{
				// Always positive
				double lengthSeg = endPoiList[endPoiList.Count() - 1] - endPoiList[0];
				// Diem dau cac doan
				double X1 = endPoiList[0] + delta_1;
				double X2 = X1 + lengthSeg/factorDivide - delta_1;
				double X5 = X2 + lengthSeg - 2 * lengthSeg/factorDivide;
				
				//Length Array
				double L1 = X2-X1;
				double L2 = X5-X2;
				double L5 = L1;
				
				
				myReturnDic.Add(X1, L1);				
				myReturnDic.Add(X2, L2);
				myReturnDic.Add(X5, L5);

				return myReturnDic;
			}
			
			else
			{
				// Always positive
				double lengthSeg = endPoiList[endPoiList.Count() - 1] - endPoiList[0];
				// Diem dau cac doan
				double X1 = endPoiList[0] + delta_1;
				double X2 = X1 + lengthSeg/factorDivide - delta_1;
				double X3 = endPoiList[1] - delta_3 - (N3)*pitch_3;
				double X4 = endPoiList[2] + delta_3;
				double X5 = X4 + (N3)*pitch_3;				
				
				double X6 = X2 + lengthSeg - 2 * lengthSeg/factorDivide;
				
				
				
				//Length Array
				double L1 = X2-X1;
				double L2 = X3-X2;
				double L3 = (N3)*pitch_3;
				double L4 = (N3)*pitch_3;
				double L5 = X6-X5;
				double L6 = L1;
				
				
				myReturnDic.Add(X1, L1);				
				myReturnDic.Add(X2, L2);
				myReturnDic.Add(X3, L3);
				myReturnDic.Add(X4, L4);				
				myReturnDic.Add(X5, L5);
				myReturnDic.Add(X6, L6);
				return myReturnDic;

			}
		
		}
		
		
		private List<ElementId> copyRebarByDistance_MaxSpace(Rebar myRebar, Dictionary<double, double> myDicDisNum)
		{
			
			List<double> myDistances = myDicDisNum.Keys.ToList();
			myDistances.Sort();
			
			List<double> myListNum = new List<double>();
			
			foreach (double key in myDistances) 
			{
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
				

				XYZ p = lineCur.GetEndPoint(0);
				XYZ q = lineCur.GetEndPoint(1);
				XYZ v = q - p;


				XYZ pE = p - 0.5*v;
				XYZ vE = q-pE;
				
				double lengCurLine = vE.GetLength();
				
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
			    
			    
    			XYZ v1 = pE - myReBarPlane.Origin;
			
    			double delta_0 = Math.Abs(myReBarPlane.Normal.DotProduct(v1));
				
				if(delta_0 == 10000000) return null;
				
				foreach (double distance in myDistances) 
				{
	
					XYZ myPointPlace = ((distance - delta_0)/lengCurLine)*vE;
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
					

								
		public void Rebar_FromCurve_Bot_Func(Element myBeam, List<Face> myListFacePicked,  double divideFac, double coverBar, 
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
			 
		 	double lengSeg = myListDisSortFace[myListDisSortFace.Count()-1] - myListDisSortFace[0];

		 	
		 	XYZ ePR1_XY = pE + ((myListDisSortFace[0] + lengSeg/divideFac)/vE.GetLength())*vE;
		 	XYZ ePR2_XY = pE + ((myListDisSortFace[myListDisSortFace.Count()-1] - lengSeg/divideFac)/vE.GetLength())*vE;
			 
		 	
		 	XYZ ePR1 = new XYZ(ePR1_XY.X, ePR1_XY.Y, myBoundBeam.Min.Z + coverBar);
		 	XYZ ePR2 = new XYZ(ePR2_XY.X, ePR2_XY.Y, myBoundBeam.Min.Z + coverBar);
		 	
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
//				Rebar myRebar = Rebar.CreateFromCurvesAndShape(doc, myRebarShape, myRebarType,
//				                                               null, null,
//				                                               myBeam,v1,
//				                                               myShape,
//				                                               RebarHookOrientation.Left,
//				                                               RebarHookOrientation.Left);
				Rebar myRebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, myRebarType,
				                                       null, null,
				                                       myBeam, vRB,
				                                       myShape,
				                                       RebarHookOrientation.Left, RebarHookOrientation.Right,
				                                       true, false);


				trans.Commit();
			}
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


		
		
        public void rebarStirrupLayout_Form_Test()
        {
			UIDocument uiDoc = this.ActiveUIDocument;
            Document doc = uiDoc.Document;


            // Thiết đặt các thông sô mặc định
            //Stirrup
            //True nếu xác nhận vẽ thép đai 1
            bool isSt1Yes = true;
            //True nếu xẩy ra
            bool isOpposite = false;

            //Chiều dài đoạn gia cường 1 (L/factor, factor = 4)
            double factor = 4;
            // Đoạn thụt vào so với mặt cột (50mm)
            double delta_1 = 50 / 304.8;
            //Bước đai đoạn đầu tiên
            double pitch_1 = 150 / 304.8;
            //Bước đai đoạn thứ 2
            double pitch_2 = 300 / 304.8;

            //Đoạn thụt vào so với mặt dầm phụ
            double delta_3 = 50 / 304.8;
            //Bước đai đoạn gần dầm phụ
            double pitch_3 = 100 / 304.8;
            //Số khoảng (số thanh thép) gia cường 2 bên dầm phụ
            int N3 = 4;


            //Stirrup C
            //True nếu xác nhận vẽ thép C
            bool isYes_C1 = true;
            //True nếu lỗi xẩy ra ngược hướng
            bool isOpposite_C1 = false;
            //hệ số chia khoảng thép đai C
            double factor_C1 = 4;
            // Đoạn thụt vào so với mặt cột (50mm)
            double delta_1_C1 = 50 / 304.8;
            //Bước đai đoạn đầu tiên
            double pitch_1_C1 = 150 / 304.8;
            //Bước đai đoạn thứ 2
            double pitch_2_C1 = 300 / 304.8;
            //Đoạn thụt vào so với mặt dầm phụ
            double delta_3_C1 = 50 / 304.8;
            //Bước đai đoạn gần dầm phụ
            double pitch_3_C1 = 100 / 304.8;
            //Số khoảng (số thanh thép) gia cường 2 bên dầm phụ
            int N3_C1 = 4;

            //Hệ số đổi đơn vị từ mm sang feet
            double myConvertFactor = 304.8;


            // Lọc tất cả các rebar shape và repbar type của project để import vào
            // Chọn để vẽ

            //Load rebarShape
            // RebarShape
            // Lọc và load tất cả các rebaerShape trong project
            FilteredElementCollector fecRebarShap = new FilteredElementCollector(doc)
            .OfClass(typeof(RebarShape));

            IEnumerable<RebarShape> iterRebarBarShapes = fecRebarShap.Cast<RebarShape>();


            // Load Rebar Type
            // Rebartype
            // Lọc và load tất cả các rebarType của project
            FilteredElementCollector fecBarType = new FilteredElementCollector(doc)
            .OfClass(typeof(RebarBarType));

            IEnumerable<RebarBarType> iterRebarBarTypes = fecBarType.Cast<RebarBarType>();

            // Các thông số để vẽ thép gia cường dưới(bụng)

            // Bottom Layer
            // Layer1
            // True để xác nhận vẽ thép gia cường bụng
            bool isLB1Yes = true;
            //Hệ  số cắt từ 2 đầu nhịp
            double FB1 = 6;
            // Khoảng cách từ mép dưới dầm lên lớp rebar thứ 1
            double CB1 = 80;//80mm

            // Hai biến lưu thông tin Rebarsahpe và rebartype của layerBottom 1
            string rebaNameShape_L1 = string.Empty;
            string rebaNameType_L1 = string.Empty;

            // Tương tự các thông số cho lớp thứ 2
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
                using (var myInputFormSetting = new SettingDialog())
                {
                    //add rebar Shape list
                    foreach (RebarShape myRebarShape in iterRebarBarShapes)
                    {
                        myInputFormSetting.RebarShap1_Cb.Items.Add(myRebarShape.Name);
                        myInputFormSetting.RebarShap2_Cb.Items.Add(myRebarShape.Name);

                    }

                    //myInputFormSetting.RebarShap1_Cb.SelectedIndex = 0;
                    //myInputFormSetting.RebarShap2_Cb.SelectedIndex = 0;

                    //add rebar type list	
                    foreach (RebarBarType myRebarType in iterRebarBarTypes)
                    {
                        myInputFormSetting.RebarType1_Cb.Items.Add(myRebarType.Name);
                        myInputFormSetting.RebarType2_Cb.Items.Add(myRebarType.Name);
                    }

                    //myInputFormSetting.RebarType1_Cb.SelectedIndex = 1;
                    //myInputFormSetting.RebarType2_Cb.SelectedIndex = 1;


                    myInputFormSetting.ShowDialog();

                    //stirrup
                    factor = Convert.ToDouble(myInputFormSetting.factorTb.Text);
                    delta_1 = Convert.ToDouble(myInputFormSetting.delta_1Tb.Text) / myConvertFactor;
                    pitch_1 = Convert.ToDouble(myInputFormSetting.pitch_1Tb.Text) / myConvertFactor;
                    pitch_2 = Convert.ToDouble(myInputFormSetting.pitch_2Tb.Text) / myConvertFactor;

                    delta_3 = Convert.ToDouble(myInputFormSetting.delta_3Tb.Text) / myConvertFactor;
                    pitch_3 = Convert.ToDouble(myInputFormSetting.pitch_3Tb.Text) / myConvertFactor;
                    N3 = Convert.ToInt32(myInputFormSetting.n3Tb.Text);
                    isOpposite = myInputFormSetting.oppDirChecked;
                    isSt1Yes = myInputFormSetting.yesST_Chb.Checked;


                    //C1
                    factor_C1 = Convert.ToDouble(myInputFormSetting.factor_C1Tb.Text);
                    delta_1_C1 = Convert.ToDouble(myInputFormSetting.delta_1_C1Tb.Text) / myConvertFactor;
                    pitch_1_C1 = Convert.ToDouble(myInputFormSetting.pitch_1_C1Tb.Text) / myConvertFactor;
                    pitch_2_C1 = Convert.ToDouble(myInputFormSetting.pitch_2_C1Tb.Text) / myConvertFactor;

                    delta_3_C1 = Convert.ToDouble(myInputFormSetting.delta_3_C1Tb.Text) / myConvertFactor;
                    pitch_3_C1 = Convert.ToDouble(myInputFormSetting.pitch_3_C1Tb.Text) / myConvertFactor;
                    N3_C1 = Convert.ToInt32(myInputFormSetting.n3_C1Tb.Text);
                    isOpposite_C1 = myInputFormSetting.oppDirChecked;
                    isYes_C1 = myInputFormSetting.yesC1_Chb.Checked;


                    //Shape
                    rebaNameShape_L1 = myInputFormSetting.RebarShap1_Cb.Text;
                    rebaNameShape_L2 = myInputFormSetting.RebarShap2_Cb.Text;

                    //type
                    rebaNameType_L1 = myInputFormSetting.RebarType1_Cb.Text;
                    rebaNameType_L2 = myInputFormSetting.RebarType2_Cb.Text;


                    //Yes/ No bottom rebar
                    if (!myInputFormSetting.yes1_Chb.Checked) { isLB1Yes = false; }
                    if (!myInputFormSetting.yes2_Chb.Checked) { isLB2Yes = false; }

                    // Layer1

                    FB1 = Convert.ToDouble(myInputFormSetting.FB1_Tb.Text);
                    CB1 = Convert.ToDouble(myInputFormSetting.CB1_Tb.Text) / myConvertFactor;


                    // Bottom Layer
                    // Layer2

                    FB2 = Convert.ToDouble(myInputFormSetting.FB2_Tb.Text);
                    CB2 = Convert.ToDouble(myInputFormSetting.CB2_Tb.Text) / myConvertFactor;



                    //if the user hits cancel just drop out of macro
                    if (myInputFormSetting.DialogResult == System.Windows.Forms.DialogResult.Cancel) return;
                    {
                        //else do all this :)    
                        myInputFormSetting.Close();
                    }

                    if (myInputFormSetting.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        //else do all this :) 
                        inputSuccess = true;
                        myInputFormSetting.Close();
                    }

                }

            }


            //Set mybeam
            Element myBeam = null;

            // Kiem tra ve stirrup

            List<Rebar> myListRebar = new List<Rebar>();
            List<Rebar> myListRebar_C1 = new List<Rebar>();

            if (isSt1Yes)
            {
                // Pick Rebar
                List<int> myListIdCategoryRebar = new List<int>();
                myListIdCategoryRebar.Add((int)BuiltInCategory.OST_Rebar);

                // Select first Element (ex beam)
                List<Reference> myListRefRebar = uiDoc.Selection.PickObjects(ObjectType.Element,
                                                                  new FilterByIdCategory(myListIdCategoryRebar),
                                                                  "Pick a stirrup Rebar...") as List<Reference>;


                foreach (Reference myRefRebar in myListRefRebar)
                {
                    Rebar myRebarpicked = doc.GetElement(myRefRebar) as Rebar;
                    myListRebar.Add(myRebarpicked);
                }
                myBeam = doc.GetElement(myListRebar[0].GetHostId());
            }

            if (isYes_C1)
            {
                // Pick Rebar
                List<int> myListIdCategoryRebar_C1 = new List<int>();
                myListIdCategoryRebar_C1.Add((int)BuiltInCategory.OST_Rebar);

                // Select first Element (ex beam)
                List<Reference> myListRefRebar_C1 = uiDoc.Selection.PickObjects(ObjectType.Element,
                                                                  new FilterByIdCategory(myListIdCategoryRebar_C1),
                                                                  "Pick a C Rebar ...") as List<Reference>;


                foreach (Reference myRefRebar_C1 in myListRefRebar_C1)
                {
                    Rebar myRebarpicked_C1 = doc.GetElement(myRefRebar_C1) as Rebar;
                    myListRebar_C1.Add(myRebarpicked_C1);
                }
                myBeam = doc.GetElement(myListRebar_C1[0].GetHostId());
            }

            if(myBeam==null)
            {
                List<int> myListIdCategoryRebar = new List<int>();
                myListIdCategoryRebar.Add((int)BuiltInCategory.OST_StructuralFraming);
                Reference myRefmyBeam = uiDoc.Selection.PickObject(ObjectType.Element,
                                                                  new FilterByIdCategory(myListIdCategoryRebar),
                                                                  "Pick a Beam...");

                myBeam = doc.GetElement(myRefmyBeam);
            }

            //TODO: Các bước tính toán hình học của dầm

            //Get location curve of beam
            LocationCurve lc = myBeam.Location as LocationCurve;
            Line line = lc.Curve as Line;

            //Get vector of location cuver beam
            XYZ p = line.GetEndPoint(0);
            XYZ q = line.GetEndPoint(1);
            XYZ v = q - p; // Vector equation of line

            XYZ pE = p - 0.5 * v;
            XYZ vE = q - p;

            //Set current Beam be Joined

            //setBeJoined(myBeam);

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
                if (myListFacePicked.Count != 2 && myListFacePicked.Count != 4)
                {
                    TaskDialog.Show("Error!", "Chua ho tro lua chon: " + myListFacePicked.Count() + " mat, Chon 2 hoac 4 mat");
                    continue;

                }

                #endregion

                else
                {
                    string caseDistributionRebar = "Case 2: 4 Faces";

                    List<double> myListSpace = new List<double>() { pitch_1, pitch_2, pitch_3, pitch_3, pitch_2, pitch_1 };
                    List<double> myListSpace_C1 = new List<double>() { pitch_1_C1, pitch_2_C1, pitch_3_C1, pitch_3_C1, pitch_2_C1, pitch_1_C1 };
                    if (myListFacePicked.Count == 2)
                    {
                        myListSpace = new List<double>() { pitch_1, pitch_2, pitch_1, pitch_2, pitch_2, pitch_1 };
                        myListSpace_C1 = new List<double>() { pitch_1_C1, pitch_2_C1, pitch_1_C1, pitch_2_C1, pitch_2_C1, pitch_1_C1 };
                        caseDistributionRebar = "Case 1: 2 Faces";

                    }
                    TaskDialog.Show("Info", caseDistributionRebar);

                    if (isSt1Yes)
                    {

                        #region Rebar stirrup

                        // List of boundaries faces
                        List<double> myListEndPointDis = getAndSortDisOfEndFaces(myListFacePicked, pE);
                        myListEndPointDis.Sort();

                        Dictionary<double, double> myDicDisNumDetail = detailListDistance_Update_MaxSpace(myListEndPointDis,
                                                                                        factor,
                                                                                        delta_1, pitch_1,
                                                                                        pitch_2,
                                                                                        delta_3, pitch_3, N3);

                        foreach (Rebar myRebar in myListRebar)
                        {
                            List<ElementId> myListRebarCopyId = copyRebarByDistance_MaxSpace(myRebar, myDicDisNumDetail);


                            List<double> myDistances = myDicDisNumDetail.Keys.ToList();
                            myDistances.Sort();

                            List<double> myListNum = new List<double>();

                            foreach (double key in myDistances)
                            {
                                myListNum.Add(myDicDisNumDetail[key]);
                            }


                            // using transcation (edit DB)
                            for (int i = 0; i < myListRebarCopyId.Count(); i++)
                            {
                                using (Transaction myTrans = new Transaction(doc, "CopyElementByCoordinate"))

                                {
                                    myTrans.Start();
                                    ElementId rebarId = myListRebarCopyId[i];
                                    Rebar myRebarI = doc.GetElement(rebarId) as Rebar;

                                    if (i == 1 || i == 4)
                                    {
                                        if (myListNum[i] / myListSpace[i] < 1)
                                        {
                                            myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
                                        }

                                        else
                                        {
                                            myRebarI.GetShapeDrivenAccessor().SetLayoutAsMaximumSpacing(myListSpace[i], (myListNum[i]), isOpposite, false, false);

                                        }
                                    }

                                    else
                                    {
                                        if (myListNum[i] / myListSpace[i] < 1)
                                        {
                                            myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
                                        }

                                        else
                                        {
                                            myRebarI.GetShapeDrivenAccessor().SetLayoutAsMaximumSpacing(myListSpace[i], (myListNum[i]), isOpposite, true, true);
                                        }
                                    }

                                    myTrans.Commit();
                                }

                            }

                        }
                        #endregion
                    }


                    if (isYes_C1)
                    {

                        #region Rebar C stirrup

                        // List of boundaries faces
                        List<double> myListEndPointDis_C1 = getAndSortDisOfEndFaces(myListFacePicked, pE);
                        myListEndPointDis_C1.Sort();

                        Dictionary<double, double> myDicDisNumDetail_C1 = detailListDistance_Update_MaxSpace(myListEndPointDis_C1,
                                                                                        factor_C1,
                                                                                        delta_1_C1, pitch_1_C1,
                                                                                        pitch_2_C1,
                                                                                        delta_3_C1, pitch_3_C1, N3_C1);

                        foreach (Rebar myRebar in myListRebar_C1)
                        {
                            List<ElementId> myListRebarCopyId_C1 = copyRebarByDistance_MaxSpace(myRebar, myDicDisNumDetail_C1);


                            List<double> myDistances_C1 = myDicDisNumDetail_C1.Keys.ToList();
                            myDistances_C1.Sort();

                            List<double> myListNum_C1 = new List<double>();

                            foreach (double key in myDistances_C1)
                            {
                                myListNum_C1.Add(myDicDisNumDetail_C1[key]);
                            }


                            // using transcation (edit DB)
                            for (int i = 0; i < myListRebarCopyId_C1.Count(); i++)
                            {
                                using (Transaction myTrans = new Transaction(doc, "CopyElementByCoordinate"))

                                {
                                    myTrans.Start();
                                    ElementId rebarId = myListRebarCopyId_C1[i];
                                    Rebar myRebarI = doc.GetElement(rebarId) as Rebar;

                                    if (i == 1 || i == 4)
                                    {
                                        if (myListNum_C1[i] / myListSpace_C1[i] < 1)
                                        {
                                            myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
                                        }

                                        else
                                        {
                                            myRebarI.GetShapeDrivenAccessor().SetLayoutAsMaximumSpacing(myListSpace_C1[i], (myListNum_C1[i]), isOpposite, false, false);

                                        }
                                    }

                                    else
                                    {
                                        if (myListNum_C1[i] / myListSpace_C1[i] < 1)
                                        {
                                            myRebarI.GetShapeDrivenAccessor().SetLayoutAsSingle();
                                        }

                                        else
                                        {
                                            myRebarI.GetShapeDrivenAccessor().SetLayoutAsMaximumSpacing(myListSpace_C1[i], (myListNum_C1[i]), isOpposite, true, true);
                                        }
                                    }

                                    myTrans.Commit();
                                }

                            }

                        }
                        #endregion
                    }


                    if (isLB1Yes)
                    {
                        Rebar_FromCurve_Bot_Func(myBeam, myListFacePicked, FB1, CB1,
                                                     rebaNameShape_L1, rebaNameType_L1);
                    }
                    if (isLB2Yes)
                    {
                        Rebar_FromCurve_Bot_Func(myBeam, myListFacePicked, FB2, CB2,
                                                     rebaNameShape_L2, rebaNameType_L2);
                    }
                }
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