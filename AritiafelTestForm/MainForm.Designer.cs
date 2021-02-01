namespace AritiafelTestForm
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.btnInputForm = new System.Windows.Forms.Button();
            this.btnShowMessageByResource = new System.Windows.Forms.Button();
            this.btnTranslateZHCN = new System.Windows.Forms.Button();
            this.txtText = new System.Windows.Forms.TextBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnSaveUTF8 = new System.Windows.Forms.Button();
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
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(45, 199);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(130, 57);
            this.btnOpenFile.TabIndex = 2;
            this.btnOpenFile.Text = "Open File";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // btnInputForm
            // 
            this.btnInputForm.Location = new System.Drawing.Point(45, 282);
            this.btnInputForm.Name = "btnInputForm";
            this.btnInputForm.Size = new System.Drawing.Size(130, 57);
            this.btnInputForm.TabIndex = 3;
            this.btnInputForm.Text = "Open Input Form";
            this.btnInputForm.UseVisualStyleBackColor = true;
            this.btnInputForm.Click += new System.EventHandler(this.btnInputForm_Click);
            // 
            // btnShowMessageByResource
            // 
            this.btnShowMessageByResource.Location = new System.Drawing.Point(202, 38);
            this.btnShowMessageByResource.Name = "btnShowMessageByResource";
            this.btnShowMessageByResource.Size = new System.Drawing.Size(130, 57);
            this.btnShowMessageByResource.TabIndex = 4;
            this.btnShowMessageByResource.Text = "Show Message By Resource";
            this.btnShowMessageByResource.UseVisualStyleBackColor = true;
            this.btnShowMessageByResource.Click += new System.EventHandler(this.btnShowMessageByResource_Click);
            // 
            // btnTranslateZHCN
            // 
            this.btnTranslateZHCN.Location = new System.Drawing.Point(648, 38);
            this.btnTranslateZHCN.Name = "btnTranslateZHCN";
            this.btnTranslateZHCN.Size = new System.Drawing.Size(140, 60);
            this.btnTranslateZHCN.TabIndex = 5;
            this.btnTranslateZHCN.Text = "翻譯繁轉簡";
            this.btnTranslateZHCN.UseVisualStyleBackColor = true;
            this.btnTranslateZHCN.Click += new System.EventHandler(this.btnTranslateZHCN_Click);
            // 
            // txtText
            // 
            this.txtText.Location = new System.Drawing.Point(390, 38);
            this.txtText.Multiline = true;
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(211, 136);
            this.txtText.TabIndex = 6;
            this.txtText.Text = "國內新冠疫情拉警報，疫苗採購至今卻仍是深陷五里霧中狀況不明，對此，前民進黨立委沈富雄直批疫情中心指揮官陳時中「稱有關疫苗的供應不要怪台灣，繼續怪台灣（處境）會更困" +
    "難，其實他應講不要怪我、不要怪時中，因時中禁不起壓力，他就會給你莫名其妙的理由」、「若將來陳要參選北市長，把他說過的話找出來就能凸顯他有多蠢」！";
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(390, 203);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(211, 136);
            this.txtOutput.TabIndex = 7;
            // 
            // btnSaveUTF8
            // 
            this.btnSaveUTF8.Location = new System.Drawing.Point(652, 115);
            this.btnSaveUTF8.Name = "btnSaveUTF8";
            this.btnSaveUTF8.Size = new System.Drawing.Size(135, 58);
            this.btnSaveUTF8.TabIndex = 8;
            this.btnSaveUTF8.Text = "存成UTF-8";
            this.btnSaveUTF8.UseVisualStyleBackColor = true;
            this.btnSaveUTF8.Click += new System.EventHandler(this.btnSaveUTF8_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnSaveUTF8);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.btnTranslateZHCN);
            this.Controls.Add(this.btnShowMessageByResource);
            this.Controls.Add(this.btnInputForm);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.btnMessageBox2);
            this.Controls.Add(this.btnMessageBox);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMessageBox;
        private System.Windows.Forms.Button btnMessageBox2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Button btnInputForm;
        private System.Windows.Forms.Button btnShowMessageByResource;
        private System.Windows.Forms.Button btnTranslateZHCN;
        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnSaveUTF8;
    }
}

