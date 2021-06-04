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
using System.Security.Cryptography.X509Certificates;

namespace ShopKeeper
{
    public partial class frmProduct : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnection dbcon = new DbConnection();
        frmProductList frmList;
        SqlDataReader dr;
        public frmProduct(frmProductList fList)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            frmList = fList;
        }

        public void LoadCategory() {
            cboCategory.Items.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT category FROM tblCategory",cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cboCategory.Items.Add(dr[0].ToString());
            }
            dr.Close();
            cn.Close();
        }

        public void LoadBrand()
        {
            cboBrand.Items.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT brand FROM tblBrand", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cboBrand.Items.Add(dr[0].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you want to Save?","Save product",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string brand_id = ""; string category_id ="";
                    cn.Open();
                    cm = new SqlCommand("SELECT id FROM tblBrand WHERE brand LIKE '"+cboBrand.Text+"'",cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        brand_id = dr[0].ToString();
                    }
                    dr.Close();
                    cn.Close();
                   
                    cn.Open();
                    cm = new SqlCommand("SELECT id FROM tblCategory WHERE category LIKE '" + cboCategory.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        category_id = dr[0].ToString();
                    }
                    dr.Close();
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("INSERT INTO tblProduct (pcode, barcode, pdescription, brand_id, category_id, price, reorder ) VALUES(@pcode, @barcode, @pdescription, @brand_id, @category_id, @price, @reorder)", cn);
                    cm.Parameters.AddWithValue("@pcode",txtCode.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@pdescription", txtDescription.Text);
                    cm.Parameters.AddWithValue("@brand_id", brand_id);
                    cm.Parameters.AddWithValue("@category_id", category_id);
                    cm.Parameters.AddWithValue("@price", double.Parse(txtPrice.Text));
                    cm.Parameters.AddWithValue("@reorder", int.Parse(txtReOrder.Text));
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Product saved successfully");
                    Clear();
                    frmList.LoadRecords();
                }
            }
            catch (Exception ex)
            {

                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        public void Clear() {
            txtCode.Clear();
            txtBarcode.Clear();
            txtDescription.Clear();
            //txtQuantity.Clear();
            txtPrice.Clear();
            cboBrand.Text = "";
            cboCategory.Text = "";
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you want to Update?", "Update product", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string brand_id = ""; string category_id = "";
                    cn.Open();
                    cm = new SqlCommand("SELECT id FROM tblBrand WHERE brand LIKE '" + cboBrand.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        brand_id = dr[0].ToString();
                    }
                    dr.Close();
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("SELECT id FROM tblCategory WHERE category LIKE '" + cboCategory.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        category_id = dr[0].ToString();
                    }
                    dr.Close();
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("UPDATE tblProduct SET barcode=@barcode, pdescription=@pdescription, brand_id=@brand_id, category_id=@category_id, price=@price, reorder=@reorder WHERE pcode LIKE @pcode", cn); //pquantity=@pquantity
                    cm.Parameters.AddWithValue("@pcode", txtCode.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@pdescription", txtDescription.Text);
                    cm.Parameters.AddWithValue("@brand_id", brand_id);
                    cm.Parameters.AddWithValue("@category_id", category_id);
                    cm.Parameters.AddWithValue("@price", double.Parse(txtPrice.Text));
                    cm.Parameters.AddWithValue("@reorder", int.Parse(txtReOrder.Text));
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Product updated successfully");
                    Clear();
                    frmList.LoadRecords();
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            this.Dispose();
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46)
            {

            }
            else if (e.KeyChar == 8)
            {

            }
            else if ((e.KeyChar < 48) || (e.KeyChar > 57))
            {
                e.Handled = true;
            }
        }

    }
}
