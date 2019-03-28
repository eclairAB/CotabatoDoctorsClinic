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

namespace Laboratory
{
    public partial class Form1 : Form
    {
        MySqlCommand cmd = null;
        MySqlConnection con = null;
        MySqlDataReader dr = null;

        public Form1()
        {
            InitializeComponent();
            con = new Connection().Connect();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                retrieve();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        public void retrieve()
        {
            listView1.Items.Clear();
            con.Close(); con.Open();
            cmd = new MySqlCommand("select distinct username from accounts order by username asc",con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                

                ListViewItem item = new ListViewItem(dr.GetString("username"));
                listView1.Items.Add(item);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            retrieve();
        }
    }
}
