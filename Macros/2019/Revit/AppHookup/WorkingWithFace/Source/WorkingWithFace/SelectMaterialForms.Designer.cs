/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 3/22/2019
 * Time: 8:55 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace WorkingWithFace
{
	partial class SelectMaterialForms
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
			this.Ok = new System.Windows.Forms.Button();
			this.listMatCb = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// Ok
			// 
			this.Ok.Location = new System.Drawing.Point(156, 19);
			this.Ok.Name = "Ok";
			this.Ok.Size = new System.Drawing.Size(75, 23);
			this.Ok.TabIndex = 0;
			this.Ok.Text = "Paint";
			this.Ok.UseCompatibleTextRendering = true;
			this.Ok.UseVisualStyleBackColor = true;
			this.Ok.Click += new System.EventHandler(this.OkClick);
			// 
			// listMatCb
			// 
			this.listMatCb.FormattingEnabled = true;
			this.listMatCb.Location = new System.Drawing.Point(6, 19);
			this.listMatCb.Name = "listMatCb";
			this.listMatCb.Size = new System.Drawing.Size(121, 21);
			this.listMatCb.TabIndex = 1;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.listMatCb);
			this.groupBox1.Controls.Add(this.Ok);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(255, 54);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Set Material";
			this.groupBox1.UseCompatibleTextRendering = true;
			// 
			// SelectMaterialForms
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(281, 84);
			this.Controls.Add(this.groupBox1);
			this.Name = "SelectMaterialForms";
			this.Text = "SelectMaterialForms";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.ComboBox listMatCb;
		private System.Windows.Forms.Button Ok;
	}
}
