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
    public partial class frmUsers : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnection dbcon = new DbConnection();
        Form1 f;
        public frmUsers(Form1 frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            f = frm;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            cn.Open();
            cm = new SqlCommand("insert into tblUser (username,password,role,name) VALUES (@username,@password,@role,@name)", cn);
            cm.Parameters.AddWithValue("@username", txtUser.Text);
            cm.Parameters.AddWithValue("@password", txtPass.Text);
            cm.Parameters.AddWithValue("@role", cboRole.Text);
            cm.Parameters.AddWithValue("@name", txtName.Text);
            cm.ExecuteNonQuery();
            cn.Close();
            MessageBox.Show("New " + cboRole.Text + " has added");
            clear();

        }

        private void clear()
        {
            txtUser.Clear();
            txtPass.Clear();
            txtConfPass.Clear();
            //cboRole.Text = "";
            txtName.Clear();
            txtUser.Focus();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                string oldpass = dbcon.GetPassword(f.lblName.Text.ToString());

                if (oldpass!= txtCurrentPassword.Text)
                {
                    MessageBox.Show("Current password not matching", "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                else if (txtNewPassword.Text!=txtConfirmPassword.Text)
                {
                    MessageBox.Show("Confirm password not matching", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (MessageBox.Show("Change Password","Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.Yes)
                    {
                        cn.Open();
                        cm = new SqlCommand("UPDATE tblUser SET password = @password WHERE name = @username",cn);
                        cm.Parameters.AddWithValue("@username",f.lblName.Text);
                        cm.Parameters.AddWithValue("@password", txtNewPassword.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Password has been successfully saved", "Save changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Dispose();
                    }
                }

              
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message,"Error");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //getpass
        }
    }
}
