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
    public partial class Form3 : Form
    {
        public int currentAdminID;

        public void LoadEventComboBoxInAdmin(ComboBox comboBoxEventNames)
        {
            
            DBManager dbMan = new DBManager();

            
            string query = @"
        SELECT DISTINCT E.Name 
        FROM Events E, Create_Event CE 
        WHERE E.Event_ID = CE.Event_ID 
        AND CE.Status = 'Pending' 
        ORDER BY E.Name";

            DataTable events = dbMan.ExecuteReader(query);

            
            comboBoxEventNames.Items.Clear();

            if (events != null && events.Rows.Count > 0)
            {
                
                foreach (DataRow row in events.Rows)
                {
                    string eventName = row["Name"].ToString();
                    comboBoxEventNames.Items.Add(eventName);
                }

                
                comboBoxEventNames.SelectedIndex = 0;
            }
            else
            {
                comboBoxEventNames.Items.Add("No events found");
                comboBoxEventNames.SelectedIndex = 0;
            }
        }

        public void LoadUshersComboBox(ComboBox comboBox1)
        {
            Controller controllerObj = new Controller();
            DataTable ushers = controllerObj.GetAvailableUshers();

            comboBox1.Items.Clear();

            if (ushers != null && ushers.Rows.Count > 0)
            {
                foreach (DataRow row in ushers.Rows)
                {
                    comboBox1.Items.Add(row["Username"].ToString());
                }

                if (comboBox1.Items.Count > 0)
                {
                    comboBox1.SelectedIndex = 0;
                }
            }
            else
            {
                comboBox1.Items.Add("No ushers available");
                comboBox1.SelectedIndex = 0;
            }
        }

        public void RefreshAllData()
        {
            
            LoadEventComboBoxInAdmin(comboBox2);

            
            LoadUshersComboBox(comboBox1);

            
            MessageBox.Show("Data refreshed!");
        }



        public Form3()
        {
            InitializeComponent();
            
            LoadEventComboBoxInAdmin(comboBox2);
            LoadUshersComboBox(comboBox1);
        }

        public int AdminID
        {
            get { return currentAdminID; }
            set { currentAdminID = value; }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Agency_Sign_Up form = new Agency_Sign_Up();
            form.Show();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null || comboBox2.SelectedItem.ToString() == "No events found")
            {
                MessageBox.Show("Please select an event first.");
                return;
            }

            string selectedEvent = comboBox2.SelectedItem.ToString();
            Controller controllerObj = new Controller();

          
            int eventID = controllerObj.GetEventID(selectedEvent);
            if (eventID == -1)
            {
                MessageBox.Show("Error: Could not find event ID.");
                return;
            }

            
            bool approved = controllerObj.ApproveEvent(eventID, currentAdminID);

            if (approved)
            {
                
                if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() != "No ushers available")
                {
                    string selectedUsher = comboBox1.SelectedItem.ToString();
                    bool assigned = controllerObj.AssignUsherToEvent(selectedUsher, eventID);

                    if (assigned)
                    {
                        MessageBox.Show("Event approved and usher assigned successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Event approved but usher assignment failed (may already be assigned).");
                    }
                }
                else
                {
                    MessageBox.Show("Event approved successfully! No usher assigned.");
                }

                
                textBox3.Clear();
                comboBox2.SelectedIndex = -1;
                comboBox1.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Failed to approve event.");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter a username to unsuspend.");
                return;
            }

            Controller controllerObj = new Controller();
            bool success = controllerObj.UnsuspendAccount(username);

            
            if (success)
            {
                MessageBox.Show($"Account '{username}' unsuspended successfully.");
                textBox1.Clear(); 
                textBox1.Focus(); 
            }
            else
            {
                MessageBox.Show($"Failed to unsuspend '{username}'. User may not be suspended.");
            }





        }

        private void button2_Click(object sender, EventArgs e)
        {
            Controller controllerObj = new Controller();
            string username = textBox1.Text;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter a username to suspend.");
                return;
            }


            string result = controllerObj.SuspendAccount(currentAdminID, username);

            switch (result)
            {
                case "SUCCESS":
                    MessageBox.Show($"Suspended: {username}");
                    break;
                case "USER_NOT_FOUND":
                    MessageBox.Show($"User '{username}' does not exist.");
                    break;
                case "CANNOT_SUSPEND_ADMIN":
                    MessageBox.Show($"Cannot suspend another administrator ({username}).");
                    break;
                case "ALREADY_SUSPENDED":
                    MessageBox.Show($"User '{username}' is already suspended.");
                    break;
                default:
                    MessageBox.Show("Suspension failed.");
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Controller controllerObj = new Controller();
            string username = textBox1.Text;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please enter a username to delete.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
       $"⚠️ WARNING: This will PERMANENTLY delete account '{username}'.\n\n" +
       "All user data will be lost forever.\n" +
       "This action cannot be undone!\n\n" +
       "Are you absolutely sure?",
       "CONFIRM PERMANENT DELETE",
       MessageBoxButtons.YesNo,
       MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
            {
                return; 
            }

            bool success = controllerObj.DeleteAccount(username);

            
            if (success)
            {
                MessageBox.Show($"✅ Account '{username}' has been permanently deleted.",
                               "Deletion Successful");
                textBox1.Clear();
            }
            else
            {
                MessageBox.Show($"❌ Failed to delete '{username}'.\n" +
                               "User may not exist or has dependencies or is as Admin.",
                               "Deletion Failed");
            }











        }

        private void button4_Click(object sender, EventArgs e)
        {
            Admin_Sign_Up form = new Admin_Sign_Up();
            form.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 form = new Form4();
            form.Show();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null || comboBox2.SelectedItem.ToString() == "No events found")
            {
                MessageBox.Show("Please select an event first.");
                return;
            }

            string selectedEvent = comboBox2.SelectedItem.ToString();
            Controller controllerObj = new Controller();

            
            int eventID = controllerObj.GetEventID(selectedEvent);
            if (eventID == -1)
            {
                MessageBox.Show("Error: Could not find event ID.");
                return;
            }

           
            bool declined = controllerObj.DeclineEvent(eventID, currentAdminID);

            if (declined)
            {
                MessageBox.Show("Event declined successfully!");

               
                textBox3.Clear();
                comboBox2.SelectedIndex = -1;
                comboBox1.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Failed to decline event.");
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                string selectedEvent = comboBox2.SelectedItem.ToString();

                if (selectedEvent != "No events found")
                {
                    Controller controllerObj = new Controller();

                    
                    string agencyName = controllerObj.GetAgencyForEvent(selectedEvent);

                    
                    textBox3.Text = agencyName;
                }
                else
                {
                    textBox3.Text = "";
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            RefreshAllData();
        }
    }
    
}
