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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace Project
{
    public partial class Form4 : Form
    {
        Controller controllerObj = new Controller();
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string firstName = textBox1.Text;
            string lastName = textBox2.Text;
            string SocialSecurityNumber = textBox3.Text;
            string Phonenumber = textBox4.Text;
            string username = textBox5.Text;
            string password = textBox6.Text;

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(SocialSecurityNumber) || string.IsNullOrEmpty(Phonenumber))
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
            bool success = controller.AddNewUsher(firstName, lastName, username, password, SocialSecurityNumber);
            bool another = controller.AddNewUsherPhone(SocialSecurityNumber, Phonenumber);

            if (success && another)
            {
                MessageBox.Show("Usher added successfully!", "Success");

                Form2 f2 = new Form2();
                f2.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Username already exists.", "Error");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //first name
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //last name
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //SSN
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //Phone number
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            //username
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            //password
        }
    }   
}