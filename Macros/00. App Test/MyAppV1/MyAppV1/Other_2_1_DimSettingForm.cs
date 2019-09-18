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
    public partial class DimSettingForm : Form
    {
        public bool onlyCenter = false;

        public DimSettingForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.

            //Read text file...
            // Stirrup setting
            if (valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "5.03_OTHER_TAB_AUTO_LOCATE_COLUMN_ON_PLAN_IS_ONLY_CENTER") == "True")
            {
                this.onlyCenter_Cb.Checked = true;
            }
            else
            {
                this.onlyCenter_Cb.Checked = false;
            }

            this.Ox_Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "5.01_OTHER_TAB_AUTO_LOCATE_COLUMN_ON_PLAN_OX");
            this.Oy_tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "5.02_OTHER_TAB_AUTO_LOCATE_COLUMN_ON_PLAN_OY");
            this.Ma_Tb.Text = valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "5.04_OTHER_TAB_AUTO_LOCATE_COLUMN_ON_PLAN_MA");



            //Tab 1

            string myListOldPicked = valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "7.01_OTHER_TAB_SELECT_CATERGORY_NAME_TO_DIM");
            List<string> myStringNameCatergoriesPicked = myListOldPicked.Split(',').ToList();


            this.pickedCatergories_Clb.CheckOnClick = true;

            //Load toan bo default catergory
            List<string> allCatergoryName = new List<string>(){"Walls",
                                                                "Columns",
                                                                "Structural Columns",
                                                                "Structural Framing",
                                                                "Structural Foundations",
                                                                "Roofs",
                                                                "Mass",
                                                                "Ceilings",
                                                                "Floors" };
            foreach (string nameCater in allCatergoryName)
            {
                if (myStringNameCatergoriesPicked.Contains(nameCater))
                {
                    this.pickedCatergories_Clb.Items.Add(nameCater, true);
                }
                else
                {
                    this.pickedCatergories_Clb.Items.Add(nameCater, false);
                }
            }


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


        void SaveSetting_BtnClick(object sender, EventArgs e)
        {

            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "5.01_OTHER_TAB_AUTO_LOCATE_COLUMN_ON_PLAN_OX",
                          this.Ox_Tb.Text);

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "5.02_OTHER_TAB_AUTO_LOCATE_COLUMN_ON_PLAN_OY",
                          this.Oy_tb.Text);

            changeSetting(@"D:\Revit Setting\RevitSetting.set", "5.03_OTHER_TAB_AUTO_LOCATE_COLUMN_ON_PLAN_IS_ONLY_CENTER",
                          this.onlyCenter_Cb.Checked.ToString());
            changeSetting(@"D:\Revit Setting\RevitSetting.set", "5.04_OTHER_TAB_AUTO_LOCATE_COLUMN_ON_PLAN_MA",
                  this.Ma_Tb.Text.ToString());


            //Tab 1
            //save setting

            List<string> newSelectedList = new List<string>();
            foreach (string mySelectedcatergory in this.pickedCatergories_Clb.CheckedItems)
            {
                string myselect = mySelectedcatergory.ToString();
                newSelectedList.Add(myselect);
            }

            string newValueString = string.Join(",", newSelectedList);
            changeSetting(@"D:\Revit Setting\RevitSetting.set", "7.01_OTHER_TAB_SELECT_CATERGORY_NAME_TO_DIM",
                          newValueString);

            this.Hide();

        }

        private void checkAll_Bt_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.pickedCatergories_Clb.Items.Count; i++)
            {
                this.pickedCatergories_Clb.SetItemChecked(i, true);
            }
        }

        private void checkNone_Bt_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.pickedCatergories_Clb.Items.Count; i++)
            {
                this.pickedCatergories_Clb.SetItemChecked(i, false);
            }
        }

        private void save_Bt_Click(object sender, EventArgs e)
        {
            //save setting

            List<string> newSelectedList = new List<string>();
            foreach (string mySelectedcatergory in this.pickedCatergories_Clb.CheckedItems)
            {
                string myselect = mySelectedcatergory.ToString();
                newSelectedList.Add(myselect);
            }

            string newValueString = string.Join(",", newSelectedList);
            changeSetting(@"D:\Revit Setting\RevitSetting.set", "7.01_OTHER_TAB_SELECT_CATERGORY_NAME_TO_DIM",
                          newValueString);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

    }




}
