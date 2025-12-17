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

    public partial class Agency_Sign_Up : Form
    {
        public Agency_Sign_Up()
        {
            InitializeComponent();
        }

        private void Agency_Sign_Up_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string AgencyName = textBox1.Text;
            string Supervisor = textBox2.Text;
            string username = textBox3.Text;
            string password = textBox4.Text;

            if (string.IsNullOrEmpty(AgencyName) || string.IsNullOrEmpty(Supervisor) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
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
            bool success = controller.AddNewAgency(AgencyName, Supervisor, username, password);

            if (success)
            {
                MessageBox.Show("Agency added successfully!", "Success");

                Form2 f2 = new Form2();
                f2.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Username already exists.", "Error");
            }
        }
    }
}
