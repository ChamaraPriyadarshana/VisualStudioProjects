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
    public partial class frmProductList : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DbConnection dbcon = new DbConnection();
        public frmProductList()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            LoadRecords();
        }

        public void LoadRecords() {
            int i = 0;
            productListGridView.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT p.pcode, p.barcode, p.pdescription, b.brand, c.category, p.price, p.reorder FROM tblProduct as p INNER JOIN tblBrand AS b on b.id = p.brand_id INNER JOIN tblCategory AS c ON c.id=p.category_id WHERE p.pdescription LIKE '" + txtSearch.Text+"%'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i += 1;
                productListGridView.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
            }
            dr.Close(); 
            cn.Close();
        }
       
        private void btnAddView_Click(object sender, EventArgs e)
        {
            frmProduct frm = new frmProduct(this);
            frm.btnSave.Enabled = true;
            frm.btnUpdate.Enabled = false;
            frm.LoadBrand();
            frm.LoadCategory();
            frm.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void productListGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = productListGridView.Columns[e.ColumnIndex].Name;
            if (colName=="Edit")
            {
                frmProduct frm = new frmProduct(this);
                frm.btnSave.Enabled = false;
                frm.btnUpdate.Enabled = true;
                frm.txtCode.Text = productListGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                frm.txtBarcode.Text = productListGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                frm.txtDescription.Text = productListGridView.Rows[e.RowIndex].Cells[3].Value.ToString();
                frm.cboBrand.Items.Add(productListGridView.Rows[e.RowIndex].Cells[4].Value.ToString());
                
                //frm.cboBrand.Text = productListGridView.Rows[e.RowIndex].Cells[4].Value.ToString();
                frm.cboCategory.Items.Add(productListGridView.Rows[e.RowIndex].Cells[5].Value.ToString())  ;
                //frm.cboCategory.Text = productListGridView.Rows[e.RowIndex].Cells[5].Value.ToString();
                frm.txtPrice.Text = productListGridView.Rows[e.RowIndex].Cells[6].Value.ToString();
                frm.txtReOrder.Text = productListGridView.Rows[e.RowIndex].Cells[7].Value.ToString();
                frm.ShowDialog();
            }
            else
            {
                if (MessageBox.Show("Are you sure to delete this record ?","Delete Record",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tblProduct WHERE  pcode LIKE '"+ productListGridView.Rows[e.RowIndex].Cells[1].Value.ToString()+ "'",cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    LoadRecords();
                }
            }
        }
    }
}
