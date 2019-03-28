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
using System.Drawing.Printing;
using Microsoft.VisualBasic;

namespace PharmacyCashier
{
    public partial class MedicineTransaction : Form
    {
        MySqlConnection con = null;
        MySqlCommand cmd = null;
        MySqlDataReader dr = null;
        string medicine_id;
        string trade_name;
        string generic_name;
        string drug_type;
        string unit;
        string unit_value;
        decimal qty;
        decimal price;
        decimal total;
        decimal total_qty;
        public MedicineTransaction()
        {
            InitializeComponent();
            con = new Connection().Connect();
            button3.Select();
            load();
            loadtransact();
        }
        void loadtransact()
        {
            con.Close();
            con.Open();
            cmd = new MySqlCommand("Select ifnull(max(id), 0) as maxid from medicine_transaction", con);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                textBox1.Text = Convert.ToString(1000 + dr.GetInt32("maxid") + 1);
            }
        }
        void load()
        {
            listView1.Items.Clear();
            con.Close();
            con.Open();
            cmd = new MySqlCommand("Select * from medicine_tbl inner join medicine_available on medicine_tbl.medicine_id = medicine_available.medicine_id", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ListViewItem item = new ListViewItem(dr.GetString("medicine_id"));
                item.SubItems.Add(dr.GetString("trade_name"));
                item.SubItems.Add(dr.GetString("generic_name"));
                item.SubItems.Add(dr.GetString("drug_type"));
                item.SubItems.Add(dr.GetString("unit"));
                item.SubItems.Add(dr.GetString("unit_value"));
                item.SubItems.Add(dr.GetString("qty"));
                item.SubItems.Add(dr.GetString("price"));
                item.SubItems.Add(dr.GetString("manufacturer"));
                item.SubItems.Add(dr.GetString("available_date"));
                item.SubItems.Add(dr.GetString("expiry_date"));
                listView1.Items.Add(item);
            }
        }
        public void CreateReceipt(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int total = 0;
            decimal cash = decimal.Parse(txtcash.Text);
            decimal change = 0;
            Graphics graphic = e.Graphics;

            Font font = new Font("Courier New", 7); //must use a mono spaced font as the spaces need to line up

            float fontHeight = font.GetHeight();

            int startX = 10;
            int startY = 10;
            int offset = 40;

            graphic.DrawString("Cotabato Doctors Clinic", new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), 9, 0);
            graphic.DrawString("#063 Sinsuat Ave.Rosary Heights 4", font, new SolidBrush(Color.Black), startX, startY + 10);
            offset = offset + (int)fontHeight;
            graphic.DrawString("      4 Cotabato City", font, new SolidBrush(Color.Black), startX, startY + 20);
            offset = offset + (int)fontHeight;
            graphic.DrawString("Cashier: " + txtname.Text, font, new SolidBrush(Color.Black), startX, startY + 30);
            offset = offset + (int)fontHeight;
            graphic.DrawString("Transaction#: "+ textBox1.Text, font, new SolidBrush(Color.Black), startX, startY + 40);
            offset = offset + (int)fontHeight; 
            string top = "Item Name".PadRight(21) + "Price";
            graphic.DrawString(top, font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight; //make the spacing consistent
            graphic.DrawString("======================================", new Font("Courier New", 12), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight + 5; //make the spacing consistent
            decimal totalprice = 0;
            for (int i = 0; listView2.Items.Count > i; i++)
            {
                string productDescription = listView2.Items[i].SubItems[1].Text;
                string productTotal = listView2.Items[i].SubItems[8].Text;
                string totalqty = listView2.Items[i].SubItems[7].Text + " x " + listView2.Items[i].SubItems[6].Text;
                decimal productPrice = decimal.Parse(productTotal);
                totalprice += productPrice;
                graphic.DrawString(productDescription, new Font("Courier New", 8, FontStyle.Regular), new SolidBrush(Color.Red), 20, startY + offset);
                graphic.DrawString(productTotal, new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Red), 140, startY + offset);
                offset = offset + (int)fontHeight + 5;
                graphic.DrawString(totalqty, new Font("Courier New", 8, FontStyle.Regular), new SolidBrush(Color.Red), 20, startY + offset);
                offset = offset + (int)fontHeight + 5;
            }
            change = (cash - totalprice);

            //when we have drawn all of the items add the total

            offset = offset + 20; //make some room so that the total stands out.

            graphic.DrawString("Total ".PadRight(19) + String.Format("{0:c}", totalprice), new Font("Courier New", 8, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);

            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Vat sales ".PadRight(21) + String.Format("{0:c}", decimal.Parse(txtvat.Text)), font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 15;
            graphic.DrawString("Subtotal ".PadRight(21) + String.Format("{0:c}", decimal.Parse(txtsub.Text)), font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 15;
            graphic.DrawString("Cash ".PadRight(21) + String.Format("{0:c}", cash), font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 15;
            graphic.DrawString("Change ".PadRight(21) + String.Format("{0:c}", change), font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("====================================", new Font("Courier New", 12), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 15;
            graphic.DrawString("Thank you!", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 15;
            graphic.DrawString("This serve as an official", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 15;
            graphic.DrawString("Receipt", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 15;
            graphic.DrawString(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"), font, new SolidBrush(Color.Black), startX, startY + offset);
        }
      
        private void MedicineTransaction_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1) 
            {
                button3.PerformClick();
            }
            if (e.KeyCode == Keys.F2)
            {
                btnRemove.PerformClick();
            }
            if (e.KeyCode == Keys.F3)
            {
                btnSettle.PerformClick();
            }
            if (e.KeyCode == Keys.F8)
            {
                btnClose.PerformClick();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void MedicineTransaction_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.Location = new Point(0, 0);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void btnSettle_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
            txtcash.Select();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
           // load();
            listView1.Items.Clear();
            con.Close();
            con.Open();
            cmd = new MySqlCommand("select * from medicine_tbl inner join medicine_available on (medicine_tbl.medicine_id = medicine_available.medicine_id) where medicine_tbl.medicine_id = '" + txtSearch.Text + "' ", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ListViewItem item = new ListViewItem(dr.GetString("id"));
                item.SubItems.Add(dr.GetString("medicine_id"));
                item.SubItems.Add(dr.GetString("trade_name"));
                item.SubItems.Add(dr.GetString("generic_name"));
                item.SubItems.Add(dr.GetString("drug_type"));
                item.SubItems.Add(dr.GetString("unit"));
                item.SubItems.Add(dr.GetString("unit_value"));
                item.SubItems.Add(dr.GetString("qty"));
                item.SubItems.Add(dr.GetString("price"));
                item.SubItems.Add(dr.GetString("manufacturer"));
                item.SubItems.Add(dr.GetDateTime("available_date").ToString("MM/dd/yyyy"));
                item.SubItems.Add(dr.GetString("expiry_date"));
                listView1.Items.Add(item);
            }
            if (txtSearch.Text == "")
            {
                load();
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
           /** if (e.KeyCode == Keys.Enter)
            {
                con.Close();
                con.Open();
                cmd = new MySqlCommand("Select * from medicine_tbl inner join medicine_available on medicine_tbl.medicine_id = medicine_available.medicine_id where medicine_tbl.medicine_id = '"+txtSearch.Text+"'", con);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    ListViewItem item = new ListViewItem(dr.GetString("medicine_id"));
                    item.SubItems.Add(dr.GetString("medicine_id"));
                    item.SubItems.Add(dr.GetString("trade_name"));
                    item.SubItems.Add(dr.GetString("generic_name"));
                    item.SubItems.Add(dr.GetString("drug_type"));
                    item.SubItems.Add(dr.GetString("unit"));
                    item.SubItems.Add(dr.GetString("unit_value"));
                    item.SubItems.Add(dr.GetString("qty"));
                    item.SubItems.Add(dr.GetString("price"));
                    item.SubItems.Add(dr.GetString("manufacturer"));
                    item.SubItems.Add(dr.GetString("available_date"));
                    item.SubItems.Add(dr.GetString("expiry_date"));
                    listView1.Items.Add(item);
                }
                decimal a = 0;
                decimal b = 0;
                decimal c = 0;
                for(int i = 0; listView1.Items.Count > i;i++)
                {
                    a += decimal.Parse(listView1.Items[i].SubItems[8].Text);
                    b += decimal.Parse(listView1.Items[i].SubItems[7].Text);
                    txtItem.Text = b.ToString();
                    txtAmount.Text = a.ToString();
                }
            }**/
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            foreach(int i in listView1.SelectedIndices)
            {
                numericUpDown1.Maximum = Convert.ToDecimal(listView1.Items[i].SubItems[6].Text);
                medicine_id = listView1.Items[i].SubItems[0].Text;
                trade_name = listView1.Items[i].SubItems[1].Text;
                generic_name = listView1.Items[i].SubItems[2].Text;
                drug_type = listView1.Items[i].SubItems[3].Text;
                unit = listView1.Items[i].SubItems[4].Text;
                unit_value = listView1.Items[i].SubItems[5].Text;
                qty = Convert.ToDecimal(listView1.Items[i].SubItems[6].Text);
                price = Convert.ToDecimal(listView1.Items[i].SubItems[7].Text);
                if(listView1.Items[i].SubItems[6].Text == "0")
                {
                    button1.Enabled = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value > qty)
            {
                MessageBox.Show("Please input quantity less than or equal to available quantity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(numericUpDown1.Value <= 0)
            {
                MessageBox.Show("Please Enter Quantity.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                total = numericUpDown1.Value * price;
                ListViewItem item = new ListViewItem(medicine_id);
                item.SubItems.Add(trade_name);
                item.SubItems.Add(generic_name);
                item.SubItems.Add(drug_type);
                item.SubItems.Add(unit);
                item.SubItems.Add(unit_value);
                item.SubItems.Add(numericUpDown1.Value.ToString());
                item.SubItems.Add(price.ToString("#,##0.00"));
                item.SubItems.Add(total.ToString("#,#00.00"));
                listView2.Items.Add(item);
                decimal a = 0;
                decimal b = 0;
                for(int i = 0; listView2.Items.Count > i;i++)
                {
                    a += decimal.Parse(listView2.Items[i].SubItems[6].Text);
                    b += decimal.Parse(listView2.Items[i].SubItems[8].Text);
                    txtItem.Text = a.ToString();
                    txtAmount.Text = b.ToString();
                }
                decimal vat = Convert.ToDecimal(txtAmount.Text) * Convert.ToDecimal(".12");
                decimal sub = Convert.ToDecimal(txtAmount.Text) - vat;
                txtvat.Text = vat.ToString("#,#00.00");
                txtsub.Text = sub.ToString("#,#00.00");
                decimal c = qty - numericUpDown1.Value;
                con.Close();
                con.Open();
                cmd = new MySqlCommand("update medicine_tbl set qty = '" + c.ToString() + "'", con);
                cmd.ExecuteNonQuery();
                numericUpDown1.Value = 0;
                load();
            }
            button1.Enabled = false;
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            textBox4.Text = txtAmount.Text;
        }

        private void txtcash_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtcash.Text.Length > 0)
                {
                    if (Convert.ToDecimal(txtcash.Text) < Convert.ToDecimal(textBox4.Text))
                    {
                        MessageBox.Show("Please pay all the payments.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        for (int i = 0; listView2.Items.Count > i; i++)
                        {
                            con.Close();
                            con.Open();
                            cmd = new MySqlCommand("insert into medicine_transaction(transact_id,medicine_id,qty,total,cash,change_cash,transactdate,transacttime)values('"
                                + textBox1.Text + "','" + listView2.Items[i].SubItems[0].Text + "','" + listView2.Items[i].SubItems[6].Text + "','" + listView2.Items[i].SubItems[7].Text + "','" + txtcash.Text + "','" + txtchange.Text + "','"
                                + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "')", con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        //MessageBox.Show("Transaction Completed", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        PrintDialog printDialog = new PrintDialog();

                        PrintDocument printDocument = new PrintDocument();

                        printDialog.Document = printDocument; //add the document to the dialog box...        

                        printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt); //add an event handler that will do the printing

                        //on a till you will not want to ask the user where to print but this is fine for the test envoironment.

                        // DialogResult result = printDialog.ShowDialog();

                        //if (result == DialogResult.OK)
                        //  {
                        if (MessageBox.Show("Do you want to print the receipt?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            printDocument.Print();
                        }
                        //  } 
                        listView2.Items.Clear();
                        load();
                        loadtransact();
                        txtAmount.Text = "";
                        txtvat.Text = "";
                        txtsub.Text = "";
                        txtItem.Text = "";
                        txtAmount.Text = "";
                        textBox4.Text = "";
                        txtcash.Text = "";
                        txtchange.Text = "";
                        panel4.Visible = false;
                    }
                }
            }
        }

        private void txtcash_TextChanged(object sender, EventArgs e)
        {
            if (txtcash.Text.Length > 0)
            {
                decimal a = Convert.ToDecimal(txtcash.Text);
                decimal b = Convert.ToDecimal(textBox4.Text);
                decimal c = b - a;
                if (Convert.ToDecimal(txtcash.Text) >= Convert.ToDecimal(textBox4.Text))
                {
                    txtchange.Text = c.ToString("#,#00.00");
                    txtchange.Text.Replace('-', ' ');
                }
            }
        }
    }
}
