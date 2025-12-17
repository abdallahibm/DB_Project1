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
using System.Windows.Forms.VisualStyles;

namespace Project
{
    public partial class Members_Booking : Form
    {

        Controller controllerObj;
        string username;
        int currentEventIDToDelete;
        string currentCategoryToDelete;
        int eventID;
        public Members_Booking()
        {
            InitializeComponent();
            controllerObj = new Controller();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Members_Booking_Load(object sender, EventArgs e)
        {
            
            DataTable dt = controllerObj.getavalaibleevents();
            
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "Name";

            comboBox1.ValueMember = "Event_ID";
            comboBox1.SelectedIndex = -1;

        }

        public void getusername(string user)
        {
            username = user;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.SelectedValue != null && comboBox1.SelectedValue is int)
            {
                eventID = Convert.ToInt32(comboBox1.SelectedValue);


                DataTable dt = controllerObj.getticketcategories(eventID);

                comboBox2.DataSource = dt;
                comboBox2.DisplayMember = "Category";
                comboBox2.ValueMember = "Price";
                textBox1.Text = "";
                comboBox2.SelectedIndex = -1;
                comboBox3.SelectedIndex = -1;
                textBox2.Text = "";
                textBox5.Text = "";

            }
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedValue != null)
            {
                textBox1.Text = comboBox2.SelectedValue.ToString();
            }
            if (!(comboBox2.SelectedIndex == -1))
            {
                DataRowView drv = (DataRowView)comboBox2.SelectedItem;
                string category = drv["Category"].ToString();


                int available = controllerObj.checkticketavailability(eventID, category);

                textBox5.Text = available.ToString();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            
              
                if (textBox1.Text != "" && comboBox3.SelectedItem != null)
                {
                    decimal price = Convert.ToDecimal(textBox1.Text);
                    int count = Convert.ToInt32(comboBox3.SelectedItem.ToString());
                    decimal total = price * count;
                    textBox2.Text = total.ToString();
                }
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
               
                if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null || comboBox3.SelectedItem == null)
                {
                    MessageBox.Show("Please select an Event, Category, and Number of Tickets.");
                    return;
                }
                string paymentMethod = "";

                if (radioButton1.Checked)
                {
                    paymentMethod = "Wallet";
                }
                else if (radioButton2.Checked)
                {
                    paymentMethod = "Visa";
                }
                else if (radioButton3.Checked)
                {
                    paymentMethod = "Cash on Door";
                }
                else
                {
                    MessageBox.Show("Please select a Payment Method.");
                    return;
                }


                int eventID = Convert.ToInt32(comboBox1.SelectedValue);

                DataRowView row = (DataRowView)comboBox2.SelectedItem;
                string category = row["Category"].ToString();

                int count = Convert.ToInt32(comboBox3.SelectedItem.ToString());

                string result = controllerObj.BookTickets(username, eventID, category, count, paymentMethod);

                if (result == "Success")
                {
                    MessageBox.Show("Booking Successful! You booked " + count + " tickets.");
                }
                else
                {
                    MessageBox.Show("Booking Failed: " + result);
                }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
                if (string.IsNullOrEmpty(username)) return;

                DataTable dt = controllerObj.GetMemberBookingsDetailed(username);
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("No bookings found.");
                    return;
                }

               
                Form5 form = new Form5(dt);
            form.ShowDialog();
               
                if (!(form.SelectedEventID==0))
                {
                    this.currentEventIDToDelete = form.SelectedEventID;
                    this.currentCategoryToDelete = form.SelectedCategory;
                    textBox3.Text = form.SelectedEventName;
                    textBox4.Text = Convert.ToString(form.SelectedCount);
                  
                }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
                // 1. Validation: Check if a booking was actually selected from the popup
                // We check 'currentEventIDToDelete' which we set in the previous button function
                if (currentEventIDToDelete == 0 || string.IsNullOrEmpty(currentCategoryToDelete))
                {
                    MessageBox.Show("Please click 'show bookings', select a row, and try again.");
                    return;
                }

                // 2. Confirmation Dialog
                DialogResult result = MessageBox.Show("Are you sure you want to delete this booking? This cannot be undone.",
                                                      "Confirm Delete",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);

                if (result == DialogResult.No) return; // User clicked No

                // 3. Get the Ticket Count (needed to update inventory)
                int count = 0;
                if (!int.TryParse(textBox4.Text, out count))
                {
                    MessageBox.Show("Error reading ticket count.");
                    return;
                }

                // 4. Call Controller
                string msg = controllerObj.DeleteMemberBooking(username, currentEventIDToDelete, currentCategoryToDelete, count);

                // 5. Handle Result
                if (msg == "Success")
                {
                    MessageBox.Show("Booking Deleted Successfully. Tickets have been returned to inventory.");

                    // Clear the UI boxes
                    textBox3.Text = "";
                    textBox4.Text = "";

                    // Reset the hidden variables so they can't delete the same thing twice
                    currentEventIDToDelete = 0;
                    currentCategoryToDelete = "";
                }
                else
                {
                    MessageBox.Show("Deletion Failed: " + msg);
                }
            
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
