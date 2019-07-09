/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 4/8/2019
 * Time: 8:05 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RebarBeam_Test
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class SettingDialog : Form
	{
		public bool oppDirChecked = false;
		
		public SettingDialog()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.

			//Read text file...
			// Stirrup setting
			if(valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_YESNO_Y") == "True")
			{
				this.yesST_Chb.Checked = true;
			}
			else
			{
				this.yesST_Chb.Checked = false;
			}
			
			this.factorTb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_F");
			this.delta_1Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_D1");
			this.pitch_1Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_P1");
			this.pitch_2Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_P2");
			this.delta_3Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_D3");
			this.pitch_3Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_P3");	
			this.n3Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_N3");
	

			// C Rebar setting
			if(valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_C1_YESNO_Y") == "True")
			{
				this.yesC1_Chb.Checked = true;
			}
			else
			{
				this.yesC1_Chb.Checked = false;
			}
			
			this.factor_C1Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_C1_F");
			this.delta_1_C1Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_C1_D1");
			this.pitch_1_C1Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_C1_P1");
			this.pitch_2_C1Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_C1_P2");
			this.delta_3_C1Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_C1_D3");
			this.pitch_3_C1Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_C1_P3");	
			this.n3_C1Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","HRS_C1_N3");

			
		
			// bottom bar setting
			//Layer1
			if(valueOfSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L1_YESNO_Y") == "True")
			{
				this.yes1_Chb.Checked = true;
			}
			else
			{
				this.yes1_Chb.Checked = false;
			}
			this.FB1_Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L1_FB");
			this.CB1_Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L1_CB");
			
			//Tach rea rebar shape
			
			this.RebarShap1_Cb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L1_BARSHAPE");
			this.RebarType1_Cb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L1_BARYPE");
			
			//Layer2
			if(valueOfSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L2_YESNO_Y") == "True")
			{
				this.yes2_Chb.Checked = true;
			}
			else
			{
				this.yes2_Chb.Checked = false;
			}
			this.FB2_Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L2_FB");
			this.CB2_Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L2_CB");
			this.RebarShap2_Cb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L2_BARSHAPE");
			this.RebarType2_Cb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L2_BARYPE");	
			
			
			
			
		}
	
	
	  	// Only accept number
	  	private void TXT_Box_KeyPress(object sender, KeyPressEventArgs e)
		{
		    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
		        (e.KeyChar != '.'))
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
			
			changeSetting(@"D:\Revit Setting\RevitSetting.set","HRS_YESNO_Y",
			              this.yesST_Chb.Checked.ToString());
			
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
			
			
			// C rebar save setting
			changeSetting(@"D:\Revit Setting\RevitSetting.set","HRS_C1_YESNO_Y",
			              this.yesC1_Chb.Checked.ToString());
			
			changeSetting(@"D:\Revit Setting\RevitSetting.set","HRS_C1_F",
			              this.factor_C1Tb.Text);
						
			changeSetting(@"D:\Revit Setting\RevitSetting.set","HRS_C1_D1",
			              this.delta_1_C1Tb.Text);
			
			changeSetting(@"D:\Revit Setting\RevitSetting.set","HRS_C1_P1",
			              this.pitch_1_C1Tb.Text);
			
			changeSetting(@"D:\Revit Setting\RevitSetting.set","HRS_C1_P2",
			              this.pitch_2_C1Tb.Text);
						
			changeSetting(@"D:\Revit Setting\RevitSetting.set","HRS_C1_D3",
			              this.delta_3_C1Tb.Text);
								
			changeSetting(@"D:\Revit Setting\RevitSetting.set","HRS_C1_P3",
              this.pitch_3_C1Tb.Text);
			
			changeSetting(@"D:\Revit Setting\RevitSetting.set","HRS_C1_N3",
			              this.n3_C1Tb.Text);
			
			
			// bottom Bar
			//Layer 1
			changeSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L1_YESNO_Y",
			              this.yes1_Chb.Checked.ToString());
						
			changeSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L1_FB",
			              this.FB1_Tb.Text);
			
			changeSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L1_CB",
			              this.CB1_Tb.Text);
			
			changeSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L1_BARSHAPE",
			              this.RebarShap1_Cb.Text);
						
			changeSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L1_BARYPE",
			              this.RebarType1_Cb.Text);
								

			//Layer 2
			changeSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L2_YESNO_Y",
			              this.yes2_Chb.Checked.ToString());
						
			changeSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L2_FB",
			              this.FB2_Tb.Text);
			
			changeSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L2_CB",
			              this.CB2_Tb.Text);
			
			changeSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L2_BARSHAPE",
			              this.RebarShap2_Cb.Text);
						
			changeSetting(@"D:\Revit Setting\RevitSetting.set","BOT_L2_BARYPE",
			              this.RebarType2_Cb.Text);
								
			
			this.Hide();			
		}
	
		
		
		void Opp_ChbCheckedChanged(object sender, EventArgs e)
		{
			this.oppDirChecked = !this.oppDirChecked;			
		}

	}
}
