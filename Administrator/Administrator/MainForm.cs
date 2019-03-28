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
    public partial class MainForm : Form
    {
        MySqlConnection con = null;
        MySqlCommand cmd = null;
        MySqlDataReader dr = null;

        public MainForm()
        {
            InitializeComponent();
            con = new Connection().Connect();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Left = Top = 0;
            Width = Screen.PrimaryScreen.WorkingArea.Width;
            Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        public void changeSize(Int32 height, Int32 width)
        {
            Int32 Area = height + width;
            Area = Area / 7;

            
                btnInventory.Height = Area;
                btnInventory.Width = Area;

                btnStaff.Height = Area;
                btnStaff.Width = Area;

                btnAuditTrail.Height = Area;

                btnPatient.Height = Area;
                btnPatient.Width = Area;

                btnLab.Height = Area;
                btnLab.Width = Area;
        }


        private void flowLayoutPanel1_Resize(object sender, EventArgs e)
        {
            changeSize(this.Height, this.Width);
        }

        private void btnInventory_MouseEnter(object sender, EventArgs e)
        {
            btnInventory.BackgroundImage = Administrator.Properties.Resources.none;//-----------------------------
            btnStaff.BackgroundImage = Administrator.Properties.Resources._021_mask;
            btnPatient.BackgroundImage = Administrator.Properties.Resources._020_patient;
            btnLab.BackgroundImage = Administrator.Properties.Resources._033_vials;
            btnAuditTrail.Visible = false;

            btnInventory.Text = "Medicines Inventory";
            btnStaff.Text = "";
            btnPatient.Text = "";
            btnLab.Text = "";
        }

        private void btnStaff_MouseEnter(object sender, EventArgs e)
        {
            btnInventory.BackgroundImage = Administrator.Properties.Resources._035_agreement;
            btnStaff.BackgroundImage = Administrator.Properties.Resources.none;//-----------------------------
            btnPatient.BackgroundImage = Administrator.Properties.Resources._020_patient;
            btnLab.BackgroundImage = Administrator.Properties.Resources._033_vials;
            btnAuditTrail.Visible = true;

            btnInventory.Text = "";
            btnStaff.Text = "Staffs";
            btnPatient.Text = "";
            btnLab.Text = "";
        }

        private void btnPatient_MouseEnter(object sender, EventArgs e)
        {
            btnInventory.BackgroundImage = Administrator.Properties.Resources._035_agreement;
            btnStaff.BackgroundImage = Administrator.Properties.Resources._021_mask;
            btnPatient.BackgroundImage = Administrator.Properties.Resources.none;//-----------------------------
            btnLab.BackgroundImage = Administrator.Properties.Resources._033_vials;
            btnAuditTrail.Visible = false;

            btnInventory.Text = "";
            btnStaff.Text = "";
            btnPatient.Text = "Patients";
            btnLab.Text = "";
        }

        private void btnLab_MouseEnter(object sender, EventArgs e)
        {
            btnInventory.BackgroundImage = Administrator.Properties.Resources._035_agreement;
            btnStaff.BackgroundImage = Administrator.Properties.Resources._021_mask;
            btnPatient.BackgroundImage = Administrator.Properties.Resources._020_patient;
            btnLab.BackgroundImage = Administrator.Properties.Resources.none;//-----------------------------
            btnAuditTrail.Visible = false;


            btnInventory.Text = "";
            btnStaff.Text = "";
            btnPatient.Text = "";
            btnLab.Text = "Laboratory";
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            //new Inventory_Med().ShowDialog();

        }

        private void btnPatient_Click(object sender, EventArgs e)
        {
            new PatientProfiles().ShowDialog();
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            new ManageStaffs().ShowDialog();
        }
    }
}
