namespace AutoDimLevelOrGrid
{
    partial class SelectMaterialForms
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
            this.Ok = new System.Windows.Forms.Button();
            this.listMatCb = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Ok);
            this.groupBox1.Controls.Add(this.listMatCb);
            this.groupBox1.Location = new System.Drawing.Point(26, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 58);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Set Material";
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(155, 19);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(75, 23);
            this.Ok.TabIndex = 1;
            this.Ok.Text = "Paint";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.OkClick);
            // 
            // listMatCb
            // 
            this.listMatCb.FormattingEnabled = true;
            this.listMatCb.Location = new System.Drawing.Point(6, 19);
            this.listMatCb.Name = "listMatCb";
            this.listMatCb.Size = new System.Drawing.Size(121, 21);
            this.listMatCb.TabIndex = 0;
            // 
            // SelectMaterialForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 89);
            this.Controls.Add(this.groupBox1);
            this.Name = "SelectMaterialForms";
            this.Text = "SelectMaterialForms";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Ok;
        public System.Windows.Forms.ComboBox listMatCb;
    }
}