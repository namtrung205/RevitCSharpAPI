namespace MakeGroupRebarByHost
{
    partial class SettingDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
		private void InitializeComponent()
        {
            this.saveSettingBtn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GroupName_Tb1 = new System.Windows.Forms.TextBox();
            this.Value1_Tb = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.Value2_Tb = new System.Windows.Forms.TextBox();
            this.Para_2Tb = new System.Windows.Forms.TextBox();
            this.Para_1Tb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveSettingBtn
            // 
            this.saveSettingBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveSettingBtn.Location = new System.Drawing.Point(236, 184);
            this.saveSettingBtn.Name = "saveSettingBtn";
            this.saveSettingBtn.Size = new System.Drawing.Size(104, 42);
            this.saveSettingBtn.TabIndex = 2;
            this.saveSettingBtn.Text = "OK";
            this.saveSettingBtn.UseCompatibleTextRendering = true;
            this.saveSettingBtn.UseVisualStyleBackColor = true;
            this.saveSettingBtn.Click += new System.EventHandler(this.SaveSettingBtnClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.Para_1Tb);
            this.groupBox3.Controls.Add(this.Para_2Tb);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.GroupName_Tb1);
            this.groupBox3.Controls.Add(this.Value1_Tb);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.Value2_Tb);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(328, 166);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Rebar Setting";
            this.groupBox3.UseCompatibleTextRendering = true;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 20);
            this.label1.TabIndex = 25;
            this.label1.Text = "Group Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.UseCompatibleTextRendering = true;
            // 
            // GroupName_Tb1
            // 
            this.GroupName_Tb1.Location = new System.Drawing.Point(114, 21);
            this.GroupName_Tb1.Name = "GroupName_Tb1";
            this.GroupName_Tb1.Size = new System.Drawing.Size(199, 20);
            this.GroupName_Tb1.TabIndex = 24;
            // 
            // Value1_Tb
            // 
            this.Value1_Tb.Location = new System.Drawing.Point(177, 97);
            this.Value1_Tb.Name = "Value1_Tb";
            this.Value1_Tb.Size = new System.Drawing.Size(136, 20);
            this.Value1_Tb.TabIndex = 5;
            this.Value1_Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(6, 50);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(74, 20);
            this.label14.TabIndex = 6;
            this.label14.Text = "Parameters:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label14.UseCompatibleTextRendering = true;
            // 
            // Value2_Tb
            // 
            this.Value2_Tb.Location = new System.Drawing.Point(177, 132);
            this.Value2_Tb.Name = "Value2_Tb";
            this.Value2_Tb.Size = new System.Drawing.Size(136, 20);
            this.Value2_Tb.TabIndex = 8;
            this.Value2_Tb.Text = "1";
            this.Value2_Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Value2_Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
            // 
            // Para_2Tb
            // 
            this.Para_2Tb.Location = new System.Drawing.Point(6, 132);
            this.Para_2Tb.Name = "Para_2Tb";
            this.Para_2Tb.Size = new System.Drawing.Size(155, 20);
            this.Para_2Tb.TabIndex = 26;
            this.Para_2Tb.Text = "COUNT_REBAR";
            // 
            // Para_1Tb
            // 
            this.Para_1Tb.Location = new System.Drawing.Point(6, 97);
            this.Para_1Tb.Name = "Para_1Tb";
            this.Para_1Tb.Size = new System.Drawing.Size(155, 20);
            this.Para_1Tb.TabIndex = 27;
            this.Para_1Tb.Text = "Partition";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(55, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 24);
            this.label2.TabIndex = 28;
            this.label2.Text = "Parameter";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.UseCompatibleTextRendering = true;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(232, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 24);
            this.label3.TabIndex = 29;
            this.label3.Text = "Value";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.UseCompatibleTextRendering = true;
            // 
            // SettingDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 235);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.saveSettingBtn);
            this.Name = "SettingDialog";
            this.Text = "Setting";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }
        public System.Windows.Forms.TextBox Value2_Tb;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.TextBox Value1_Tb;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button saveSettingBtn;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox GroupName_Tb1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox Para_1Tb;
        public System.Windows.Forms.TextBox Para_2Tb;
    }
    #endregion




}