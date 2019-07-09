/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 7/2/2019
 * Time: 5:01 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace WorkWithParameters
{
	partial class ShowElemByParameter_Form
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.paraName_Tb = new System.Windows.Forms.TextBox();
			this.show_Bt = new System.Windows.Forms.Button();
			this.paraNameValue_Cb = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(15, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(133, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Paramater Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label1.UseCompatibleTextRendering = true;
			// 
			// paraName_Tb
			// 
			this.paraName_Tb.Location = new System.Drawing.Point(15, 30);
			this.paraName_Tb.Name = "paraName_Tb";
			this.paraName_Tb.Size = new System.Drawing.Size(133, 20);
			this.paraName_Tb.TabIndex = 1;
			this.paraName_Tb.Text = "INNO_Ten-CK";
			// 
			// show_Bt
			// 
			this.show_Bt.Location = new System.Drawing.Point(290, 62);
			this.show_Bt.Name = "show_Bt";
			this.show_Bt.Size = new System.Drawing.Size(75, 23);
			this.show_Bt.TabIndex = 2;
			this.show_Bt.Text = "Show";
			this.show_Bt.UseCompatibleTextRendering = true;
			this.show_Bt.UseVisualStyleBackColor = true;
			this.show_Bt.Click += new System.EventHandler(this.Show_BtClick);
			// 
			// paraNameValue_Cb
			// 
			this.paraNameValue_Cb.FormattingEnabled = true;
			this.paraNameValue_Cb.Location = new System.Drawing.Point(154, 30);
			this.paraNameValue_Cb.Name = "paraNameValue_Cb";
			this.paraNameValue_Cb.Size = new System.Drawing.Size(211, 21);
			this.paraNameValue_Cb.TabIndex = 5;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(154, 4);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(211, 23);
			this.label3.TabIndex = 6;
			this.label3.Text = "Value";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label3.UseCompatibleTextRendering = true;
			// 
			// ShowElemByParameter_Form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(379, 97);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.paraNameValue_Cb);
			this.Controls.Add(this.show_Bt);
			this.Controls.Add(this.paraName_Tb);
			this.Controls.Add(this.label1);
			this.Name = "ShowElemByParameter_Form";
			this.Text = "Show Elem By Parameter";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Label label3;
		public System.Windows.Forms.ComboBox paraNameValue_Cb;
		private System.Windows.Forms.Button show_Bt;
		public System.Windows.Forms.TextBox paraName_Tb;
		private System.Windows.Forms.Label label1;
	}
}
