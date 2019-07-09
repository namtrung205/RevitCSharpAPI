/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 5/8/2019
 * Time: 8:43 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace TestCSharp
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("3BF35AA2-AA91-485E-8174-3110488B5E9A")]
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
	
		public void testCompareXYZ()
		{
			XYZ one = new XYZ(9,0,0);
			XYZ two = new XYZ(1,1.4000,0);
			XYZ three = new XYZ(2,1,0);
			XYZ four = new XYZ(3,1,0);
			
			XYZ testXYZ = new XYZ(1,1.4,0);
			
			List<XYZ> myListXYZ = new List<XYZ>(){one, two, three, four};
			
			
			if(myListXYZ.Exists(p => xyzEquals(p, testXYZ, 6)))
		   {
				TaskDialog.Show("abc", "Ok");
		   }
		}
		
		public bool xyzEquals(XYZ p1, XYZ p2, int tolerance)
		{
			if(Math.Round(p1.X,tolerance) == Math.Round(p2.X,tolerance) &&
			   Math.Round(p1.Y,tolerance) == Math.Round(p2.Y,tolerance) &&
			  Math.Round(p1.Z,tolerance) == Math.Round(p2.Z,tolerance))
			{
				return true;
			}
			
			else
			{
				return false;
			}
		}
	}
	
	public static class exEquals
	{
		public static bool Equals(this XYZ firstXYZ, XYZ secondXYZ )
		{
			if(Math.Round(firstXYZ.X, 6) == Math.Round(secondXYZ.X, 6) &&
			   Math.Round(firstXYZ.Y, 6) == Math.Round(secondXYZ.Y, 6) &&
			   Math.Round(firstXYZ.Z, 6) == Math.Round(secondXYZ.Z, 6))
			{
				return true;
			}
			return false;
		}
	}
	
	
	// Tao 1 class moi ke thua class XYZ voi 
	//1. Override Equals Method
	
	public class XYZ_2: XYZ
	{
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			XYZ_2 other = obj as XYZ_2;
			if (other == null || other.X != this.X || other.Y != this.Y || other.Z !=this.Z)
				return false;
			return true;
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			return hashCode;
		}
		
		public static bool operator ==(XYZ_2 lhs, XYZ_2 rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
		
		public static bool operator !=(XYZ_2 lhs, XYZ_2 rhs)
		{
			return !(lhs == rhs);
		}
		#endregion

	}
	
	
	// Tao 1 class moi dan xuat tu Interface ICompare
	
	public class sortByX : IComparer<XYZ>
	{
		
		
		public int Compare(XYZ lhs, XYZ rhs)
		{
			if(Math.Round(lhs.X, 6) == Math.Round(rhs.X))
			{
				return lhs.Y.CompareTo(rhs.Y);
			}
			else
			{
				return lhs.X.CompareTo(rhs.X);
			}
		}
	}

	
	public class sortByY : IComparer<XYZ>
	{
		
		
		public int Compare(XYZ lhs, XYZ rhs)
		{
			if(Math.Round(lhs.X, 6) != Math.Round(rhs.X))
			{
				return lhs.Y.CompareTo(rhs.Y);
			}
			else
			{
				return lhs.X.CompareTo(rhs.X);
			}
		}
	}





}
	
