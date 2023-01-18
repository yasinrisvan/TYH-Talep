using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTProje.Models.EntityFramework;
namespace BTProje.Controllers
{
    public class HomeController : Controller
    {
        TYHTALEPEntities db = new TYHTALEPEntities();   
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Index()
        {
            var dt = db.KullaniciTablosu.Find(Session["kullanici"]);
            return View(dt);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}