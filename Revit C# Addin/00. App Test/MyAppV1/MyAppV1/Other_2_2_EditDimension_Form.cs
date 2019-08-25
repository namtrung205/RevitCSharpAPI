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


        }



        void UpdateValue_BtClick(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            buttonClicked = "UPDATE";

            this.Hide();
        }

        void Button1Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            buttonClicked = "RESET";

            this.Hide();
        }


    }

}
