namespace AutoMakeLiningConcrete_Form
{
    partial class SelectFamilyNameForms
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.offsetLb = new System.Windows.Forms.Label();
            this.offsetDisTb = new System.Windows.Forms.TextBox();
            this.NameLb = new System.Windows.Forms.Label();
            this.OkBt = new System.Windows.Forms.Button();
            this.FamilysCb = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.offsetLb);
            this.groupBox1.Controls.Add(this.offsetDisTb);
            this.groupBox1.Controls.Add(this.NameLb);
            this.groupBox1.Controls.Add(this.OkBt);
            this.groupBox1.Controls.Add(this.FamilysCb);
            this.groupBox1.Location = new System.Drawing.Point(26, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(411, 87);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Setting";
            // 
            // offsetLb
            // 
            this.offsetLb.AutoSize = true;
            this.offsetLb.Location = new System.Drawing.Point(7, 56);
            this.offsetLb.Name = "offsetLb";
            this.offsetLb.Size = new System.Drawing.Size(102, 13);
            this.offsetLb.TabIndex = 5;
            this.offsetLb.Text = "Offset Distance(mm)";
            this.offsetLb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // offsetDisTb
            // 
            this.offsetDisTb.Location = new System.Drawing.Point(161, 49);
            this.offsetDisTb.Name = "offsetDisTb";
            this.offsetDisTb.Size = new System.Drawing.Size(51, 20);
            this.offsetDisTb.TabIndex = 4;
            this.offsetDisTb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
            // 
            // NameLb
            // 
            this.NameLb.AutoSize = true;
            this.NameLb.Location = new System.Drawing.Point(7, 26);
            this.NameLb.Name = "NameLb";
            this.NameLb.Size = new System.Drawing.Size(126, 13);
            this.NameLb.TabIndex = 2;
            this.NameLb.Text = "Family Name( Floor Type)";
            this.NameLb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OkBt
            // 
            this.OkBt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OkBt.Location = new System.Drawing.Point(306, 19);
            this.OkBt.Name = "OkBt";
            this.OkBt.Size = new System.Drawing.Size(75, 50);
            this.OkBt.TabIndex = 1;
            this.OkBt.Text = "OK";
            this.OkBt.UseVisualStyleBackColor = true;
            this.OkBt.Click += new System.EventHandler(this.OkClick);
            // 
            // FamilysCb
            // 
            this.FamilysCb.FormattingEnabled = true;
            this.FamilysCb.Location = new System.Drawing.Point(161, 18);
            this.FamilysCb.Name = "FamilysCb";
            this.FamilysCb.Size = new System.Drawing.Size(121, 21);
            this.FamilysCb.TabIndex = 0;
            // 
            // SelectFamilyNameForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 114);
            this.Controls.Add(this.groupBox1);
            this.Name = "SelectFamilyNameForms";
            this.Text = "AMLC Setting";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button OkBt;
        public System.Windows.Forms.ComboBox FamilysCb;
        private System.Windows.Forms.Label offsetLb;
        public System.Windows.Forms.TextBox offsetDisTb;
        private System.Windows.Forms.Label NameLb;
    }
}