using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace ShopKeeper
{
    public class DbConnection
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        private double dailysales=0.0;
        private int productline;
        private int stockonhand;
        private int criticalitems;
        public string con="";


        public string MyConnection() {
            con= "Data Source=CHAMARA-ASPIRE;Initial Catalog=shopkeeper_db;Integrated Security=True";
            return con;
        }

        public double DailySales() {
            string sdate = DateTime.Now.ToShortDateString();
            cn = new SqlConnection();
            cn.ConnectionString = con;
            cn.Open();
            cm = new SqlCommand("SELECT isnull(sum(total),0) as total FROM tblCart WHERE sdate BETWEEN '"+sdate+"' AND '"+sdate+"' AND status LIKE 'Sold'",cn);
            dailysales = double.Parse(cm.ExecuteScalar().ToString());

            cn.Close();
            return dailysales;
        }
        public double ProductLine()
        {
            string sdate = DateTime.Now.ToShortDateString();
            cn = new SqlConnection();
            cn.ConnectionString = con;
            cn.Open();
            cm = new SqlCommand("SELECT count(*) FROM tblProduct", cn);
            productline = int.Parse(cm.ExecuteScalar().ToString());

            cn.Close();
            return productline;
        }

        public double StockonHand()
        {
            string sdate = DateTime.Now.ToShortDateString();
            cn = new SqlConnection();
            cn.ConnectionString = con;
            cn.Open();
            cm = new SqlCommand("SELECT isnull(sum(qty),0) as qty FROM tblProduct", cn);
            stockonhand = int.Parse(cm.ExecuteScalar().ToString());

            cn.Close();
            return stockonhand;
        }

        public double CriticalItems()
        {
            string sdate = DateTime.Now.ToShortDateString();
            cn = new SqlConnection();
            cn.ConnectionString = con;
            cn.Open();
            cm = new SqlCommand("SELECT COUNT(*) FROM vwCriticalItems", cn);
            criticalitems = int.Parse(cm.ExecuteScalar().ToString());

            cn.Close();
            return criticalitems;
        }

        public double getValue() {
            double vat=0.0;
            cn.ConnectionString = MyConnection();
            cn.Open();
            cm = new SqlCommand("SELECT * FROM tblVat",cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                vat = Double.Parse(dr["vat"].ToString());
            }
            dr.Close();
            cn.Close();
            return vat;
        }

        public string GetPassword(string user)
        {
            string password="";
            cn.ConnectionString = MyConnection();
            cn.Open();
            cm = new SqlCommand("SELECT * FROM tblUser WHERE name=@username",cn);
            cm.Parameters.AddWithValue("@username",user);
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                password = dr["password"].ToString();
            }
           
            dr.Close();
            cn.Close();
            return password;
        }
    }
}
