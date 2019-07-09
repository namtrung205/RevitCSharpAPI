/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 4/1/2019
 * Time: 10:48 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace WorkingWithSelection
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("CBCDF8C8-4494-410D-9035-EA8788511A52")]
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
	
		public void getElementSelected()
		{

			
			try
            {
                // Select some elements in Revit before invoking this command

                // Get the handle of current document.
    			UIDocument uiDoc = this.ActiveUIDocument;
				Document doc = uiDoc.Document;
                
                // Get the element selection of current document.
                Selection selection = uiDoc.Selection;
                ICollection<ElementId> selectedIds = uiDoc.Selection.GetElementIds();

                if (0 == selectedIds.Count)
                {
                    // If no elements selected.
                    TaskDialog.Show("Revit","You haven't selected any elements.");
                }
                else
                {
                    String info = "Ids of selected elements in the document are: ";
                    foreach (ElementId id in selectedIds)
                    {
                       info += "\n\t" + id.IntegerValue;
                    }

                    TaskDialog.Show("Revit",info);
                }
            }
            catch (Exception e)
            {
				
            }

		
		
		}
	
	
	}
}