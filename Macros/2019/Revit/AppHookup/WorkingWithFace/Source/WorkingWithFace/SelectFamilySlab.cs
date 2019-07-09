/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 4/1/2019
 * Time: 12:35 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */


using uiRevit = Autodesk.Revit.UI;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WorkingWithFace
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class SelectFamilySlab : Form
	{
		public SelectFamilySlab()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void OkBtClick(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			
			//Change family name
			changeSetting(@"C:\Revit Setting\RevitSetting.set","LiningConcreteFamily",
			              this.FamilysCb.SelectedItem.ToString());

			// Change offset
			changeSetting(@"C:\Revit Setting\RevitSetting.set","LiningConcreteOffset",
              this.thickTb.Text.ToString());
			
			this.Hide();
		}
		
	  	private void TXT_Box_KeyPress(object sender, KeyPressEventArgs e)
		{
		    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
		        (e.KeyChar != '.') )
		    {
		            e.Handled = true;
		    }
		
		    // only allow one decimal point
		    if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
		    {
		        e.Handled = true;
		    }
		    
		}

		/// <summary>
		/// Chaneg vuale of setting, save it to file
		/// </summary>
		/// <param name="pathSettingFile"></param>
		/// <param name="settingName"></param>
		/// <param name="settingValue"></param>
		private void changeSetting(string pathSettingFile, string settingName, string settingValue)
		{
			//Check file exixst, if Ok ...
			
			try 
			{
				if(File.Exists(pathSettingFile))
				{
					//Read all Line in file
					string[] myFullSetting = File.ReadAllLines(pathSettingFile);
					//Create a Dictionay wiht key and 
					Dictionary<string, string> myDicSetting = new Dictionary<string, string>();
					
					string[] mySettingList;
					// Add pair of setting to Dic (transfer setting to Dic)
	
					foreach (string pairSetting in myFullSetting) 
					{
						// if  satisfy conditions, add to Dic{List[0]: List[1],...}
	
						if(pairSetting.Count(f => f == '|') == 1)
						{
							// Split line to list
							mySettingList = pairSetting.Split('|');
							// Add to Dic
							myDicSetting.Add(mySettingList[0], mySettingList[1]);
						}
					}
					//Change setting
					if(myDicSetting.Keys.Contains(settingName))
				   {
						myDicSetting[settingName] = settingValue;			   
				   }
					
					// Clear Setting file
					using (var fs = new FileStream(pathSettingFile, FileMode.Truncate))
					{
					}
					
					// ReWrite Setting
					using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathSettingFile))
			        {
			            foreach (string mySetName in myDicSetting.Keys)
			            {
			            	string lineToWrite = mySetName + "|" + myDicSetting[mySetName];
		                    file.WriteLine(lineToWrite);
			            }
			        }
				}
			
			} 
			catch (Exception e) {
				
				uiRevit.TaskDialog.Show("Error", e.Message);
			}
			

			
		}
	
	  	
		
	}
}
