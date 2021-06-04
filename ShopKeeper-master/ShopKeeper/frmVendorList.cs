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
    public partial class frmVendorList : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DbConnection dbcon = new DbConnection();
        public frmVendorList()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            LoadRecords();
        }

        public void LoadRecords()
        {
            int i = 0;
            panelVendor.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT * FROM tblVendor", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i += 1;
                panelVendor.Rows.Add(i, dr["id"].ToString(), dr["vendor"].ToString(), dr["address"].ToString(), dr["contactperson"].ToString(), dr["telephone"].ToString(), dr["email"].ToString(), dr["fax"].ToString());
            }
            cn.Close();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmVendor frm = new frmVendor(this);
            frm.btnSave.Enabled = true;
            frm.btnUpdate.Enabled = false;
            frm.ShowDialog();

        }

        private void panelVendor_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = panelVendor.Columns[e.ColumnIndex].Name;
            if (colname == "Edit")
            {
                frmVendor frm = new frmVendor(this);
                frm.lblID.Text = panelVendor[1, e.RowIndex].Value.ToString();
                frm.txtVendor.Text = panelVendor[2, e.RowIndex].Value.ToString();
                frm.txtAddress.Text = panelVendor[3, e.RowIndex].Value.ToString();
                frm.txtContactPerson.Text = panelVendor[4, e.RowIndex].Value.ToString();
                frm.txtTelephone.Text = panelVendor[5, e.RowIndex].Value.ToString();
                frm.txtEmail.Text = panelVendor[6, e.RowIndex].Value.ToString();
                frm.txtFax.Text = panelVendor[7, e.RowIndex].Value.ToString();
                frm.btnSave.Enabled = false;
                frm.ShowDialog();
            }
            else if (colname == "Delete")
            {
                if (MessageBox.Show("Sure to Delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("Delete from tblVendor where id like '" + panelVendor[1, e.RowIndex].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Vendor has Successfully deleted", "POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecords();
                }
            }
        }
    }
}
