namespace Project
{
    partial class Form5
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
            this.mybookings = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.mybookings)).BeginInit();
            this.SuspendLayout();
            // 
            // mybookings
            // 
            this.mybookings.BackgroundColor = System.Drawing.Color.CornflowerBlue;
            this.mybookings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mybookings.Location = new System.Drawing.Point(25, 32);
            this.mybookings.Name = "mybookings";
            this.mybookings.RowHeadersWidth = 51;
            this.mybookings.RowTemplate.Height = 24;
            this.mybookings.Size = new System.Drawing.Size(741, 369);
            this.mybookings.TabIndex = 0;
            this.mybookings.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.mybookings_CellContentClick);
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mybookings);
            this.Name = "Form5";
            this.Text = "my bookings";
            this.Load += new System.EventHandler(this.Form5_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mybookings)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView mybookings;
    }
}