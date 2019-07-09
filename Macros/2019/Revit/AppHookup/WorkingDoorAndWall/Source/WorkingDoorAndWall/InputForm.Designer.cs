/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 6/28/2019
 * Time: 2:53 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace WorkingDoorAndWall
{
	partial class InputForm
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
			this.distance_tb = new System.Windows.Forms.TextBox();
			this.saveSet_bt = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(63, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Distance:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label1.UseCompatibleTextRendering = true;
			// 
			// distance_tb
			// 
			this.distance_tb.Location = new System.Drawing.Point(81, 12);
			this.distance_tb.Name = "distance_tb";
			this.distance_tb.Size = new System.Drawing.Size(100, 20);
			this.distance_tb.TabIndex = 1;
			this.distance_tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// saveSet_bt
			// 
			this.saveSet_bt.Location = new System.Drawing.Point(218, 9);
			this.saveSet_bt.Name = "saveSet_bt";
			this.saveSet_bt.Size = new System.Drawing.Size(99, 25);
			this.saveSet_bt.TabIndex = 2;
			this.saveSet_bt.Text = "Save Setting";
			this.saveSet_bt.UseCompatibleTextRendering = true;
			this.saveSet_bt.UseVisualStyleBackColor = true;
			this.saveSet_bt.Click += new System.EventHandler(this.SaveSet_btClick);
			// 
			// InputForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(334, 46);
			this.Controls.Add(this.saveSet_bt);
			this.Controls.Add(this.distance_tb);
			this.Controls.Add(this.label1);
			this.Name = "InputForm";
			this.Text = "InputForm";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button saveSet_bt;
		public System.Windows.Forms.TextBox distance_tb;
		private System.Windows.Forms.Label label1;
	}
}
