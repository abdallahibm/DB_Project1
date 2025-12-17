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
    public partial class Form5 : Form
    {

       public int SelectedEventID=0;
       public string SelectedCategory;
       public  int SelectedCount;
       public  string SelectedEventName;
        public Form5( DataTable dt)
        {
            InitializeComponent();
            mybookings.DataSource = dt;
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }
        private void mybookings_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // 1. Capture the data from the selected row
                DataGridViewRow row = mybookings.Rows[e.RowIndex];

                SelectedEventID = Convert.ToInt32(row.Cells["Event_ID"].Value);
                SelectedCategory = row.Cells["Category"].Value.ToString();
                SelectedCount = Convert.ToInt32(row.Cells["TicketCount"].Value);
                SelectedEventName = row.Cells["Name"].Value.ToString(); // Or "EventName" based on query

                this.Close();
            }
        }

        private void mybookings_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // 1. Capture the data from the selected row
                DataGridViewRow row = mybookings.Rows[e.RowIndex];

                SelectedEventID = Convert.ToInt32(row.Cells["Event_ID"].Value);
                SelectedCategory = row.Cells["Category"].Value.ToString();
                SelectedCount = Convert.ToInt32(row.Cells["TicketCount"].Value);
                SelectedEventName = row.Cells["Name"].Value.ToString(); // Or "EventName" based on query

                this.Close();
            }
        }
    }
}
