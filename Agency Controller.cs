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

namespace Project
{
    public partial class AgencyForm : Form
    {
        Controller controllerObj;
        int currentAgencyID;
        public AgencyForm(int id)
        {
            InitializeComponent();
            controllerObj = new Controller();
            currentAgencyID = id;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void New_event_name_TextChanged(object sender, EventArgs e)
        {
            //event name
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            //venue
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //event date
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //start at
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //max capacity
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            //event category
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            //inventory item 1
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            //inventory item 2
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            //inventory item 3
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            //discount %
        }

        private void wallet_SelectedIndexChanged(object sender, EventArgs e)
        {
            //tickets category
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // 1. Validation: Ensure required fields are filled
            if (New_event_name.Text == "" || comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Please fill in the Event Name and select a Venue.");
                return;
            }

            // 2. Get Venue ID safely (Prevent crash if nothing is selected)
            int selectedVenueID;
            if (comboBox1.SelectedValue != null)
            {
                selectedVenueID = Convert.ToInt32(comboBox1.SelectedValue);
            }
            else
            {
                MessageBox.Show("Invalid Venue Selection");
                return;
            }

            // 3. Call the Controller Function
            // We must pass EXACTLY 13 arguments to match your new function definition.
            int result = controllerObj.Insert_New_Event(
                New_event_name.Text,                            // 1. Name
                selectedVenueID,                                // 2. VenueID (int)
                dateTimePicker1.Value.ToString("yyyy-MM-dd"),   // 3. Date
                textBox3.Text,                                  // 4. StartTime
                textBox2.Text,                                  // 5. Capacity
                textBox5.Text,                                  // 6. Event Category
                wallet.Text,                                    // 7. Tickets Category
                "0",                                            // 8. Price (Set to "0" or your textbox)
                currentAgencyID,                                // 9. AgencyID
                textBox9.Text,                                  // 10. Discount
                textBox6.Text,                                  // 11. Inventory Item 1 <--- MISSING PART FIXED
                textBox8.Text,                                  // 12. Inventory Item 2 <--- MISSING PART FIXED
                textBox7.Text                                   // 13. Inventory Item 3 <--- MISSING PART FIXED
            );

            // 4. Check Result
            if (result > 0)
            {
                MessageBox.Show("Event and Inventory Created Successfully!");
            }
            else
            {
                MessageBox.Show("Error: Event creation failed.");
            }
        }

        private void AgencyForm_Load(object sender, EventArgs e)
        {
                DataTable dt = controllerObj.getVenues();

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "Venue_ID";
                comboBox1.SelectedIndex = -1;
        }
    }
}
