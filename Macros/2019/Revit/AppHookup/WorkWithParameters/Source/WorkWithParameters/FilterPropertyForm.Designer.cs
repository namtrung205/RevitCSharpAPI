/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 5/17/2019
 * Time: 2:07 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace WorkWithParameters
{
	partial class FilterForm
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
			this.paraName_Cb = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.selectByFil_Bt = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.value_Tb = new System.Windows.Forms.TextBox();
			this.saveSel_Btn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// paraName_Cb
			// 
			this.paraName_Cb.FormattingEnabled = true;
			this.paraName_Cb.Location = new System.Drawing.Point(12, 35);
			this.paraName_Cb.Name = "paraName_Cb";
			this.paraName_Cb.Size = new System.Drawing.Size(227, 21);
			this.paraName_Cb.TabIndex = 0;
			this.paraName_Cb.SelectedIndexChanged += new System.EventHandler(this.ParaName_CbSelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(227, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "Parameter Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.label1.UseCompatibleTextRendering = true;
			// 
			// selectByFil_Bt
			// 
			this.selectByFil_Bt.Location = new System.Drawing.Point(278, 82);
			this.selectByFil_Bt.Name = "selectByFil_Bt";
			this.selectByFil_Bt.Size = new System.Drawing.Size(132, 23);
			this.selectByFil_Bt.TabIndex = 2;
			this.selectByFil_Bt.Text = "Filter By Parameter";
			this.selectByFil_Bt.UseCompatibleTextRendering = true;
			this.selectByFil_Bt.UseVisualStyleBackColor = true;
			this.selectByFil_Bt.Click += new System.EventHandler(this.SelectByFil_BtClick);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(269, 10);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(149, 23);
			this.label2.TabIndex = 3;
			this.label2.Text = "Parameter Value";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.label2.UseCompatibleTextRendering = true;
			// 
			// value_Tb
			// 
			this.value_Tb.Location = new System.Drawing.Point(269, 36);
			this.value_Tb.Name = "value_Tb";
			this.value_Tb.Size = new System.Drawing.Size(149, 20);
			this.value_Tb.TabIndex = 4;
			// 
			// saveSel_Btn
			// 
			this.saveSel_Btn.Location = new System.Drawing.Point(278, 111);
			this.saveSel_Btn.Name = "saveSel_Btn";
			this.saveSel_Btn.Size = new System.Drawing.Size(132, 23);
			this.saveSel_Btn.TabIndex = 5;
			this.saveSel_Btn.Text = "Save Selection";
			this.saveSel_Btn.UseCompatibleTextRendering = true;
			this.saveSel_Btn.UseVisualStyleBackColor = true;
			this.saveSel_Btn.Click += new System.EventHandler(this.SaveSel_BtnClick);
			// 
			// FilterForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(430, 143);
			this.Controls.Add(this.saveSel_Btn);
			this.Controls.Add(this.value_Tb);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.selectByFil_Bt);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.paraName_Cb);
			this.Name = "FilterForm";
			this.Text = "Filter By Parameter";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		public System.Windows.Forms.Button saveSel_Btn;
		public System.Windows.Forms.TextBox value_Tb;
		private System.Windows.Forms.Label label2;
		public System.Windows.Forms.Button selectByFil_Bt;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.ComboBox paraName_Cb;
	
	
	
	}
}
