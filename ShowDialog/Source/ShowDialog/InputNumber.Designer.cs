/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 3/5/2019
 * Time: 9:48 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ShowDialog
{
	partial class InputNumber
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
			this.TXT_Box = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.PickAD_Btn = new System.Windows.Forms.Button();
			this.Pick3P_Btn = new System.Windows.Forms.Button();
			this.OK_Btn = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// TXT_Box
			// 
			this.TXT_Box.Location = new System.Drawing.Point(6, 19);
			this.TXT_Box.Name = "TXT_Box";
			this.TXT_Box.Size = new System.Drawing.Size(72, 20);
			this.TXT_Box.TabIndex = 0;
			this.TXT_Box.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.PickAD_Btn);
			this.groupBox1.Controls.Add(this.Pick3P_Btn);
			this.groupBox1.Controls.Add(this.TXT_Box);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(227, 47);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Text To Display";
			this.groupBox1.UseCompatibleTextRendering = true;
			// 
			// PickAD_Btn
			// 
			this.PickAD_Btn.Location = new System.Drawing.Point(84, 16);
			this.PickAD_Btn.Name = "PickAD_Btn";
			this.PickAD_Btn.Size = new System.Drawing.Size(29, 23);
			this.PickAD_Btn.TabIndex = 2;
			this.PickAD_Btn.Text = "PA";
			this.PickAD_Btn.UseCompatibleTextRendering = true;
			this.PickAD_Btn.UseVisualStyleBackColor = true;
			this.PickAD_Btn.Click += new System.EventHandler(this.PickAD_BtnClick);
			// 
			// Pick3P_Btn
			// 
			this.Pick3P_Btn.Location = new System.Drawing.Point(117, 16);
			this.Pick3P_Btn.Name = "Pick3P_Btn";
			this.Pick3P_Btn.Size = new System.Drawing.Size(29, 23);
			this.Pick3P_Btn.TabIndex = 1;
			this.Pick3P_Btn.Text = "3P";
			this.Pick3P_Btn.UseCompatibleTextRendering = true;
			this.Pick3P_Btn.UseVisualStyleBackColor = true;
			this.Pick3P_Btn.Click += new System.EventHandler(this.Pick3P_BtnClick);
			// 
			// OK_Btn
			// 
			this.OK_Btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OK_Btn.Location = new System.Drawing.Point(255, 17);
			this.OK_Btn.Name = "OK_Btn";
			this.OK_Btn.Size = new System.Drawing.Size(75, 42);
			this.OK_Btn.TabIndex = 5;
			this.OK_Btn.Text = "OK";
			this.OK_Btn.UseCompatibleTextRendering = true;
			this.OK_Btn.Click += new System.EventHandler(this.OK_BtnClick);
			// 
			// InputNumber
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(344, 73);
			this.Controls.Add(this.OK_Btn);
			this.Controls.Add(this.groupBox1);
			this.Name = "InputNumber";
			this.Text = "Input Number";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button PickAD_Btn;
		private System.Windows.Forms.Button Pick3P_Btn;
		private System.Windows.Forms.Button OK_Btn;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox TXT_Box;
		
	}
}
