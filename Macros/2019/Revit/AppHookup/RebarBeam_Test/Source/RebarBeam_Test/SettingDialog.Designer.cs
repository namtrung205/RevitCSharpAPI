/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 4/8/2019
 * Time: 8:05 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace RebarBeam_Test
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
			this.delta_1Tb = new System.Windows.Forms.TextBox();
			this.pitch_2Tb = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.factorTb = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.pitch_1Tb = new System.Windows.Forms.TextBox();
			this.saveSettingBtn = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label15 = new System.Windows.Forms.Label();
			this.yesST_Chb = new System.Windows.Forms.CheckBox();
			this.opp_Chb = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.n3Tb = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.delta_3Tb = new System.Windows.Forms.TextBox();
			this.pitch_3Tb = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.RebarType2_Cb = new System.Windows.Forms.ComboBox();
			this.RebarType1_Cb = new System.Windows.Forms.ComboBox();
			this.label13 = new System.Windows.Forms.Label();
			this.RebarShap2_Cb = new System.Windows.Forms.ComboBox();
			this.RebarShap1_Cb = new System.Windows.Forms.ComboBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.yes2_Chb = new System.Windows.Forms.CheckBox();
			this.FB2_Tb = new System.Windows.Forms.TextBox();
			this.CB2_Tb = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.yes1_Chb = new System.Windows.Forms.CheckBox();
			this.label12 = new System.Windows.Forms.Label();
			this.FB1_Tb = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.CB1_Tb = new System.Windows.Forms.TextBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label16 = new System.Windows.Forms.Label();
			this.yesC1_Chb = new System.Windows.Forms.CheckBox();
			this.opp_C1_Chb = new System.Windows.Forms.CheckBox();
			this.label17 = new System.Windows.Forms.Label();
			this.n3_C1Tb = new System.Windows.Forms.TextBox();
			this.label18 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.delta_3_C1Tb = new System.Windows.Forms.TextBox();
			this.pitch_3_C1Tb = new System.Windows.Forms.TextBox();
			this.label20 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.pitch_2_C1Tb = new System.Windows.Forms.TextBox();
			this.label22 = new System.Windows.Forms.Label();
			this.factor_C1Tb = new System.Windows.Forms.TextBox();
			this.label23 = new System.Windows.Forms.Label();
			this.delta_1_C1Tb = new System.Windows.Forms.TextBox();
			this.pitch_1_C1Tb = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.ErrorImage = null;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.InitialImage = null;
			this.pictureBox1.Location = new System.Drawing.Point(6, 19);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(912, 226);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.pictureBox1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(931, 251);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Preview Beam Rebar Setting";
			this.groupBox1.UseCompatibleTextRendering = true;
			// 
			// delta_1Tb
			// 
			this.delta_1Tb.Location = new System.Drawing.Point(34, 83);
			this.delta_1Tb.Name = "delta_1Tb";
			this.delta_1Tb.Size = new System.Drawing.Size(33, 20);
			this.delta_1Tb.TabIndex = 8;
			this.delta_1Tb.Text = "50";
			this.delta_1Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.delta_1Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// pitch_2Tb
			// 
			this.pitch_2Tb.Location = new System.Drawing.Point(118, 122);
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
			this.label3.Location = new System.Drawing.Point(6, 82);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(22, 20);
			this.label3.TabIndex = 6;
			this.label3.Text = "D1: ";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label3.UseCompatibleTextRendering = true;
			// 
			// factorTb
			// 
			this.factorTb.Location = new System.Drawing.Point(34, 50);
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
			this.label2.Location = new System.Drawing.Point(6, 50);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(22, 20);
			this.label2.TabIndex = 4;
			this.label2.Text = "F :";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label2.UseCompatibleTextRendering = true;
			// 
			// pitch_1Tb
			// 
			this.pitch_1Tb.Location = new System.Drawing.Point(118, 83);
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
			this.saveSettingBtn.Location = new System.Drawing.Point(896, 464);
			this.saveSettingBtn.Name = "saveSettingBtn";
			this.saveSettingBtn.Size = new System.Drawing.Size(90, 50);
			this.saveSettingBtn.TabIndex = 2;
			this.saveSettingBtn.Text = "Save Setting";
			this.saveSettingBtn.UseCompatibleTextRendering = true;
			this.saveSettingBtn.UseVisualStyleBackColor = true;
			this.saveSettingBtn.Click += new System.EventHandler(this.SaveSettingBtnClick);
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(90, 82);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(22, 20);
			this.label6.TabIndex = 12;
			this.label6.Text = "P1: ";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label6.UseCompatibleTextRendering = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label15);
			this.groupBox2.Controls.Add(this.yesST_Chb);
			this.groupBox2.Controls.Add(this.opp_Chb);
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
			this.groupBox2.Location = new System.Drawing.Point(18, 279);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(252, 235);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Stirrup Rebar Setting";
			this.groupBox2.UseCompatibleTextRendering = true;
			// 
			// label15
			// 
			this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label15.Location = new System.Drawing.Point(6, 20);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(52, 20);
			this.label15.TabIndex = 22;
			this.label15.Text = "Yes/No";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label15.UseCompatibleTextRendering = true;
			// 
			// yesST_Chb
			// 
			this.yesST_Chb.Checked = true;
			this.yesST_Chb.CheckState = System.Windows.Forms.CheckState.Checked;
			this.yesST_Chb.Location = new System.Drawing.Point(64, 19);
			this.yesST_Chb.Name = "yesST_Chb";
			this.yesST_Chb.Size = new System.Drawing.Size(47, 24);
			this.yesST_Chb.TabIndex = 21;
			this.yesST_Chb.Text = "Yes";
			this.yesST_Chb.UseCompatibleTextRendering = true;
			this.yesST_Chb.UseVisualStyleBackColor = true;
			// 
			// opp_Chb
			// 
			this.opp_Chb.Location = new System.Drawing.Point(6, 199);
			this.opp_Chb.Name = "opp_Chb";
			this.opp_Chb.Size = new System.Drawing.Size(129, 24);
			this.opp_Chb.TabIndex = 20;
			this.opp_Chb.Text = "Opposite Direction";
			this.opp_Chb.UseCompatibleTextRendering = true;
			this.opp_Chb.UseVisualStyleBackColor = true;
			this.opp_Chb.CheckedChanged += new System.EventHandler(this.Opp_ChbCheckedChanged);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(171, 156);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(22, 20);
			this.label1.TabIndex = 19;
			this.label1.Text = "N3: ";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label1.UseCompatibleTextRendering = true;
			// 
			// n3Tb
			// 
			this.n3Tb.Location = new System.Drawing.Point(199, 157);
			this.n3Tb.Name = "n3Tb";
			this.n3Tb.Size = new System.Drawing.Size(33, 20);
			this.n3Tb.TabIndex = 18;
			this.n3Tb.Text = "4";
			this.n3Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.n3Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(89, 156);
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
			this.label8.Location = new System.Drawing.Point(5, 156);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(22, 20);
			this.label8.TabIndex = 15;
			this.label8.Text = "D3: ";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label8.UseCompatibleTextRendering = true;
			// 
			// delta_3Tb
			// 
			this.delta_3Tb.Location = new System.Drawing.Point(33, 157);
			this.delta_3Tb.Name = "delta_3Tb";
			this.delta_3Tb.Size = new System.Drawing.Size(33, 20);
			this.delta_3Tb.TabIndex = 16;
			this.delta_3Tb.Text = "50";
			this.delta_3Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.delta_3Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// pitch_3Tb
			// 
			this.pitch_3Tb.Location = new System.Drawing.Point(117, 157);
			this.pitch_3Tb.Name = "pitch_3Tb";
			this.pitch_3Tb.Size = new System.Drawing.Size(33, 20);
			this.pitch_3Tb.TabIndex = 14;
			this.pitch_3Tb.Text = "100";
			this.pitch_3Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.pitch_3Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(90, 122);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(22, 20);
			this.label5.TabIndex = 13;
			this.label5.Text = "P2: ";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label5.UseCompatibleTextRendering = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.RebarType2_Cb);
			this.groupBox3.Controls.Add(this.RebarType1_Cb);
			this.groupBox3.Controls.Add(this.label13);
			this.groupBox3.Controls.Add(this.RebarShap2_Cb);
			this.groupBox3.Controls.Add(this.RebarShap1_Cb);
			this.groupBox3.Controls.Add(this.label11);
			this.groupBox3.Controls.Add(this.label10);
			this.groupBox3.Controls.Add(this.yes2_Chb);
			this.groupBox3.Controls.Add(this.FB2_Tb);
			this.groupBox3.Controls.Add(this.CB2_Tb);
			this.groupBox3.Controls.Add(this.label9);
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Controls.Add(this.yes1_Chb);
			this.groupBox3.Controls.Add(this.label12);
			this.groupBox3.Controls.Add(this.FB1_Tb);
			this.groupBox3.Controls.Add(this.label14);
			this.groupBox3.Controls.Add(this.CB1_Tb);
			this.groupBox3.Location = new System.Drawing.Point(571, 279);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(319, 235);
			this.groupBox3.TabIndex = 14;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Bottom Rebar Setting";
			this.groupBox3.UseCompatibleTextRendering = true;
			// 
			// RebarType2_Cb
			// 
			this.RebarType2_Cb.FormattingEnabled = true;
			this.RebarType2_Cb.Location = new System.Drawing.Point(226, 172);
			this.RebarType2_Cb.Name = "RebarType2_Cb";
			this.RebarType2_Cb.Size = new System.Drawing.Size(78, 21);
			this.RebarType2_Cb.TabIndex = 33;
			// 
			// RebarType1_Cb
			// 
			this.RebarType1_Cb.FormattingEnabled = true;
			this.RebarType1_Cb.Location = new System.Drawing.Point(118, 170);
			this.RebarType1_Cb.Name = "RebarType1_Cb";
			this.RebarType1_Cb.Size = new System.Drawing.Size(78, 21);
			this.RebarType1_Cb.TabIndex = 32;
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
			// RebarShap2_Cb
			// 
			this.RebarShap2_Cb.FormattingEnabled = true;
			this.RebarShap2_Cb.Location = new System.Drawing.Point(226, 133);
			this.RebarShap2_Cb.Name = "RebarShap2_Cb";
			this.RebarShap2_Cb.Size = new System.Drawing.Size(78, 21);
			this.RebarShap2_Cb.TabIndex = 30;
			// 
			// RebarShap1_Cb
			// 
			this.RebarShap1_Cb.FormattingEnabled = true;
			this.RebarShap1_Cb.Location = new System.Drawing.Point(118, 131);
			this.RebarShap1_Cb.Name = "RebarShap1_Cb";
			this.RebarShap1_Cb.Size = new System.Drawing.Size(78, 21);
			this.RebarShap1_Cb.TabIndex = 29;
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
			// yes2_Chb
			// 
			this.yes2_Chb.Checked = true;
			this.yes2_Chb.CheckState = System.Windows.Forms.CheckState.Checked;
			this.yes2_Chb.Location = new System.Drawing.Point(226, 35);
			this.yes2_Chb.Name = "yes2_Chb";
			this.yes2_Chb.Size = new System.Drawing.Size(52, 24);
			this.yes2_Chb.TabIndex = 26;
			this.yes2_Chb.Text = "Yes";
			this.yes2_Chb.UseCompatibleTextRendering = true;
			this.yes2_Chb.UseVisualStyleBackColor = true;
			// 
			// FB2_Tb
			// 
			this.FB2_Tb.Location = new System.Drawing.Point(226, 67);
			this.FB2_Tb.Name = "FB2_Tb";
			this.FB2_Tb.Size = new System.Drawing.Size(33, 20);
			this.FB2_Tb.TabIndex = 24;
			this.FB2_Tb.Text = "3";
			this.FB2_Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.FB2_Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// CB2_Tb
			// 
			this.CB2_Tb.Location = new System.Drawing.Point(226, 96);
			this.CB2_Tb.Name = "CB2_Tb";
			this.CB2_Tb.Size = new System.Drawing.Size(33, 20);
			this.CB2_Tb.TabIndex = 25;
			this.CB2_Tb.Text = "100";
			this.CB2_Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.CB2_Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(10, 67);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(34, 20);
			this.label9.TabIndex = 23;
			this.label9.Text = "FB:";
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
			// yes1_Chb
			// 
			this.yes1_Chb.Checked = true;
			this.yes1_Chb.CheckState = System.Windows.Forms.CheckState.Checked;
			this.yes1_Chb.Location = new System.Drawing.Point(118, 35);
			this.yes1_Chb.Name = "yes1_Chb";
			this.yes1_Chb.Size = new System.Drawing.Size(52, 24);
			this.yes1_Chb.TabIndex = 21;
			this.yes1_Chb.Text = "Yes";
			this.yes1_Chb.UseCompatibleTextRendering = true;
			this.yes1_Chb.UseVisualStyleBackColor = true;
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
			// FB1_Tb
			// 
			this.FB1_Tb.Location = new System.Drawing.Point(118, 67);
			this.FB1_Tb.Name = "FB1_Tb";
			this.FB1_Tb.Size = new System.Drawing.Size(33, 20);
			this.FB1_Tb.TabIndex = 5;
			this.FB1_Tb.Text = "6";
			this.FB1_Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.FB1_Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// label14
			// 
			this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label14.Location = new System.Drawing.Point(6, 96);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(38, 20);
			this.label14.TabIndex = 6;
			this.label14.Text = "CB: ";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label14.UseCompatibleTextRendering = true;
			// 
			// CB1_Tb
			// 
			this.CB1_Tb.Location = new System.Drawing.Point(118, 96);
			this.CB1_Tb.Name = "CB1_Tb";
			this.CB1_Tb.Size = new System.Drawing.Size(33, 20);
			this.CB1_Tb.TabIndex = 8;
			this.CB1_Tb.Text = "50";
			this.CB1_Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.CB1_Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.label16);
			this.groupBox4.Controls.Add(this.yesC1_Chb);
			this.groupBox4.Controls.Add(this.opp_C1_Chb);
			this.groupBox4.Controls.Add(this.label17);
			this.groupBox4.Controls.Add(this.n3_C1Tb);
			this.groupBox4.Controls.Add(this.label18);
			this.groupBox4.Controls.Add(this.label19);
			this.groupBox4.Controls.Add(this.delta_3_C1Tb);
			this.groupBox4.Controls.Add(this.pitch_3_C1Tb);
			this.groupBox4.Controls.Add(this.label20);
			this.groupBox4.Controls.Add(this.label21);
			this.groupBox4.Controls.Add(this.pitch_2_C1Tb);
			this.groupBox4.Controls.Add(this.label22);
			this.groupBox4.Controls.Add(this.factor_C1Tb);
			this.groupBox4.Controls.Add(this.label23);
			this.groupBox4.Controls.Add(this.delta_1_C1Tb);
			this.groupBox4.Controls.Add(this.pitch_1_C1Tb);
			this.groupBox4.Location = new System.Drawing.Point(276, 279);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(289, 235);
			this.groupBox4.TabIndex = 23;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "C Rebar Setting";
			this.groupBox4.UseCompatibleTextRendering = true;
			// 
			// label16
			// 
			this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label16.Location = new System.Drawing.Point(6, 20);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(52, 20);
			this.label16.TabIndex = 22;
			this.label16.Text = "Yes/No";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label16.UseCompatibleTextRendering = true;
			// 
			// yesC1_Chb
			// 
			this.yesC1_Chb.Checked = true;
			this.yesC1_Chb.CheckState = System.Windows.Forms.CheckState.Checked;
			this.yesC1_Chb.Location = new System.Drawing.Point(64, 19);
			this.yesC1_Chb.Name = "yesC1_Chb";
			this.yesC1_Chb.Size = new System.Drawing.Size(47, 24);
			this.yesC1_Chb.TabIndex = 21;
			this.yesC1_Chb.Text = "Yes";
			this.yesC1_Chb.UseCompatibleTextRendering = true;
			this.yesC1_Chb.UseVisualStyleBackColor = true;
			// 
			// opp_C1_Chb
			// 
			this.opp_C1_Chb.Location = new System.Drawing.Point(6, 199);
			this.opp_C1_Chb.Name = "opp_C1_Chb";
			this.opp_C1_Chb.Size = new System.Drawing.Size(129, 24);
			this.opp_C1_Chb.TabIndex = 20;
			this.opp_C1_Chb.Text = "Opposite Direction";
			this.opp_C1_Chb.UseCompatibleTextRendering = true;
			this.opp_C1_Chb.UseVisualStyleBackColor = true;
			// 
			// label17
			// 
			this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label17.Location = new System.Drawing.Point(186, 155);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(46, 20);
			this.label17.TabIndex = 19;
			this.label17.Text = "N3_C: ";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label17.UseCompatibleTextRendering = true;
			// 
			// n3_C1Tb
			// 
			this.n3_C1Tb.Location = new System.Drawing.Point(238, 155);
			this.n3_C1Tb.Name = "n3_C1Tb";
			this.n3_C1Tb.Size = new System.Drawing.Size(33, 20);
			this.n3_C1Tb.TabIndex = 18;
			this.n3_C1Tb.Text = "4";
			this.n3_C1Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.n3_C1Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// label18
			// 
			this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label18.Location = new System.Drawing.Point(89, 156);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(38, 20);
			this.label18.TabIndex = 17;
			this.label18.Text = "P3_C: ";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label18.UseCompatibleTextRendering = true;
			// 
			// label19
			// 
			this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label19.Location = new System.Drawing.Point(5, 156);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(40, 20);
			this.label19.TabIndex = 15;
			this.label19.Text = "D3_C: ";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label19.UseCompatibleTextRendering = true;
			// 
			// delta_3_C1Tb
			// 
			this.delta_3_C1Tb.Location = new System.Drawing.Point(50, 157);
			this.delta_3_C1Tb.Name = "delta_3_C1Tb";
			this.delta_3_C1Tb.Size = new System.Drawing.Size(33, 20);
			this.delta_3_C1Tb.TabIndex = 16;
			this.delta_3_C1Tb.Text = "50";
			this.delta_3_C1Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.delta_3_C1Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// pitch_3_C1Tb
			// 
			this.pitch_3_C1Tb.Location = new System.Drawing.Point(132, 156);
			this.pitch_3_C1Tb.Name = "pitch_3_C1Tb";
			this.pitch_3_C1Tb.Size = new System.Drawing.Size(33, 20);
			this.pitch_3_C1Tb.TabIndex = 14;
			this.pitch_3_C1Tb.Text = "100";
			this.pitch_3_C1Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.pitch_3_C1Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// label20
			// 
			this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label20.Location = new System.Drawing.Point(90, 122);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(37, 20);
			this.label20.TabIndex = 13;
			this.label20.Text = "P2_C: ";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label20.UseCompatibleTextRendering = true;
			// 
			// label21
			// 
			this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label21.Location = new System.Drawing.Point(6, 50);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(39, 20);
			this.label21.TabIndex = 4;
			this.label21.Text = "FC :";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label21.UseCompatibleTextRendering = true;
			// 
			// pitch_2_C1Tb
			// 
			this.pitch_2_C1Tb.Location = new System.Drawing.Point(133, 121);
			this.pitch_2_C1Tb.Name = "pitch_2_C1Tb";
			this.pitch_2_C1Tb.Size = new System.Drawing.Size(33, 20);
			this.pitch_2_C1Tb.TabIndex = 7;
			this.pitch_2_C1Tb.Text = "200";
			this.pitch_2_C1Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.pitch_2_C1Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// label22
			// 
			this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label22.Location = new System.Drawing.Point(90, 82);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(37, 20);
			this.label22.TabIndex = 12;
			this.label22.Text = "P1_C: ";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label22.UseCompatibleTextRendering = true;
			// 
			// factor_C1Tb
			// 
			this.factor_C1Tb.Location = new System.Drawing.Point(51, 50);
			this.factor_C1Tb.Name = "factor_C1Tb";
			this.factor_C1Tb.Size = new System.Drawing.Size(33, 20);
			this.factor_C1Tb.TabIndex = 5;
			this.factor_C1Tb.Text = "4";
			this.factor_C1Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.factor_C1Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// label23
			// 
			this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label23.Location = new System.Drawing.Point(6, 82);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(39, 20);
			this.label23.TabIndex = 6;
			this.label23.Text = "D1_C: ";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label23.UseCompatibleTextRendering = true;
			// 
			// delta_1_C1Tb
			// 
			this.delta_1_C1Tb.Location = new System.Drawing.Point(51, 83);
			this.delta_1_C1Tb.Name = "delta_1_C1Tb";
			this.delta_1_C1Tb.Size = new System.Drawing.Size(33, 20);
			this.delta_1_C1Tb.TabIndex = 8;
			this.delta_1_C1Tb.Text = "50";
			this.delta_1_C1Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.delta_1_C1Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// pitch_1_C1Tb
			// 
			this.pitch_1_C1Tb.Location = new System.Drawing.Point(133, 82);
			this.pitch_1_C1Tb.Name = "pitch_1_C1Tb";
			this.pitch_1_C1Tb.Size = new System.Drawing.Size(33, 20);
			this.pitch_1_C1Tb.TabIndex = 2;
			this.pitch_1_C1Tb.Text = "100";
			this.pitch_1_C1Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.pitch_1_C1Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// SettingDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(992, 526);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.saveSettingBtn);
			this.Controls.Add(this.groupBox1);
			this.Name = "SettingDialog";
			this.Text = "Setting";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.ResumeLayout(false);
		}
		public System.Windows.Forms.TextBox pitch_1_C1Tb;
		public System.Windows.Forms.TextBox delta_1_C1Tb;
		private System.Windows.Forms.Label label23;
		public System.Windows.Forms.TextBox factor_C1Tb;
		private System.Windows.Forms.Label label22;
		public System.Windows.Forms.TextBox pitch_2_C1Tb;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label20;
		public System.Windows.Forms.TextBox pitch_3_C1Tb;
		public System.Windows.Forms.TextBox delta_3_C1Tb;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label18;
		public System.Windows.Forms.TextBox n3_C1Tb;
		private System.Windows.Forms.Label label17;
		public System.Windows.Forms.CheckBox opp_C1_Chb;
		public System.Windows.Forms.CheckBox yesC1_Chb;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.GroupBox groupBox4;
		public System.Windows.Forms.CheckBox yesST_Chb;
		private System.Windows.Forms.Label label15;
		public System.Windows.Forms.TextBox CB1_Tb;
		private System.Windows.Forms.Label label14;
		public System.Windows.Forms.TextBox FB1_Tb;
		private System.Windows.Forms.Label label12;
		public System.Windows.Forms.CheckBox yes1_Chb;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label9;
		public System.Windows.Forms.TextBox CB2_Tb;
		public System.Windows.Forms.TextBox FB2_Tb;
		public System.Windows.Forms.CheckBox yes2_Chb;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		public System.Windows.Forms.ComboBox RebarShap1_Cb;
		public System.Windows.Forms.ComboBox RebarShap2_Cb;
		private System.Windows.Forms.Label label13;
		public System.Windows.Forms.ComboBox RebarType1_Cb;
		public System.Windows.Forms.ComboBox RebarType2_Cb;
		private System.Windows.Forms.GroupBox groupBox3;
		public System.Windows.Forms.CheckBox opp_Chb;
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
