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

            if (New_event_name.Text == "" || comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Please fill in the Event Name and select a Venue.");
                return;
            }

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

            int result = controllerObj.Insert_New_Event(
                New_event_name.Text,                            
                selectedVenueID,                                
                dateTimePicker1.Value.ToString("yyyy-MM-dd"),   
                textBox3.Text,                                  
                textBox2.Text,                                  
                textBox5.Text,                                  
                wallet.Text,                                    
                "0",                                            
                currentAgencyID,                                
                textBox9.Text,                                  
                textBox6.Text,                                  
                textBox8.Text,                                  
                textBox7.Text                                   
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


            DataTable dtEvents = controllerObj.GetAgencyEvents(currentAgencyID);

            comboBox2.DataSource = dtEvents;
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "Event_ID";  

            comboBox2.SelectedIndex = -1;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox2.SelectedValue == null) return;

            string val = comboBox2.SelectedValue.ToString();
            if (val == "System.Data.DataRowView") return;

            int eventID = Convert.ToInt32(val);
            string status = controllerObj.GetEventStatus(eventID);

            textBox4.Text = status;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
