﻿namespace AritiafelTestForm
{
    partial class MainForm
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
            this.btnMessageBox = new System.Windows.Forms.Button();
            this.btnMessageBox2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnMessageBox
            // 
            this.btnMessageBox.Location = new System.Drawing.Point(45, 38);
            this.btnMessageBox.Name = "btnMessageBox";
            this.btnMessageBox.Size = new System.Drawing.Size(130, 57);
            this.btnMessageBox.TabIndex = 0;
            this.btnMessageBox.Text = "Show Message";
            this.btnMessageBox.UseVisualStyleBackColor = true;
            this.btnMessageBox.Click += new System.EventHandler(this.btnMessageBox_Click);
            // 
            // btnMessageBox2
            // 
            this.btnMessageBox2.Location = new System.Drawing.Point(45, 117);
            this.btnMessageBox2.Name = "btnMessageBox2";
            this.btnMessageBox2.Size = new System.Drawing.Size(130, 57);
            this.btnMessageBox2.TabIndex = 1;
            this.btnMessageBox2.Text = "Show Message";
            this.btnMessageBox2.UseVisualStyleBackColor = true;
            this.btnMessageBox2.Click += new System.EventHandler(this.btnMessageBox2_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnMessageBox2);
            this.Controls.Add(this.btnMessageBox);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnMessageBox;
        private System.Windows.Forms.Button btnMessageBox2;
    }
}
