namespace IncreaseParameterForm
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
            this.OK_Bt = new System.Windows.Forms.Button();
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
            this.label1.Location = new System.Drawing.Point(23, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Parameter Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.UseCompatibleTextRendering = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(191, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Prefix";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label2.UseCompatibleTextRendering = true;
            // 
            // Pre_Tb
            // 
            this.Pre_Tb.Location = new System.Drawing.Point(191, 40);
            this.Pre_Tb.Name = "Pre_Tb";
            this.Pre_Tb.Size = new System.Drawing.Size(62, 20);
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
            this.label3.Location = new System.Drawing.Point(278, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "Start Number";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label3.UseCompatibleTextRendering = true;
            // 
            // Suf_Tb
            // 
            this.Suf_Tb.Location = new System.Drawing.Point(379, 40);
            this.Suf_Tb.Name = "Suf_Tb";
            this.Suf_Tb.Size = new System.Drawing.Size(62, 20);
            this.Suf_Tb.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(379, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 23);
            this.label4.TabIndex = 6;
            this.label4.Text = "Suffix";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label4.UseCompatibleTextRendering = true;
            // 
            // OK_Bt
            // 
            this.OK_Bt.Location = new System.Drawing.Point(469, 37);
            this.OK_Bt.Name = "OK_Bt";
            this.OK_Bt.Size = new System.Drawing.Size(75, 23);
            this.OK_Bt.TabIndex = 8;
            this.OK_Bt.Text = "OK";
            this.OK_Bt.UseCompatibleTextRendering = true;
            this.OK_Bt.UseVisualStyleBackColor = true;
            this.OK_Bt.Click += new System.EventHandler(this.OK_BtClick);
            // 
            // IncreaseParameterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 88);
            this.Controls.Add(this.OK_Bt);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.Button OK_Bt;
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