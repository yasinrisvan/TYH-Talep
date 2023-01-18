using BTProje.Models.EntityFramework;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTProje.Controllers
{
    public class KargoTransferController : Controller
    {
        // GET: KargoHareket
        //kargoTakipOtomasyonEntities db = new kargoTakipOtomasyonEntities();
        TYHTALEPEntities db = new TYHTALEPEntities();
        [Authorize]

        public ActionResult Index(string id, int sayfa = 1)
        {
            //var kargoHareket = db.KargoHareketleri.Where(x=> x.Durum==true).ToList();
            var kargolar = db.KargoHareketleri.Where
                (x => x.Durum == "Yolda" || x.Durum == "Teslim Alındı")
               .OrderByDescending(x => x.KargoNo).ThenByDescending(x => x.Tarih).ThenByDescending(x => x.TeslimAlanPersonelId)
               .ToList().ToPagedList(sayfa, 10);

            if (!String.IsNullOrEmpty(id))
            {
                kargolar = db.KargoHareketleri.Where(x => x.KargoNo.ToString() == id)
                    .Where(x => x.Durum == "Yolda" || x.Durum == "Teslim Alındı")
                    .OrderByDescending(x => x.Tarih)
                   .ToList().ToPagedList(sayfa, 10);
                //kargolar = db.Kargo.Where(x => x.Durum == true && x.Id.ToString() == id)
            }

            return View(kargolar);
        }
        [Authorize]
        public ActionResult Sil(int id)
        {
            //string kargo = id.ToString();
            var kargoHareket = db.KargoHareketleri.Find(id);
            kargoHareket.Durum = "Silindi";
            db.SaveChanges();
            //var kargolar = db.KargoHareketleri.Where(x => x.Id == id)
            return RedirectToAction("Index", "Kargolar");
        }

        [Authorize]
        public ActionResult TransferEkle()

        {

            List<SelectListItem> kargoNo = (from i in db.Kargo.Where(x => x.Durum == "Yolda").ToList()
                                            select new SelectListItem
                                            {
                                                Text = i.Id.ToString(),
                                                Value = i.Id.ToString()
                                            }).OrderByDescending(x => x.Text).ToList();
            ViewBag.kargoNo = kargoNo;

            List<SelectListItem> degerler = (from i in db.KullaniciTablosu.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = i.Ad + " " + i.Soyad,
                                                 Value = i.Kullanici_id.ToString(),
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
        public JsonResult CascadName(int p)      //cascading dropdownlist
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
        public ActionResult TransferEkle(KargoHareketleri k)
        {
            if (!ModelState.IsValid)
            {
                List<SelectListItem> kargoNo = (from i in db.Kargo.Where(x => x.Durum == "Yolda").ToList()
                                                select new SelectListItem
                                                {
                                                    Text = i.Id.ToString(),
                                                    Value = i.Id.ToString()
                                                }).OrderByDescending(x => x.Text).ToList();
                ViewBag.kargoNo = kargoNo;

                List<SelectListItem> degerler = (from i in db.KullaniciTablosu.ToList()
                                                 select new SelectListItem
                                                 {
                                                     Text = i.Ad + " " + i.Soyad,
                                                     Value = i.Kullanici_id.ToString(),
                                                 }).OrderBy(x => x.Text).ToList();
                ViewBag.dgr = degerler;

                List<SelectListItem> bolgea = (from i in db.BölgelerTablosu.ToList()
                                               select new SelectListItem
                                               {
                                                   Text = i.Bölge,
                                                   Value = i.Bölge_id.ToString()
                                               }).OrderBy(x => x.Text).ToList();
                ViewBag.bolge = bolgea;


                List<SelectListItem> departmana = (from i in db.DepartmanlarTablosu.ToList()
                                                   select new SelectListItem
                                                   {
                                                       Text = i.Ad,
                                                       Value = i.Departman_id.ToString()
                                                   }).OrderBy(x => x.Text).ToList();
                ViewBag.departman = departmana;

                List<SelectListItem> gonderimTuru = (from i in db.GonderimTuru.ToList()
                                                     select new SelectListItem
                                                     {
                                                         Text = i.Ad,
                                                         Value = i.Id.ToString()
                                                     }).OrderBy(x => x.Text).ToList();
                ViewBag.gonderimTuru = gonderimTuru;
                TempData["Message"] = "TRANSFER OLUSTURULAMADI..!";
                return View("TransferEkle");
            }
            k.Tarih = DateTime.Now;
            k.Durum = "Yolda";
            if (k.AracPlakaNo != null)
            {
                k.AracPlakaNo = (k.AracPlakaNo).ToUpper();
            }

            if (k.TeslimAlanPersonelAdSoyad != null)
            {
                k.TeslimAlanPersonelAdSoyad = (k.TeslimAlanPersonelAdSoyad).ToUpper();
            }
            //k.TeslimAlanPersonelAdSoyad = (k.TeslimAlanPersonelAdSoyad).ToUpper();
            //k.AracPlakaNo = (k.AracPlakaNo).ToUpper();
            //   kargo.Id = k.Id;

            ///
            ///Teslim Alan
            ///

            var pers = db.KullaniciTablosu.Where(m => m.Kullanici_id == k.TeslimAlanPersonelId).FirstOrDefault();
            k.KullaniciTablosu = pers;

            var bolge = db.BölgelerTablosu.Where(m => m.Bölge_id == k.TeslimAlanBolgeId).FirstOrDefault();
            k.BölgelerTablosu = bolge;

            var departman = db.DepartmanlarTablosu.Where(m => m.Departman_id == k.TeslimAlanDepartmanId).FirstOrDefault();
            k.DepartmanlarTablosu = departman;

            ///
            ///Gönderim Turu
            ///

            var gonderim = db.GonderimTuru.Where(m => m.Id == k.GonderimTuruId).FirstOrDefault();
            k.GonderimTuru = gonderim;


            db.KargoHareketleri.Add(k);
            db.SaveChanges();
            TempData["MessageSuccess"] = "TRANSFER OLUSTURULDU";
            return RedirectToAction("Index", "Kargolar");


        }

        [Authorize]
        public ActionResult Getir(int id)
        {
            var kargo = db.KargoHareketleri.Find(id);

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
        public ActionResult Guncelle(KargoHareketleri k)
        {
            //if (!ModelState.IsValid)
            //{
            List<SelectListItem> kargoNo = new List<SelectListItem>();
            foreach (var item in db.Kargo.ToList())
            {
                kargoNo.Add(
                    new SelectListItem
                    {
                        Text = item.Id.ToString(),
                        Value = item.Id.ToString()

                    });
            }
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

            ///
            ///GetirGüncelle
            ///

            var kargo = db.KargoHareketleri.Find(k.Id);
            if (kargo != null)
            {
                //  kargo.KargoNo = k.KargoNo;
                kargo.TeslimAlanBolgeId = k.TeslimAlanBolgeId;
                kargo.TeslimAlanDepartmanId = k.TeslimAlanDepartmanId;
                kargo.TeslimAlanPersonelId = k.TeslimAlanPersonelId;
                kargo.TeslimAlanPersonelAdSoyad = k.TeslimAlanPersonelAdSoyad;
                kargo.Tarih = DateTime.Now;
                kargo.AracPlakaNo = k.AracPlakaNo;
                kargo.GonderimTuruId = k.GonderimTuruId;
                kargo.KargoTakipNo = k.KargoTakipNo;

                db.SaveChanges();

                TempData["MessageSuccess"] = "TRANSFER GUNCELLENDI";
                return RedirectToAction("Index", "Kargolar");
            }
            else
            {
                TempData["Message"] = "TRANSFER GUNCELLENEMEDI..!";
            }

            return View("Getir");
        }

    }
}