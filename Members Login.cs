using DBapplication;
using System.Windows.Forms;
using System;

namespace Project
{
    public partial class Members_Login : Form
    {
        Controller controllerObj;

        public Members_Login()
        {
            InitializeComponent();
            controllerObj = new Controller();
        }

        private void Members_Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = textBox1.Text;
            string pass = textBox2.Text;
            
            if (user == "" || pass == "")
            {
                MessageBox.Show("Please enter both Username and Password.");
                return;
            }

            if (controllerObj.issuspended(user))
            {
                MessageBox.Show("you are suspended");
                return;
            }

            string ssn = controllerObj.Login_Member(user, pass);

            if (ssn != null)
            {
                Members_Booking b = new Members_Booking();
                b.getusername(user);
                b.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid Username or Password. Please try again.");
            }
        }


    } 
}
