using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace Administrator
{
    public partial class Login : Form
    {
        MySqlConnection con = null;
        MySqlCommand cmd = null;
        MySqlDataReader dr = null;

        String uname, pword;

        int counter = 3;

        public Login()
        {
            InitializeComponent();
            con = new Connection().Connect();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;

            try 
            {

                if (!IsProcessOpen("xampp-control"))
                {
                    System.Diagnostics.Process.Start("C:\\xampp/xampp-control.exe");
                }

            }
            catch (Exception ae)
            {
                MessageBox.Show("XAMPP not activated. Please activate manually or Install it.", "XAMPP required", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool IsProcessOpen(string name)
        {
            //here we're going to get a list of all running processes on
            //the computer
            foreach (Process clsProcess in Process.GetProcesses())
            {
                //now we're going to see if any of the running processes
                //match the currently running processes. Be sure to not
                //add the .exe to the name you provide, i.e: NOTEPAD,
                //not NOTEPAD.EXE or false is always returned even if
                //notepad is running.
                //Remember, if you have the process running more than once, 
                //say IE open 4 times the loop thr way it is now will close all 4,
                //if you want it to just close the first one it finds
                //then add a return; after the Kill
                if (clsProcess.ProcessName.Contains(name))
                {
                    //if the process is found to be running then we
                    //return a true
                    return true;
                }
            }
            //otherwise we return a false
            return false;
        }

        public void login()
        {
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
                }

                if (txtUsername.Text.Trim() == uname && txtPassword.Text.Trim() == pword)
                {
                    new MainForm().Show();
                    this.Hide();
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //login();

            new MainForm().Show();
            this.Hide();
        }
    }
}
