/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 3/12/2019
 * Time: 5:22 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace WorkWithRoom
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("28BC9638-76D0-4DA8-8891-2B36923C17BB")]
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
		public void getInforRoom()
		{
			// Set active document
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			using (Transaction trans = new Transaction(doc,"add level"))
			{
				trans.Start();
							
				Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element);
				ElementId MyElementId = uiDoc.Selection.PickObject(ObjectType.Element).ElementId;
				
				Room r = doc.GetElement(myRef) as Room;
				
				Getinfo_Room(r);
				
				trans.Commit();
			}
		}
		
		
		private void Getinfo_Room(Room room)
		{
		    string message = "Room: ";
		
		    //get the name of the room
		    message += "\nRoom Name: " + room.Name;
		
		    //get the room position
		    LocationPoint location = room.Location as LocationPoint;
		    XYZ point = location.Point;
		    message += "\nRoom position: " + XYZToString(point);
		
		    //get the room number
		    message += "\nRoom number: " + room.Number;
		
		    IList<IList<Autodesk.Revit.DB.BoundarySegment>> segments = room.GetBoundarySegments(new SpatialElementBoundaryOptions());
		    if (null != segments)  //the room may not be bound
		    {
		    	TaskDialog.Show("Segment Info", segments[0].Count().ToString());
		    }
		
		}
		
		
		// output the point's three coordinates
		string XYZToString(XYZ point)
		{
		    return "(" + point.X + ", " + point.Y + ", " + point.Z + ")";
		}
		
		
	}
}