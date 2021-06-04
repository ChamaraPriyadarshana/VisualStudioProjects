using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopKeeper
{
    public partial class frmBrandList : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DbConnection dbcon = new DbConnection();

        public frmBrandList()
        {
            InitializeComponent();

            /*for (int i = 0; i < 10; i++)
            {
                guna2DataGridView1.Rows.Add(1, "1", "Brand " + i);
            }*/
            cn = new SqlConnection(dbcon.MyConnection());
            LoadRecords();

        }
        public void LoadRecords() {
            int i = 0;
            guna2DataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT * FROM tblbrand order by brand",cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i += 1;
                guna2DataGridView1.Rows.Add(i,dr["id"].ToString(),dr["brand"].ToString());
            }
            cn.Close();
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            frmBrand frm = new frmBrand(this);
            frm.ShowDialog();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = guna2DataGridView1.Columns[e.ColumnIndex].Name;
            if (colname == "Edit")
            {
                frmBrand frm = new frmBrand(this);
                frm.lblID.Text = guna2DataGridView1[1, e.RowIndex].Value.ToString();
                frm.txtBrand.Text = guna2DataGridView1[2,e.RowIndex].Value.ToString();
                frm.ShowDialog();
            }
            else if (colname == "Delete")
            {
                if (MessageBox.Show("Sure to Delete?","Delete Record",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("Delete from tblBrand where id like '"+ guna2DataGridView1[1,e.RowIndex].Value.ToString()+ "'",cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Brand has Successfully deleted","POS",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    LoadRecords();
                }
            }
        }
    }
}
