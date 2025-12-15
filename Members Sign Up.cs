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
    public partial class Members_Sign_Up_form : Form
    {
        Controller controllerObj;
        public Members_Sign_Up_form()
        {
            InitializeComponent();
            controllerObj = new Controller();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private bool isvalidemail(string email)
        {
            if (!email.Contains("@") || !email.Contains("."))
            {
                return false;
            }

            if (email.IndexOf('@') == 0 || email.IndexOf('@') == email.Length - 1)
            {
                return false;
            }

            if (email.IndexOf('.') == 0 || email.IndexOf('.') == email.Length - 1)
            {
                return false;
            }
            return true;
        }
        private bool isvalidnumber(string phone)
        {
            if (phone.Length != 11)
            {
                return false;
            }
            string prefix = phone.Substring(0, 3);
            if (prefix != "010" && prefix != "011" && prefix != "012" && prefix != "015")
            {
                return false;
            }
            for (int i = 3; i < phone.Length; i++)
            {

                if (phone[i] < '0' || phone[i] > '9')
                {
                    return false;
                }
            }

            return true;
        }
        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Members_Sign_Up_form_Load(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        private bool checkdateformat(string date)
        {
            if (date.Length != 10)
            {
                return false;
            }
            for (int i = 0; i < date.Length; i++)
            {
                if (i == 4 || i == 7)
                {
                    if (date[i] != '-')
                    {
                        return false;
                    }
                }
                else
                {
                    if (date[i] < '0' || date[i] > '9')
                    {
                        return false; 
                    }
                }
            }
            return true;
        }
        private bool isvaliddate(string date)
        {
            int year =Convert.ToInt32( date.Substring(0, 4));
           
            if( year>2009)
            {
                MessageBox.Show("you must be above 16 ");
                return false;
            }
            
            int month= Convert.ToInt32(date.Substring(5, 2));
            if(month>12 || month<01)
            {
                return false;
            }
            int day= Convert.ToInt32(date.Substring(8, 2));
             
            if(month==01 || month==03|| month==05||month==7||month ==8||month==10||month==12 )
            {
                if(day<01 ||day>31)
                {
                  
                    return false;
                }
            }else if( month==02)
            {
                if (day < 01 || day > 28)
                {
                    return false;
                }

            }
            else
            {
                if (day < 01 || day > 30)
                {
                    return false;
                }
            }
            return true;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string fname = textBox3.Text;
            string lname = textBox4.Text;
            string dob = textBox6.Text;
            string gender =comboBox1.Text;
            string ssn = textBox5.Text;
            string email = textBox7.Text;
            string phone = textBox8.Text;
            string nation = textBox9.Text;
            string user = textBox1.Text;
            string pass = textBox2.Text;
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a gender.");
                return; 
            }
            if (fname == ""|| lname == "" || dob == "" || ssn == "" || email == "" || phone == "" || nation == "" || user == "" || pass == "")
            {
                MessageBox.Show("Please fill in all required fields!");
                return; 
            }
            if (controllerObj.checkuser(user))
            {
                MessageBox.Show("User already exist please change it. ");
                return;
            }
            if(controllerObj.Checkemail(email))
            {
                MessageBox.Show("Email already exist please login. ");
                this.Close();
            }
           if(!checkdateformat(dob))
            {
                MessageBox.Show("Invalid Date Format! Please use format: yyyy-mm-dd");
                return;
            }
           if(!isvaliddate(dob))
            {
                MessageBox.Show("enter a valid date");
                return;
            }
            if (ssn.Length!= 14)
            {
                MessageBox.Show("Invalid SSN. It must be exactly 14 digits.");
                return;
            }
            if(controllerObj.checkssn(ssn))
            {
                MessageBox.Show("The SSN is already exist. please revise it. ");
                return;
            }
            if(!isvalidemail(email))
            {
                MessageBox.Show("enter a valid email");
                return;
            }
            if(!isvalidnumber(phone))
            {
                MessageBox.Show("enter a valid phone");
                return;
            }

            int result = controllerObj.Insert_New_Member(fname,lname,dob, gender,ssn,email,phone,nation,user,pass);

            if (result > 0)
            {
                MessageBox.Show("Account Created Successfully.");
                this.Close(); 
            }
            else
            {
                MessageBox.Show("Registration Failed, Please check your data.");
            }
        }
    }
}
