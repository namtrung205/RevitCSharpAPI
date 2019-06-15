using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



using System.IO;

using uiRevit = Autodesk.Revit.UI;
using dbRevit = Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace PlaceFamilyAtLocBlock
{
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


            // bottom bar setting
            //Layer1
            if (valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L1_YESNO_Y") == "True")
            {
                this.yes1Top_Chb.Checked = true;
            }
            else
            {
                this.yes1Top_Chb.Checked = false;
            }
            this.FT1_Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L1_FT");
            this.CT1_Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L1_CT");

            //Tach rea rebar shape

            this.RebarShap1Top_Cb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L1_BARSHAPE");
            this.RebarType1Top_Cb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L1_BARYPE");

            //Layer2
            if (valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L2_YESNO_Y") == "True")
            {
                this.yes2Top_Chb.Checked = true;
            }
            else
            {
                this.yes2Top_Chb.Checked = false;
            }
            this.FT2_Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L2_FT");
            this.CT2_Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L2_CT");
            this.RebarShap2Top_Cb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L2_BARSHAPE");
            this.RebarType2Top_Cb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L2_BARYPE");




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
                if (File.Exists(pathSettingFile))
                {
                    //Read all Line in file
                    string[] myFullSetting = File.ReadAllLines(pathSettingFile);
                    //Create a Dictionay wiht key and 
                    string[] mySettingList;
                    foreach (string pairSetting in myFullSetting)
                    {
                        // if  satisfy conditions, add to Dic{List[0]: List[1],...}

                        if (pairSetting.Count(f => f == '|') == 1)
                        {
                            // Split line to list
                            mySettingList = pairSetting.Split('|');
                            // Add to Dic
                            if (mySettingList[0] == settingName)
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
            catch (Exception e)
            {

                //TaskDialog.Show("Error", e.Message);
                return "";
            }


        }



        private void changeSetting(string pathSettingFile, string settingName, string settingValue)
        {
            //Check file exixst, if Ok ...

            try
            {
                if (File.Exists(pathSettingFile))
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

                        if (pairSetting.Count(f => f == '|') == 1)
                        {
                            // Split line to list
                            mySettingList = pairSetting.Split('|');
                            // Add to Dic
                            myDicSetting.Add(mySettingList[0], mySettingList[1]);
                        }
                    }
                    //Change setting
                    if (myDicSetting.Keys.Contains(settingName))
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
            catch (Exception e)
            {

                //TaskDialog.Show("Error", e.Message);
            }

        }



        void SaveSettingBtnClick(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            // Top Bar
            //Layer 1
            changeSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L1_YESNO_Y",
                          this.yes1Top_Chb.Checked.ToString());

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L1_FT",
                          this.FT1_Tb.Text);

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L1_CT",
                          this.CT1_Tb.Text);

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L1_BARSHAPE",
                          this.RebarShap1Top_Cb.Text);

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L1_BARYPE",
                          this.RebarType1Top_Cb.Text);


            //Layer 2
            changeSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L2_YESNO_Y",
                          this.yes2Top_Chb.Checked.ToString());

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L2_FT",
                          this.FT2_Tb.Text);

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L2_CT",
                          this.CT2_Tb.Text);

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L2_BARSHAPE",
                          this.RebarShap2Top_Cb.Text);

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "TOP_L2_BARYPE",
                          this.RebarType2Top_Cb.Text);

            this.Hide();
        }



        void Opp_ChbCheckedChanged(object sender, EventArgs e)
        {
            this.oppDirChecked = !this.oppDirChecked;
        }

    }
}
