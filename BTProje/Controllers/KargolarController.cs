
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTProje.Models.EntityFramework;
using PagedList;
using PagedList.Mvc;

namespace BTProje.Controllers
{
    public class KargolarController : Controller
    {
        TYHTALEPEntities db = new TYHTALEPEntities();

        [Authorize]
        public ActionResult Index(string id)
        {
            var kargolar = db.Kargo
                .Where(x => x.Durum == "Yolda" || x.Durum == "Gönderime Hazır")
    .OrderByDescending(x => x.Id)
    .ToList();
            var degerler = from d in db.Kargo select d;
            if (!String.IsNullOrEmpty(id))
            {
                kargolar = db.Kargo.Where(x => x.Id.ToString() == id).OrderByDescending(x => x.Id)
                   .ToList();
            }
            var kargo = from k in db.KargoHareketleri
                        select k;

            return View(kargolar);
        }


        [Authorize]
        public ActionResult FullPackages()
        {
            //var kargolar = db.Kargo.Where(x => x.Durum == true).ToList();
            var kargolar = db.Kargo.OrderByDescending(x => x.Id)
    .ToList();
            var degerler = from d in db.Kargo select d;

            return View("FullPackages", kargolar);
        }

        [Authorize]
        public ActionResult PickUpPackages()
        {
            //var kargolar = db.Kargo.Where(x => x.Durum == true).ToList();
            var kargolar = db.Kargo.Where
    (x => x.Durum == "Teslim Alındı").OrderByDescending(x => x.Id)
    .ToList();
            var degerler = from d in db.Kargo select d;

            return View("PickUpPackages", kargolar);
        }

        [Authorize]
        public ActionResult OnTheWayPackages()
        {
            //var kargolar = db.Kargo.Where(x => x.Durum == true).ToList();
            var kargolar = db.Kargo.Where
    (x => x.Durum == "Yolda").OrderByDescending(x => x.Id)
    .ToList();
            var degerler = from d in db.Kargo select d;

            return View("OnTheWayPackages", kargolar);
        }


        [Authorize]
        public ActionResult DeletedPackages()
        {
            //var kargolar = db.Kargo.Where(x => x.Durum == true).ToList();
            var kargolar = db.Kargo.Where
    (x => x.Durum == "Silindi" || x.Durum == "Gönderime Hazır").OrderByDescending(x => x.Id)
    .ToList();
            var degerler = from d in db.Kargo select d;

            return View("DeletedPackages", kargolar);
        }


        [Authorize]
        public ActionResult PickUp(int id)
        {
            var kargo = db.Kargo.Find(id);
            if (kargo.Durum != "Silindi")
            {
                kargo.Durum = "Teslim Alındı";
            }

            ViewBag.id = id;
            Session.Add("cu", id);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult PickUp(int id, string aciklama)
        {
            KullaniciTablosu kullanici = (KullaniciTablosu)Session["kullanici"];
            if (/*id == null || */id == 0)
            {
                return RedirectToAction("Index");
            }
            var kargo = db.Kargo.Find(id);
            if (!(aciklama == null))
            {
                aciklama.Trim();
            }
            if (aciklama == null || aciklama == "")
            {
                kargo.Donus = "Sorunsuz";
            }
            else
            {
                kargo.Donus = aciklama;
            }
            var persGonderen = db.KullaniciTablosu.Where(m => m.Kullanici_id == kullanici.Kullanici_id).FirstOrDefault();
            int region = persGonderen.BölgelerTablosu.Bölge_id;
            int department = persGonderen.DepartmanlarTablosu.Departman_id;
            int user = persGonderen.Kullanici_id;

            KargoHareketleri kargoHareket = new KargoHareketleri
            {
                KargoNo = id,
                TeslimAlanBolgeId = region,
                TeslimAlanDepartmanId = department,
                TeslimAlanPersonelId = user,
                Tarih = DateTime.Now,
                Durum = "Teslim Alındı",
                GonderimTuruId = 12
            };

            db.KargoHareketleri.Add(kargoHareket);
            db.SaveChanges();

            var persG = db.KullaniciTablosu.Where(m => m.Kullanici_id == user).FirstOrDefault();

            kargo.Durum = "Teslim Alındı";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize]

        public ActionResult Sil(int id)
        {
            var kargo = db.Kargo.Find(id);
            kargo.Durum = "Silindi";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize]

        public ActionResult Getir(int id)
        {
            var kargo = db.Kargo.Find(id);
            List<SelectListItem> kargoNo = (from i in db.Kargo.ToList()
                                            select new SelectListItem
                                            {
                                                Text = i.Id.ToString(),
                                                Value = i.Id.ToString()
                                            }).OrderBy(x => x.Text).ToList();
            ViewBag.kargoNo = kargoNo;
            List<SelectListItem> degerler = (from i in db.KullaniciTablosu.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.Ad + " " + i.Soyad,
                                                 Value = i.Kullanici_id.ToString()
                                             }).OrderBy(x => x.Text).ToList();
            ViewBag.dgr = degerler;

            List<SelectListItem> bolge = (from i in db.BölgelerTablosu.ToList()
                                          select new SelectListItem
                                          {
                                              Text = i.Bölge,
                                              Value = i.Bölge_id.ToString()
                                          }).OrderBy(x => x.Text).ToList();
            ViewBag.bolge = bolge;

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
            return View("Getir", kargo);
        }

        [Authorize]
        public ActionResult Guncelle(Kargo k)
        {
            if (ModelState.IsValid)
            {
                var kargo = db.Kargo.Find(k.Id);
                //kargo.GonderenPersonelId = k.GonderenPersonelId;
                kargo.GonderenAdSoyad = k.GonderenAdSoyad.ToUpper();
                kargo.CikisBolgesi = k.CikisBolgesi;

                if (k.CikisDepartman == null)
                    k.CikisDepartman = 1;
                kargo.CikisDepartman = k.CikisDepartman;

                kargo.Tarih = DateTime.Now;
                kargo.Aciklama = k.Aciklama.ToUpper();
                kargo.Adet = k.Adet;
                //kargo.AliciPersonelId = k.AliciPersonelId;
                kargo.AliciAdSoyad = k.AliciAdSoyad.ToUpper();
                kargo.VarisBolgesi = k.VarisBolgesi;
                kargo.VarisDepartman = k.VarisDepartman;

                db.SaveChanges();
                TempData["MessageSuccess"] = "GUNCELLENDI";

            }
            else
            {

                TempData["Message"] = "GUNCELLENEMEDI";
            }
            return RedirectToAction("Index", "Kargolar");

        }
    }
}