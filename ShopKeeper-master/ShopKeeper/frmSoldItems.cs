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
    public partial class frmSoldItems : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DbConnection dbcon = new DbConnection();
        public string suser;
        public frmSoldItems()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
           
            dt2.Value = DateTime.UtcNow;
            
            LoadRecords();
            loadCashier();
            
            
        }
        
        public void LoadRecords()
        {
            int i = 0;
            double total = 0.0;
            productListGridView.Rows.Clear();
            cn.Open();
            if (cboCashier.Text == "All Cashier")
            {
                cm = new SqlCommand("SELECT c.id,c.tr_no,c.pid,p.pdescription,c.price,c.qty,c.disc , c.total FROM tblCart as c INNER JOIN tblProduct as p ON c.pid=p.pcode WHERE status LIKE 'Sold' and c.sdate BETWEEN '" + dt1.Value + "' AND '" + dt2.Value + "' ", cn);
            }
            else
            {
                cm = new SqlCommand("SELECT c.id,c.tr_no,c.pid,p.pdescription,c.price,c.qty,c.disc , c.total FROM tblCart as c INNER JOIN tblProduct as p ON c.pid=p.pcode WHERE status LIKE 'Sold' and c.sdate BETWEEN '" + dt1.Value + "' AND '" + dt2.Value + "' AND cashier LIKE '" + cboCashier.Text + "' ", cn);
            }

            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i += 1;
                total += double.Parse(dr["total"].ToString()); 
                productListGridView.Rows.Add(i, dr["id"].ToString(), dr["tr_no"].ToString(), dr["pid"].ToString(), dr["pdescription"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["disc"].ToString(), dr["total"].ToString());
            }
            lblTotal.Text = total.ToString("#,##0.00");
            dr.Close();
            cn.Close();
        }

        private void dt1_ValueChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void dt2_ValueChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            frmSoldReport soldReport = new frmSoldReport(this);
            soldReport.LoadReport();
            soldReport.ShowDialog();

        }

        private void cboCashier_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        public void loadCashier() {
            cboCashier.Items.Clear();
            cboCashier.Items.Add("All Cashier");
            cn.Open();
            cm = new SqlCommand("SELECT * FROM tblUser WHERE role LIKE 'Cashier'",cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cboCashier.Items.Add(dr["name"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void cboCashier_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void productListGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = productListGridView.Columns[e.ColumnIndex].Name;
            if (colName=="colCancel")
            {
                frmCancelDetails f = new frmCancelDetails(this);
                f.txtID.Text = productListGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                f.txtTrNo.Text = productListGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                f.txtPCode.Text = productListGridView.Rows[e.RowIndex].Cells[3].Value.ToString();
                f.txtDescription.Text = productListGridView.Rows[e.RowIndex].Cells[4].Value.ToString();
                f.txtPrice.Text = productListGridView.Rows[e.RowIndex].Cells[5].Value.ToString();
                f.txtQty.Text = productListGridView.Rows[e.RowIndex].Cells[6].Value.ToString();
                f.txtDiscount.Text = productListGridView.Rows[e.RowIndex].Cells[7].Value.ToString();
                f.txtTotal.Text = productListGridView.Rows[e.RowIndex].Cells[8].Value.ToString();
                f.txtCancelledBy.Text = suser;
                f.txtVoidBy.Text = cboCashier.Text;
                f.ShowDialog();
            }
        }
    }
}
