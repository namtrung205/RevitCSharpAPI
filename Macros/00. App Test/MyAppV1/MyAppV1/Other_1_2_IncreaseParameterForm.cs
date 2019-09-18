using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyAppV1
{
    public partial class IncreaseParameterForm : Form
    {
        public int method = 0;

        public bool isDecreaseChecked = false;

        public IncreaseParameterForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //


            this.paraName_Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_NANE");
            this.Pre_Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_PRE");

            this.startNum_Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_START");
            this.Suf_Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_SUF");
        }

        void OK_1_BtClick(object sender, EventArgs e)
        {

            this.method = 1;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;


            changeSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_NANE",
              this.paraName_Tb.Text);


            changeSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_PRE",
                          this.Pre_Tb.Text);

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_START",
                          this.startNum_Tb.Text);

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_SUF",
                          this.Suf_Tb.Text);

            //this.Hide();

        }


        void OK_2_BtClick(object sender, EventArgs e)
        {

            this.method = 2;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;


            changeSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_NANE",
              this.paraName_Tb.Text);


            changeSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_PRE",
                          this.Pre_Tb.Text);

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_START",
                          this.startNum_Tb.Text);

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_SUF",
                          this.Suf_Tb.Text);

            //this.Hide();

        }


        void OK_3_BtClick(object sender, EventArgs e)
        {

            this.method = 3;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;


            changeSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_NANE",
              this.paraName_Tb.Text);


            changeSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_PRE",
                          this.Pre_Tb.Text);

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_START",
                          this.startNum_Tb.Text);

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_SUF",
                          this.Suf_Tb.Text);

            //this.Hide();

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

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_NANE",
              this.paraName_Tb.Text);


            changeSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_PRE",
                          this.Pre_Tb.Text);

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_START",
                          this.startNum_Tb.Text);

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_SUF",
                          this.Suf_Tb.Text);

            this.Hide();
        }


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


        void DeCrease1_CbCheckedChanged(object sender, EventArgs e)
        {

            isDecreaseChecked = this.deCrease1_Cb.Checked;
        }

    }
}
