using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTProje.Models.EntityFramework;
using System.Security;
using System.Web.Security;
using Microsoft.Web.Infrastructure.DynamicValidationHelper;

namespace BTProje.Controllers
{
    public class LoginPanelController : Controller
    {
        // GET: LoginPanel

        TYHTALEPEntities db = new TYHTALEPEntities();
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<SelectListItem> bolge = (from i in db.BölgelerTablosu.ToList()
                                          select new SelectListItem
                                          {
                                              Text = i.Bölge,
                                              Value = i.Bölge_id.ToString()
                                          }).OrderBy(x => x.Text).ToList();
            ViewBag.bolge = bolge;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(int bolgee, string SicilNo, string sifre)
        {
            List<SelectListItem> bolge = (from i in db.BölgelerTablosu.ToList()
                                          select new SelectListItem
                                          {
                                              Text = i.Bölge,
                                              Value = i.Bölge_id.ToString()
                                          }).OrderBy(x => x.Text).ToList();
            ViewBag.bolge = bolge;

            var sehir = db.BölgelerTablosu.Where(x => x.Bölge_id == bolgee).FirstOrDefault();
            var kullanici = db.KullaniciTablosu.FirstOrDefault(x => x.SicilNo == SicilNo && x.Sifre == sifre && x.BölgelerTablosu.Sehir == sehir.Sehir);
            if (kullanici != null && kullanici.RolTipi != null && kullanici.YetkiTipi != null)
            {
                var find = db.RolYetkiAtama.Where(m => m.Kullanici_Id == kullanici.Kullanici_id).ToList();
                Session.Add("yetki", find);
                FormsAuthentication.SetAuthCookie(kullanici.Kullanici_id.ToString(), false);
                Session.Add("kullanici", kullanici);
                Session.Add("bolge", sehir);
                return RedirectToAction("Index", "HomePage");
            }
            else
            {
                ViewBag.msg = "Giriş Bilgileriniz Veya Yetkiniz Geçersiz...!";
                return View();
            }
        }
        [AllowAnonymous]
        public ActionResult ForgetPassword()
        { 
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ForgetPassword(string SicilNo,string TC, string Sifre, string Tekrar)
        {
            List<SelectListItem> bolge = (from i in db.BölgelerTablosu.ToList()
                                          select new SelectListItem
                                          {
                                              Text = i.Bölge,
                                              Value = i.Bölge_id.ToString()
                                          }).OrderBy(x => x.Text).ToList();
            ViewBag.bolge = bolge;           
            if (Sifre == Tekrar )
            {
            var user = db.KullaniciTablosu.Where(m=> m.TC == TC.ToString() && m.SicilNo == SicilNo.ToString()).FirstOrDefault();                                       
                if(user != null)
                {
                user.Sifre = Sifre;
                db.SaveChanges();
                } 
            }
                return RedirectToAction("Index", "LoginPanel");           
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            List<SelectListItem> bolgeler = (from i in db.BölgelerTablosu.ToList()

                                             select new SelectListItem
                                             {
                                                 Text = i.Bölge,
                                                 Value = i.Bölge_id.ToString()

                                             }).OrderBy(x => x.Text).ToList();
            ViewBag.dgrb = bolgeler;

            List<SelectListItem> departmanlar = (from i in db.DepartmanlarTablosu.ToList()

                                                 select new SelectListItem
                                                 {
                                                     Text = i.Ad,
                                                     Value = i.Departman_id.ToString()

                                                 }).OrderBy(x => x.Text).ToList();
            ViewBag.dgrd = departmanlar;

            return View("Register", new KullaniciTablosu());
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(KullaniciTablosu kullanici)
        {
            var user = db.KullaniciTablosu.Where(m => m.TC == kullanici.TC && m.SicilNo == kullanici.SicilNo).FirstOrDefault();
            if (user == null)
            {
                kullanici.YetkiTipi = 2;
                kullanici.RolTipi = 8;
                kullanici.Ad = (kullanici.Ad).ToUpper();
                kullanici.Soyad = (kullanici.Soyad).ToUpper();
                db.KullaniciTablosu.Add(kullanici);
                db.SaveChanges();
                //TempData["kulladi"] = kullanici.KullaniciAdi;
            }
            return RedirectToAction("Index", "LoginPanel");
        }
    }
}