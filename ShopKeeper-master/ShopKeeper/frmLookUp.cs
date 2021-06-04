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
    public partial class frmLookUp : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DbConnection dbcon = new DbConnection();
        string sItem="";
        frmPOS pOS;
        public frmLookUp(frmPOS frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            txtSearch.Focus();
            pOS = frm;
        }
        public void LoadRecords()
        {
            int i = 0;
            productListGridView.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT p.pcode, p.barcode, p.pdescription, b.brand, c.category, p.price, p.qty FROM tblProduct as p INNER JOIN tblBrand AS b on b.id = p.brand_id INNER JOIN tblCategory AS c ON c.id=p.category_id WHERE p.pdescription LIKE '" + txtSearch.Text + "%'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i += 1;
                productListGridView.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void productListGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = productListGridView.Columns[e.ColumnIndex].Name;
            if (colName.Equals("Select"))
            {
                frmQty frm = new frmQty(pOS);
                frm.productDetails(productListGridView.Rows[e.RowIndex].Cells[1].Value.ToString(), productListGridView.Rows[e.RowIndex].Cells[6].Value.ToString(), pOS.txtTrNo.Text.ToString(), int.Parse(productListGridView.Rows[e.RowIndex].Cells[7].Value.ToString()));
                Console.Beep(3500, 250);
                dr.Close();
                cn.Close();
                frm.ShowDialog();
                Console.Beep(2500, 250);
            }
        }

        private void productListGridView_KeyDown(object sender, KeyEventArgs e)
        {
             if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }

        /* public void addItem() {
             cn.Open();
             cm = new SqlCommand("SELECT * FROM tblProduct WHERE barcode LIKE '"+sItem.ToString()+"' ", cn);
             dr = cm.ExecuteReader();
             dr.Read();
             if (dr.HasRows)
             {
                 frmPOS pos = new frmPOS();
                 frmQty2 frm = new frmQty2(this);
                 frm.productDetails(dr["pcode"].ToString(), dr["price"].ToString(), pos.tr_no);

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
         }*/
    }
}
