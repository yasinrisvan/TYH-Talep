using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using BTProje.Models.EntityFramework;

namespace BTProje.Controllers
{
    public class AracController : Controller
    {
        TYHTALEPEntities db = new TYHTALEPEntities();
        // GET: Arac
        public ActionResult TalepIndex(int? id)
        {
            // var arc = (from a in db.AraçTablosu select a);    bu şekilde listeleme yaptığında arc=arc.where gibi yazmana gerek kalmıyo arc.where yazmak yeterli oluyor
            var arc = db.AraçTablosu.ToList();
            if (id == null || id == 2)
            {
                arc.ToList();
            }
            else
            {
                arc = arc.Where(m => m.Durumu == id).ToList();
            }
            return View(arc);
        }
        [HttpGet]
        public ActionResult Rezervasyon(int id)
        {
            var arac = (from i in db.AraçTablosu where i.Aracid == id select i.Plaka).FirstOrDefault();
            // ViewBag.arac = arac;
            Sirket_Arac bul = new Sirket_Arac();
            bul.aracid = id;
            bul.ikincikullanici = arac.ToString();
            List<SelectListItem> kll = (from i in db.KullaniciTablosu.ToList()
                                        select new SelectListItem
                                        {
                                            Text = i.Ad + " " + i.Soyad,
                                            Value = i.Kullanici_id.ToString()
                                        }).ToList();
            ViewBag.kll = kll.Where(m => m.Text.Contains("-") == false).OrderBy(x => x.Text).ToList();
            return View("Rezervasyon", bul);
        }
        [HttpPost]
        public ActionResult Rezervasyon(Sirket_Arac rez, int[] Yolcular)
        {
            int yolcusayac = 0;
            if (Yolcular == null)
            {
                yolcusayac = 0;
            }
            else
            {
                yolcusayac = Yolcular.Count();
            }
            var dgr = db.AraçTablosu.Find(rez.aracid);
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];
            {
                //List<string> ys = new List<string>();
                //ys.Add(rez.yolcu1.ToString());
                //ys.Add(rez.yolcu2.ToString());
                //ys.Add(rez.yolcu3.ToString());
                //ys.Add(rez.yolcu4.ToString());
                //int syc = 0;
                //foreach (var item in ys)
                //{
                //    if (item != 0.ToString())
                //    {
                //        syc++;
                //    }
                //}
            }
            if (rez.sirketaracid == 0)
            {
                rez.yolcusayisi = yolcusayac;
                rez.durum = 1;
                rez.kullaniciid = kid.Kullanici_id;
                rez.bolgeid = bid.Bölge_id;
                if (rez.gorevgidistarih == null)
                {
                    rez.gorevgidistarih = Convert.ToDateTime(DateTime.Now);
                }
                db.Sirket_Arac.Add(rez);
                dgr.Durumu = 1;
            }
            else
            {
                //var sil = (from i in db.Yolcu where i.gorevid == rez.sirketaracid select i.yolcuid).ToList();
                //if (sil != null)
                //{
                //    foreach (var item in sil)
                //    {
                //        var bll = db.Yolcu.Find(item);
                //        db.Yolcu.Remove(bll);
                //    }
                //}
                var deger = db.Sirket_Arac.Find(rez.sirketaracid);
                deger.gorevgidistarih = rez.gorevgidistarih;
                deger.gorevdonustarih = rez.gorevdonustarih;
                deger.yolcusayisi = yolcusayac;
                deger.soforadsoyad = rez.soforadsoyad;
                deger.gorev = rez.gorev;
                deger.ikincikullanici = kid.Ad + " " + kid.Soyad;
            }
            db.SaveChanges();   //----------------------------------------------------
            Yolcu ylc = new Yolcu();
            var ara = (from x in db.Yolcu where x.gorevid == rez.sirketaracid select x).ToList();
            int artan = 0;
            if (Yolcular != null)
            {
                foreach (var y in Yolcular)
                {
                    if (ara != null)
                    {
                        foreach (var a in ara)
                        {
                            if (y == a.ykullaniciid)
                            {
                                artan++;
                            }
                        }
                    }
                    if (artan == 0)
                    {
                        // kullanıcıyı ekle.(item i ykullaniciid ye ekle)
                        ylc.gorevid = rez.sirketaracid;
                        ylc.ykullaniciid = y;
                        db.Yolcu.Add(ylc);
                        db.SaveChanges();
                    }
                    artan = 0;
                }
            }
            //------------------------------------------------------------------
            artan = 0;
            if (ara != null)
            {
                foreach (var a in ara)
                {
                    if (Yolcular != null)
                    {
                        foreach (var y in Yolcular)
                        {
                            if (a.ykullaniciid == y)
                            {
                                artan++;
                            }
                        }
                    }
                    if (artan == 0)
                    {
                        //kullanıcıyı sil.
                        var bll = db.Yolcu.Find(a.yolcuid);
                        db.Yolcu.Remove(bll);
                        db.SaveChanges();
                    }
                    artan = 0;
                }
            }
            return RedirectToAction("OnayIndex", "Arac");
        }
        public ActionResult Update(int id)
        {
            var model = db.Sirket_Arac.Find(id);
            model.ikincikullanici = model.AraçTablosu.Plaka;
            if (model == null)
            { return HttpNotFound(); }
            List<SelectListItem> kll = (from i in db.KullaniciTablosu.ToList()
                                        select new SelectListItem
                                        {
                                            Text = i.Ad + " " + i.Soyad,
                                            Value = i.Kullanici_id.ToString()
                                        }).ToList();
            var bulyolcu = db.Yolcu.Where(m => m.gorevid == id).ToList();
            if (bulyolcu != null)
            {
                foreach (var item2 in bulyolcu)
                {
                    foreach (var item in kll)
                    {
                        if (item.Value == item2.ykullaniciid.ToString())
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
            ViewBag.kll = kll.Where(m => m.Text.Contains("-") == false).OrderBy(x => x.Text).ToList();
            return View("Rezervasyon", model);
        }
        //public JsonResult YolcuArama(string p)      //cascading dropdownlist
        //{
        //    List<SelectListItem> kll = (from i in db.KullaniciTablosu.ToList()
        //                                select new SelectListItem
        //                                {
        //                                    Text = i.Ad + " " + i.Soyad,
        //                                    Value = i.Kullanici_id.ToString()
        //                                }).ToList();
        //    if (p == null || p == "")
        //    {
        //        kll = kll.Where(m => m.Text.Contains("-") == false).OrderBy(x => x.Text).ToList();
        //        return Json(kll.ToList(), JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        p = p.ToUpper();
        //        kll = kll.Where(m => m.Text.Contains(p) && m.Text.Contains("-") == false).OrderBy(x => x.Text).ToList();
        //        return Json(kll, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public JsonResult YolcuEkle(int p)
        //{
        //    var arac = (from i in db.KullaniciTablosu
        //                where i.Kullanici_id == p
        //                select new SelectListItem
        //                {
        //                    Text = i.Ad + " " + i.Soyad,
        //                    Value = i.Kullanici_id.ToString(),
        //                    Selected = true
        //                }).ToList();
        //    return Json(arac, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult OnayTest(int id)
        {
            var sorgu = db.Sirket_Arac.Where(m => m.aracid == id && m.durum != 4).FirstOrDefault();
            var bul = db.Yolcu.Where(m => m.gorevid == sorgu.sirketaracid).ToList();
            //List<int?> list = new List<int?>();
            //list.Add(sorgu.yolcu1);
            //list.Add(sorgu.yolcu2);
            //list.Add(sorgu.yolcu3);
            //list.Add(sorgu.yolcu4);
            string tablo = "";
            string deger = "";
            if (bul != null)
            {
                foreach (var item in bul)
                {
                    //if (item != 0 && item != null)
                    //{
                    var user = db.KullaniciTablosu.Where(m => m.Kullanici_id == item.ykullaniciid).FirstOrDefault();
                    tablo = "<ul><li>" + user.Ad + " " + user.Soyad + "</li></ul>";
                    //}
                    //else
                    //{
                    //    tablo = "";
                    //}
                    deger = deger + tablo;
                }
            }
            string durum = "";
            if (sorgu.durum == 1)
            {
                durum = "Personelde Onay Bekliyor";
            }
            else if (sorgu.durum == 2)
            {
                durum = "Fabrika Müdüründe Onay Bekliyor";
            }
            else if (sorgu.durum == 3)
            {
                durum = "Talep Edildi";
            }
            ViewData["diger"] = "<table class=" + '"' + "table table-bordered" + '"' + "><thead><tr><td><b>GÖREV TARİH</b></td><td><b>ARAÇ PLAKASI</b></td><td><b>ŞOFÖR ADI</b></td>" +
                "<td><b>GİDECEĞİ YER</b></td><td><b>YOLCULAR</b></td><td><b>TALEP EDEN</b></td><td><b>DURUM</b></td></tr></thead>" +
                "<tbody><tr> <td>" + sorgu.gorevgidistarih + "</td> <td>" + sorgu.AraçTablosu.Plaka + "</td> <td>" + sorgu.soforadsoyad + "</td> <td>" + sorgu.gorev + "</td>" +
                "<td>" + deger + "</td> <td>" + sorgu.KullaniciTablosu.Ad + " " + sorgu.KullaniciTablosu.Soyad + "</td> <td>" + durum + "</td> </tr></tbody></table>";
            return Json(ViewData["diger"], JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Yolcu(int id)
        {
            ViewData["gorevid"] = id + " Nolu Görev Yolcuları";
            var bul = db.Yolcu.Where(m => m.gorevid == id).ToList();
            string tablo = "";
            string deger = "";
            if (bul != null)
            {
                foreach (var item in bul)
                {
                    var user = db.KullaniciTablosu.Find(item.ykullaniciid);
                    tablo = "<tr><td>" + user.Ad + " " + user.Soyad + "</td>" +
                        "<td>" + user.DepartmanlarTablosu.Ad + "</td>" +
                        "<td>" + user.MaliyetMNo + "</td></tr>";
                    deger = deger + tablo;
                }
            }
            ViewData["diger"] = "<table class=" + '"' + "table table-bordered" + '"' + "><thead><tr><td><b>ADI SOYADI</b></td>" +
                       "<td><b>DEPARTMANI</b></td>" +
                       "<td><b>MALİYET MERKEZİ NO</b></td></tr></thead><tbody>" + deger + "</tbody></table>";
            return Json(ViewData.Values, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult OnayIndex()
        {
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];

            //var arc = (from a in db.Sirket_Arac select a);
            var arc = db.Sirket_Arac.Where(m => m.bolgeid == bid.Bölge_id && m.durum != 4).ToList();   //burada modele tüm kayıtları değilde uygun şartları sağlayan kayıtları göndemreye çalış.
            List<Sirket_Arac> onayliste = new List<Sirket_Arac>();
            if (kid.DepatmanID == 1)    //onay sıralamasında değişecek durumları buradaki if lerde değiştir.
            {
                foreach (var item in arc)
                {
                    if (item.durum == 1)
                    {
                        onayliste.Add(item);
                    }
                }
            }
            if (kid.Kullanici_id == 10052)
            {
                foreach (var item in arc)
                {
                    if (item.durum == 2)
                    {
                        onayliste.Add(item);
                    }
                }
            }
            if ((kid.RolTipi == 3 || kid.RolTipi == 5 || kid.RolTipi == 6 || kid.RolTipi == 7))
            {
                foreach (var item in arc)
                {
                    if (item.durum == 3)
                    {
                        onayliste.Add(item);
                    }
                }
            }
            return View(onayliste.ToList());
        }
        public ActionResult Onay(int id)
        {

            var srk = db.Sirket_Arac.Find(id);
            var arc = db.AraçTablosu.Find(srk.aracid);
            if (srk.durum == 1)
            {
                srk.durum = 2;
            }
            else if (srk.durum == 2)
            {
                srk.durum = 3;
                arc.Durumu = 3;      //araç rez yapıldı    
            }
            else if (srk.durum == 3)
            {
                srk.durum = 4;
                arc.Durumu = 4;      //araç görevde
                srk.gidiskm = arc.AracDönüsKM;
            }
            db.SaveChanges();
            return RedirectToAction("OnayIndex");
        }
        public ActionResult Reddet(int id)
        {
            var srk = db.Sirket_Arac.Find(id);
            var arc = db.AraçTablosu.Find(srk.aracid);
            arc.Durumu = 2;
            db.Sirket_Arac.Remove(srk);
            db.SaveChanges();
            return RedirectToAction("OnayIndex");
        }
    }
}