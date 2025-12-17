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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Project
{
    public partial class Agency_Login : Form
    {
        Controller controllerObj = new Controller();


        public Agency_Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Password_Agency_Login_TextChanged(object sender, EventArgs e)
        {

        }

        private void Username_Agency_Login_TextChanged(object sender, EventArgs e)
        {

        }

        private void Agency_Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (Password_Agency_Login.Text == "" || Username_Agency_Login.Text == "")
            {
                MessageBox.Show("Please Enter a Valid Username and Password");
            }

            string result = controllerObj.ValidateAgencyLogin(Username_Agency_Login.Text, Password_Agency_Login.Text);

            if (result != null)
            {
                switch (result)
                {
                    case "SUCCESS":
                        int Agency_ID = controllerObj.GetAgencyID(Username_Agency_Login.Text);

                        AgencyForm f3 = new AgencyForm(Agency_ID);
                        f3.Show();
                        this.Hide();
                        break;

                    case "WRONG_PASSWORD":
                        MessageBox.Show("Password is Incorrect!");
                        break;

                    case "NOT_AGENCY":
                        MessageBox.Show("This username is not registered as an agency.");
                        break;

                    default:
                        MessageBox.Show("An error occurred. Please try again.");
                        break;
                }
            }
        }
    }
}