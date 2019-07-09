/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 4/8/2019
 * Time: 8:05 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace rebarBeamTop
{
	partial class SettingDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingDialog));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.saveSettingBtn = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.RebarType2Top_Cb = new System.Windows.Forms.ComboBox();
			this.RebarType1Top_Cb = new System.Windows.Forms.ComboBox();
			this.label13 = new System.Windows.Forms.Label();
			this.RebarShap2Top_Cb = new System.Windows.Forms.ComboBox();
			this.RebarShap1Top_Cb = new System.Windows.Forms.ComboBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.yes2Top_Chb = new System.Windows.Forms.CheckBox();
			this.FT2_Tb = new System.Windows.Forms.TextBox();
			this.CT2_Tb = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.yes1Top_Chb = new System.Windows.Forms.CheckBox();
			this.label12 = new System.Windows.Forms.Label();
			this.FT1_Tb = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.CT1_Tb = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.ErrorImage = null;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.InitialImage = null;
			this.pictureBox1.Location = new System.Drawing.Point(6, 26);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(601, 167);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.pictureBox1);
			this.groupBox1.Location = new System.Drawing.Point(12, 22);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(617, 202);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Preview Beam Rebar Setting";
			this.groupBox1.UseCompatibleTextRendering = true;
			// 
			// saveSettingBtn
			// 
			this.saveSettingBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.saveSettingBtn.Location = new System.Drawing.Point(835, 239);
			this.saveSettingBtn.Name = "saveSettingBtn";
			this.saveSettingBtn.Size = new System.Drawing.Size(104, 42);
			this.saveSettingBtn.TabIndex = 2;
			this.saveSettingBtn.Text = "Save Setting";
			this.saveSettingBtn.UseCompatibleTextRendering = true;
			this.saveSettingBtn.UseVisualStyleBackColor = true;
			this.saveSettingBtn.Click += new System.EventHandler(this.SaveSettingBtnClick);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.RebarType2Top_Cb);
			this.groupBox3.Controls.Add(this.RebarType1Top_Cb);
			this.groupBox3.Controls.Add(this.label13);
			this.groupBox3.Controls.Add(this.RebarShap2Top_Cb);
			this.groupBox3.Controls.Add(this.RebarShap1Top_Cb);
			this.groupBox3.Controls.Add(this.label11);
			this.groupBox3.Controls.Add(this.label10);
			this.groupBox3.Controls.Add(this.yes2Top_Chb);
			this.groupBox3.Controls.Add(this.FT2_Tb);
			this.groupBox3.Controls.Add(this.CT2_Tb);
			this.groupBox3.Controls.Add(this.label9);
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Controls.Add(this.yes1Top_Chb);
			this.groupBox3.Controls.Add(this.label12);
			this.groupBox3.Controls.Add(this.FT1_Tb);
			this.groupBox3.Controls.Add(this.label14);
			this.groupBox3.Controls.Add(this.CT1_Tb);
			this.groupBox3.Location = new System.Drawing.Point(635, 22);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(319, 202);
			this.groupBox3.TabIndex = 14;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Top Rebar Setting";
			this.groupBox3.UseCompatibleTextRendering = true;
			// 
			// RebarType2Top_Cb
			// 
			this.RebarType2Top_Cb.FormattingEnabled = true;
			this.RebarType2Top_Cb.Location = new System.Drawing.Point(226, 172);
			this.RebarType2Top_Cb.Name = "RebarType2Top_Cb";
			this.RebarType2Top_Cb.Size = new System.Drawing.Size(78, 21);
			this.RebarType2Top_Cb.TabIndex = 33;
			// 
			// RebarType1Top_Cb
			// 
			this.RebarType1Top_Cb.FormattingEnabled = true;
			this.RebarType1Top_Cb.Location = new System.Drawing.Point(118, 170);
			this.RebarType1Top_Cb.Name = "RebarType1Top_Cb";
			this.RebarType1Top_Cb.Size = new System.Drawing.Size(78, 21);
			this.RebarType1Top_Cb.TabIndex = 32;
			// 
			// label13
			// 
			this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label13.Location = new System.Drawing.Point(0, 171);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(105, 20);
			this.label13.TabIndex = 31;
			this.label13.Text = "REBAR TYPE:";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label13.UseCompatibleTextRendering = true;
			// 
			// RebarShap2Top_Cb
			// 
			this.RebarShap2Top_Cb.FormattingEnabled = true;
			this.RebarShap2Top_Cb.Location = new System.Drawing.Point(226, 133);
			this.RebarShap2Top_Cb.Name = "RebarShap2Top_Cb";
			this.RebarShap2Top_Cb.Size = new System.Drawing.Size(78, 21);
			this.RebarShap2Top_Cb.TabIndex = 30;
			// 
			// RebarShap1Top_Cb
			// 
			this.RebarShap1Top_Cb.FormattingEnabled = true;
			this.RebarShap1Top_Cb.Location = new System.Drawing.Point(118, 131);
			this.RebarShap1Top_Cb.Name = "RebarShap1Top_Cb";
			this.RebarShap1Top_Cb.Size = new System.Drawing.Size(78, 21);
			this.RebarShap1Top_Cb.TabIndex = 29;
			// 
			// label11
			// 
			this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.Location = new System.Drawing.Point(6, 133);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(105, 20);
			this.label11.TabIndex = 28;
			this.label11.Text = "REBAR SHAPE:";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label11.UseCompatibleTextRendering = true;
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.Location = new System.Drawing.Point(217, 16);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(52, 20);
			this.label10.TabIndex = 27;
			this.label10.Text = "Layer 2";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label10.UseCompatibleTextRendering = true;
			// 
			// yes2Top_Chb
			// 
			this.yes2Top_Chb.Checked = true;
			this.yes2Top_Chb.CheckState = System.Windows.Forms.CheckState.Checked;
			this.yes2Top_Chb.Location = new System.Drawing.Point(226, 35);
			this.yes2Top_Chb.Name = "yes2Top_Chb";
			this.yes2Top_Chb.Size = new System.Drawing.Size(52, 24);
			this.yes2Top_Chb.TabIndex = 26;
			this.yes2Top_Chb.Text = "Yes";
			this.yes2Top_Chb.UseCompatibleTextRendering = true;
			this.yes2Top_Chb.UseVisualStyleBackColor = true;
			// 
			// FT2_Tb
			// 
			this.FT2_Tb.Location = new System.Drawing.Point(226, 67);
			this.FT2_Tb.Name = "FT2_Tb";
			this.FT2_Tb.Size = new System.Drawing.Size(33, 20);
			this.FT2_Tb.TabIndex = 24;
			this.FT2_Tb.Text = "3";
			this.FT2_Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.FT2_Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// CT2_Tb
			// 
			this.CT2_Tb.Location = new System.Drawing.Point(226, 96);
			this.CT2_Tb.Name = "CT2_Tb";
			this.CT2_Tb.Size = new System.Drawing.Size(33, 20);
			this.CT2_Tb.TabIndex = 25;
			this.CT2_Tb.Text = "100";
			this.CT2_Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.CT2_Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(10, 67);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(34, 20);
			this.label9.TabIndex = 23;
			this.label9.Text = "FT:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label9.UseCompatibleTextRendering = true;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(109, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(52, 20);
			this.label4.TabIndex = 22;
			this.label4.Text = "Layer 1";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label4.UseCompatibleTextRendering = true;
			// 
			// yes1Top_Chb
			// 
			this.yes1Top_Chb.Checked = true;
			this.yes1Top_Chb.CheckState = System.Windows.Forms.CheckState.Checked;
			this.yes1Top_Chb.Location = new System.Drawing.Point(118, 35);
			this.yes1Top_Chb.Name = "yes1Top_Chb";
			this.yes1Top_Chb.Size = new System.Drawing.Size(52, 24);
			this.yes1Top_Chb.TabIndex = 21;
			this.yes1Top_Chb.Text = "Yes";
			this.yes1Top_Chb.UseCompatibleTextRendering = true;
			this.yes1Top_Chb.UseVisualStyleBackColor = true;
			// 
			// label12
			// 
			this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.Location = new System.Drawing.Point(0, 39);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(58, 20);
			this.label12.TabIndex = 4;
			this.label12.Text = "Yes/No:";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label12.UseCompatibleTextRendering = true;
			// 
			// FT1_Tb
			// 
			this.FT1_Tb.Location = new System.Drawing.Point(118, 67);
			this.FT1_Tb.Name = "FT1_Tb";
			this.FT1_Tb.Size = new System.Drawing.Size(33, 20);
			this.FT1_Tb.TabIndex = 5;
			this.FT1_Tb.Text = "4";
			this.FT1_Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.FT1_Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// label14
			// 
			this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label14.Location = new System.Drawing.Point(6, 96);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(38, 20);
			this.label14.TabIndex = 6;
			this.label14.Text = "CT: ";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label14.UseCompatibleTextRendering = true;
			// 
			// CT1_Tb
			// 
			this.CT1_Tb.Location = new System.Drawing.Point(118, 96);
			this.CT1_Tb.Name = "CT1_Tb";
			this.CT1_Tb.Size = new System.Drawing.Size(33, 20);
			this.CT1_Tb.TabIndex = 8;
			this.CT1_Tb.Text = "50";
			this.CT1_Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.CT1_Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// SettingDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(973, 285);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.saveSettingBtn);
			this.Controls.Add(this.groupBox1);
			this.Name = "SettingDialog";
			this.Text = "Setting";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);
		}
		public System.Windows.Forms.TextBox CT1_Tb;
		private System.Windows.Forms.Label label14;
		public System.Windows.Forms.TextBox FT1_Tb;
		private System.Windows.Forms.Label label12;
		public System.Windows.Forms.CheckBox yes1Top_Chb;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label9;
		public System.Windows.Forms.TextBox CT2_Tb;
		public System.Windows.Forms.TextBox FT2_Tb;
		public System.Windows.Forms.CheckBox yes2Top_Chb;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		public System.Windows.Forms.ComboBox RebarShap1Top_Cb;
		public System.Windows.Forms.ComboBox RebarShap2Top_Cb;
		private System.Windows.Forms.Label label13;
		public System.Windows.Forms.ComboBox RebarType1Top_Cb;
		public System.Windows.Forms.ComboBox RebarType2Top_Cb;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button saveSettingBtn;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}
