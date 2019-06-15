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

using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace FilterByParameterValue
{
    public partial class FilterForm : Form
    {
        public List<string> myListName = new List<string>();
        public List<string> myListValue = new List<string>();
        public bool saveSelection = false;

        public FilterForm(List<string> myListName, List<string> myListValue)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            this.myListName = myListName;
            this.myListValue = myListValue;


            foreach (string paraname in myListName)
            {
                this.paraName_Cb.Items.Add(paraname);
            }


            this.paraName_Cb.AutoCompleteSource = AutoCompleteSource.ListItems;
            this.paraName_Cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        }


        void ParaName_CbSelectedIndexChanged(object sender, EventArgs e)
        {
            this.value_Tb.Text = myListValue[paraName_Cb.SelectedIndex];

        }

        void SelectByFil_BtClick(object sender, EventArgs e)
        {
            this.saveSelection = false;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            this.Hide();
        }

        void SaveSel_BtnClick(object sender, EventArgs e)
        {
            this.saveSelection = true;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            this.Hide();
        }

    }
}
