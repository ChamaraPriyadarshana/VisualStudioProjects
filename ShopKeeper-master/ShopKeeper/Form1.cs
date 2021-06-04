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
using Tulpep.NotificationWindow;

namespace ShopKeeper
{
    public partial class Form1 : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DbConnection dbcon = new DbConnection();
        public Form1()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            //cn.Open();
            // MessageBox.Show("Connected");
            NotifyCriticalItems();
            loadDashboard();

        }

        public void NotifyCriticalItems() {
            string critical = "";
            int i = 0;
            cn.Open();
            cm = new SqlCommand("SELECT * FROM vwCriticalItems", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                critical += i +". "+ dr["pdescription"].ToString()+" "+dr["qty"] + Environment.NewLine;//😍
            }
            dr.Close();
            cn.Close();

            PopupNotifier popup = new PopupNotifier();
            popup.Image = Properties.Resources.inventory_mgmt;
            popup.TitleText = "Critical Items";
            popup.ContentText = critical;
            popup.Popup();
        }

        private void bunifuGradientPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnBrand_Click(object sender, EventArgs e)
        {
            guna2Panel1.Controls.Clear();
            frmBrandList frm = new frmBrandList();
            frm.TopLevel = false;
            guna2Panel1.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            guna2Panel1.Controls.Clear();
            frmCategoryList frm = new frmCategoryList();
            frm.TopLevel = false;
            guna2Panel1.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            guna2Panel1.Controls.Clear();
            frmProductList frm = new frmProductList();
            frm.TopLevel = false;
            guna2Panel1.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnStockIn_Click(object sender, EventArgs e)
        {
            guna2Panel1.Controls.Clear();
            frmStockIn frm = new frmStockIn();
            //frmStockInHistory frm = new frmStockInHistory();

            frm.TopLevel = false;
            guna2Panel1.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnStockInHistory_Click(object sender, EventArgs e)
        {
            guna2Panel1.Controls.Clear();
            
            frmStockInHistory frm = new frmStockInHistory();

            frm.TopLevel = false;
            guna2Panel1.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnPosView_Click(object sender, EventArgs e)
        {
            frmPOS frm = new frmPOS();
            frm.label1.Text = "Administrator";
            frm.label2.Text = lblName.Text;
            frm.Show();
            

        }

        private void btnUserView_Click(object sender, EventArgs e)
        {
            guna2Panel1.Controls.Clear();
            frmUsers userAccount = new frmUsers(this);
            userAccount.txtCurrentPassword.Text = dbcon.GetPassword(lblName.Text);
            userAccount.TopLevel = false;
            guna2Panel1.Controls.Add(userAccount);
            userAccount.BringToFront();
            userAccount.Show();
        }

        private void btnSalesHistory_Click(object sender, EventArgs e)
        {
            guna2Panel1.Controls.Clear();
            frmSoldItems frm = new frmSoldItems();
            frm.suser = lblName.Text;
            frm.TopLevel = false;
            guna2Panel1.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnRecords_Click(object sender, EventArgs e)
        {
            guna2Panel1.Controls.Clear();
            frmRecords frm = new frmRecords();
            frm.TopLevel = false;
            frm.LoadCriticalItems();
            frm.LoadInventory();
            guna2Panel1.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            loadDashboard();
        }

        public void loadDashboard() {
            frmDashboard frm = new frmDashboard();
            guna2Panel1.Controls.Clear();

            frm.TopLevel = false;
            frm.lblDailySales.Text = dbcon.DailySales().ToString("#,##0.00");
            frm.lblProductLine.Text = dbcon.ProductLine().ToString();
            frm.lblStockOnHand.Text = dbcon.StockonHand().ToString();
            frm.lblCriticalItems.Text = dbcon.CriticalItems().ToString();
            guna2Panel1.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnVendor_Click(object sender, EventArgs e)
        {
            guna2Panel1.Controls.Clear();
            frmVendorList frm = new frmVendorList();
            frm.TopLevel = false;
            guna2Panel1.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void btnAdjustment_Click(object sender, EventArgs e)
        {
            guna2Panel1.Controls.Clear();
            frmAdjustment frm = new frmAdjustment(this);
            frm.LoadRecords();
            frm.txtUser.Text = lblName.Text;
            frm.RefNo();
            frm.TopLevel = false;
            guna2Panel1.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }
    }
}
