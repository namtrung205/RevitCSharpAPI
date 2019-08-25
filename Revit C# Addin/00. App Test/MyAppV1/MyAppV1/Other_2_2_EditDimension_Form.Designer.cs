namespace MyAppV1
{
    partial class EditDimension_Form
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
            this.updateValue_Bt = new System.Windows.Forms.Button();
            this.listValue_Tb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // updateValue_Bt
            // 
            this.updateValue_Bt.Location = new System.Drawing.Point(393, 11);
            this.updateValue_Bt.Name = "updateValue_Bt";
            this.updateValue_Bt.Size = new System.Drawing.Size(111, 23);
            this.updateValue_Bt.TabIndex = 0;
            this.updateValue_Bt.Text = "Update Value";
            this.updateValue_Bt.UseCompatibleTextRendering = true;
            this.updateValue_Bt.UseVisualStyleBackColor = true;
            this.updateValue_Bt.Click += new System.EventHandler(this.UpdateValue_BtClick);
            // 
            // listValue_Tb
            // 
            this.listValue_Tb.Location = new System.Drawing.Point(118, 11);
            this.listValue_Tb.Name = "listValue_Tb";
            this.listValue_Tb.Size = new System.Drawing.Size(247, 20);
            this.listValue_Tb.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "List Dim Value";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.UseCompatibleTextRendering = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(393, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Reset Dimension";
            this.button1.UseCompatibleTextRendering = true;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // EditDimension_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 87);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listValue_Tb);
            this.Controls.Add(this.updateValue_Bt);
            this.Name = "EditDimension_Form";
            this.Text = "EditDim_Form";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox listValue_Tb;
        private System.Windows.Forms.Button updateValue_Bt;


    }
}