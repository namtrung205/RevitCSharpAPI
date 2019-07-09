/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 6/4/2019
 * Time: 10:36 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace WorkingWithApplication
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("EBE4E4AB-D879-4988-A3E0-5D9641036F0D")]
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
	
		static View FindElevationView(Document doc, ICollection<ElementId> ids)
		{
			View view = null;
			
			foreach (ElementId id in ids) 
			{
				view = doc.GetElement(id) as View;
				
				if(view !=null && ViewType.Elevation == view.ViewType)
				{break;}
				
				view = null;
				
			}
			return view;
		}
		
		
		
		static void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
		{
			Document doc = e.GetDocument();
			View view = FindElevationView(doc, e.GetAddedElementIds());
			
			if(null != view)
			{
				string msg = string.Format("You just create an" 
				                           + "elevation view '{0}'. Are you"
				                           + "sure you want to do that? "
				                           + "Elevations don't show hidden line"
				                           + "detail, which makes them unsuitable"
				                           + "for core wall elevations etc.",
				                           view.Name);
				TaskDialog.Show("ElevationChecker", msg);
			}
		}
		
		
		public void DocumentChangeEvent_Test()
		{
			this.Application.DocumentChanged+= new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);	
			return;
		}
	}
}