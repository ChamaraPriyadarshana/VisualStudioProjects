using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;

namespace ShopKeeper
{
    public partial class frmInventoryReport : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnection dbcon = new DbConnection();
        
        public frmInventoryReport()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        public void LoadReport()
        {
            ReportDataSource reportDataSource;
            try
            {
                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\Report4.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                cn.Open();

                da.SelectCommand = new SqlCommand("SELECT p.pcode,p.barcode,p.pdescription,b.brand,c.category,p.price,p.reorder,p.qty FROM tblProduct AS p INNER JOIN tblBrand AS b ON p.brand_id=b.id INNER JOIN tblCategory AS c ON p.category_id=c.id", cn);
                
                da.Fill(ds.Tables["dtInventory"]);
                cn.Close();
                //ReportParameter pCashier = new ReportParameter("pCashier", f.cboCashier.Text);
                //reportViewer1.LocalReport.SetParameters(pCashier);
                reportDataSource = new ReportDataSource("DataSet", ds.Tables["dtInventory"]);
                reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void frmInventoryReport_Load(object sender, EventArgs e)
        {

        }
    }
}
