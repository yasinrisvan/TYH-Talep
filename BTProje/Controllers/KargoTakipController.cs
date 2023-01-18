using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTProje.Controllers
{
    public class KargoTakipController : Controller
    {
        SqlConnection dbf = new SqlConnection("Server=192.168.100.12;Database=TYH802;Trusted_Connection=True;");
        // GET: KargoTakip
        [Authorize]
        public ActionResult Index()
        {
            List<SelectListItem> followPackages = new List<SelectListItem>();
            dbf.Open();
            string sql = "SELECT * from FollowPackages";
            SqlCommand cmd = new SqlCommand(sql, dbf);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                followPackages.Add(new SelectListItem
                {
                    Text = dr["Text"].ToString(),
                    Value = dr["Value"].ToString()
                });
            }
            dbf.Close();
            return View(followPackages);
        }
    }
}