using BTProje.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTProje.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        TYHTALEPEntities db = new TYHTALEPEntities();

        public void method()
        {
            List<SelectListItem> user = (from i in db.KullaniciTablosu.ToList()
                                         select new SelectListItem
                                         {
                                             Text = i.SicilNo + " - " + i.Ad + " " + i.Soyad,
                                             Value = i.Kullanici_id.ToString()
                                         }).ToList();
            ViewBag.user = user;
            List<SelectListItem> rol = (from i in db.Roller.ToList()
                                        select new SelectListItem
                                        {
                                            Text = i.RolAdı,
                                            Value = i.Rol_id.ToString()
                                        }).ToList();
            ViewBag.rol = rol;
            List<SelectListItem> yetki = (from i in db.Yetki.ToList()
                                          select new SelectListItem
                                          {
                                              Text = i.YetkiTipi,
                                              Value = i.Yetki_id.ToString()
                                          }).ToList();
            ViewBag.yetki = yetki;
        }

        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            method();
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            int kullanici = Convert.ToInt32(formCollection["kullanici"]);
            int rol = Convert.ToInt32(formCollection["rol"]);
            int yetki = Convert.ToInt32(formCollection["yetki"]);
            RolYetkiAtama liste = new RolYetkiAtama();
            var bul = db.RolYetkiAtama.Where(m => m.Kullanici_Id == kullanici && m.Rol_Id == rol).FirstOrDefault();
            if (bul == null)
            {
                liste.Kullanici_Id = kullanici;
                liste.Rol_Id = rol;
                liste.Yetki_Id = yetki;
                db.RolYetkiAtama.Add(liste);
            }
            else
            {
                ViewBag.message = "Rol ve Yetki Zaten Mevcut";
                method();
                return View("Index");
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public JsonResult Arama(string p)      //cascading dropdownlist
        {
            List<SelectListItem> bul = (from i in db.KullaniciTablosu.ToList()
                                        select new SelectListItem
                                        {
                                            Text = i.SicilNo + " - " + i.Ad + " " + i.Soyad,
                                            Value = i.Kullanici_id.ToString()
                                        }).ToList();

            if (p == null || p == "")
            {
                bul = bul.OrderBy(x => x.Text).ToList();
                return Json(bul.ToList(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                p = p.ToUpper();
                bul = bul.Where(m => m.Text.Contains(p)).OrderByDescending(x => x.Text).ToList();
                return Json(bul, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Yetkiler(int p)
        {
            var bul = (from i in db.RolYetkiAtama
                       where i.Kullanici_Id == p
                       select new SelectListItem
                       {
                           Text = i.KullaniciTablosu.Ad + " " + i.KullaniciTablosu.Soyad + "-" + i.Roller.RolAdı + "-" + i.Yetki.YetkiTipi,
                           Value = i.Atama_Id.ToString()
                       }).ToList();
            return Json(bul, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Sil(int p)
        {
            var bul = db.RolYetkiAtama.Find(p);
            db.RolYetkiAtama.Remove(bul);
            db.SaveChanges();
            return Json(bul,JsonRequestBehavior.AllowGet);
        }
    }
}
