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
    public partial class frmVoid : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DbConnection dbcon = new DbConnection();
        frmCancelDetails f;
        public frmVoid(frmCancelDetails frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            f = frm;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (TextBox1.Text != string.Empty)
                {
                    string user;
                    cn.Open();
                    cm = new SqlCommand("select * from tblUser where username=@username and password=@password", cn);
                    cm.Parameters.AddWithValue("@username", TextBox1.Text);
                    cm.Parameters.AddWithValue("@password", TextBox2.Text);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        user = dr["username"].ToString();
                        dr.Close();
                        cn.Close();

                        SaveCancelOrder(user);
                        if (f.cboAction.Text == "Yes")
                        {
                            UpdateData("UPDATE tblProduct SET qty=qty + " + int.Parse(f.txtCancelQty.Text) + " WHERE pcode = '" + f.txtPCode.Text + "'");
                        }
                        UpdateData("UPDATE tblCart SET qty = qty - " + int.Parse(f.txtCancelQty.Text) + " WHERE id LIKE '" + f.txtID.Text +"'");//" + f.txtID.Text + "
                        MessageBox.Show("Transaction Cancelled", "Cancel Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Dispose();
                        f.RefreshList();
                        f.Dispose();
                    }
                    dr.Close();
                    cn.Close();
                }
                else
                {
                    MessageBox.Show("Textbox is empty");
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message,"Oops");
            }
        }

        public void SaveCancelOrder(String user)
        {
            cn.Open();
            cm = new SqlCommand("INSERT INTO tblCancel(transno,pcode,price,qty,total,sdate,voidby,cancelledby,reason,action) " + "VALUES(@transno,@pcode,@price,@qty,@total,@sdate,@voidby,@cancelledby,@reason,@action)", cn);
            cm.Parameters.AddWithValue("@transno",f.txtTrNo.Text);
            cm.Parameters.AddWithValue("@pcode",f.txtPCode.Text);
            cm.Parameters.AddWithValue("@price",double.Parse(f.txtPrice.Text));
            cm.Parameters.AddWithValue("@qty",int.Parse(f.txtQty.Text));
            cm.Parameters.AddWithValue("@total", double.Parse(f.txtTotal.Text));
            cm.Parameters.AddWithValue("@sdate", DateTime.Now);
            cm.Parameters.AddWithValue("@voidby", f.txtVoidBy.Text);
            cm.Parameters.AddWithValue("@cancelledby", user);
            cm.Parameters.AddWithValue("@reason",f.txtReason.Text);
            cm.Parameters.AddWithValue("@action", f.cboAction.Text);
            cm.ExecuteNonQuery();
            cn.Close();
        }

        public void UpdateData(string sql)
        {
            cn.Open();
            cm = new SqlCommand(sql,cn);
            cm.ExecuteNonQuery();
            cn.Close();
        }
    }
}
