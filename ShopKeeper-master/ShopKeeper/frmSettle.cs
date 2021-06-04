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
    public partial class frmSettle : Form
    {
        frmPOS pOS;
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DbConnection dbcon = new DbConnection();
        public frmSettle(frmPOS frmpos)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            this.ActiveControl = txtCache;
            pOS = frmpos;
        }

        private void txtCache_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double sale = double.Parse(txtSale.Text);
                double cache = double.Parse(txtCache.Text);
                double change = cache - sale;
                txtChange.Text = change.ToString("#,##0.00");
            }
            catch (Exception ex)
            {
                txtCache.Text = "0.00";
            }
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                if ((double.Parse(txtChange.Text) < 0) || (txtCache.Text == String.Empty))
                {
                    MessageBox.Show("Please settle the amount", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else {
                    
                    for (int i = 0; i < pOS.productListGridView.Rows.Count; i++)
                    {
                        cn.Open();
                        cm = new SqlCommand( "UPDATE tblProduct SET qty = qty - " + int.Parse(pOS.productListGridView.Rows[i].Cells[3].Value.ToString()) + " WHERE pcode = '" + pOS.productListGridView.Rows[i].Cells[6].Value.ToString() + "' ",cn);
                        cm.ExecuteNonQuery();
                        cn.Close();

                        cn.Open();
                        cm = new SqlCommand("UPDATE tblCart SET status = 'Sold' WHERE id LIKE '" + pOS.productListGridView.Rows[i].Cells[0].Value.ToString() + "' ", cn);
                        cm.ExecuteNonQuery();
                        cn.Close(); 
                    }

                    frmReceipt frm = new frmReceipt(pOS);
                    frm.LoadReport();
                    frm.ShowDialog();

                    MessageBox.Show("Saved successfully");
                    pOS.getTransNo();
                    pOS.loadCart();
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
