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
    public partial class frmQty : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DbConnection dbcon = new DbConnection();
        frmPOS frmPOS;
        private int pcode;
        private double price;
        private int qty;
        private string tr_no;
        public frmQty(frmPOS frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            frmPOS = frm;
        }

        private void frmQty_Load(object sender, EventArgs e)
        {
            txtQty.Text = "0";
            
        }

        public void productDetails(string pcode, string price, string tr_no,int qty) {
            this.pcode = int.Parse(pcode);
            this.price = double.Parse(price);
            this.tr_no = tr_no;
            this.qty = qty;
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == 13)&&(txtQty.Text != String.Empty))
            {
                String id="";
                int cart_qty=0;
                bool found = false;
                cn.Open();
                if (qty<int.Parse(txtQty.Text))
                {
                    MessageBox.Show("Unable to proceed. remaining qty on hand is " + qty,"Warning");
                    Dispose();
                    return;
                }
                cm = new SqlCommand("SELECT * FROM tblCart WHERE tr_no = @tr_no AND pid = @pid",cn);
                cm.Parameters.AddWithValue("@tr_no",frmPOS.txtTrNo.Text);
                cm.Parameters.AddWithValue("@pid", pcode);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    found = true;
                    id = dr["id"].ToString();
                    cart_qty = int.Parse(dr["qty"].ToString());
                }
                else
                {
                    found = false;
                }
                dr.Close();
                cn.Close();
                if (found==true)
                {
                    if (qty < (int.Parse(txtQty.Text) + cart_qty))
                    {
                        MessageBox.Show("Unable to proceed. remaining qty on hand is " + qty, "Warning");
                        return;
                    }
                    cn.Open();
                    cm = new SqlCommand("UPDATE tblCart SET qty = (qty + "+ int.Parse(txtQty.Text) + ") WHERE id = '"+id+"' ", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    frmPOS.txtBarcode.Clear();
                    frmPOS.txtBarcode.Focus();
                    frmPOS.loadCart();
                    this.Dispose();
                }
                else
                {
                    if (qty < int.Parse(txtQty.Text))
                    {
                        MessageBox.Show("Unable to proceed. remaining qty on hand is " + qty, "Warning");
                        return;
                    }
                    cn.Open();
                    cm = new SqlCommand("INSERT INTO tblCart (tr_no,pid,price,qty,sdate,cashier) VALUES (@tr_no,@pid,@price,@qty,@sdate,@cashier)", cn);
                    cm.Parameters.AddWithValue("@tr_no", tr_no);
                    cm.Parameters.AddWithValue("@pid", pcode);
                    cm.Parameters.AddWithValue("@price", price);
                    cm.Parameters.AddWithValue("@qty", int.Parse(txtQty.Text));
                    cm.Parameters.AddWithValue("@sdate", DateTime.Now);
                    cm.Parameters.AddWithValue("@cashier", frmPOS.label2.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    frmPOS.txtBarcode.Clear();
                    frmPOS.txtBarcode.Focus();
                    frmPOS.loadCart();

                   
                    this.Dispose();
                }

            }
        }
    }
}
