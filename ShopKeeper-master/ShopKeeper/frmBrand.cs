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
    public partial class frmBrand : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnection dbcon = new DbConnection();
        frmBrandList frmList;
        public frmBrand(frmBrandList fList)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            frmList = fList;
        }

        private void Clear() {
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            txtBrand.Clear();
            txtBrand.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you want to save this brand?","",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("INSERT INTO tblBrand(Brand)VALUES(@brand)",cn);
                    cm.Parameters.AddWithValue("@brand",txtBrand.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Record Saved Successfully");
                    Clear();
                    frmList.LoadRecords();
                    
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message) ;
            }
            finally
            {
                cn.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Sure to update brand ?","Update record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("update tblBrand set brand = @brand where id like'"+ lblID.Text +"'",cn );
                    cm.Parameters.AddWithValue("@brand",txtBrand.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Brand Updated!");
                    Clear();
                    frmList.LoadRecords();
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
