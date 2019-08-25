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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Oy_tb = new System.Windows.Forms.TextBox();
            this.onlyCenter_Cb = new System.Windows.Forms.CheckBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Ma_Tb = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // saveSetting_Btn
            // 
            this.saveSetting_Btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveSetting_Btn.Location = new System.Drawing.Point(613, 392);
            this.saveSetting_Btn.Name = "saveSetting_Btn";
            this.saveSetting_Btn.Size = new System.Drawing.Size(93, 33);
            this.saveSetting_Btn.TabIndex = 1;
            this.saveSetting_Btn.Text = "Save Setting";
            this.saveSetting_Btn.UseCompatibleTextRendering = true;
            this.saveSetting_Btn.UseVisualStyleBackColor = true;
            this.saveSetting_Btn.Click += new System.EventHandler(this.SaveSetting_BtnClick);
            // 
            // Ox_Tb
            // 
            this.Ox_Tb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Ox_Tb.Location = new System.Drawing.Point(570, 161);
            this.Ox_Tb.Name = "Ox_Tb";
            this.Ox_Tb.Size = new System.Drawing.Size(43, 20);
            this.Ox_Tb.TabIndex = 3;
            this.Ox_Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(323, 358);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "OX(mm)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.UseCompatibleTextRendering = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(323, 399);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 23);
            this.label2.TabIndex = 6;
            this.label2.Text = "OY(mm)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.UseCompatibleTextRendering = true;
            // 
            // Oy_tb
            // 
            this.Oy_tb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Oy_tb.Location = new System.Drawing.Point(546, 240);
            this.Oy_tb.Name = "Oy_tb";
            this.Oy_tb.Size = new System.Drawing.Size(47, 20);
            this.Oy_tb.TabIndex = 5;
            this.Oy_tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
            // 
            // onlyCenter_Cb
            // 
            this.onlyCenter_Cb.Location = new System.Drawing.Point(581, 358);
            this.onlyCenter_Cb.Name = "onlyCenter_Cb";
            this.onlyCenter_Cb.Size = new System.Drawing.Size(125, 24);
            this.onlyCenter_Cb.TabIndex = 7;
            this.onlyCenter_Cb.Text = "Only Locate Center";
            this.onlyCenter_Cb.UseCompatibleTextRendering = true;
            this.onlyCenter_Cb.UseVisualStyleBackColor = true;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::MyAppV1.Properties.Resources.SnapCrab_NoName_2019_8_19_16_36_57_No_00;
            this.pictureBox3.Location = new System.Drawing.Point(103, 306);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(214, 43);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 9;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::MyAppV1.Properties.Resources.SnapCrab_NoName_2019_8_19_16_15_42_No_00;
            this.pictureBox2.Location = new System.Drawing.Point(12, 13);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(305, 303);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MyAppV1.Properties.Resources.SnapCrab_NoName_2019_8_19_16_23_4_No_00;
            this.pictureBox1.Location = new System.Drawing.Point(323, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(386, 337);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(323, 424);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(270, 40);
            this.label3.TabIndex = 10;
            this.label3.Text = "***(OX, OY Để trống nếu chấp nhận giá trị mặc định = 8 lần tỉ lệ View)";
            this.label3.UseCompatibleTextRendering = true;
            // 
            // Ma_Tb
            // 
            this.Ma_Tb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.Ma_Tb.Location = new System.Drawing.Point(555, 63);
            this.Ma_Tb.Name = "Ma_Tb";
            this.Ma_Tb.Size = new System.Drawing.Size(47, 20);
            this.Ma_Tb.TabIndex = 11;
            this.Ma_Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
            // 
            // DimSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 469);
            this.Controls.Add(this.Ma_Tb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.onlyCenter_Cb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Oy_tb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Ox_Tb);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.saveSetting_Btn);
            this.Name = "DimSettingForm";
            this.Text = "DimPlanColumnForm";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.CheckBox onlyCenter_Cb;
        private System.Windows.Forms.TextBox Oy_tb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Ox_Tb;
        private System.Windows.Forms.Button saveSetting_Btn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Ma_Tb;
    }
}