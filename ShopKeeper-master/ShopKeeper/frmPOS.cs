using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopKeeper
{
    public partial class frmPOS : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DbConnection dbcon = new DbConnection();
        public string tr_no;
        string id,price;
        int qty;
        public frmPOS()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            this.KeyPreview = true;
            txtTrDate.Text = DateTime.Now.ToLongDateString();
        }

        public void setName(string name, string role) {
            label2.Text = role;
            label1.Text = name;
        }

        public void getTransNo() {
            try
            {
                
                int count;
                string sdate = DateTime.Now.ToString("yyyyMMddHHmmss");
                cn.Open();
                cm = new SqlCommand("SELECT top 1 tr_no FROM tblCart WHERE tr_no LIKE '"+sdate+ "%' ORDER BY id ", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    tr_no = dr[0].ToString();
                    count = int.Parse(tr_no.Substring(8,4));
                    txtTrNo.Text = sdate ;
                }
                else
                {
                    tr_no = sdate;
                    txtTrNo.Text = tr_no;
                }
                dr.Close();
                cn.Close();
                
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message,"Error ",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnNewTransaction_Click(object sender, EventArgs e)
        {
            if (productListGridView.Rows.Count > 0)
            {
                return;
            }
            getTransNo();
            txtBarcode.Enabled = true;
            txtBarcode.Focus();
        }

 

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtBarcode.Text == String.Empty)
                {
                    return;
                }
                else
                {
                    cn.Open();
                    cm = new SqlCommand("SELECT * FROM tblProduct WHERE barcode LIKE '" + txtBarcode.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        qty = int.Parse(dr["qty"].ToString());
                        frmQty frm = new frmQty(this);
                        frm.productDetails(dr["pcode"].ToString(), dr["price"].ToString(), txtTrNo.Text.ToString(),qty);
                        Console.Beep(3500, 250);
                        dr.Close();
                        cn.Close();
                        frm.ShowDialog();
                        Console.Beep(2500, 250);
                        
                    }
                    else
                    {
                        dr.Close();
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public void loadCart() {
            try
            {
                productListGridView.Rows.Clear();
                int x = 0;
                double total = 0;
                double discount = 0;
                cn.Open();
                cm = new SqlCommand("SELECT c.id, c.pid, p.pdescription, c.price, c.qty, c.disc, c.total FROM tblCart AS c INNER JOIN tblProduct AS p ON c.pid = p.pcode WHERE tr_no LIKE '" + txtTrNo.Text+"' AND status LIKE 'Pending' ",cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    x++;
                    total += double.Parse(dr["total"].ToString());
                    discount += double.Parse(dr["disc"].ToString());
                    //productListGridView.Rows.Add(x,dr["id"].ToString(), dr["pdescription"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["disc"].ToString(), dr["total"].ToString());
                    productListGridView.Rows.Add(dr["id"].ToString(), dr["pdescription"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["disc"].ToString(), dr["total"].ToString(), dr["pid"].ToString());
                }
                dr.Close();
                cn.Close();
                lblTotal.Text = total.ToString("#,##0.00");
                lblDiscount.Text = discount.ToString("#,##0.00");
                getCartTotal();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error");
                cn.Close();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtTrNo.Text == String.Empty)
            {
                return;
            }
            frmLookUp frm = new frmLookUp(this);
            frm.LoadRecords();
            frm.ShowDialog();
        }

        private void productListGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = productListGridView.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"C:\Users\Public\Music\delete.wav");
                player.Play();
                if (MessageBox.Show("Remove from cart?","Delete",MessageBoxButtons.YesNo,MessageBoxIcon.Question)== DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tblCart WHERE id LIKE '" + productListGridView.Rows[e.RowIndex].Cells[0].Value.ToString() + "'", cn); ;
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Successfully Deleted","Delete");
                    loadCart();
                }
            }
        }

        private void btnClearCart_Click(object sender, EventArgs e)
        {
            cn.Open();
            cm = new SqlCommand("DELETE FROM tblCart WHERE tr_no LIKE '" + txtTrNo.Text + "'", cn);
            cm.ExecuteNonQuery();
            cn.Close();
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"C:\Windows\Media\notify.wav");
            player.Play();
            loadCart();
        }

        public void getCartTotal() {
            //double subTot = double.Parse(lblTotal.Text);
            double discount = double.Parse(lblDiscount.Text);
            double sales = double.Parse(lblTotal.Text);
            double vat = sales * dbcon.getValue();
            double vatable = sales - vat;
            lblVat.Text = vat.ToString("#,##0.00");
            lblVatable.Text = vatable.ToString("#,##0.00");
            lblDisplayTotal.Text = sales.ToString("#,##0.00");
        }

        private void btnDiscountView_Click(object sender, EventArgs e)
        {
            frmDiscount frm = new frmDiscount(this);
            frm.lblID.Text = id;
            frm.txtPrice.Text = price;
            frm.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            lblDate.Text = DateTime.Now.ToLongDateString();
        }

        private void btnSettleView_Click(object sender, EventArgs e)
        {
            frmSettle frm = new frmSettle(this);
            frm.txtSale.Text = lblDisplayTotal.Text;
            frm.ShowDialog();
        }

        private void btnDailySales_Click(object sender, EventArgs e)
        {
            /*if (label1.Text == "Cashier")
            {
                MessageBox.Show("Administrator Privileges Required","Oops");
                return;
            }*/
            frmSoldItems soldItems = new frmSoldItems();
            soldItems.dt1.Enabled = false;
            soldItems.dt2.Enabled = false;
            soldItems.suser = label2.Text;
            soldItems.cboCashier.Enabled = false;
            soldItems.cboCashier.Text = label1.Text;
            soldItems.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (productListGridView.Rows.Count>0)
            {
                MessageBox.Show("Please Settle the cart","Oops",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show("Wanna Logout application?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void frmPOS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.F1)
            {
                btnNewTransaction_Click(sender,e);
            }
            else if (e.KeyCode == Keys.F2)
            {
                btnSearch_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F3)
            {
                btnDiscountView_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F4)
            {
                btnSettleView_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F6)
            {
               btnDailySales_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F7)
            {
                btnClearCart_Click(sender, e);
            }
        }

        private void productListGridView_SelectionChanged(object sender, EventArgs e)
        {
            int i = productListGridView.CurrentRow.Index;
            id = productListGridView[0, i].Value.ToString();
            price = productListGridView[2, i].Value.ToString();
        }


    }
}
