/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 3/30/2019
 * Time: 4:50 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace WorkingWithBeam
{
	partial class InputDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputDialog));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.delta_1Tb = new System.Windows.Forms.TextBox();
			this.pitch_2Tb = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.factorTb = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.pitch_1Tb = new System.Windows.Forms.TextBox();
			this.saveSettingBtn = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.delta_3Tb = new System.Windows.Forms.TextBox();
			this.pitch_3Tb = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.n3Tb = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.ErrorImage")));
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
			this.pictureBox1.Location = new System.Drawing.Point(6, 19);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(588, 226);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.pictureBox1);
			this.groupBox1.Location = new System.Drawing.Point(12, 24);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(600, 251);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Hoop Reinforcement";
			this.groupBox1.UseCompatibleTextRendering = true;
			// 
			// delta_1Tb
			// 
			this.delta_1Tb.Location = new System.Drawing.Point(35, 53);
			this.delta_1Tb.Name = "delta_1Tb";
			this.delta_1Tb.Size = new System.Drawing.Size(33, 20);
			this.delta_1Tb.TabIndex = 8;
			this.delta_1Tb.Text = "50";
			this.delta_1Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.delta_1Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// pitch_2Tb
			// 
			this.pitch_2Tb.Location = new System.Drawing.Point(119, 92);
			this.pitch_2Tb.Name = "pitch_2Tb";
			this.pitch_2Tb.Size = new System.Drawing.Size(33, 20);
			this.pitch_2Tb.TabIndex = 7;
			this.pitch_2Tb.Text = "200";
			this.pitch_2Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.pitch_2Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(7, 52);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(22, 20);
			this.label3.TabIndex = 6;
			this.label3.Text = "D1: ";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label3.UseCompatibleTextRendering = true;
			// 
			// factorTb
			// 
			this.factorTb.Location = new System.Drawing.Point(35, 20);
			this.factorTb.Name = "factorTb";
			this.factorTb.Size = new System.Drawing.Size(33, 20);
			this.factorTb.TabIndex = 5;
			this.factorTb.Text = "4";
			this.factorTb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.factorTb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(7, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(22, 20);
			this.label2.TabIndex = 4;
			this.label2.Text = "F :";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label2.UseCompatibleTextRendering = true;
			// 
			// pitch_1Tb
			// 
			this.pitch_1Tb.Location = new System.Drawing.Point(119, 53);
			this.pitch_1Tb.Name = "pitch_1Tb";
			this.pitch_1Tb.Size = new System.Drawing.Size(33, 20);
			this.pitch_1Tb.TabIndex = 2;
			this.pitch_1Tb.Text = "100";
			this.pitch_1Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.pitch_1Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// saveSettingBtn
			// 
			this.saveSettingBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.saveSettingBtn.Location = new System.Drawing.Point(731, 246);
			this.saveSettingBtn.Name = "saveSettingBtn";
			this.saveSettingBtn.Size = new System.Drawing.Size(104, 23);
			this.saveSettingBtn.TabIndex = 2;
			this.saveSettingBtn.Text = "Save Setting";
			this.saveSettingBtn.UseCompatibleTextRendering = true;
			this.saveSettingBtn.UseVisualStyleBackColor = true;
			this.saveSettingBtn.Click += new System.EventHandler(this.SaveSettingBtnClick);
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(91, 52);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(22, 20);
			this.label6.TabIndex = 12;
			this.label6.Text = "P1: ";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label6.UseCompatibleTextRendering = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.n3Tb);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.delta_3Tb);
			this.groupBox2.Controls.Add(this.pitch_3Tb);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.pitch_2Tb);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.factorTb);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.delta_1Tb);
			this.groupBox2.Controls.Add(this.pitch_1Tb);
			this.groupBox2.Location = new System.Drawing.Point(641, 24);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(252, 172);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Input";
			this.groupBox2.UseCompatibleTextRendering = true;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(91, 92);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(22, 20);
			this.label5.TabIndex = 13;
			this.label5.Text = "P2: ";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label5.UseCompatibleTextRendering = true;
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(90, 126);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(22, 20);
			this.label7.TabIndex = 17;
			this.label7.Text = "P3: ";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label7.UseCompatibleTextRendering = true;
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(6, 126);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(22, 20);
			this.label8.TabIndex = 15;
			this.label8.Text = "D3: ";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label8.UseCompatibleTextRendering = true;
			// 
			// delta_3Tb
			// 
			this.delta_3Tb.Location = new System.Drawing.Point(34, 127);
			this.delta_3Tb.Name = "delta_3Tb";
			this.delta_3Tb.Size = new System.Drawing.Size(33, 20);
			this.delta_3Tb.TabIndex = 16;
			this.delta_3Tb.Text = "50";
			this.delta_3Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// pitch_3Tb
			// 
			this.pitch_3Tb.Location = new System.Drawing.Point(118, 127);
			this.pitch_3Tb.Name = "pitch_3Tb";
			this.pitch_3Tb.Size = new System.Drawing.Size(33, 20);
			this.pitch_3Tb.TabIndex = 14;
			this.pitch_3Tb.Text = "100";
			this.pitch_3Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(172, 126);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(22, 20);
			this.label1.TabIndex = 19;
			this.label1.Text = "N3: ";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label1.UseCompatibleTextRendering = true;
			// 
			// n3Tb
			// 
			this.n3Tb.Location = new System.Drawing.Point(200, 127);
			this.n3Tb.Name = "n3Tb";
			this.n3Tb.Size = new System.Drawing.Size(33, 20);
			this.n3Tb.TabIndex = 18;
			this.n3Tb.Text = "5";
			this.n3Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// InputDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(920, 299);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.saveSettingBtn);
			this.Controls.Add(this.groupBox1);
			this.Name = "InputDialog";
			this.Text = "Setting";
			this.Load += new System.EventHandler(this.InputDialogLoad);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
		}
		public System.Windows.Forms.TextBox n3Tb;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label5;
		public System.Windows.Forms.TextBox pitch_3Tb;
		public System.Windows.Forms.TextBox delta_3Tb;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button saveSettingBtn;
		public System.Windows.Forms.TextBox delta_1Tb;
		private System.Windows.Forms.Label label3;
		public System.Windows.Forms.TextBox pitch_2Tb;
		private System.Windows.Forms.Label label2;
		public System.Windows.Forms.TextBox factorTb;
		public System.Windows.Forms.TextBox pitch_1Tb;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}
