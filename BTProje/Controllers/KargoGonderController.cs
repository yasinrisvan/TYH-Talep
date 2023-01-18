using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTProje.Models.EntityFramework;

namespace BTProje.Controllers
{

    public class KargoGonderController : Controller
    {
        // GET: KargoGonder
        // kargoTakipOtomasyonEntities db = new kargoTakipOtomasyonEntities();
        TYHTALEPEntities db = new TYHTALEPEntities();

        public ActionResult Index()
        {
            KullaniciTablosu kullanici = (KullaniciTablosu)Session["kullanici"];
            List<SelectListItem> bolge = (from i in db.BölgelerTablosu.ToList()
                                          select new SelectListItem
                                          {
                                              Text = i.Bölge,
                                              Value = i.Bölge_id.ToString()
                                          }).OrderBy(x => x.Text).ToList();
            ViewBag.bolge = bolge;

            var persG = db.KullaniciTablosu.Where(m => m.Kullanici_id == kullanici.Kullanici_id).FirstOrDefault();
            ViewBag.region = persG.BölgelerTablosu.Bölge;
            ViewBag.department = persG.DepartmanlarTablosu.Ad;
            ViewBag.user = persG.Ad + " " + persG.Soyad;


            List<SelectListItem> degerler = (from i in db.KullaniciTablosu.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.Ad + " " + i.Soyad,
                                                 Value = i.Kullanici_id.ToString()
                                             }).OrderBy(x => x.Text).ToList();
            ViewBag.dgr = degerler;

            //List<SelectListItem> degerler = (from i in db.Personeller.Where(x => x.Bolge == Selected(bolge)).ToList()
            //                                 select new SelectListItem
            //                                 {
            //                                     Text = i.Ad + " " + i.Soyad,
            //                                     Value = i.Id.ToString()
            //                                 }).ToList();

            List<SelectListItem> departman = (from i in db.DepartmanlarTablosu.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.Ad,
                                                  Value = i.Departman_id.ToString()
                                              }).OrderBy(x => x.Text).ToList();
            ViewBag.departman = departman;

            List<SelectListItem> gonderimTuru = (from i in db.GonderimTuru.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = i.Ad,
                                                     Value = i.Id.ToString()
                                                 }).OrderBy(x => x.Text).ToList();
            ViewBag.gonderimTuru = gonderimTuru;
            return View();
        }

        public JsonResult Cascad(int p)      //cascading dropdownlist
        {
            var dgr = (from i in db.KullaniciTablosu
                       where i.DepatmanID == p
                       select new SelectListItem
                       {
                           Text = i.Ad + " " + i.Soyad,
                           Value = i.Kullanici_id.ToString()
                       }).ToList();
            return Json(dgr, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Index(Kargo kargo)
        {
            if (!ModelState.IsValid)
            {
                List<SelectListItem> bolge = (from i in db.BölgelerTablosu.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.Bölge,
                                                  Value = i.Bölge_id.ToString(),

                                              }).OrderBy(x => x.Text).ToList();
                ViewBag.bolge = bolge;

                List<SelectListItem> degerler = (from i in db.KullaniciTablosu.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = i.Ad + " " + i.Soyad,
                                                     Value = i.Kullanici_id.ToString()
                                                 }).OrderBy(x => x.Text).ToList();
                ViewBag.dgr = degerler;

                List<SelectListItem> departman = (from i in db.DepartmanlarTablosu.ToList()
                                                  select new SelectListItem
                                                  {
                                                      Text = i.Ad,
                                                      Value = i.Departman_id.ToString()
                                                  }).OrderBy(x => x.Text).ToList();
                ViewBag.departman = departman;

                List<SelectListItem> gonderimTuru = (from i in db.GonderimTuru.ToList()
                                                     select new SelectListItem
                                                     {
                                                         Text = i.Ad,
                                                         Value = i.Id.ToString()
                                                     }).OrderBy(x => x.Text).ToList();
                ViewBag.gonderimTuru = gonderimTuru;
                TempData["Message"] = "KARGO OLUSTURULAMADI..!";
                return View("Index");
                //return RedirectToRoute(new
                //{
                //    controller = "KargoGonder",
                //    action = "Index"
                //});
            }

            else if (ModelState.IsValid)
            {
                KullaniciTablosu kullanici = (KullaniciTablosu)Session["kullanici"];
                kargo.Tarih = DateTime.Now;
                kargo.Durum = "Yolda";
                if (kargo.AliciAdSoyad != null)
                {
                    kargo.AliciAdSoyad = (kargo.AliciAdSoyad).ToUpper();
                }
                if (kargo.GonderenAdSoyad != null)
                {
                    kargo.GonderenAdSoyad = (kargo.GonderenAdSoyad).ToUpper();
                }
                if (kargo.Aciklama != null)
                {
                    kargo.Aciklama = (kargo.Aciklama).ToUpper();
                }

                var persGonderen = db.KullaniciTablosu.Where(m => m.Kullanici_id == kullanici.Kullanici_id).FirstOrDefault();
                int region = persGonderen.BölgelerTablosu.Bölge_id;
                int department = persGonderen.DepartmanlarTablosu.Departman_id;
                int user = persGonderen.Kullanici_id;


                ///
                ///Gönderen
                ///

                var persG = db.KullaniciTablosu.Where(m => m.Kullanici_id == user).FirstOrDefault();
                kargo.KullaniciTablosu1 = persG;

                var bolgeC = db.BölgelerTablosu.Where(m => m.Bölge_id == region).FirstOrDefault();
                kargo.BölgelerTablosu = bolgeC;

                var departmanC = db.DepartmanlarTablosu.Where(m => m.Departman_id == department).FirstOrDefault();
                if (departmanC == null)
                {
                    kargo.CikisDepartman = 1;
                    kargo.DepartmanlarTablosu = db.DepartmanlarTablosu.Where(m => m.Departman_id == kargo.CikisDepartman).FirstOrDefault();
                }
                kargo.DepartmanlarTablosu = departmanC;

                ///
                /// Alıcı 
                ///

                var persA = db.KullaniciTablosu.Where(m => m.Kullanici_id == kargo.AliciPersonelId).FirstOrDefault();
                kargo.KullaniciTablosu = persA;

                var bolgeV = db.BölgelerTablosu.Where(m => m.Bölge_id == kargo.VarisBolgesi).FirstOrDefault();

                kargo.BölgelerTablosu1 = bolgeV;

                var departmanV = db.DepartmanlarTablosu.Where(m => m.Departman_id == kargo.VarisDepartman).FirstOrDefault();
                kargo.DepartmanlarTablosu1 = departmanV;


                db.Kargo.Add(kargo);
                db.SaveChanges();
                var followNo = db.Kargo.Max(m => m.Id);

                TempData["MessageSuccess"] = "KARGO NUMARANIZ: " + followNo;

                return RedirectToRoute(new
                {
                    controller = "Kargolar",
                    action = "Index"
                });
            }
            else
            {
                TempData["Message"] = "KARGO OLUSTURULAMADI..!";
                return RedirectToAction("Index");
            }

        }
    }
}