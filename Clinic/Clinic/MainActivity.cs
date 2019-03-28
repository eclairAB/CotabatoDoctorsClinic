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

namespace Clinic
{
    public partial class MainActivity : Form
    {
        MySqlConnection con = null;
        MySqlCommand cmd = null;
        MySqlDataReader dr = null;

        public MainActivity()
        {
            InitializeComponent();
            con = new Connection().Connect();
        }

        private void MainActivity_Load(object sender, EventArgs e)
        {
            loadProfile();
            groupBox1.BringToFront();
        }

        void loadProfile()
        {
            listView1.Items.Clear();
            con.Close();
            con.Open();
            cmd = new MySqlCommand("select distinct fullname from patient_records where firstname like '%" + txtSearch.Text.Trim()
                                  + "%' or lastname like '%" + txtSearch.Text.Trim() + "%' or fullname like '%" + txtSearch.Text.Trim() + "%' order by fullname asc", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ListViewItem item = new ListViewItem(dr.GetString("fullname"));
                listView1.Items.Add(item);
            }
        }

        void loadPast()
        {
            lvPast.Items.Clear();
            con.Close();
            con.Open();
            cmd = new MySqlCommand("select date_admitted from patient_records where fullname = '"+ groupBox2.Text +"' order by fullname asc", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ListViewItem item = new ListViewItem(dr.GetString("date_admitted"));
                lvPast.Items.Add(item);
            }
        }

        void insert()
        {
            String fullname = tFirst.Text.Trim().Replace("'", "''") + " " + tLast.Text.Trim().Replace("'", "''");

            con.Close();
            con.Open();
            cmd = new MySqlCommand("insert into patient_records (id, firstname, lastname, fullname, address, contactno, remarks, date_admitted) values('', '"+
                tFirst.Text.Trim().Replace("'","''")+"', '"+
                tLast.Text.Trim().Replace("'","''")+"', '"+
                fullname + "', '" +
                tAddress.Text.Trim().Replace("'","''")+"', '"+
                tContact.Text.Trim().Replace("'","''")+"', '"+
                tRemarks.Text.Trim().Replace("'","''")+"', '"+
                DateTime.Now.ToString("yyyy-MM-dd") +"')", con);
            dr = cmd.ExecuteReader();

            MessageBox.Show("Profile added successfully.");
            loadProfile();

            tFirst.Text = "";
            tLast.Text = "";
            tAddress.Text = "";
            tContact.Text = "";
            tRemarks.Text = "";
        }

        Boolean isNoPastRecord(String fullname)
        {
            con.Close();
            con.Open();
            cmd = new MySqlCommand("select id from patient_records where fullname = '" + fullname + "' having count(date_admitted) > 1", con);
            dr = cmd.ExecuteReader();

            if (dr.Read()) return false;
            else return true;
        }

        Boolean isExisting(String fullname)
        {
            con.Close();
            con.Open();
            cmd = new MySqlCommand("select id from patient_records where fullname = '"+ fullname +"'", con);
            dr = cmd.ExecuteReader();

            if (dr.Read()) return true;
            else return false;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadProfile();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            String fullname = tFirst.Text.Trim().Replace("'", "''") + " " + tLast.Text.Trim().Replace("'", "''");

            try
            {
                if (tFirst.Text.Trim() == "" || tLast.Text.Trim() == "" || tAddress.Text.Trim() == "" || tContact.Text.Trim() == "" || tRemarks.Text.Trim() == "")
                {
                    MessageBox.Show("Please fill all fields!");
                }
                else if (isExisting(fullname))   
                {
                    DialogResult a = MessageBox.Show("You already have a record of \""+ fullname + "\". Add record anyway?", "System Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (a == DialogResult.Yes)
                    {
                        insert();
                    }
                }
                else
                {
                    insert();
                }          
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            groupBox1.BringToFront();
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            groupBox2.Text = listView1.SelectedItems[0].Text;
            groupBox2.BringToFront();

            String displayRemarkAndDate;

            con.Close();
            con.Open();
            cmd = new MySqlCommand("select * from patient_records where fullname = '" + groupBox2.Text + "'", con);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                displayFirst.Text = dr.GetString("firstname");
                displayLast.Text = dr.GetString("lastname");
                displayAddress.Text = dr.GetString("address");
                displayContact.Text = dr.GetString("contactno");
                displayRemarkAndDate = "" + dr.GetString("date_admitted") + "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t" + dr.GetString("remarks");

                displayRemarks.Text = displayRemarkAndDate;

                if (isNoPastRecord(groupBox2.Text))
                {
                    lvPast.Visible = false;
                }
                else
                {
                    lvPast.Visible = true;
                    loadPast();
                }
            }   
        }

        private void lvPast_Click(object sender, EventArgs e)
        {
            String selected = lvPast.SelectedItems[0].Text;
            String displayRemarkAndDate;

            con.Close();
            con.Open();
            cmd = new MySqlCommand("select date_admitted, remarks from patient_records where fullname = '" + groupBox2.Text + "' and date_admitted = '" + selected + "'", con);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                displayRemarkAndDate = "" + dr.GetString("date_admitted") + "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t" + dr.GetString("remarks");

                displayRemarks.Text = displayRemarkAndDate;

            }
        }
    }
}
