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
    public partial class frmAdjustment : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DbConnection dbcon = new DbConnection();
        Form1 f;
        int qty = 0;
        public frmAdjustment(Form1 f)
        {
            InitializeComponent();
            this.f = f;
            cn = new SqlConnection(dbcon.MyConnection());
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

        public void Clear() {
            txtSearch.Clear();
            txtRemarks.Clear();
            txtPcode.Clear();
            txtDesc.Clear();
            txtQty.Clear();
        }

        private void productListGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = productListGridView.Columns[e.ColumnIndex].Name;
            if (colName == "Select")
            {
                Clear();
                txtDesc.Text = productListGridView.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtPcode.Text = productListGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtQty.Text = productListGridView.Rows[e.RowIndex].Cells[7].Value.ToString();

                qty = int.Parse(productListGridView.Rows[e.RowIndex].Cells[7].Value.ToString());
            }
        }

        public void RefNo() {
            Random rnd = new Random();
            txtRef.Text = rnd.Next().ToString();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar==13)
            {
                LoadRecords();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(txtQty.Text)>qty)
                {
                    MessageBox.Show("Stock quantity should be greater than adjustment quantity","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                if (cboCommand.SelectedItem.ToString()== "Add to Inventory")
                {
                    SqlStatement("UPDATE tblProduct SET qty = (qty+ "+ int.Parse(txtQty.Text)+") WHERE pcode LIKE '"+txtPcode.Text+"'");

                    MessageBox.Show("Successfully added the qty", "Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (cboCommand.SelectedItem.ToString() == "Remove From Inventory")
                {
                    SqlStatement("UPDATE tblProduct SET qty = (qty- " + int.Parse(txtQty.Text) + ") WHERE pcode LIKE '" + txtPcode.Text + "'");

                    MessageBox.Show("Successfully reduced the qty", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                SqlStatement("INSERT INTO tblAdjustment (pcode,qty,action,remarks,sdate,[user],referenceno) VALUES ('" + txtPcode.Text+"','"+txtQty.Text+"','"+ cboCommand.SelectedItem.ToString() + "','"+txtRemarks.Text+"','"+DateTime.Now.ToShortDateString()+"','"+txtUser.Text+"','"+txtRef.Text+"')");
                LoadRecords();
                Clear();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        public void SqlStatement(string sql) {
            cn.Open();
            cm = new SqlCommand(sql,cn);
            cm.ExecuteNonQuery();
            cn.Close();
        }
    }
}
