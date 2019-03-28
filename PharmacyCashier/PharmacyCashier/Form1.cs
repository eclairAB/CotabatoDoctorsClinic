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
                }

                if (txtUsername.Text.Trim() == uname && txtPassword.Text.Trim() == pword)
                {
                    this.Hide();
                    MedicineTransaction m = new MedicineTransaction();
                    m.txtname.Text = fullname;
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
    }
}
