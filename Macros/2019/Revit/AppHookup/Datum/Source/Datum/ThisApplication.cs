/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 3/12/2019
 * Time: 3:24 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace Datum
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("08F34E29-38CE-4981-8B3C-952B02FE4A67")]
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
		
		
		
		public void createLevel()
		{
			// Set active document
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			using (Transaction trans = new Transaction(doc,"add level"))
			{
				trans.Start();
				
				foreach (var i in Enumerable.Range(1,5))
	         	{				
					Level newLevel = Level.Create(doc,(i+1)*2000/304.8);
					newLevel.Name = "Test" + i.ToString();
				}

				trans.Commit();
			}
			return;		
		}
		
				
		public void createGrid()
		{
			// Set active document
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			using (Transaction trans = new Transaction(doc,"add level"))
			{
				trans.Start();
				
				foreach (var i in Enumerable.Range(1,2))
	         	{		
				// Tao luoi X
					Line myLine2GridX = Line.CreateBound(new XYZ(0, 0, 0),  new XYZ(100, 0, 0));
					Grid newGridX = Grid.Create(doc, myLine2GridX);

				// Tao luoi Y
//					Line myLine2GridY = Line.CreateBound(new XYZ(i*5000/304.8,0,0),  new XYZ(i*5000/304.8, 100,0));
//					Grid newGridY = Grid.Create(doc, myLine2GridY);				
				}

				trans.Commit();
			}
			return;		
		}
	}
}