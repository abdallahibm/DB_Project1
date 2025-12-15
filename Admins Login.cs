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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            Controller controller = new Controller();
            string result = controller.ValidateAdminLogin(username, password);

            switch (result)
            {
                case "SUCCESS":
                    // Login successful
                    MessageBox.Show("Login successful!");

                    Form3 f3 = new Form3();
                    f3.Show();
                    
                    
                    this.Hide(); // Hide login form
                    break;

                case "WRONG_PASSWORD":
                    MessageBox.Show("Password is incorrect!");
                    break;

                case "USER_NOT_FOUND":
                    MessageBox.Show("Username not found. You need to sign up as an administrator.");

                    // Open Admin Sign Up form
                    Admin_Sign_Up signUpForm = new Admin_Sign_Up();
                    signUpForm.Show();
                    this.Hide(); // Optional: hide login form
                    break;

                case "NOT_ADMIN":
                    MessageBox.Show("This username is not registered as an administrator.");
                    break;

                default:
                    MessageBox.Show("An error occurred. Please try again.");
                    break;
            }

        }
    }
}
