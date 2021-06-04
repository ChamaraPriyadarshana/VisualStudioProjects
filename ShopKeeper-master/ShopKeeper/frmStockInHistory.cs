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
    public partial class frmStockInHistory : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DbConnection dbcon = new DbConnection();
        public frmStockInHistory()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            date2.Value = DateTime.UtcNow;
            LoadStockInHistory();

        }

        private void LoadStockInHistory() {
            int i = 0;
            dataGridView2.Rows.Clear();
            cn.Open();
            //cm = new SqlCommand("SELECT * FROM View_1 WHERE refno LIKE '"+txtRefNo.Text+ "' and status LIKE 'Pending'", cn);
            cm = new SqlCommand("SELECT * FROM vwStockIn WHERE cast(sdate as date) between '"+date1.Value.ToShortDateString()+"' and '"+date2.Value.ToShortDateString() + "' and status LIKE 'Done'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                //dataGridView2.Rows.Add(i,dr["id"].ToString(), dr["refno"].ToString(), dr["pcode"].ToString(), dr["pdescription"].ToString(), dr["qty"].ToString(), dr["sdate"].ToString(), dr["stockinby"].ToString());
                dataGridView2.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[8].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[7].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadStockInHistory();
        }


    }
}
