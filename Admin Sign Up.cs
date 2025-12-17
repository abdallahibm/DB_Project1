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
    public partial class Admin_Sign_Up : Form
    {
        public Admin_Sign_Up()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string firstName = textBox1.Text;       
            string lastName = textBox2.Text;         
            string username = textBox3.Text; 
            string password = textBox4.Text;

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters.");
                return;
            }

            Controller controller = new Controller();
            bool success = controller.AddNewAdmin(firstName, lastName, username, password);

            if (success)
            {
                MessageBox.Show("Administrator added successfully!", "Success");

               
                Form2 f2 = new Form2();
                f2.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Username already exists.", "Error");
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Admin_Sign_Up_Load(object sender, EventArgs e)
        {

        }
    }
}