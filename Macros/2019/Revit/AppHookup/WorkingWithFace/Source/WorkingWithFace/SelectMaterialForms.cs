/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 3/22/2019
 * Time: 8:55 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using wf = System.Windows.Forms;
using System.IO;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace WorkingWithFace
{
	/// <summary>
	/// Description of SelectMaterialForms.
	/// </summary>
	public partial class SelectMaterialForms : wf.Form
	{
		public SelectMaterialForms()
		{
			
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			// Load all Curent Materials
			
		}
		
		void OkClick(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			changeSetting(@"C:\Revit Setting\RevitSetting.set","LiningConcreteFamily",
			              this.listMatCb.SelectedItem.ToString());
			this.Hide();
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
				
				TaskDialog.Show("Error", e.Message);
			}
			

			
		}
	
	
	
	}
}
