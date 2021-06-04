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
using System.Windows.Forms.DataVisualization.Charting;

namespace ShopKeeper
{
    public partial class frmDashboard : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnection dbcon = new DbConnection();
        SqlDataReader dr;
        public frmDashboard()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            LoadChart();
        }
        public void LoadChart() {
            cn.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT YEAR(sdate)as year, ISNULL(SUM(total),0.0) as total from tblCart WHERE status LIKE 'Sold' GROUP BY YEAR(sdate)", cn);
            DataSet ds = new DataSet();
            da.Fill(ds,"Sales");
            chart1.DataSource = ds.Tables["Sales"];

            Series series1 = chart1.Series["Series1"];
            series1.ChartType = SeriesChartType.Doughnut;
            series1.Name = "SALES";

            var chart = chart1;
            chart.Series[series1.Name].XValueMember = "year";
            chart.Series[series1.Name].YValueMembers = "total";
            chart.Series[0].IsValueShownAsLabel = true;
            //chart.Series[0].LegendText = "#";

            cn.Close();
        }
    }
}
