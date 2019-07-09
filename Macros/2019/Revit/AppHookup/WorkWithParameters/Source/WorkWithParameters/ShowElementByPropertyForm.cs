/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 7/2/2019
 * Time: 5:01 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WorkWithParameters
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class ShowElemByParameter_Form : Form
	{
		public ShowElemByParameter_Form(List<string> myListNameValue)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			
//			this.myListValue = myListNameValue;
			
			foreach (string paraNameValue in myListNameValue) 
			{
				this.paraNameValue_Cb.Items.Add(paraNameValue);
			}	
			
			
			this.paraNameValue_Cb.AutoCompleteSource = AutoCompleteSource.ListItems;
			this.paraNameValue_Cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			
			
			this.paraName_Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","FIND_BY_NAM_NAMEPARA");
		}
		
		
		void Show_BtClick(object sender, EventArgs e)
		{
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

			changeSetting(@"D:\Revit Setting\RevitSetting.set","FIND_BY_NAM_NAMEPARA",
              this.paraName_Tb.Text);
            this.Hide();
		}
		
		
		
				
		private string valueOfSetting(string pathSettingFile, string settingName)
		
		{
		
			// Open file setting
			try 
			{
				if(File.Exists(pathSettingFile))
				{
					//Read all Line in file
					string[] myFullSetting = File.ReadAllLines(pathSettingFile);
					//Create a Dictionay wiht key and 
					string[] mySettingList;
					foreach (string pairSetting in myFullSetting) 
					{
						// if  satisfy conditions, add to Dic{List[0]: List[1],...}
	
						if(pairSetting.Count(f => f == '|') == 1)
						{
							// Split line to list
							mySettingList = pairSetting.Split('|');
							// Add to Dic
							if(mySettingList[0] == settingName)
							{
								return mySettingList[1];
							}

							continue;
						}
						continue;
					}
					return "";
				}
				else
				{
					return "";
				}
			
			} 
			catch (Exception e) {

				//TaskDialog.Show("Error", e.Message);
				return "";
			}
		}
	

		
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
				
				//TaskDialog.Show("Error", e.Message);
			}
			
		}

		
	
	
		
		
	}
}
