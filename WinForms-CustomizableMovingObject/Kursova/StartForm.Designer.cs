namespace Kursova
{
    partial class StartForm
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
            this.btnStartApp = new System.Windows.Forms.Button();
            this.btnInfoForm = new System.Windows.Forms.Button();
            this.btnExitStartForm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStartApp
            // 
            this.btnStartApp.Location = new System.Drawing.Point(342, 119);
            this.btnStartApp.Name = "btnStartApp";
            this.btnStartApp.Size = new System.Drawing.Size(150, 23);
            this.btnStartApp.TabIndex = 0;
            this.btnStartApp.Text = "Start Application";
            this.btnStartApp.UseVisualStyleBackColor = true;
            this.btnStartApp.Click += new System.EventHandler(this.btnStartApp_Click);
            // 
            // btnInfoForm
            // 
            this.btnInfoForm.Location = new System.Drawing.Point(342, 194);
            this.btnInfoForm.Name = "btnInfoForm";
            this.btnInfoForm.Size = new System.Drawing.Size(150, 23);
            this.btnInfoForm.TabIndex = 1;
            this.btnInfoForm.Text = "Information";
            this.btnInfoForm.UseVisualStyleBackColor = true;
            this.btnInfoForm.Click += new System.EventHandler(this.btnInfoForm_Click);
            // 
            // btnExitStartForm
            // 
            this.btnExitStartForm.Location = new System.Drawing.Point(342, 280);
            this.btnExitStartForm.Name = "btnExitStartForm";
            this.btnExitStartForm.Size = new System.Drawing.Size(150, 23);
            this.btnExitStartForm.TabIndex = 2;
            this.btnExitStartForm.Text = "Exit";
            this.btnExitStartForm.UseVisualStyleBackColor = true;
            this.btnExitStartForm.Click += new System.EventHandler(this.btnExitStartForm_Click);
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnExitStartForm);
            this.Controls.Add(this.btnInfoForm);
            this.Controls.Add(this.btnStartApp);
            this.Name = "StartForm";
            this.Text = " Starting Menu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStartApp;
        private System.Windows.Forms.Button btnInfoForm;
        private System.Windows.Forms.Button btnExitStartForm;
    }
}