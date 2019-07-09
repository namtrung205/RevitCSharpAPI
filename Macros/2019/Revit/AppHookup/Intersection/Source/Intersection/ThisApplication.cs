/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 3/15/2019
 * Time: 7:23 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace Intersection
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("0E9D0EC8-FBD0-491E-B448-906817AE4A88")]
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
		public void Test()
		{
			
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			// Find intersections between family instances and a selected element
			Reference reference = uidoc.Selection.PickObject(ObjectType.Element, "Select element that will be checked for intersection with all family instances");
			Element element = doc.GetElement(reference);
			GeometryElement geomElement = element.get_Geometry(new Options());
			Solid solid = null;
			foreach (GeometryObject geomObj in geomElement)
			{
			    solid = geomObj as Solid;
			    if (solid != null) break;
			}
			
			
			

			
			FilteredElementCollector collector = new FilteredElementCollector(doc);
			collector.WherePasses(new ElementIntersectsSolidFilter(solid)); // Apply intersection filter to find matches
			
			//TaskDialog.Show("Revit", collector.Count() + " family instances intersect with the selected element (" + element.Category.Name + " id:" + element.Id.ToString() + ")");

			List<ElementId> myListInter = new List<ElementId>();
			foreach (Element myE in collector) {
				if(myE.Id != element.Id)
				{
					myListInter.Add(myE.Id);
				}
			}
			
			
//			uidoc.Selection.SetElementIds(myListInter);
			
			// Joint
			
			try {
				
				using (Transaction trans = new Transaction(doc, "Join Floor")) 
				{
					trans.Start();
					foreach (ElementId myEId in myListInter) 
					{
						if(!JoinGeometryUtils.AreElementsJoined(doc, doc.GetElement(myEId), element))
						{
							JoinGeometryUtils.JoinGeometry(doc, doc.GetElement(myEId), element);
							//JoinGeometryUtils.UnjoinGeometry(doc, doc.GetElement(myEId), element);
						}
					
					}
					trans.Commit();
				}
			} catch (Exception) {
				
				throw;
			}
			
			
			
		}
		
	}
	

	
}