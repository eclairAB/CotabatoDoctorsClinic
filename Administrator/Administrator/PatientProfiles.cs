using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Administrator
{
    public partial class PatientProfiles : Form
    {
        MySqlConnection con = null;
        MySqlCommand cmd = null;
        MySqlDataReader dr = null;
        String[] index = { };

        public PatientProfiles()
        {
            InitializeComponent();
            con = new Connection().Connect();
        }

        private void PatientProfiles_Load(object sender, EventArgs e)
        {
            loadProfile();
        }

        void loadProfile()
        {
            lvProfile.Items.Clear();
            con.Close();
            con.Open();
            cmd = new MySqlCommand("select distinct id, fullname from patient_records where firstname like '%"+ txtSearch.Text.Trim() 
                                  +"%' or lastname like '%"+ txtSearch.Text.Trim() +"%' or fullname like '%"+ txtSearch.Text.Trim() +"%' order by fullname asc", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ListViewItem item = new ListViewItem(dr.GetString("id"));
                item.SubItems.Add(dr.GetString("fullname"));
                lvProfile.Items.Add(item);
            }
        }

        private void lvAvailable_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                loadProfile();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
