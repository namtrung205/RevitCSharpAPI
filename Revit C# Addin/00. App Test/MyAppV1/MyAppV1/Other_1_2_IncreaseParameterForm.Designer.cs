namespace MyAppV1
{
    partial class IncreaseParameterForm
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
            this.paraName_Tb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Pre_Tb = new System.Windows.Forms.TextBox();
            this.startNum_Tb = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Suf_Tb = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.OK_1_Bt = new System.Windows.Forms.Button();
            this.OK_2_Bt = new System.Windows.Forms.Button();
            this.OK_3_Bt = new System.Windows.Forms.Button();
            this.deCrease1_Cb = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.step_numUD = new System.Windows.Forms.NumericUpDown();
            this.X_Rb = new System.Windows.Forms.RadioButton();
            this.Y_Rb = new System.Windows.Forms.RadioButton();
            this.Z_Rb = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.step_numUD)).BeginInit();
            this.SuspendLayout();
            // 
            // paraName_Tb
            // 
            this.paraName_Tb.Location = new System.Drawing.Point(23, 40);
            this.paraName_Tb.Name = "paraName_Tb";
            this.paraName_Tb.Size = new System.Drawing.Size(128, 20);
            this.paraName_Tb.TabIndex = 0;
            this.paraName_Tb.Text = "INNO_Ten-CK";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(21, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Parameter Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.UseCompatibleTextRendering = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(189, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Prefix";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label2.UseCompatibleTextRendering = true;
            // 
            // Pre_Tb
            // 
            this.Pre_Tb.Location = new System.Drawing.Point(191, 40);
            this.Pre_Tb.Name = "Pre_Tb";
            this.Pre_Tb.Size = new System.Drawing.Size(75, 20);
            this.Pre_Tb.TabIndex = 3;
            // 
            // startNum_Tb
            // 
            this.startNum_Tb.Location = new System.Drawing.Point(278, 40);
            this.startNum_Tb.Name = "startNum_Tb";
            this.startNum_Tb.Size = new System.Drawing.Size(72, 20);
            this.startNum_Tb.TabIndex = 5;
            this.startNum_Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(275, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Start Number";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label3.UseCompatibleTextRendering = true;
            // 
            // Suf_Tb
            // 
            this.Suf_Tb.Location = new System.Drawing.Point(366, 40);
            this.Suf_Tb.Name = "Suf_Tb";
            this.Suf_Tb.Size = new System.Drawing.Size(75, 20);
            this.Suf_Tb.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(366, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "Suffix";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label4.UseCompatibleTextRendering = true;
            // 
            // OK_1_Bt
            // 
            this.OK_1_Bt.Location = new System.Drawing.Point(267, 75);
            this.OK_1_Bt.Name = "OK_1_Bt";
            this.OK_1_Bt.Size = new System.Drawing.Size(75, 23);
            this.OK_1_Bt.TabIndex = 8;
            this.OK_1_Bt.Text = "One-One";
            this.OK_1_Bt.UseCompatibleTextRendering = true;
            this.OK_1_Bt.UseVisualStyleBackColor = true;
            this.OK_1_Bt.Click += new System.EventHandler(this.OK_1_BtClick);
            // 
            // OK_2_Bt
            // 
            this.OK_2_Bt.Location = new System.Drawing.Point(354, 75);
            this.OK_2_Bt.Name = "OK_2_Bt";
            this.OK_2_Bt.Size = new System.Drawing.Size(75, 23);
            this.OK_2_Bt.TabIndex = 9;
            this.OK_2_Bt.Text = "Many-One";
            this.OK_2_Bt.UseCompatibleTextRendering = true;
            this.OK_2_Bt.UseVisualStyleBackColor = true;
            this.OK_2_Bt.Click += new System.EventHandler(this.OK_2_BtClick);
            // 
            // OK_3_Bt
            // 
            this.OK_3_Bt.Location = new System.Drawing.Point(442, 75);
            this.OK_3_Bt.Name = "OK_3_Bt";
            this.OK_3_Bt.Size = new System.Drawing.Size(75, 23);
            this.OK_3_Bt.TabIndex = 10;
            this.OK_3_Bt.Text = "Many-Many";
            this.OK_3_Bt.UseCompatibleTextRendering = true;
            this.OK_3_Bt.UseVisualStyleBackColor = true;
            this.OK_3_Bt.Click += new System.EventHandler(this.OK_3_BtClick);
            // 
            // deCrease1_Cb
            // 
            this.deCrease1_Cb.Location = new System.Drawing.Point(12, 69);
            this.deCrease1_Cb.Name = "deCrease1_Cb";
            this.deCrease1_Cb.Size = new System.Drawing.Size(122, 36);
            this.deCrease1_Cb.TabIndex = 11;
            this.deCrease1_Cb.Text = "Decrease/Reverse";
            this.deCrease1_Cb.UseCompatibleTextRendering = true;
            this.deCrease1_Cb.UseVisualStyleBackColor = true;
            this.deCrease1_Cb.CheckedChanged += new System.EventHandler(this.DeCrease1_CbCheckedChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(445, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 18);
            this.label5.TabIndex = 13;
            this.label5.Text = "Step";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label5.UseCompatibleTextRendering = true;
            // 
            // step_numUD
            // 
            this.step_numUD.Location = new System.Drawing.Point(447, 40);
            this.step_numUD.Minimum = new decimal(new int[] {
                                    1,
                                    0,
                                    0,
                                    0});
            this.step_numUD.Name = "step_numUD";
            this.step_numUD.Size = new System.Drawing.Size(62, 20);
            this.step_numUD.TabIndex = 14;
            this.step_numUD.Value = new decimal(new int[] {
                                    1,
                                    0,
                                    0,
                                    0});
            // 
            // X_Rb
            // 
            this.X_Rb.Checked = true;
            this.X_Rb.Location = new System.Drawing.Point(140, 78);
            this.X_Rb.Name = "X_Rb";
            this.X_Rb.Size = new System.Drawing.Size(26, 24);
            this.X_Rb.TabIndex = 15;
            this.X_Rb.TabStop = true;
            this.X_Rb.Text = "X";
            this.X_Rb.UseCompatibleTextRendering = true;
            this.X_Rb.UseVisualStyleBackColor = true;
            // 
            // Y_Rb
            // 
            this.Y_Rb.Location = new System.Drawing.Point(184, 78);
            this.Y_Rb.Name = "Y_Rb";
            this.Y_Rb.Size = new System.Drawing.Size(26, 24);
            this.Y_Rb.TabIndex = 16;
            this.Y_Rb.TabStop = true;
            this.Y_Rb.Text = "Y";
            this.Y_Rb.UseCompatibleTextRendering = true;
            this.Y_Rb.UseVisualStyleBackColor = true;
            // 
            // Z_Rb
            // 
            this.Z_Rb.Location = new System.Drawing.Point(223, 78);
            this.Z_Rb.Name = "Z_Rb";
            this.Z_Rb.Size = new System.Drawing.Size(26, 24);
            this.Z_Rb.TabIndex = 17;
            this.Z_Rb.TabStop = true;
            this.Z_Rb.Text = "Z";
            this.Z_Rb.UseCompatibleTextRendering = true;
            this.Z_Rb.UseVisualStyleBackColor = true;
            // 
            // IncreaseParameterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 114);
            this.Controls.Add(this.Z_Rb);
            this.Controls.Add(this.Y_Rb);
            this.Controls.Add(this.X_Rb);
            this.Controls.Add(this.step_numUD);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.deCrease1_Cb);
            this.Controls.Add(this.OK_3_Bt);
            this.Controls.Add(this.OK_2_Bt);
            this.Controls.Add(this.OK_1_Bt);
            this.Controls.Add(this.Suf_Tb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.startNum_Tb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Pre_Tb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.paraName_Tb);
            this.Name = "IncreaseParameterForm";
            this.Text = "Increase Parameter";
            ((System.ComponentModel.ISupportInitialize)(this.step_numUD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        public System.Windows.Forms.RadioButton Z_Rb;
        public System.Windows.Forms.RadioButton Y_Rb;
        public System.Windows.Forms.RadioButton X_Rb;
        public System.Windows.Forms.NumericUpDown step_numUD;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.CheckBox deCrease1_Cb;
        private System.Windows.Forms.Button OK_3_Bt;
        private System.Windows.Forms.Button OK_2_Bt;
        private System.Windows.Forms.Button OK_1_Bt;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox Suf_Tb;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox startNum_Tb;
        public System.Windows.Forms.TextBox Pre_Tb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox paraName_Tb;
    }
}