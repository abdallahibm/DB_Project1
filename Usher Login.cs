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
    public partial class Ushers_Login : Form
    {
        public Ushers_Login()
        {
            InitializeComponent();
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
            string result = controller.ValidateUsherLogin(username, password);

            switch (result)
            {
                case "SUCCESS":
                    MessageBox.Show("Login successful!");

                    long Usher_ID = controller.GetUsherID(username);
                    Usher f3 = new Usher(Usher_ID);
                    f3.Show();
                    this.Hide();
                    break;

                case "WRONG_PASSWORD":
                    MessageBox.Show("Password is incorrect!");
                    break;

                case "NOT_Usher":
                    MessageBox.Show("This username is not registered as an Usher.");
                    break;

                default:
                    MessageBox.Show("An error occurred. Please try again.");
                    break;
            }
        }

        private void Ushers_Login_Load(object sender, EventArgs e)
        {

        }
    }
}
