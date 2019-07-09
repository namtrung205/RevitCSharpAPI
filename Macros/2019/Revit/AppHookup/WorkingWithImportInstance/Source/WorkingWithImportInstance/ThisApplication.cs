/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 4/17/2019
 * Time: 2:44 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WorkingWithImportInstance
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("FE1D0B39-03AC-45FA-AE78-979887E10A82")]
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
		
		public void getBlock()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			

			
			Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element, "Select an Import Instance");
			ImportInstance myImportInstance = doc.GetElement(myRef) as ImportInstance ;
			Element e = doc.GetElement(myRef) ;
			
			XYZ originInstance = myImportInstance.GetTransform().Origin;
			
			GeometryElement myGeoElem = e.get_Geometry(new Options());
			foreach (GeometryObject GeoOj in myGeoElem) 
			{
				GeometryInstance instance = GeoOj as GeometryInstance;
				if(instance!= null)
				{
					foreach( GeometryObject instObj in instance.SymbolGeometry )
			        {
						if (instObj is GeometryInstance)
						{
							GeometryInstance blockInstance = instObj as GeometryInstance;
							
							string name = blockInstance.Symbol.Name;
							
							if (  name == "test.dwg.*U26") 
							{
	                            Transform transform = blockInstance.Transform;
	
	                            XYZ origin = transform.Origin;
	
	                            XYZ vectorTran = transform.OfVector(transform.BasisX.Normalize());
	                            
	                            double rot = transform.BasisX.AngleOnPlaneTo(vectorTran, transform.BasisZ.Normalize()); // radians
	                            
	                            rot = rot * (180 / Math.PI); // degrees
							}
						}
					}
				}
	
	  		}
			//TaskDialog.Show("GeometryInstance Symbol Geometry", "Curve Count: "  +  curveCounter +  "polylineCount: " + polylineCounter);

		}
	

		
		public void placeFamilyByBlock()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
						
			//Select ImportInstance
			Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element, "Select Import DWG...");
			Element dwgImportElement = doc.GetElement(myRef);
			

            string myTextClipBoard = Clipboard.GetText();


            Dictionary<XYZ, double> myDicCoorAndRot = getOriginAndRotByBlock(dwgImportElement, myTextClipBoard);
            if (myDicCoorAndRot.Keys.Count < 1)
            {
                TaskDialog.Show("Error!!", "CLipboard không có dữ liệu, hoặc file .DWG không chứa block có tên như trong clipboard");
                return;
            }
			
						
			Reference myRefFamily = uiDoc.Selection.PickObject(ObjectType.Element, "Select Instance Family...");
			ImportInstance myImportInstance = doc.GetElement(myRef) as ImportInstance ;
			Element myFamilyElement = doc.GetElement(myRefFamily);


			XYZ originInstance = myImportInstance.GetTransform().Origin;
			
			LocationCurve locCurve = myFamilyElement.Location as LocationCurve;
			if (null == locCurve)
			{
				XYZ pointRef = ((LocationPoint)myFamilyElement.Location).Point;

		
				XYZ deltaXYZ = new XYZ();
				List<ElementId> myElemIdCopiedColTotal = new List<ElementId>();
				
				foreach (XYZ myXYZ in myDicCoorAndRot.Keys) 
				{
					List<ElementId> myElemIdCopiedCol = new List<ElementId>();
					
					//Copy Element
					using (Transaction myTrans = new Transaction(doc, "Copy Element") )
					{
						myTrans.Start();
						deltaXYZ = originInstance + myXYZ - pointRef; 
						myElemIdCopiedCol = ElementTransformUtils.CopyElement(doc, myFamilyElement.Id, deltaXYZ).ToList();
						myTrans.Commit();
					}
					

						

					using (Transaction myTrans = new Transaction(doc, "RotateElement Location Point") )
					{
						foreach (ElementId myIdEleCopied in myElemIdCopiedCol) 
						{
						myTrans.Start();
						// Code here
						//ElementTransformUtils.RotateElement(doc, myEle.Id, axis, DegreesToRadians(degrees));
						Element myElemCopied = doc.GetElement(myIdEleCopied);
						XYZ point = ((LocationPoint)myElemCopied.Location).Point;
						XYZ point2 = point.Add(XYZ.BasisZ);
		
						Line axis = Line.CreateBound(point, point2);
							
						ElementTransformUtils.RotateElement(doc, myIdEleCopied, axis, myDicCoorAndRot[myXYZ]);
						
						myElemIdCopiedColTotal.Add(myIdEleCopied);

					}
					myTrans.Commit();
					}
				}
				
				
                // Make group From element Cp=opied
                using (Transaction trans = new Transaction(doc, "Make group from Copied Element"))
                {
                    trans.Start();
                    if (myElemIdCopiedColTotal.Count > 0)
                    {
                        Group myGroupRebar = doc.Create.NewGroup(myElemIdCopiedColTotal);
                        //	myGroupRebar.GroupType.Name = rebarGroupName;

                    }
                    else
                    {
                        TaskDialog.Show("Warning!", "No rebar was hosted by this element, so no any group was created!");
                    }
                    trans.Commit();
                }
			}
		}
		
	
		
		public void placeFamilyByBlockOneAngle()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
						
			//Select ImportInstance
			Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element, "Select Import DWG...");
			Element dwgImportElement = doc.GetElement(myRef);
			

            string myTextClipBoard = Clipboard.GetText();


            Dictionary<XYZ, double> myDicCoorAndRot = getOriginAndRotByBlock(dwgImportElement, myTextClipBoard);
            if (myDicCoorAndRot.Keys.Count < 1)
            {
                TaskDialog.Show("Error!!", "CLipboard không có dữ liệu, hoặc file .DWG không chứa block có tên như trong clipboard");
                return;
            }
			
						
			Reference myRefFamily = uiDoc.Selection.PickObject(ObjectType.Element, "Select Instance Family...");
			ImportInstance myImportInstance = doc.GetElement(myRef) as ImportInstance ;
			Element myFamilyElement = doc.GetElement(myRefFamily);
			
			LocationPoint myLocPoint = myFamilyElement.Location as LocationPoint;
			double myAngleFamily = myLocPoint.Rotation;
			


			XYZ originInstance = myImportInstance.GetTransform().Origin;
			
			LocationCurve locCurve = myFamilyElement.Location as LocationCurve;
			if (null == locCurve)
			{
				XYZ pointRef = ((LocationPoint)myFamilyElement.Location).Point;

		
				XYZ deltaXYZ = new XYZ();
				List<ElementId> myElemIdCopiedColTotal = new List<ElementId>();
				
				foreach (XYZ myXYZ in myDicCoorAndRot.Keys) 
				{
					List<ElementId> myElemIdCopiedCol = new List<ElementId>();
					
					//Copy Element
					
					if(Math.Round(myAngleFamily,3) == Math.Round(myDicCoorAndRot[myXYZ], 3))
                    {
						using (Transaction myTrans = new Transaction(doc, "Copy Element") )
						{
							myTrans.Start();
							deltaXYZ = originInstance + myXYZ - pointRef; 
							myElemIdCopiedCol = ElementTransformUtils.CopyElement(doc, myFamilyElement.Id, deltaXYZ).ToList();
							myTrans.Commit();
						}
                    }

					foreach (ElementId myIdEleCopied in myElemIdCopiedCol) 
					{
						myElemIdCopiedColTotal.Add(myIdEleCopied);
					}
				}
				
				
                // Make group From element Cp=opied
                using (Transaction trans = new Transaction(doc, "Make group from Copied Element"))
                {
                    trans.Start();
                    if (myElemIdCopiedColTotal.Count > 0)
                    {
                        Group myGroupRebar = doc.Create.NewGroup(myElemIdCopiedColTotal);
                        //	myGroupRebar.GroupType.Name = rebarGroupName;

                    }
                    else
                    {
                        TaskDialog.Show("Warning!", "No rebar was hosted by this element, so no any group was created!");
                    }
                    trans.Commit();
                }
			}
		}
		
	

		
		private Dictionary<XYZ, double> getOriginAndRotByBlock(Element myDwgImportElement, string nameOfBlock)
		{
			
			Dictionary<XYZ, double> myReturnDic = new Dictionary<XYZ, double>();
			GeometryElement myGeoElem = myDwgImportElement.get_Geometry(new Options());
			foreach (GeometryObject GeoOj in myGeoElem) 
			{
				GeometryInstance instance = GeoOj as GeometryInstance;
				if(instance!= null)
				{
					foreach( GeometryObject instObj in instance.SymbolGeometry )
			        {
						if (instObj is GeometryInstance)
						{
							GeometryInstance blockInstance = instObj as GeometryInstance;
							
							string name = blockInstance.Symbol.Name;
							
							if (name == nameOfBlock) 
							{
	                            Transform transform = blockInstance.Transform;
	
	                            XYZ origin = transform.Origin;
	                            if(!myReturnDic.Keys.Contains(origin))
	                            {
		                            XYZ vectorTran = transform.OfVector(transform.BasisX.Normalize());
		                            double rot = transform.BasisX.AngleOnPlaneTo(vectorTran, transform.BasisZ.Normalize()); // radians
		                            //rot = rot * (180 / Math.PI); // degrees
		                            
		                            myReturnDic.Add(origin, rot);
	                            }
							}
						}
					}
				}
	
	  		}
			return myReturnDic;
		}
	
			
		private string XYZtoString(XYZ myXYZ)
		{
			return string.Format("X: {0}, Y: {1}, Z: {2}", Math.Round(myXYZ.X,2), Math.Round(myXYZ.Y, 2), Math.Round(myXYZ.Z,2));
		
		}
	
	}
}
