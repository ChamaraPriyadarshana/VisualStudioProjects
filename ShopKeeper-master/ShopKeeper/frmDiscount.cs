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
    public partial class frmDiscount : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DbConnection dbcon = new DbConnection();
        frmPOS pOS;
        public frmDiscount(frmPOS frmpos)
        {
            InitializeComponent();
            this.ActiveControl = txtPercent;
            cn = new SqlConnection(dbcon.MyConnection());
            pOS = frmpos;
           
        }

        private void txtPercent_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double discount = Double.Parse(txtPrice.Text) * double.Parse(txtPercent.Text);
                txtAmount.Text = discount.ToString();
            }
            catch (Exception ex)
            {

                txtAmount.Text = "0.00";
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("UPDATE tblCart SET disc = @disc WHERE id = @id",cn);
                cm.Parameters.AddWithValue("@disc", double.Parse(txtAmount.Text));
                cm.Parameters.AddWithValue("@id", int.Parse(lblID.Text));
                cm.ExecuteNonQuery();
                cn.Close();
                pOS.loadCart();
                this.Dispose();
            }
            catch (Exception)
            {
                cn.Close();
            }
        }
    }
}
