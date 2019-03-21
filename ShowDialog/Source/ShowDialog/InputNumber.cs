/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 3/5/2019
 * Time: 9:48 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

using System.Collections.Generic;

using System.Linq;
using System.IO;
using ShowDialog;


namespace ShowDialog
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
public partial class InputNumber : System.Windows.Forms.Form
 {
  //Public variable added for retrieval in main method
  public string TextString {get;set;}
  public string Mode {get;set;}
  
  
  
  public InputNumber()
  {
   InitializeComponent();
  }
  
  //When OK is clicked return the TextBox Text 
  void OK_BtnClick(object sender, EventArgs e)
  {
   TextString = TXT_Box.Text;
   this.DialogResult =  System.Windows.Forms.DialogResult.OK;
  }
  
  private void TXT_Box_KeyPress(object sender, KeyPressEventArgs e)
	{
	    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
	        (e.KeyChar != '.') && (e.KeyChar != '-') )
	    {
	            e.Handled = true;
	    }
	
	    // only allow one decimal point
	    if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
	    {
	        e.Handled = true;
	    }
	    // accept nagative, minus symbol
	    if ((e.KeyChar == '-') && ((sender as TextBox).Text.IndexOf('-') > -1))
	    {
	        e.Handled = true;
	    }
	    
	}
  
		
		public void Pick3P_BtnClick(object sender, EventArgs e)
		{
			this.Mode = "3P";
			
			this.DialogResult =  System.Windows.Forms.DialogResult.Yes;
			this.Hide();
		}
		
		
		void PickAD_BtnClick(object sender, EventArgs e)
		{
			this.Mode = "PickAngular";
			
			this.DialogResult =  System.Windows.Forms.DialogResult.Yes;
			this.Hide();
		}
 }
}
