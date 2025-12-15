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
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Members_Login_Button_Click(object sender, EventArgs e)
        {
            Members_Login login = new Members_Login();
            login.Show();
            this.Hide();
        }

        private void Sign_Up_Button_Click(object sender, EventArgs e)
        {
            Members_Sign_Up_form sign = new Members_Sign_Up_form();
            this.Hide();
            sign.ShowDialog();
            this.Show();
        }





        private void Admin_Login_Button_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }
    }
}
