using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ShopKeeper
{
    public partial class frmRecords : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DbConnection dbcon = new DbConnection();
        public frmRecords()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }
        public void LoadData() {
            try
            {
                productListGridView.Rows.Clear();
                int i = 0;
                cn.Open();
                cm = new SqlCommand("SELECT TOP 10 pid, isnull(sum(qty),0) AS qty, isnull(sum(total),0) as total,pdescription FROM vwSoldItems WHERE sdate BETWEEN '" + DateTimePicker1.Value.ToString() + "' AND '" + DateTimePicker2.Value.ToString() + "' AND status LIKE 'Sold' GROUP BY pid,pdescription ORDER BY qty DESC", cn);
                
                dr = cm.ExecuteReader();
                
                while (dr.Read())
                {
                    i++;
                    
                    productListGridView.Rows.Add(i,dr["pid"].ToString(),dr["pdescription"],dr["qty"].ToString(), double.Parse(dr["total"].ToString()).ToString("#,##0.00"));

                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        public void LoadData2()
        {
            try
            {
                productListGridView2.Rows.Clear();
                int i = 0;
                cn.Open();
                cm = new SqlCommand("SELECT  c.pid, p.pdescription, c.price, sum(c.qty) AS tot_qty, sum(c.disc) AS tot_disc, sum(c.total) AS total FROM tblCart AS c INNER JOIN tblProduct AS p ON c.pid = p.pcode WHERE status LIKE 'Sold' AND sdate BETWEEN '" + DateTimePicker3.Value.ToString() + "' AND '" + DateTimePicker4.Value.ToString() + "' GROUP BY c.pid,p.pdescription,c.price", cn);
                dr = cm.ExecuteReader();

                while (dr.Read())
                {
                    i++;

                    productListGridView2.Rows.Add(i, dr["pid"].ToString(), dr["pdescription"], Double.Parse(dr["price"].ToString()).ToString("#,##0.00"), dr["tot_qty"].ToString(), dr["tot_disc"].ToString(), Double.Parse(dr["total"].ToString()).ToString("#,##0.00"));

                }
                dr.Close();
                cn.Close();

                cn.Open();
                cm = new SqlCommand("SELECT isnull (sum(total),0) AS total FROM tblCart WHERE status LIKE 'Sold' AND sdate BETWEEN '" + DateTimePicker3.Value.ToString() + "' AND '" + DateTimePicker4.Value.ToString() + "' ", cn);
                lblTotal.Text = double.Parse(cm.ExecuteScalar().ToString()).ToString("#,##00.00");
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        public void LoadCriticalItems()
        {
            try
            {
                productListGridView.Rows.Clear();
                int i = 0;
                cn.Open();
                cm = new SqlCommand("SELECT * FROM vwCriticalItems", cn);
                dr = cm.ExecuteReader();

                while (dr.Read())
                {
                    i++;

                    productListGridView3.Rows.Add(i, dr["pcode"].ToString(), dr["barcode"], dr["pdescription"].ToString(), dr["brand"].ToString(), dr["category"].ToString(), dr["price"].ToString(), dr["reorder"].ToString(), dr["qty"].ToString());

                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        public void LoadInventory()
        {
            try
            {
                productListGridView4.Rows.Clear();
                int i = 0;
                cn.Open();
                cm = new SqlCommand("SELECT p.pcode,p.barcode,p.pdescription,b.brand,c.category,p.price,p.reorder,p.qty FROM tblProduct AS p INNER JOIN tblBrand AS b ON p.brand_id=b.id INNER JOIN tblCategory AS c ON p.category_id=c.id", cn);
                dr = cm.ExecuteReader();

                while (dr.Read())
                {
                    i++;

                    productListGridView4.Rows.Add(i, dr["pcode"].ToString(), dr["barcode"], dr["pdescription"].ToString(), dr["brand"].ToString(), dr["category"].ToString(), dr["price"].ToString(), dr["reorder"].ToString(), dr["qty"].ToString());

                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnFilter2_Click(object sender, EventArgs e)
        {
            try
            {
                LoadData2();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPrintInventory_Click(object sender, EventArgs e)
        {
            frmInventoryReport frm = new frmInventoryReport();
            frm.LoadReport();
            frm.ShowDialog();
        }
    }
}
