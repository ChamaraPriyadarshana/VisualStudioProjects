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
    public partial class frmVendor : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DbConnection dbcon = new DbConnection();
        frmVendorList frmList;

        public frmVendor(frmVendorList fList)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            frmList = fList;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
            Clear();
        }
        public void Save() {
            cn.Open();
            cm = new SqlCommand("INSERT INTO tblVendor (vendor,address,contactperson,telephone,email,fax) VALUES(@vendor,@address,@contactperson,@telephone,@email,@fax)", cn);
            cm.Parameters.AddWithValue("@vendor", txtVendor.Text);
            cm.Parameters.AddWithValue("@address", txtAddress.Text);
            cm.Parameters.AddWithValue("@contactperson", txtContactPerson.Text);
            cm.Parameters.AddWithValue("@telephone", txtTelephone.Text);
            cm.Parameters.AddWithValue("@email", txtEmail.Text);
            cm.Parameters.AddWithValue("@fax", txtFax.Text);
            cm.ExecuteNonQuery();
            cn.Close();
            MessageBox.Show("Records has been successfully saved!", "Saved");
        }
        public void Clear() {
            txtVendor.Clear();
            txtAddress.Clear();
            txtContactPerson.Clear();
            txtTelephone.Clear();
            txtEmail.Clear();
            txtFax.Clear();

            txtVendor.Focus();
            btnUpdate.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure to update vendor ?", "Update record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                cn.Open();
                cm = new SqlCommand("update tblVendor set vendor = @vendor, address=@address,contactperson=@contactperson,telephone=@telephone,email=@email,fax=@fax where id like'" + lblID.Text + "'", cn);

                cm.Parameters.AddWithValue("@vendor", txtVendor.Text);
                cm.Parameters.AddWithValue("@address", txtAddress.Text);
                cm.Parameters.AddWithValue("@contactperson", txtContactPerson.Text);
                cm.Parameters.AddWithValue("@telephone", txtTelephone.Text);
                cm.Parameters.AddWithValue("@email", txtEmail.Text);
                cm.Parameters.AddWithValue("@fax", txtFax.Text);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Category Updated!");
                Clear();
                frmList.LoadRecords();
                this.Dispose();
            }
        }
    }
}
