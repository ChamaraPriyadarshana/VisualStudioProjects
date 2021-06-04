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
    public partial class frmCategoryList : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DbConnection dbcon = new DbConnection();
        public frmCategoryList()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            LoadRecords();
        }
        public void LoadRecords()
        {
            int i = 0;
            panelCategory.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT * FROM tblCategory order by category", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i += 1;
                panelCategory.Rows.Add(i, dr["id"].ToString(), dr["category"].ToString());
            }
            cn.Close();
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            frmCategory frm = new frmCategory(this);
            frm.btnSave.Enabled = true;
            frm.btnUpdate.Enabled = false;
            frm.ShowDialog();
        }

        private void panelCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = panelCategory.Columns[e.ColumnIndex].Name;
            if (colname == "Edit")
            {
                frmCategory frm = new frmCategory(this);
                frm.lblID.Text = panelCategory[1, e.RowIndex].Value.ToString();
                frm.txtCategory.Text = panelCategory[2, e.RowIndex].Value.ToString();
                frm.btnSave.Enabled = false;
                frm.ShowDialog();
            }
            else if (colname == "Delete")
            {
                if (MessageBox.Show("Sure to Delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("Delete from tblCategory where id like '" + panelCategory[1, e.RowIndex].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Brand has Successfully deleted", "POS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRecords();
                }
            }
        }
    }
}
