using Microsoft.Reporting.WinForms;
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
    public partial class frmSoldReport : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnection dbcon = new DbConnection();
        frmSoldItems f;
        public frmSoldReport(frmSoldItems frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            f = frm;
        }

        private void frmSoldReport_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
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
                if (f.cboCashier.Text == "All Cashier") { da.SelectCommand = new SqlCommand("SELECT c.id,c.tr_no,c.pid,p.pdescription,c.price,c.qty,c.disc,c.total FROM tblCart AS c INNER JOIN tblProduct AS p ON c.pid = p.pcode WHERE status LIKE 'Sold' AND sdate BETWEEN '" + f.dt1.Value + "' AND '" + f.dt2.Value + "'", cn); }
                else
                {
                    da.SelectCommand = new SqlCommand("SELECT c.id,c.tr_no,c.pid,p.pdescription,c.price,c.qty,c.disc,c.total FROM tblCart AS c INNER JOIN tblProduct AS p ON c.pid = p.pcode WHERE status LIKE 'Sold' AND sdate BETWEEN '" + f.dt1.Value + "' AND '" + f.dt2.Value + "' AND cashier LIKE '" + f.cboCashier.Text + "'", cn);
                }
            

                da.Fill(ds.Tables["dtSoldItemList"]);
                cn.Close();
                ReportParameter pCashier = new ReportParameter("pCashier", f.cboCashier.Text);
                reportViewer1.LocalReport.SetParameters(pCashier);
                reportDataSource = new ReportDataSource("DataSet", ds.Tables["dtSoldItemList"]);
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
    }
}
