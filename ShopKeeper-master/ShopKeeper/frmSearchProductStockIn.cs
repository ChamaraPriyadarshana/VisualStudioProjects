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
    public partial class frmSearchProductStockIn : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DbConnection dbcon = new DbConnection();
        frmStockIn slist;
        public frmSearchProductStockIn(frmStockIn fList)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }
        public void LoadProduct()
        {
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT pcode,pdescription,pquantity FROM tblProduct WHERE pdescription LIKE '%" + txtSearch.Text + "%' ORDER BY pdescription", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                dataGridView1.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "Select")
            {
                if (slist.txtRefNo.Text == string.Empty)
                {
                    MessageBox.Show("Please enter Ref No.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    slist.txtRefNo.Focus();
                    return;
                }
                if (slist.txtStockInBy.Text == string.Empty)
                {
                    MessageBox.Show("Please enter your name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    slist.txtRefNo.Focus();
                    return;
                }
                if (MessageBox.Show("Wanna add this item?", "Add Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("INSERT INTO tblStockIn(refno,pcode,sdate,stockinby,vendorid) VALUES(@refno,@pcode,@sdate,@stockinby,@vendorid)", cn);
                    cm.Parameters.AddWithValue("@refno", slist.txtRefNo.Text);
                    cm.Parameters.AddWithValue("@pcode", dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    cm.Parameters.AddWithValue("@sdate", slist.dt1.Value);
                    cm.Parameters.AddWithValue("@vendorid", slist.lblVendorID);
                    cm.Parameters.AddWithValue("@stockinby", slist.txtStockInBy.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Successfully added");
                    slist.LoadStockIn();
                }

            }
        }
    }
}
