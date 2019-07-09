/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 4/27/2019
 * Time: 4:04 PM
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

namespace WorkingWithAreaSys
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("7C2A3064-F069-4181-9398-06525FBA7242")]
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
	
		public void testRebarSys()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			
			// Load rebar Shape
			FilteredElementCollector fecRebarShap = new FilteredElementCollector(doc)
			.OfClass(typeof(RebarShape));
			
			IEnumerable<RebarShape> iterRebarBarShapes =  fecRebarShap.Cast<RebarShape>();
			
			RebarShape myRebarShape = iterRebarBarShapes.FirstOrDefault<RebarShape>();
			ElementId myRebarShapeId = myRebarShape.Id;

			
			// Select first Element (ex RebarSystem)
			Reference myRefRebar = uiDoc.Selection.PickObject(ObjectType.Element,
			                                                  "Pick a Rebar...");
			Element mySysRebar = doc.GetElement(myRefRebar);
			RebarInSystem myRebarInSys = mySysRebar as RebarInSystem;

			 List<Subelement> myList = myRebarInSys.GetSubelements() as List<Subelement>;
			
			 
			 
			 TaskDialog.Show("abc", "xyz" + myList.Count);
			
		
		}
	
	
	}
}