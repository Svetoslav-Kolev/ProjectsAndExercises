namespace Kursova
{
    partial class InformationForm
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
            this.Close = new System.Windows.Forms.Button();
            this.lblText = new System.Windows.Forms.Label();
            this.lblText2 = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblText3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Close
            // 
            this.Close.Location = new System.Drawing.Point(713, 415);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(75, 23);
            this.Close.TabIndex = 0;
            this.Close.Text = "Close";
            this.Close.UseVisualStyleBackColor = true;
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(148, 20);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(516, 13);
            this.lblText.TabIndex = 1;
            this.lblText.Text = "Author : Svetoslav Kolev , STD first course , faculty number : 1801681012,  email" +
    ": svetoslav_kolev@mail.bg";
            // 
            // lblText2
            // 
            this.lblText2.AutoSize = true;
            this.lblText2.Location = new System.Drawing.Point(197, 82);
            this.lblText2.Name = "lblText2";
            this.lblText2.Size = new System.Drawing.Size(412, 13);
            this.lblText2.TabIndex = 2;
            this.lblText2.Text = "This Application has been created as a Course project for the first Course of uni" +
    "versity ";
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(12, 415);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 3;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // lblText3
            // 
            this.lblText3.AutoSize = true;
            this.lblText3.Location = new System.Drawing.Point(249, 149);
            this.lblText3.Name = "lblText3";
            this.lblText3.Size = new System.Drawing.Size(286, 13);
            this.lblText3.TabIndex = 4;
            this.lblText3.Text = "Created on Visual Studio with C# Windows application form";
            // 
            // InformationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblText3);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.lblText2);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.Close);
            this.Name = "InformationForm";
            this.Text = "InformationForm";
            this.Load += new System.EventHandler(this.InformationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Close;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Label lblText2;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblText3;
    }
}