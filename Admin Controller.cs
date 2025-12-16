using DBapplication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class Form3 : Form
    {
        public int currentAdminID;

        public Form3()
        {
            InitializeComponent();
            Controller controllerObj = new Controller();
            controllerObj.LoadEventComboBoxInAdmin(comboBox2);
        }

        public int AdminID
        {
            get { return currentAdminID; }
            set { currentAdminID = value; }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter a username to unsuspend.");
                return;
            }

            Controller controllerObj = new Controller();
            bool success = controllerObj.UnsuspendAccount(username);

            // 4. Show result
            if (success)
            {
                MessageBox.Show($"Account '{username}' unsuspended successfully.");
                textBox1.Clear(); // Clear the textbox
                textBox1.Focus(); // Put cursor back for next entry
            }
            else
            {
                MessageBox.Show($"Failed to unsuspend '{username}'. User may not be suspended.");
            }





        }

        private void button2_Click(object sender, EventArgs e)
        {
            Controller controllerObj = new Controller();
            string username = textBox1.Text;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter a username to suspend.");
                return;
            }


            string result = controllerObj.SuspendAccount(currentAdminID, username);

            switch (result)
            {
                case "SUCCESS":
                    MessageBox.Show($"Suspended: {username}");
                    break;
                case "USER_NOT_FOUND":
                    MessageBox.Show($"User '{username}' does not exist.");
                    break;
                case "CANNOT_SUSPEND_ADMIN":
                    MessageBox.Show($"Cannot suspend another administrator ({username}).");
                    break;
                case "ALREADY_SUSPENDED":
                    MessageBox.Show($"User '{username}' is already suspended.");
                    break;
                default:
                    MessageBox.Show("Suspension failed.");
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Controller controllerObj = new Controller();
            string username = textBox1.Text;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter a username to delete.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
       $"⚠️ WARNING: This will PERMANENTLY delete account '{username}'.\n\n" +
       "All user data will be lost forever.\n" +
       "This action cannot be undone!\n\n" +
       "Are you absolutely sure?",
       "CONFIRM PERMANENT DELETE",
       MessageBoxButtons.YesNo,
       MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
            {
                return; // User cancelled
            }

            bool success = controllerObj.DeleteAccount(username);

            // Show result
            if (success)
            {
                MessageBox.Show($"✅ Account '{username}' has been permanently deleted.",
                               "Deletion Successful");
                textBox1.Clear();
            }
            else
            {
                MessageBox.Show($"❌ Failed to delete '{username}'.\n" +
                               "User may not exist or has dependencies or is as Admin.",
                               "Deletion Failed");
            }











        }
    }
    
}
