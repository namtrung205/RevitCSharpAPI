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
    /// <summary>
    /// Description of Form1.
    /// </summary>
    public partial class EditDimension_Form : Form
    {
        public string buttonClicked = "";

        public EditDimension_Form(List<string> myListDimValue)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //Set Values Text Box with value of listDimvalue


            string setToTextBox = string.Join(";", myListDimValue);
            this.listValue_Tb.Text = setToTextBox;

            //Load beginAjustAt
            this.beginMoveAt_Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "6.01_OTHER_TAB_AUTO_ADJUST_DIM_TEXT_BEGIN_MOVE_AT");

        }



        void UpdateValue_BtClick(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            buttonClicked = "UPDATE";

            this.Hide();
        }

        void reset_BtClick(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            buttonClicked = "RESET";

            this.Hide();
        }



        void AdjustText_BtClick(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            buttonClicked = "ADJUST";
            changeSetting(@"D:\Revit Setting\RevitSetting.set", "6.01_OTHER_TAB_AUTO_ADJUST_DIM_TEXT_BEGIN_MOVE_AT",
                          this.beginMoveAt_Tb.Text);
            this.Hide();

        }


        // Only accept number
        private void TXT_Box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.') &&
                (e.KeyChar != '@'))
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


    }

}
