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
    public partial class Usher : Form
    {
        Controller controllerObj;
        long currentUsherID;
        public Usher(long id)
        {
            InitializeComponent();
            controllerObj = new Controller();
            currentUsherID = id;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedValue == null) return;
            string eventID = comboBox2.SelectedValue.ToString();
            if (eventID == "System.Data.DataRowView")
            {
                return;
            }
            DataTable dt = controllerObj.getEventDate(eventID);

            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0][0] != DBNull.Value)
                {
                    DateTime dateVal = Convert.ToDateTime(dt.Rows[0][0]);
                    textBox3.Text = dateVal.ToShortDateString();
                }
            }
            else
            {
                textBox3.Text = "";
            }

            DataTable dtDate = controllerObj.getEventDate(eventID);
            if (dtDate != null && dtDate.Rows.Count > 0 && dtDate.Rows[0][0] != DBNull.Value)
            {
                textBox3.Text = Convert.ToDateTime(dtDate.Rows[0][0]).ToShortDateString();
            }
            else textBox3.Text = "";

            DataTable dtTickets = controllerObj.GetEventTickets(eventID);
            dataGridView1.DataSource = dtTickets;
            dataGridView1.Refresh();
        }

        private void Usher_Load(object sender, EventArgs e)
        {
            DataTable dt = controllerObj.getEventsOfUshers(currentUsherID);

            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "Event_ID";
            comboBox2.SelectedIndex = -1;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}