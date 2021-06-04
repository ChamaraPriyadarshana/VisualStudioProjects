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
    public partial class frmStockIn : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DbConnection dbcon = new DbConnection();
        public frmStockIn()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            LoadProduct();
            LoadVendor();
            dt1.Value = DateTime.UtcNow;
        }

        public void LoadProduct()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT pcode,pdescription,qty FROM tblProduct WHERE pdescription LIKE '" + txtSearch.Text + "%'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i += 1;
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
                if (txtRefNo.Text == string.Empty)
                {
                    MessageBox.Show("Please enter Ref No.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRefNo.Focus();
                    return;
                }
                if (txtStockInBy.Text == string.Empty)
                {
                    MessageBox.Show("Please enter your name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRefNo.Focus();
                    return;
                }
                else if (MessageBox.Show("Wanna add this item?", "Add Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("INSERT INTO tblStockIn(refno,pcode,sdate,stockinby,vendorid) VALUES(@refno,@pcode,@sdate,@stockinby,@vendorid)", cn);
                    /*
                     INSERT INTO [dbo].[tblStockIn]
           ([refno]
           ,[pcode]
           ,[qty]
           ,[sdate]
           ,[stockinby]
           ,[status]
           ,[vendorid])
     VALUES
           (<refno, varchar(50),>
           ,<pcode, varchar(50),>
           ,<qty, int,>
           ,<sdate, datetime,>
           ,<stockinby, varchar(50),>
           ,<status, varchar(50),>
           ,<vendorid, int,>)
GO
                     */

                    cm.Parameters.AddWithValue("@refno", txtRefNo.Text);
                    cm.Parameters.AddWithValue("@pcode", dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    cm.Parameters.AddWithValue("@sdate", dt1.Value);
                    cm.Parameters.AddWithValue("@stockinby", txtStockInBy.Text);
                    cm.Parameters.AddWithValue("@vendorid", lblVendorID.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Successfully added");
                    LoadStockIn();
                }

            }
        }

        public void LoadStockIn()
        {
            int i = 0;
            dataGridView2.Rows.Clear();
            cn.Open();
           // cm = new SqlCommand("SELECT * FROM View_1 WHERE refno LIKE '"+txtRefNo.Text+ "' and status LIKE 'Pending'", cn);
            cm = new SqlCommand("SELECT * FROM vwStockIn WHERE refno LIKE '" + txtRefNo.Text + "' and status LIKE 'Pending'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                //dataGridView2.Rows.Add(i,dr["id"].ToString(), dr["refno"].ToString(), dr["vendor"].ToString(), dr["pcode"].ToString(), dr["pdescription"].ToString(), dr["qty"].ToString(), dr["sdate"].ToString(), dr["stockinby"].ToString());
                dataGridView2.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[6].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr["vendor"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView2.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                if (MessageBox.Show("Wanna Remove this?", "Delete Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM tblStockIn WHERE id = '" + dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Item has been removed", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStockIn();
                }
            }
        }
        public void Clear()
        {
            txtStockInBy.Clear();
            txtRefNo.Clear();
            dt1.Value = DateTime.Now;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.Rows.Count > 0)
                {
                    if (MessageBox.Show("Wanna save this record?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        for (int i = 0; i < dataGridView2.Rows.Count; i++)
                        {
                            cn.Open();
                            cm = new SqlCommand("UPDATE tblProduct SET qty = qty +'" + int.Parse(dataGridView2.Rows[i].Cells[4].Value.ToString()) + "' WHERE pcode LIKE '" + dataGridView2.Rows[i].Cells[2].Value.ToString() + "'", cn);
                            cm.ExecuteNonQuery();
                            cn.Close();

                            cn.Open();
                            cm = new SqlCommand("UPDATE tblStockIn SET qty = qty + " + int.Parse(dataGridView2.Rows[i].Cells[4].Value.ToString()) + ", status = 'Done' WHERE id LIKE '" + dataGridView2.Rows[i].Cells[0].Value.ToString() + "'", cn);
                            cm.ExecuteNonQuery();
                            cn.Close();

                        }
                        Clear();
                        LoadStockIn();
                        LoadProduct();
                    }
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "oops", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewHistory_Click(object sender, EventArgs e)
        {


            //history.ShowDialog();
            //this.Dispose();


            frmStockInHistory frm = new frmStockInHistory();

            frm.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void cboVendor_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        public void LoadVendor() {
            int i = 0;
            cboVendor.Clear();
            cn.Open();
            cm = new SqlCommand("SELECT * FROM tblVendor",cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i += 0;
               
                cboVendor.AddItem(dr["vendor"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void cboVendor_TextChanged(object sender, EventArgs e)
        {

            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT * FROM tblVendor WHERE vendor LIKE '"+ cboVendor.selectedValue+ "'", cn);
                dr = cm.ExecuteReader();
                if (dr.Read())
                {
                    lblVendorID.Text = dr["id"].ToString();
                    txtPerson.Text = dr["contactperson"].ToString();
                    txtAddress.Text = dr["address"].ToString();
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                dr.Close();
                cn.Close();
                MessageBox.Show(ex.Message);

            }
            
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            txtRefNo.Clear();
            txtRefNo.Text += rnd.Next();
        }
    }
}
