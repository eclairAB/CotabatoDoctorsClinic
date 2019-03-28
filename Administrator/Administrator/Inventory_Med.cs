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

namespace Administrator//3000, 698 maximum
                       //1214, 698 default
{
    public partial class Inventory_Med : Form
    {
        MySqlConnection con = null;
        MySqlCommand cmd = null;
        MySqlDataReader dr = null;

        string stock;
        string name;
        string manufacturer;
        string distributor;
        DateTime arrival;
        DateTime expiry;
        public Inventory_Med()
        {
            InitializeComponent();
            con = new Connection().Connect();
        }

        void loadavailable()
        {
            lvAvailable.Items.Clear();
            con.Close();
            con.Open();
            cmd = new MySqlCommand("Select * from medicine_tbl", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ListViewItem item = new ListViewItem(dr.GetString("id"));
                item.SubItems.Add(dr.GetString("name"));
                item.SubItems.Add(dr.GetString("manufacturer"));
                item.SubItems.Add(dr.GetString("supplier"));
                item.SubItems.Add(dr.GetDateTime("arrivaldate").ToString("MMMM-dd-yyyy"));
                item.SubItems.Add(dr.GetDateTime("expirydate").ToString("MMMM-dd-yyyy"));
                lvAvailable.Items.Add(item);
            }
        }

        void loadincoming()
        {
            lvIncoming.Items.Clear();
            con.Close();
            con.Open();
            cmd = new MySqlCommand("Select * from inv_stocks_incoming", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ListViewItem item = new ListViewItem(dr.GetString("id"));
                item.SubItems.Add(dr.GetString("name"));
                item.SubItems.Add(dr.GetString("manufacturer"));
                item.SubItems.Add(dr.GetString("supplier"));
                item.SubItems.Add(dr.GetDateTime("arrivaldate").ToString("MMMM-dd-yyyy"));
                item.SubItems.Add(dr.GetDateTime("expirydate").ToString("MMMM-dd-yyyy"));
                lvIncoming.Items.Add(item);
            }
        }

        void loaddamaged()
        {
            lvDamaged.Items.Clear();
            con.Close();
            con.Open();
            cmd = new MySqlCommand("Select * from inv_stocks_damage", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ListViewItem item = new ListViewItem(dr.GetString("id"));
                item.SubItems.Add(dr.GetString("name"));
                item.SubItems.Add(dr.GetString("manufacturer"));
                item.SubItems.Add(dr.GetString("supplier"));
                item.SubItems.Add(dr.GetDateTime("arrivaldate").ToString("MMMM-dd-yyyy"));
                item.SubItems.Add(dr.GetDateTime("expirydate").ToString("MMMM-dd-yyyy"));
                lvDamaged.Items.Add(item);
            }
        }

        void loadexpired()
        {
            lvExpired.Items.Clear();
            con.Close();
            con.Open();
            cmd = new MySqlCommand("Select * from inv_stocks_expired", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ListViewItem item = new ListViewItem(dr.GetString("id"));
                item.SubItems.Add(dr.GetString("name"));
                item.SubItems.Add(dr.GetString("manufacturer"));
                item.SubItems.Add(dr.GetString("supplier"));
                item.SubItems.Add(dr.GetDateTime("arrivaldate").ToString("MMMM-dd-yyyy"));
                item.SubItems.Add(dr.GetDateTime("expirydate").ToString("MMMM-dd-yyyy"));
                lvExpired.Items.Add(item);
            }
        }

        private void Inventory_Med_Load(object sender, EventArgs e)
        {
            loadavailable();
            loaddamaged();
            loadincoming();
            loadexpired();
        }

        private void Inventory_Med_Activated(object sender, EventArgs e)
        {
            

            
        }

        private void listView1_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if(txtSearch.Text == "")
            {
                loadavailable();
                loaddamaged();
                loadincoming();
                loadexpired();
            }
            if(tabControl1.SelectedIndex == 0)
            {
                lvAvailable.Items.Clear();
                con.Close();
                con.Open();
                cmd = new MySqlCommand("Select * from inv_stocks_available where id like '%"+txtSearch.Text.Trim()+"%' or name like '%"+txtSearch.Text.Trim()+"%' or manufacturer like '%"+txtSearch.Text.Trim()+"%' or supplier like '%"+txtSearch.Text.Trim()+"%'", con);
                dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    ListViewItem item = new ListViewItem(dr.GetString("id"));
                    item.SubItems.Add(dr.GetString("name"));
                    item.SubItems.Add(dr.GetString("manufacturer"));
                    item.SubItems.Add(dr.GetString("supplier"));
                    item.SubItems.Add(dr.GetDateTime("arrivaldate").ToString("MMMM-dd-yyyy"));
                    item.SubItems.Add(dr.GetDateTime("expirydate").ToString("MMMM-dd-yyyy"));
                    lvAvailable.Items.Add(item);
                }
                if(txtSearch.Text.Length <= 0)
                {
                    loadavailable();
                }
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                lvIncoming.Items.Clear();
                con.Close();
                con.Open();
                cmd = new MySqlCommand("Select * from inv_stocks_incoming where id like '%" + txtSearch.Text.Trim() + "%' or name like '%" + txtSearch.Text.Trim() + "%' or manufacturer like '%" + txtSearch.Text.Trim() + "%' or supplier like '%" + txtSearch.Text.Trim() + "%'", con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListViewItem item = new ListViewItem(dr.GetString("id"));
                    item.SubItems.Add(dr.GetString("name"));
                    item.SubItems.Add(dr.GetString("manufacturer"));
                    item.SubItems.Add(dr.GetString("supplier"));
                    item.SubItems.Add(dr.GetDateTime("arrivaldate").ToString("MMMM-dd-yyyy"));
                    item.SubItems.Add(dr.GetDateTime("expirydate").ToString("MMMM-dd-yyyy"));
                    lvIncoming.Items.Add(item);
                }
                if (txtSearch.Text.Length <= 0)
                {
                    loadincoming();
                }
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                lvDamaged.Items.Clear();
                con.Close();
                con.Open();
                cmd = new MySqlCommand("Select * from inv_stocks_damage where id like '%" + txtSearch.Text.Trim() + "%' or name like '%" + txtSearch.Text.Trim() + "%' or manufacturer like '%" + txtSearch.Text.Trim() + "%' or supplier like '%" + txtSearch.Text.Trim() + "%'", con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListViewItem item = new ListViewItem(dr.GetString("id"));
                    item.SubItems.Add(dr.GetString("name"));
                    item.SubItems.Add(dr.GetString("manufacturer"));
                    item.SubItems.Add(dr.GetString("supplier"));
                    item.SubItems.Add(dr.GetDateTime("arrivaldate").ToString("MMMM-dd-yyyy"));
                    item.SubItems.Add(dr.GetDateTime("expirydate").ToString("MMMM-dd-yyyy"));
                    lvDamaged.Items.Add(item);
                }
                if (txtSearch.Text.Length <= 0)
                {
                    loaddamaged();
                }
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                lvExpired.Items.Clear();
                con.Close();
                con.Open();
                cmd = new MySqlCommand("Select * from inv_stocks_expired where id like '%" + txtSearch.Text.Trim() + "%' or name like '%" + txtSearch.Text.Trim() + "%' or manufacturer like '%" + txtSearch.Text.Trim() + "%' or supplier like '%" + txtSearch.Text.Trim() + "%'", con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListViewItem item = new ListViewItem(dr.GetString("id"));
                    item.SubItems.Add(dr.GetString("name"));
                    item.SubItems.Add(dr.GetString("manufacturer"));
                    item.SubItems.Add(dr.GetString("supplier"));
                    item.SubItems.Add(dr.GetDateTime("arrivaldate").ToString("MMMM-dd-yyyy"));
                    item.SubItems.Add(dr.GetDateTime("expirydate").ToString("MMMM-dd-yyyy"));
                    lvExpired.Items.Add(item);
                }
                if (txtSearch.Text.Length <= 0)
                {
                    loadexpired();
                }
            }
        }

        private void lvIncoming_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (int i in lvIncoming.SelectedIndices)
            {
                stock = lvIncoming.Items[i].SubItems[0].Text;
                name = lvIncoming.Items[i].SubItems[1].Text;
                manufacturer = lvIncoming.Items[i].SubItems[2].Text;
                distributor = lvIncoming.Items[i].SubItems[3].Text;
                arrival = Convert.ToDateTime(lvIncoming.Items[i].SubItems[4].Text);
                expiry = Convert.ToDateTime(lvIncoming.Items[i].SubItems[5].Text);
            }
        }

        private void lvAvailable_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (int i in lvAvailable.SelectedIndices)
            {
                stock = lvAvailable.Items[i].SubItems[0].Text;
                name = lvAvailable.Items[i].SubItems[1].Text;
                manufacturer = lvAvailable.Items[i].SubItems[2].Text;
                distributor = lvAvailable.Items[i].SubItems[3].Text;
                arrival = Convert.ToDateTime(lvAvailable.Items[i].SubItems[4].Text);
                expiry = Convert.ToDateTime(lvAvailable.Items[i].SubItems[5].Text);
            }
        }
    }
}
