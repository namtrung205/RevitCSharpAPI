/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 3/11/2019
 * Time: 4:56 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace Dimension
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("81C8F591-593B-4E82-B7EA-7CD8F3417AE4")]
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

			XYZ2 xyz_1 = new XYZ2(0, 0, 0);
			XYZ2 xyz_2 = new XYZ2(0, 0, 1);
			XYZ2 xyz_3 = new XYZ2(0, 1, 0);
			
			XYZ a = new XYZ(0,1.0,0);
			
			TaskDialog.Show("xyz", (a.IsAlmostEqualTo(xyz_3)).ToString());

			List<XYZ2> myListXYZ = new List<XYZ2>(){xyz_1, xyz_2, xyz_3};
			
			XYZ2 xyz_test = new XYZ2(0,0,0);
			
			if(xyz_1 == xyz_test)
		   {
				TaskDialog.Show("abc", "Yes");
		   
		   }
			
			if(myListXYZ.Contains(xyz_test))
			{
				
				TaskDialog.Show("abc", "Yes");
			}
			else
			{
				TaskDialog.Show("abc", "No");			
				
			}

		}
	}
	
	
	public class XYZ2: XYZ
	{
		public XYZ2(double x, double y, double z):base(x,y,z)
		{
		
		}
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			XYZ2 other = obj as XYZ2;
			if (other == null)
				return false;
			else
			{
				if(this.X == other.X && this.Y == other.Y && this.Z == other.Z)
				{
					return true;
				}
				
				return false;
			}
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			return hashCode;
		}
		
		public static bool operator ==(XYZ2 lhs, XYZ2 rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(XYZ2 lhs, XYZ2 rhs)
		{
			return !(lhs == rhs);
		}
		#endregion

	
	
	}
	
}