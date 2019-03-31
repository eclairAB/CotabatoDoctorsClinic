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

namespace PharmacyCashier
{
    public partial class Form1 : Form
    {
        MySqlConnection con = null;
        MySqlCommand cmd = null;
        MySqlDataReader dr = null;
        string uname;
        string pword;
        string fullname;
        string id;
        string position;
        public Form1()
        {
            InitializeComponent();
            con = new Connection().Connect();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            int counter = 3;

            if (counter > 1)
            {
                con.Close(); con.Open();
                cmd = new MySqlCommand("select * from accounts where (username = '" + txtUsername.Text.Trim().Replace("'", "''") +
                                                               "' and password = '" + txtPassword.Text.Trim().Replace("'", "''") + "')", con);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    uname = dr.GetString("username");
                    pword = dr.GetString("password");
                    fullname = dr.GetString("fullname");
                    id = dr.GetString("id");
                    position = dr.GetString("userlevel");
                }

                if (txtUsername.Text.Trim() == uname && txtPassword.Text.Trim() == pword)
                {
                    con.Close();
                    con.Open();
                    cmd = new MySqlCommand("insert into audit_trail(account_id,datein,timein)values('"
                        + id + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    this.Hide();
                    MedicineTransaction m = new MedicineTransaction();
                    m.txtname.Text = fullname;
                    m.txtpos.Text = position;
                    m.txtiduser.Text = id;
                    m.ShowDialog();

                }
                else
                {
                    counter--;
                    if (counter != 1)
                        MessageBox.Show("Username or password are incorrect.\nAttempts remaining: " + counter, "System message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("Username or password are incorrect.\nAttempt remaining: " + counter, "System message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
            {
                MessageBox.Show("You have no attempt remaining, now closing application.", "System message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();

            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }
    }
}
