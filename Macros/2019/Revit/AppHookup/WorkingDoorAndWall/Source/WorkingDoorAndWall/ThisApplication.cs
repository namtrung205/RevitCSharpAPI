/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 6/27/2019
 * Time: 4:23 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace WorkingDoorAndWall
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("F8EF6040-9CCE-4937-B4C4-039D8079E57F")]
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
	
	public void DoorWallOffset()
{
    UIDocument uidoc = this.ActiveUIDocument;
    Document doc = uidoc.Document;

    // establish an offset of 20cm that will be applied to the door and wall
    // Revit's internal units for distance is feet
    double doorToWallDistance = 20 / (12 * 2.54); // convert 20 cm to 1 ft
    
    using (InputForm myInputDisForm = new InputForm())
    {
    	myInputDisForm.ShowDialog();
    	
		//if the user hits cancel just drop out of macro
	    if(myInputDisForm.DialogResult == System.Windows.Forms.DialogResult.Cancel) return;
	    {
	    	//else do all this :)    
	    	myInputDisForm.Close();
	    }
	    
	    if(myInputDisForm.DialogResult == System.Windows.Forms.DialogResult.OK)
	    {
	    	//else do all this :) 
	    	doorToWallDistance = Convert.ToDouble(myInputDisForm.distance_tb.Text)/ (12 * 25.4);
	    	myInputDisForm.Close();
	    }
    }

    try // try/catch block with PickObject is used to give the user a way to get out of the infinite loop of while (true)
        // PickObject throws an exception when user aborts from selection
    {
        while (true)
        {
			List<int> myListIdCategoryDoor= new List<int>();
			myListIdCategoryDoor.Add((int)BuiltInCategory.OST_Doors);
			myListIdCategoryDoor.Add((int)BuiltInCategory.OST_Windows);

        	
			FamilyInstance door = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element,new FilterByIdCategory(myListIdCategoryDoor),
        	                                                                "Select a door or window. ESC when finished.").ElementId) as FamilyInstance;
            double doorWidth = door.Symbol.get_Parameter(BuiltInParameter.DOOR_WIDTH).AsDouble();

            
			List<int> myListIdCategoryWall= new List<int>();
			myListIdCategoryWall.Add((int)BuiltInCategory.OST_Walls);
            
            Wall sideWall = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element,new FilterByIdCategory(myListIdCategoryWall),
                                                                      "Select a wall. ESC when finished.").ElementId) as Wall;
            double sideWallWidth = sideWall.Width;

            // get the curve that defines the centerline of the side wall
            LocationCurve sideWallLocationCurve = sideWall.Location as LocationCurve;
            Curve sideWallCurve = sideWallLocationCurve.Curve;

            // get the curve that defines the centerline of the wall that hosts the door
            Wall hostWall = door.Host as Wall;
            LocationCurve hostWallLocationCurve = hostWall.Location as LocationCurve;
            Curve hostWallCurve = hostWallLocationCurve.Curve;

            // find the point of intersection of these two wall curves (intersectionXYZ)
            // and the position of this point on the wall that hosts the door (intersectionParam)
            IntersectionResultArray ira = new IntersectionResultArray();
            SetComparisonResult scr = sideWallCurve.Intersect(hostWallCurve, out ira);
            var iter = ira.GetEnumerator();
            iter.MoveNext();
            IntersectionResult ir = iter.Current as IntersectionResult;
            XYZ intersectionXYZ = ir.XYZPoint;
            double intersectionParam = hostWallCurve.Project(intersectionXYZ).Parameter;

            // find the position of the door in its host wall
            LocationPoint doorPoint = door.Location as LocationPoint;
            XYZ doorXYZ = doorPoint.Point;
            double doorParam = hostWallCurve.Project(doorXYZ).Parameter;

            // compute the translation vector between the edge of the door closest to the wall and the side face of the wall
            XYZ translation = null;
            XYZ doorEdgeXYZ = null;
            XYZ intersectionOffsetXYZ = null;
            if (intersectionParam > doorParam)
            {
                intersectionOffsetXYZ = hostWallCurve.Evaluate(intersectionParam - doorToWallDistance - sideWallWidth/2, false);
                doorEdgeXYZ = hostWallCurve.Evaluate(doorParam + doorWidth/2, false);
                translation = intersectionOffsetXYZ.Subtract(doorEdgeXYZ);
            }
            else
            {
                intersectionOffsetXYZ = hostWallCurve.Evaluate(intersectionParam + doorToWallDistance + sideWallWidth/2, false);
                doorEdgeXYZ = hostWallCurve.Evaluate(doorParam - doorWidth/2, false);
                translation = doorEdgeXYZ.Subtract(intersectionOffsetXYZ).Negate();
            }

            // Move the door
            using (Transaction t = new Transaction(doc,"move door"))
            {
                t.Start();
                ElementTransformUtils.MoveElement(doc,door.Id,translation);
                t.Commit();
            }
        }
    }
    catch{}
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