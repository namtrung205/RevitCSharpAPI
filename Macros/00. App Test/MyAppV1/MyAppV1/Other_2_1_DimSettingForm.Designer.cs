namespace MyAppV1
{
    partial class DimSettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.saveSetting_Btn = new System.Windows.Forms.Button();
            this.Ox_Tb = new System.Windows.Forms.TextBox();
            this.Oy_tb = new System.Windows.Forms.TextBox();
            this.onlyCenter_Cb = new System.Windows.Forms.CheckBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Ma_Tb = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pickedCatergories_Clb = new System.Windows.Forms.CheckedListBox();
            this.checkAll_Bt = new System.Windows.Forms.Button();
            this.checkNone_Bt = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveSetting_Btn
            // 
            this.saveSetting_Btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveSetting_Btn.Location = new System.Drawing.Point(594, 435);
            this.saveSetting_Btn.Name = "saveSetting_Btn";
            this.saveSetting_Btn.Size = new System.Drawing.Size(125, 33);
            this.saveSetting_Btn.TabIndex = 1;
            this.saveSetting_Btn.Text = "Save Setting";
            this.saveSetting_Btn.UseCompatibleTextRendering = true;
            this.saveSetting_Btn.UseVisualStyleBackColor = true;
            this.saveSetting_Btn.Click += new System.EventHandler(this.SaveSetting_BtnClick);
            // 
            // Ox_Tb
            // 
            this.Ox_Tb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Ox_Tb.Location = new System.Drawing.Point(556, 148);
            this.Ox_Tb.Name = "Ox_Tb";
            this.Ox_Tb.Size = new System.Drawing.Size(43, 20);
            this.Ox_Tb.TabIndex = 3;
            this.Ox_Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
            // 
            // Oy_tb
            // 
            this.Oy_tb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Oy_tb.Location = new System.Drawing.Point(530, 220);
            this.Oy_tb.Name = "Oy_tb";
            this.Oy_tb.Size = new System.Drawing.Size(47, 20);
            this.Oy_tb.TabIndex = 5;
            this.Oy_tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
            // 
            // onlyCenter_Cb
            // 
            this.onlyCenter_Cb.Location = new System.Drawing.Point(565, 336);
            this.onlyCenter_Cb.Name = "onlyCenter_Cb";
            this.onlyCenter_Cb.Size = new System.Drawing.Size(125, 24);
            this.onlyCenter_Cb.TabIndex = 7;
            this.onlyCenter_Cb.Text = "Only Locate Center";
            this.onlyCenter_Cb.UseCompatibleTextRendering = true;
            this.onlyCenter_Cb.UseVisualStyleBackColor = true;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox3.Image = global::MyAppV1.Properties.Resources.SnapCrab_NoName_2019_8_19_16_36_57_No_00;
            this.pictureBox3.Location = new System.Drawing.Point(72, 293);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(226, 37);
            this.pictureBox3.TabIndex = 9;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox2.Image = global::MyAppV1.Properties.Resources.SnapCrab_NoName_2019_8_19_16_15_42_No_00;
            this.pictureBox2.Location = new System.Drawing.Point(-4, 6);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(305, 303);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MyAppV1.Properties.Resources.SnapCrab_NoName_2019_8_19_16_23_4_No_00;
            this.pictureBox1.Location = new System.Drawing.Point(304, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(386, 325);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Brown;
            this.label3.Location = new System.Drawing.Point(6, 351);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(307, 21);
            this.label3.TabIndex = 10;
            this.label3.Text = "*(Các giá trị  để trống nếu chấp nhận giá trị mặc định )";
            this.label3.UseCompatibleTextRendering = true;
            // 
            // Ma_Tb
            // 
            this.Ma_Tb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Ma_Tb.Location = new System.Drawing.Point(557, 67);
            this.Ma_Tb.Name = "Ma_Tb";
            this.Ma_Tb.Size = new System.Drawing.Size(47, 20);
            this.Ma_Tb.TabIndex = 11;
            this.Ma_Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(711, 404);
            this.tabControl1.TabIndex = 12;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pickedCatergories_Clb);
            this.tabPage1.Controls.Add(this.checkAll_Bt);
            this.tabPage1.Controls.Add(this.checkNone_Bt);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(703, 378);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Select Catergories to Dim";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // pickedCatergories_Clb
            // 
            this.pickedCatergories_Clb.CheckOnClick = true;
            this.pickedCatergories_Clb.FormattingEnabled = true;
            this.pickedCatergories_Clb.Location = new System.Drawing.Point(6, 9);
            this.pickedCatergories_Clb.Name = "pickedCatergories_Clb";
            this.pickedCatergories_Clb.Size = new System.Drawing.Size(187, 364);
            this.pickedCatergories_Clb.Sorted = true;
            this.pickedCatergories_Clb.TabIndex = 3;
            // 
            // checkAll_Bt
            // 
            this.checkAll_Bt.Location = new System.Drawing.Point(204, 9);
            this.checkAll_Bt.Name = "checkAll_Bt";
            this.checkAll_Bt.Size = new System.Drawing.Size(86, 23);
            this.checkAll_Bt.TabIndex = 2;
            this.checkAll_Bt.Text = "Check All";
            this.checkAll_Bt.UseVisualStyleBackColor = true;
            this.checkAll_Bt.Click += new System.EventHandler(this.checkAll_Bt_Click);
            // 
            // checkNone_Bt
            // 
            this.checkNone_Bt.Location = new System.Drawing.Point(204, 38);
            this.checkNone_Bt.Name = "checkNone_Bt";
            this.checkNone_Bt.Size = new System.Drawing.Size(86, 38);
            this.checkNone_Bt.TabIndex = 1;
            this.checkNone_Bt.Text = "Check None (Allow All)";
            this.checkNone_Bt.UseVisualStyleBackColor = true;
            this.checkNone_Bt.Click += new System.EventHandler(this.checkNone_Bt_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pictureBox3);
            this.tabPage2.Controls.Add(this.pictureBox2);
            this.tabPage2.Controls.Add(this.Ox_Tb);
            this.tabPage2.Controls.Add(this.onlyCenter_Cb);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.Oy_tb);
            this.tabPage2.Controls.Add(this.Ma_Tb);
            this.tabPage2.Controls.Add(this.pictureBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(703, 378);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Locate Columns On Plan";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // DimSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 482);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.saveSetting_Btn);
            this.Name = "DimSettingForm";
            this.Text = "DimPlanColumnForm";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.CheckBox onlyCenter_Cb;
        private System.Windows.Forms.TextBox Oy_tb;
        private System.Windows.Forms.TextBox Ox_Tb;
        private System.Windows.Forms.Button saveSetting_Btn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Ma_Tb;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckedListBox pickedCatergories_Clb;
        private System.Windows.Forms.Button checkAll_Bt;
        private System.Windows.Forms.Button checkNone_Bt;
    }
}