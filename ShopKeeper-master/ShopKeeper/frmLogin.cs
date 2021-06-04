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
    public partial class frmLogin : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DbConnection dbcon = new DbConnection();
        public frmLogin()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            this.ActiveControl = txtUser;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtPass.Clear();
            txtUser.Clear();
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username= "";
            string role = "";
            string name = "";

            try
            {
                bool found = false;
                cn.Open(); 
                cm = new SqlCommand("SELECT * FROM tblUser WHERE username = @username AND password = @password",cn);
                cm.Parameters.AddWithValue("@username",txtUser.Text);
                cm.Parameters.AddWithValue("@password",txtPass.Text);
                dr = cm.ExecuteReader();
                if (dr.Read())
                {
                    found = true;
                    username = dr["username"].ToString();
                    role = dr["role"].ToString();
                    name = dr["name"].ToString();
                }
                else
                {
                    found = false;
                }

                dr.Close();
                cn.Close();

                if (found==true)
                {
                    if (role == "Cashier")
                    {
                        MessageBox.Show("Welcome " + role + " " + name);
                        txtPass.Clear();
                        txtUser.Clear();
                        this.Hide();
                        frmPOS frm = new frmPOS();
                        frm.label2.Text = name;
                        frm.label1.Text = role;
                        frm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Welcome " + role + " " + name);
                        txtPass.Clear();
                        txtUser.Clear();
                        this.Hide();
                        
                        Form1 frm = new Form1();
                        frm.lblName.Text = name;
                        frm.ShowDialog();
                    }

                }
                else
                {
                    cn.Close();
                    MessageBox.Show("Invalid User", "Oops", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message,"Exception");
            }
        }

        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender,e);
            }
        }
    }
}
