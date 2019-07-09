/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 4/1/2019
 * Time: 12:35 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace WorkingWithFace
{
	partial class SelectFamilySlab
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
			this.OkBt = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.FamilysCb = new System.Windows.Forms.ComboBox();
			this.thickTb = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// OkBt
			// 
			this.OkBt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OkBt.Location = new System.Drawing.Point(267, 19);
			this.OkBt.Name = "OkBt";
			this.OkBt.Size = new System.Drawing.Size(69, 47);
			this.OkBt.TabIndex = 0;
			this.OkBt.Text = "OK";
			this.OkBt.UseCompatibleTextRendering = true;
			this.OkBt.UseVisualStyleBackColor = true;
			this.OkBt.Click += new System.EventHandler(this.OkBtClick);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.FamilysCb);
			this.groupBox1.Controls.Add(this.OkBt);
			this.groupBox1.Controls.Add(this.thickTb);
			this.groupBox1.Location = new System.Drawing.Point(24, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(354, 77);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Select Family Name";
			this.groupBox1.UseCompatibleTextRendering = true;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(7, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 23);
			this.label2.TabIndex = 4;
			this.label2.Text = "Offset(mm)";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label2.UseCompatibleTextRendering = true;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(7, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(77, 23);
			this.label1.TabIndex = 3;
			this.label1.Text = "Family Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label1.UseCompatibleTextRendering = true;
			// 
			// FamilysCb
			// 
			this.FamilysCb.FormattingEnabled = true;
			this.FamilysCb.Location = new System.Drawing.Point(100, 19);
			this.FamilysCb.Name = "FamilysCb";
			this.FamilysCb.Size = new System.Drawing.Size(121, 21);
			this.FamilysCb.TabIndex = 2;
			// 
			// thickTb
			// 
			this.thickTb.Location = new System.Drawing.Point(100, 50);
			this.thickTb.Name = "thickTb";
			this.thickTb.Size = new System.Drawing.Size(43, 20);
			this.thickTb.TabIndex = 1;
			this.thickTb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// SelectFamilySlab
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(383, 102);
			this.Controls.Add(this.groupBox1);
			this.Name = "SelectFamilySlab";
			this.Text = "Auto Make Slab";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
		}
		public System.Windows.Forms.TextBox thickTb;
		public System.Windows.Forms.ComboBox FamilysCb;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.Button OkBt;
	}
}
