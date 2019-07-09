/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 3/30/2019
 * Time: 4:50 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WorkingWithBeam
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class InputDialog : Form
	{
		public InputDialog()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//Read text file...
			this.factorTb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_F");
			this.delta_1Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_D1");
			this.pitch_1Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_P1");
			this.pitch_2Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_P2");
			this.delta_3Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_D3");
			this.pitch_3Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_P3");	
			this.n3Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_N3");
		
		}
		
		void InputDialogLoad(object sender, EventArgs e)
		{
			
		}
		
		  // Only accept number
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

		
	  
		void SaveSettingBtnClick(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			
			
			changeSetting(@"D:\Revit Setting\RevitSetting.set","HRS_F",
			              this.factorTb.Text);
						
			changeSetting(@"D:\Revit Setting\RevitSetting.set","HRS_D1",
			              this.delta_1Tb.Text);
			
			changeSetting(@"D:\Revit Setting\RevitSetting.set","HRS_P1",
			              this.pitch_1Tb.Text);
			
			changeSetting(@"D:\Revit Setting\RevitSetting.set","HRS_P2",
			              this.pitch_2Tb.Text);
						
			changeSetting(@"D:\Revit Setting\RevitSetting.set","HRS_D3",
			              this.delta_3Tb.Text);
								
			changeSetting(@"D:\Revit Setting\RevitSetting.set","HRS_P3",
              this.pitch_3Tb.Text);
			
			changeSetting(@"D:\Revit Setting\RevitSetting.set","HRS_N3",
			              this.n3Tb.Text);
			this.Hide();			
		}
	
	
	
	
	}
}
