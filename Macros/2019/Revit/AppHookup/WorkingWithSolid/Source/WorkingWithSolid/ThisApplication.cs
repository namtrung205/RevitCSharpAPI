/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 4/6/2019
 * Time: 12:23 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace WorkingWithSolid
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("DEDB22E8-ADE0-4429-829D-EB5DF79628B6")]
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
		public void getInfor()
		{
			// set active document
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			// prompt select face 
			
			Reference myRef = uiDoc.Selection.PickObject(ObjectType.Subelement);
			
			// Get element by pickface
			Element e = doc.GetElement(myRef) as Element;
			
			Level myLevel = doc.GetElement(e.LevelId) as Level;
			
			
			//Get GeoObject from element;
			GeometryObject myGeoObj =  e.GetGeometryObjectFromReference(myRef) as Solid;
			
			
		}
	}
}