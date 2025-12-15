namespace Project
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Sign_Up_Button = new System.Windows.Forms.Button();
            this.Members_Login_Button = new System.Windows.Forms.Button();
            this.Admin_Login_Button = new System.Windows.Forms.Button();
            this.Ushers_Login_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Sitka Banner", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(69, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = "New Member?";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Sitka Banner", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.Location = new System.Drawing.Point(69, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(285, 50);
            this.label2.TabIndex = 1;
            this.label2.Text = "Already Registered?";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // Sign_Up_Button
            // 
            this.Sign_Up_Button.Font = new System.Drawing.Font("Sitka Banner", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Sign_Up_Button.Location = new System.Drawing.Point(440, 48);
            this.Sign_Up_Button.Name = "Sign_Up_Button";
            this.Sign_Up_Button.Size = new System.Drawing.Size(87, 36);
            this.Sign_Up_Button.TabIndex = 2;
            this.Sign_Up_Button.Text = "Sign Up";
            this.Sign_Up_Button.UseVisualStyleBackColor = true;
            // 
            // Members_Login_Button
            // 
            this.Members_Login_Button.Font = new System.Drawing.Font("Sitka Banner", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Members_Login_Button.Location = new System.Drawing.Point(440, 121);
            this.Members_Login_Button.Name = "Members_Login_Button";
            this.Members_Login_Button.Size = new System.Drawing.Size(87, 36);
            this.Members_Login_Button.TabIndex = 3;
            this.Members_Login_Button.Text = "Login";
            this.Members_Login_Button.UseVisualStyleBackColor = true;
            this.Members_Login_Button.Click += new System.EventHandler(this.Members_Login_Button_Click);
            // 
            // Admin_Login_Button
            // 
            this.Admin_Login_Button.Font = new System.Drawing.Font("Sitka Banner", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Admin_Login_Button.Location = new System.Drawing.Point(67, 235);
            this.Admin_Login_Button.Name = "Admin_Login_Button";
            this.Admin_Login_Button.Size = new System.Drawing.Size(192, 36);
            this.Admin_Login_Button.TabIndex = 4;
            this.Admin_Login_Button.Text = "Login as an Administrator";
            this.Admin_Login_Button.UseVisualStyleBackColor = true;
            // 
            // Ushers_Login_Button
            // 
            this.Ushers_Login_Button.Font = new System.Drawing.Font("Sitka Banner", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Ushers_Login_Button.Location = new System.Drawing.Point(440, 235);
            this.Ushers_Login_Button.Name = "Ushers_Login_Button";
            this.Ushers_Login_Button.Size = new System.Drawing.Size(131, 36);
            this.Ushers_Login_Button.TabIndex = 5;
            this.Ushers_Login_Button.Text = "Login as an Usher";
            this.Ushers_Login_Button.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 417);
            this.Controls.Add(this.Ushers_Login_Button);
            this.Controls.Add(this.Admin_Login_Button);
            this.Controls.Add(this.Members_Login_Button);
            this.Controls.Add(this.Sign_Up_Button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Welcome Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Sign_Up_Button;
        private System.Windows.Forms.Button Members_Login_Button;
        private System.Windows.Forms.Button Admin_Login_Button;
        private System.Windows.Forms.Button Ushers_Login_Button;
    }
}

