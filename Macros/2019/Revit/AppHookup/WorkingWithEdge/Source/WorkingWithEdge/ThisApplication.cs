/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 3/18/2019
 * Time: 3:07 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace WorkingWithEdge
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("A397288D-D55B-4D20-8F03-A0397B51B24E")]
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
		public void getInfoEdge()
		{
			// set active document
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			// prompt select face 
			
			Reference myRef = uiDoc.Selection.PickObject(ObjectType.Edge);
			
			// Get element by pickface
			Element e = doc.GetElement(myRef) as Element;
			

			//Get GeoObject from element;
			GeometryObject myGeoObj =  e.GetGeometryObjectFromReference(myRef);
			
			//Get face from element Object:
			Edge myPickedEdge = myGeoObj as Edge;
				
		}
		
		public void getInfoPoint()
		{
			// set active document
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			// prompt select face 
			
			XYZ myRef = uiDoc.Selection.PickPoint(ObjectSnapTypes.Endpoints);
			
			TaskDialog.Show("Abc", "def: " + XYZtoString(myRef));
				
		}
		
		private string XYZtoString(XYZ myXYZ)
		{
			return string.Format("X: {0}, Y: {1}, Z: {2}", Math.Round(myXYZ.X,2), Math.Round(myXYZ.Y, 2), Math.Round(myXYZ.Z,2));
		
		}
		

	}
}