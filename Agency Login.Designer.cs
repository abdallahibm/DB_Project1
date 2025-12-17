namespace Project
{
    partial class Agency_Login
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
            this.button1 = new System.Windows.Forms.Button();
            this.Password_Agency_Login = new System.Windows.Forms.TextBox();
            this.Username_Agency_Login = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Sitka Banner", 12F);
            this.button1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button1.Location = new System.Drawing.Point(435, 225);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 44);
            this.button1.TabIndex = 21;
            this.button1.Text = "Login";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Password_Agency_Login
            // 
            this.Password_Agency_Login.Location = new System.Drawing.Point(661, 113);
            this.Password_Agency_Login.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Password_Agency_Login.Name = "Password_Agency_Login";
            this.Password_Agency_Login.Size = new System.Drawing.Size(217, 22);
            this.Password_Agency_Login.TabIndex = 20;
            // 
            // Username_Agency_Login
            // 
            this.Username_Agency_Login.Location = new System.Drawing.Point(243, 113);
            this.Username_Agency_Login.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Username_Agency_Login.Name = "Username_Agency_Login";
            this.Username_Agency_Login.Size = new System.Drawing.Size(217, 22);
            this.Username_Agency_Login.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Sitka Banner", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.Location = new System.Drawing.Point(499, 91);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 48);
            this.label2.TabIndex = 18;
            this.label2.Text = "Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Sitka Banner", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(72, 91);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 48);
            this.label1.TabIndex = 17;
            this.label1.Text = "Username:";
            // 
            // Agency_Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(953, 361);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Password_Agency_Login);
            this.Controls.Add(this.Username_Agency_Login);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Agency_Login";
            this.Text = "Agency Login Form";
            this.Load += new System.EventHandler(this.Agency_Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox Password_Agency_Login;
        private System.Windows.Forms.TextBox Username_Agency_Login;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}