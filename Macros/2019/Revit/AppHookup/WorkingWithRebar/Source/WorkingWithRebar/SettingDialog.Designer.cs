/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 5/10/2019
 * Time: 8:37 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace WorkingWithRebar
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
			this.saveSettingBtn = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.GroupName_Tb1 = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.Partition1_Tb = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.RebarCount_Tb = new System.Windows.Forms.TextBox();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// saveSettingBtn
			// 
			this.saveSettingBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.saveSettingBtn.Location = new System.Drawing.Point(201, 128);
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
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Controls.Add(this.GroupName_Tb1);
			this.groupBox3.Controls.Add(this.label9);
			this.groupBox3.Controls.Add(this.Partition1_Tb);
			this.groupBox3.Controls.Add(this.label14);
			this.groupBox3.Controls.Add(this.RebarCount_Tb);
			this.groupBox3.Location = new System.Drawing.Point(12, 12);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(293, 110);
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
			this.GroupName_Tb1.Size = new System.Drawing.Size(168, 20);
			this.GroupName_Tb1.TabIndex = 24;
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(6, 51);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(88, 20);
			this.label9.TabIndex = 23;
			this.label9.Text = "Partition Name:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label9.UseCompatibleTextRendering = true;
			// 
			// Partition1_Tb
			// 
			this.Partition1_Tb.Location = new System.Drawing.Point(114, 51);
			this.Partition1_Tb.Name = "Partition1_Tb";
			this.Partition1_Tb.Size = new System.Drawing.Size(168, 20);
			this.Partition1_Tb.TabIndex = 5;
			// 
			// label14
			// 
			this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label14.Location = new System.Drawing.Point(2, 80);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(92, 20);
			this.label14.TabIndex = 6;
			this.label14.Text = "Rebar Count:";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label14.UseCompatibleTextRendering = true;
			// 
			// RebarCount_Tb
			// 
			this.RebarCount_Tb.Location = new System.Drawing.Point(114, 80);
			this.RebarCount_Tb.Name = "RebarCount_Tb";
			this.RebarCount_Tb.Size = new System.Drawing.Size(30, 20);
			this.RebarCount_Tb.TabIndex = 8;
			this.RebarCount_Tb.Text = "1";
			this.RebarCount_Tb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.RebarCount_Tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_Box_KeyPress);
			// 
			// SettingDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(317, 172);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.saveSettingBtn);
			this.Name = "SettingDialog";
			this.Text = "Setting";
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);
        }
        public System.Windows.Forms.TextBox RebarCount_Tb;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.TextBox Partition1_Tb;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button saveSettingBtn;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox GroupName_Tb1;
		}
	}
