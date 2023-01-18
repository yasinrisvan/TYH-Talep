using BTProje.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Security;

namespace BTProje.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        TYHTALEPEntities db = new TYHTALEPEntities();
        [Authorize]
        
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string sifre, string password, string confirmPassword)
        {
            KullaniciTablosu kullanici = (KullaniciTablosu)Session["kullanici"];
            if(kullanici.Sifre == sifre && password == confirmPassword)
            {
                var k = db.KullaniciTablosu.Find(kullanici.Kullanici_id);
                k.Sifre = confirmPassword;
                db.SaveChanges();
                kullanici.Sifre = confirmPassword;
                return RedirectToAction("Index", "LoginPanel");
            }
            return View();


        }
    }
}