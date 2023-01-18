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
//using Newtonsoft.Json.Linq;
//using static System.Net.Mime.MediaTypeNames;

namespace BTProje.Controllers
{
    public class SirketController : Controller
    {
        TYHTALEPEntities db = new TYHTALEPEntities();
        DateTime sinirlandirma = DateTime.Now.AddDays(-15);
        // GET: Sirket
        public ActionResult SirketIndex()
        {
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];
            List<RolYetkiAtama> yetki = (List<RolYetkiAtama>)Session["yetki"];

            List<SelectListItem> deger = (from i in db.AraçTablosu.ToList()
                                          select new SelectListItem
                                          {
                                              Text = i.Plaka + " (" + i.Model + ")",
                                              Value = i.Aracid.ToString()
                                          }).OrderBy(x => x.Text).ToList();
            deger.Add(new SelectListItem
            {
                Text = "Tümü",
                Value = "0",
            });
            ViewBag.plk = deger;
            ViewBag.bslk = "ŞİRKET ARAÇ KAYIT LİSTESİ";
            var srky = db.Sirket_Arac.Where(m => m.gorevgidistarih > sinirlandirma).OrderByDescending(m => m.sirketaracid).ToList();
            foreach (var item in yetki)
            {
                if (item.Rol_Id == 3 && item.Yetki_Id != 1)
                {
                    srky = srky.Where(m => m.bolgeid == bid.Bölge_id).ToList();
                    break;
                }
            }
            //if (kid.YetkiTipi == 1)
            //{
            //    srky = srky.ToList();
            //}
            //else
            //{
            //    srky = srky.Where(m => m.bolgeid == bid.Bölge_id).ToList();   //burada düzenleme yap
            //}
            return View(srky.ToList());
        }
        public ActionResult EskiKayitlar()
        {
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];
            List<RolYetkiAtama> yetki = (List<RolYetkiAtama>)Session["yetki"];

            List<SelectListItem> deger = (from i in db.AraçTablosu.ToList()
                                          select new SelectListItem
                                          {
                                              Text = i.Plaka + " (" + i.Model + ")",
                                              Value = i.Aracid.ToString()
                                          }).OrderBy(x => x.Text).ToList();
            deger.Add(new SelectListItem
            {
                Text = "Tümü",
                Value = "0",
            });
            ViewBag.plk = deger;
            ViewBag.bslk = "ŞİRKET ARAÇ KAYIT LİSTESİ";
            var srky = db.Sirket_Arac.Where(m => m.gorevgidistarih <= sinirlandirma).OrderByDescending(m => m.sirketaracid).ToList();
            foreach (var item in yetki)
            {
                if (item.Rol_Id == 3 && item.Yetki_Id != 1)
                {
                    srky = srky.Where(m => m.bolgeid == bid.Bölge_id).ToList();
                    break;
                }
            }
            //if (kid.YetkiTipi == 1)
            //{
            //    srky = srky.ToList();
            //}
            //else
            //{
            //    srky = srky.Where(m => m.bolgeid == bid.Bölge_id).ToList();
            //}
            return View("SirketIndex", srky.ToList());
        }
        public ActionResult Filtre()
        {
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];
            List<RolYetkiAtama> yetki = (List<RolYetkiAtama>)Session["yetki"];

            List<SelectListItem> deger = (from i in db.AraçTablosu.ToList()
                                          select new SelectListItem
                                          {
                                              Text = i.Plaka + " (" + i.Model + ")",
                                              Value = i.Aracid.ToString()
                                          }).OrderBy(x => x.Text).ToList();
            deger.Add(new SelectListItem
            {
                Text = "Tümü",
                Value = "0",
            });
            ViewBag.plk = deger;
            ViewBag.bslk = "GÖREVİ TAMAMLANMAYAN ARAÇLAR LİSTESİ";
            var srky = db.Sirket_Arac.Where(m => m.gorevdonustarih == null).OrderByDescending(m => m.sirketaracid).ToList();
            foreach (var item in yetki)
            {
                if (item.Rol_Id == 3 && item.Yetki_Id != 1)
                {
                    srky = srky.Where(m => m.bolgeid == bid.Bölge_id).ToList();
                    break;
                }
            }
            //if (kid.YetkiTipi == 1)
            //{
            //    srky = srky.ToList();
            //}
            //else
            //{
            //    srky = srky.Where(m => m.bolgeid == bid.Bölge_id).ToList();
            //}
            return View("SirketIndex", srky.ToList());
        }
        public ActionResult PlakaFiltre(int id)
        {
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];
            List<RolYetkiAtama> yetki = (List<RolYetkiAtama>)Session["yetki"];

            ViewBag.bslk = "ŞİRKET ARAÇ KAYIT LİSTESİ";
            List<SelectListItem> deger = (from i in db.AraçTablosu.ToList()
                                          select new SelectListItem
                                          {
                                              Text = i.Plaka + " (" + i.Model + ")",
                                              Value = i.Aracid.ToString()
                                          }).OrderBy(x => x.Text).ToList();
            deger.Add(new SelectListItem
            {
                Text = "Tümü",
                Value = "0",
            });
            ViewBag.plk = deger;
            if (id == 0)
            {
                return RedirectToAction("SirketIndex");
            }
            else
            {
                var srky = db.Sirket_Arac.Where(m => m.aracid == id).OrderByDescending(m => m.sirketaracid).ToList();
                foreach (var item in yetki)
                {
                    if (item.Rol_Id == 3 && item.Yetki_Id != 1)
                    {
                        srky = srky.Where(m => m.bolgeid == bid.Bölge_id).ToList();   //burada düzenleme yap
                        break;
                    }
                }
                //if (kid.YetkiTipi == 1)
                //{
                //    srky = srky.ToList();
                //}
                //else
                //{
                //    srky = srky.Where(m => m.bolgeid == bid.Bölge_id).ToList();
                //}
                return View("SirketIndex", srky.ToList());
            }

        }
        public JsonResult Arama(string p)      //cascading dropdownlist
        {
            List<SelectListItem> asd = (from i in db.AraçTablosu.ToList()
                                        select new SelectListItem
                                        {
                                            Text = i.Plaka + " (" + i.Model + ")",
                                            Value = i.Aracid.ToString()
                                        }).ToList();

            if (p == null || p == "")
            {
                asd = asd.OrderBy(x => x.Text).ToList();
                return Json(asd.ToList(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                p = p.ToUpper();
                asd = asd.Where(m => m.Text.Contains(p)).OrderByDescending(x => x.Text).ToList();
                return Json(asd, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult YolcuListele(int id)
        {
            ViewData["gorevid"] = id + " Nolu Görev Yolcuları";
            var bul = db.Yolcu.Where(m => m.gorevid == id).ToList();
            string tablo = "";
            string deger = "";
            int sayac = 1;
            if (bul != null)
            {
                foreach (var item in bul)
                {
                    //var user = db.KullaniciTablosu.Where(m => m.Kullanici_id == item.ykullaniciid).FirstOrDefault();
                    //var user = db.KullaniciTablosu.Find(item.ykullaniciid);
                    //tablo = "<tr><td>" + user.Ad + " " + user.Soyad + "</td>" +
                    //    "<td>" + user.DepartmanlarTablosu.Ad + "</td>" +
                    //    "<td>" + user.MaliyetMNo + "</td></tr>";
                    tablo = "<tr><td>" + sayac + "</td>" +
                        "<td>" + item.yolcuadsoyad + "</td>" +
                        "<td>" + item.yolcubirim + "</td></tr>";
                    deger = deger + tablo;
                    sayac++;
                }
            }
            //ViewData["diger"] = "<table class=" + '"' + "table table-bordered" + '"' + "><thead><tr><td><b>ADI SOYADI</b></td>" +
            //           "<td><b>ADI SOYADI</b></td>" +
            //           "<td><b>MALİYET MERKEZİ NO</b></td></tr></thead><tbody>" + deger + "</tbody></table>";
            ViewData["diger"] = "<table class=" + '"' + "table table-bordered" + '"' + "><thead><tr><td><b>NO</b></td>" +
                       "<td><b>ADI SOYADI</b></td>" +
                       "<td><b>DEPARTMANI</b></td></tr></thead><tbody>" + deger + "</tbody></table>";
            return Json(ViewData.Values, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SirketKayıt()
        {

            List<SelectListItem> asd = (from i in db.AraçTablosu.ToList()
                                        select new SelectListItem
                                        {
                                            Text = i.Plaka + " (" + i.Model + ")",
                                            Value = i.Aracid.ToString()
                                        }).OrderBy(x => x.Text).ToList();

            ViewBag.srk = asd.ToList();
            //List<SelectListItem> kll = (from i in db.KullaniciTablosu.ToList()
            //                            select new SelectListItem
            //                            {
            //                                Text = i.Ad + " " + i.Soyad,
            //                                Value = i.Kullanici_id.ToString()
            //                            }).ToList();
            //ViewBag.kll = kll.Where(m => m.Text.Contains("-") == false).OrderBy(x => x.Text).ToList();
            return View("SirketKayıt", new Sirket_Arac());
        }
        [HttpPost]
        public ActionResult SirketKayıt(Sirket_Arac srk, int[] Yolcular)
        {
            string[] adsoy = { };
            string[] dept = { };
            if (srk.yolcuad != null)
            {
                adsoy = srk.yolcuad.Split('\n');
            }
            if (srk.yolcubirim != null)
            {
                dept = srk.yolcubirim.Split('\n');
            }
            int yolcusayac = adsoy.Count()<=dept.Count()? adsoy.Count() : dept.Count();
            //int yolcusayac = 0;
            //if (Yolcular == null)
            //{
            //    yolcusayac = 0;
            //}
            //else
            //{
            //    yolcusayac = Yolcular.Count();
            //}
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];
            List<RolYetkiAtama> yetki = (List<RolYetkiAtama>)Session["yetki"];

            var dgr = db.AraçTablosu.Find(srk.aracid);
            List<SelectListItem> asdf = (from i in db.AraçTablosu.ToList()
                                         select new SelectListItem
                                         {
                                             Text = i.Plaka + " (" + i.Model + ")",
                                             Value = i.Aracid.ToString(),
                                         }).OrderBy(x => x.Text).ToList();
            ViewBag.srk = asdf;
            //List<SelectListItem> kll2 = (from i in db.KullaniciTablosu.ToList()
            //                             select new SelectListItem
            //                             {
            //                                 Text = i.Ad + " " + i.Soyad,
            //                                 Value = i.Kullanici_id.ToString()
            //                             }).ToList();
            //ViewBag.kll = kll2.Where(m => m.Text.Contains("-") == false).OrderBy(x => x.Text).ToList();
            if (srk.sirketaracid == 0)
            {
                if (srk.gidiskm < dgr.AracDönüsKM)
                {
                    TempData["msg"] = "Gidis Km Arac Km sinden Dusuk Olamaz";
                    srk.ikincikullanici = dgr.KullaniciTablosu.Ad + " " + dgr.KullaniciTablosu.Soyad;
                    return View("SirketKayıt", srk);
                }
                else
                {
                    //srk.yolcusayisi = syc;
                    if (srk.gorevgidistarih == null)
                    {
                        srk.gorevgidistarih = Convert.ToDateTime(DateTime.Now);
                    }
                    srk.kullaniciid = kid.Kullanici_id;
                    srk.bolgeid = bid.Bölge_id;
                    srk.durum = 4;
                    srk.yolcusayisi = yolcusayac;
                    //dgr.Durumu = (int)TempData["durum"];
                    if (dgr.Durumu != 1 && dgr.Durumu != 3)
                    {
                        dgr.Durumu = 4;
                    }
                    if (srk.gidiskm != null)
                    {
                        dgr.AracCikisKM = srk.gidiskm;
                    }
                    if (srk.donuskm >= srk.gidiskm)
                    {
                        dgr.AracDönüsKM = srk.donuskm;
                    }
                    if (srk.donuskm < srk.gidiskm)
                    {
                        TempData["msg"] = "Donus Km Gidis Km den Dusuk Olamaz";
                        srk.ikincikullanici = dgr.KullaniciTablosu.Ad + " " + dgr.KullaniciTablosu.Soyad;
                        return View("SirketKayıt", srk);
                    }

                    if ((srk.donuskm != null || srk.donussoforad != null) && (srk.gorevdonustarih == null))
                    {
                        srk.gorevdonustarih = Convert.ToDateTime(DateTime.Now);
                        srk.ikincikullanici = kid.Ad + " " + kid.Soyad;
                    }
                    if (srk.soforadsoyad.Contains("-"))
                    {
                        TempData["msg"] = "Sofor Adini Kontrol Ediniz";
                        srk.ikincikullanici = dgr.KullaniciTablosu.Ad + " " + dgr.KullaniciTablosu.Soyad;
                        return View("SirketKayıt", srk);
                    }
                    db.Sirket_Arac.Add(srk);  //veritabanına ekleme satırı-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
                }
            }
            else
            {
                var ara = (from x in db.Yolcu where x.gorevid == srk.sirketaracid select x).ToList();
                db.Yolcu.RemoveRange(ara);                     //daha önce kaydedilen yolcuları bulup silen satır

                var deger = db.Sirket_Arac.Find(srk.sirketaracid);
                deger.yolcusayisi = yolcusayac;
                deger.yolcuad = srk.yolcuad;
                deger.yolcubirim = srk.yolcubirim;  //bu iki satır textareadaki değerleri tek alanda tuttuğum değeri update için
                deger.donussoforad = srk.donussoforad;
                deger.ikincikullanici = kid.Ad + " " + kid.Soyad;
                if (deger == null)
                {
                    return HttpNotFound();
                }
                if (srk.donuskm != null)
                {
                    dgr.AracDönüsKM = srk.donuskm;
                }
                if (srk.gidiskm != null)
                {
                    dgr.AracCikisKM = srk.gidiskm;
                }
                foreach (var item in yetki)
                {
                    if (item.Rol_Id == 3 && (item.Yetki_Id == 1 || item.Yetki_Id == 3))
                    {
                        deger.soforadsoyad = srk.soforadsoyad;
                        deger.gorev = srk.gorev;
                        deger.gidiskm = srk.gidiskm;
                        deger.donuskm = srk.donuskm;
                        deger.gorevgidistarih = srk.gorevgidistarih;
                        deger.gorevdonustarih = srk.gorevdonustarih;
                        if (srk.donuskm < srk.gidiskm)      //burda srk.gidiskm yerine dgr.AracCikisKm yazdıgım için hata alıyormuşum
                        {
                            TempData["msg"] = "Donus Km Gidis Km den Dusuk Olamaz";
                            srk.ikincikullanici = deger.AraçTablosu.KullaniciTablosu.Ad + " " + deger.AraçTablosu.KullaniciTablosu.Soyad;
                            return View("SirketKayıt", srk);
                        }
                        break;
                    }
                }
                if (srk.donuskm < deger.gidiskm)  //burası güncellemede admin şartının else kod bloğuydu. oradan çıkarıp yazdım.
                {
                    TempData["msg"] = "Donus Km yi Kontrol Ediniz";
                    srk.ikincikullanici = deger.AraçTablosu.KullaniciTablosu.Ad + " " + deger.AraçTablosu.KullaniciTablosu.Soyad;
                    return View("SirketKayıt", srk);
                }
                else
                {
                    deger.donuskm = srk.donuskm;
                }
                //if (kid.YetkiTipi == 1 || kid.YetkiTipi == 3)
                //{
                //    deger.soforadsoyad = srk.soforadsoyad;
                //    deger.gorev = srk.gorev;
                //    deger.gidiskm = srk.gidiskm;
                //    deger.donuskm = srk.donuskm;
                //    //deger.yolcu1 = srk.yolcu1;
                //    //deger.yolcu2 = srk.yolcu2;
                //    //deger.yolcu3 = srk.yolcu3;
                //    //deger.yolcu4 = srk.yolcu4;
                //    //------------------------------
                //    //deger.yolcusayisi = yolcusayac;
                //    //deger.yolcuad = srk.yolcuad;
                //    //deger.yolcubirim = srk.yolcubirim;  //bu iki satır textareadaki değerleri tek alanda tuttuğum değeri update için
                //    //deger.donussoforad = srk.donussoforad;
                //    deger.gorevgidistarih = srk.gorevgidistarih;
                //    deger.gorevdonustarih = srk.gorevdonustarih;
                //    if (srk.donuskm < srk.gidiskm)      //burda srk.gidiskm yerine dgr.AracCikisKm yazdıgım için hata alıyormuşum
                //    {
                //        TempData["msg"] = "Donus Km Gidis Km den Dusuk Olamaz";
                //        //if (Yolcular != null)
                //        //{
                //        //    foreach (var item2 in Yolcular)
                //        //    {
                //        //        foreach (var item in kll2)
                //        //        {
                //        //            if (item.Value == item2.ToString())
                //        //            {
                //        //                item.Selected = true;
                //        //            }
                //        //        }
                //        //    }   //burada sadece ilk değer i seçili yapıyo ve yandaki sonuç hep false oluyo(bunun sebebi dropdownlist yazdığım içinmiş listbox yazınca sorun düzeldi)
                //        //}
                //        //ViewBag.kll = kll2.Where(m => m.Text.Contains("-") == false).OrderBy(x => x.Text).ToList();
                //        srk.ikincikullanici = deger.AraçTablosu.KullaniciTablosu.Ad + " " + deger.AraçTablosu.KullaniciTablosu.Soyad;
                //        return View("SirketKayıt", srk);
                //    }                   
                //}
                //else
                //{
                //    //deger.donussoforad = srk.donussoforad;
                //    //deger.yolcuad = srk.yolcuad;
                //    //deger.yolcubirim = srk.yolcubirim;
                //    if (srk.donuskm < deger.gidiskm)
                //    {
                //        TempData["msg"] = "Donus Km yi Kontrol Ediniz";
                //        //if (Yolcular != null)
                //        //{
                //        //    foreach (var item2 in Yolcular)
                //        //    {
                //        //        foreach (var item in kll2)
                //        //        {
                //        //            if (item.Value == item2.ToString())
                //        //            {
                //        //                item.Selected = true;
                //        //            }
                //        //        }
                //        //    }
                //        //}
                //        //ViewBag.kll = kll2.Where(m => m.Text.Contains("-") == false).OrderBy(x => x.Text).ToList();
                //        srk.ikincikullanici = deger.AraçTablosu.KullaniciTablosu.Ad + " " + deger.AraçTablosu.KullaniciTablosu.Soyad;
                //        return View("SirketKayıt", srk);
                //    }
                //    else
                //    {
                //        deger.donuskm = srk.donuskm;
                //    }
                //}
                if ((deger.donuskm != null || deger.donussoforad != null) && (srk.gorevdonustarih == null))
                {
                    deger.gorevdonustarih = Convert.ToDateTime(DateTime.Now);
                    if (dgr.Durumu != 1 && dgr.Durumu != 3)
                    {
                        dgr.Durumu = 2;
                    }                                  //araç durumunu buraya ekledim
                    //deger.ikincikullanici = kid.Ad + " " + kid.Soyad;         //buradaki şartı genel update şartına ekledim her güncelleme için tıkalyan kişinin adını kaydedecek
                }
            }
            db.SaveChanges();    //yolcu kontrollerini save den sonra yap 2 işlemide bunun altında yaz
            Yolcu ylc = new Yolcu();
            if (adsoy.Length != 0 || dept.Length!=0)
            {
                int a = adsoy.Length; 
                int d=dept.Length;
                int kucuk= a <= d ? a : d;
                for (int i = 0; i < kucuk; i++)
                {
                    //ylc.ykullaniciid = 1778;
                    ylc.yolcuadsoyad = adsoy[i];
                    ylc.yolcubirim = dept[i];
                    ylc.gorevid = srk.sirketaracid;
                    db.Yolcu.Add(ylc);
                    db.SaveChanges();
                }
            }
            //var ara = (from x in db.Yolcu where x.gorevid == srk.sirketaracid select x).ToList();
            //int artan = 0;
            //if (Yolcular != null)
            //{
            //    foreach (var y in Yolcular)
            //    {
            //        if (ara != null)
            //        {
            //            foreach (var a in ara)
            //            {
            //                if (y == a.ykullaniciid)
            //                {
            //                    artan++;
            //                }
            //            }
            //        }
            //        if (artan == 0)
            //        {
            //            // kullanıcıyı ekle.(item i ykullaniciid ye ekle)
            //            ylc.gorevid = srk.sirketaracid;
            //            ylc.ykullaniciid = y;
            //            db.Yolcu.Add(ylc);
            //            db.SaveChanges();
            //        }
            //        artan = 0;
            //    }
            //}  //------------------------------------------------------------------
            //artan = 0;
            //if (ara != null)
            //{
            //    foreach (var a in ara)
            //    {
            //        if (Yolcular != null)
            //        {
            //            foreach (var y in Yolcular)
            //            {
            //                if (a.ykullaniciid == y)
            //                {
            //                    artan++;
            //                }
            //            }
            //        }
            //        if (artan == 0)
            //        {
            //            //kullanıcıyı sil.
            //            var bll = db.Yolcu.Find(a.yolcuid);
            //            db.Yolcu.Remove(bll);
            //            db.SaveChanges();
            //        }
            //        artan = 0;
            //    }
            //}
            return RedirectToAction("SirketIndex", "Sirket");
        }
        public ActionResult Update(int id)
        {
            var model = db.Sirket_Arac.Find(id);
            model.ikincikullanici = model.AraçTablosu.KullaniciTablosu.Ad + " " + model.AraçTablosu.KullaniciTablosu.Soyad; //yetkili adı için kullanıyorum zaten disbled olduğu için null olucak o yüzden karışmıyor
            if (model == null)
            { return HttpNotFound(); }
            List<SelectListItem> deger = (from i in db.AraçTablosu
                                          select new SelectListItem
                                          {
                                              //Selected = true,
                                              Text = i.Plaka + " (" + i.Model + ")",
                                              Value = i.Aracid.ToString()
                                          }).OrderBy(x => x.Text).ToList();
            ViewBag.srk = deger;
            //List<SelectListItem> dene = (from i in db.KullaniciTablosu.ToList()
            //                             select new SelectListItem
            //                             {
            //                                 Text = i.Ad + " " + i.Soyad,
            //                                 Value = i.Kullanici_id.ToString()
            //                             }).ToList();
            //var bulyolcu = db.Yolcu.Where(m => m.gorevid == id).ToList();
            //if (bulyolcu != null)
            //{
            //    foreach (var item2 in bulyolcu)
            //    {
            //        foreach (var item in dene)
            //        {
            //            if (item.Value == item2.ykullaniciid.ToString())
            //            {
            //                item.Selected = true;
            //            }
            //        }
            //    }
            //}
            //ViewBag.kll = dene.Where(m => m.Text.Contains("-") == false).OrderBy(x => x.Text).ToList();
            return View("SirketKayıt", model);
        }
        public JsonResult YetkiliAd(int p)      //cascading dropdownlist
        {
            var arac = (from i in db.AraçTablosu
                        where i.Aracid == p
                        select new SelectListItem
                        {
                            Text = i.KullaniciTablosu.Ad + " " + i.KullaniciTablosu.Soyad,
                            Value = i.Aracid.ToString()
                        }).OrderBy(x => x.Text).ToList();
            return Json(arac, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GidisKm(int p)
        {
            var arac2 = (from i in db.AraçTablosu
                         where i.Aracid == p
                         select new SelectListItem
                         {
                             Text = i.AracDönüsKM.ToString(),
                             Value = i.Aracid.ToString()
                         }).OrderBy(x => x.Text).ToList();
            return Json(arac2, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Sil(int id)
        {
            var yllc = db.Yolcu.Where(x => x.gorevid == id).ToList();
            db.Yolcu.RemoveRange(yllc);
            var srk = db.Sirket_Arac.Find(id);
            var dgr = db.AraçTablosu.Find(srk.aracid);
            if (dgr.Durumu != 1 && dgr.Durumu != 3)
            {
                dgr.Durumu = 2;
            }
            db.Sirket_Arac.Remove(srk);
            db.SaveChanges();
            return RedirectToAction("SirketIndex");
        }
        public void GridExportToExcel(DateTime? ilktrh, DateTime? sontrh)
        {
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];
            List<RolYetkiAtama> yetki = (List<RolYetkiAtama>)Session["yetki"];
            string tarih = DateTime.Now.ToString("dd-MM-yyyy");
            string dosyaAdi = "Şirket Araç Kayıtları";
            var table = (from i in db.Sirket_Arac
                         where i.durum == 4
                         select new
                         {
                             GÖREV_GİDİŞ_TARİHİ = i.gorevgidistarih,   //alias tanımlama
                             ARAÇ_PLAKASI = i.AraçTablosu.Plaka,
                             YETKİLİ_AD_SOYAD = i.AraçTablosu.KullaniciTablosu.Ad + "" + i.AraçTablosu.KullaniciTablosu.Soyad,
                             ŞOFÖR_AD_SOYAD = i.soforadsoyad,
                             GİDECEĞİ_YER = i.gorev,
                             ARAÇ_GİDİŞ_KM = i.gidiskm,
                             ARAÇ_DÖNÜŞ_KM = i.donuskm,
                             YOLCULAR = i.yolcuad,
                             DÖNÜŞ_ŞOFÖR_AD_SOYAD = i.donussoforad,
                             GÖREV_DÖNÜŞ_TARİH = i.gorevdonustarih,
                             BÖLGE = i.BölgelerTablosu.Bölge,
                             KULLANICI = i.KullaniciTablosu.Ad + " " + i.KullaniciTablosu.Soyad
                         }).ToList();
            foreach (var item in yetki)
            {
                if (item.Rol_Id == 3 && item.Yetki_Id != 1)
                {
                    if (ilktrh != null && sontrh != null)
                    {
                        table = table.Where(m => m.GÖREV_GİDİŞ_TARİHİ >= ilktrh && m.BÖLGE == bid.Bölge && m.GÖREV_GİDİŞ_TARİHİ <= sontrh).ToList();
                    }
                    else
                    {
                        table = table.Where(m => m.BÖLGE == bid.Bölge).ToList();
                    }
                    break;
                }
                else
                {
                    if (ilktrh != null && sontrh != null)
                    {
                        table = table.Where(m => m.GÖREV_GİDİŞ_TARİHİ >= ilktrh && m.GÖREV_GİDİŞ_TARİHİ <= sontrh).ToList();
                    }
                }
            }
            //if (kid.YetkiTipi == 1)
            //{
            //    if (ilktrh != null && sontrh != null)
            //    {
            //        table = table.Where(m => m.GÖREV_GİDİŞ_TARİHİ >= ilktrh && m.GÖREV_GİDİŞ_TARİHİ <= sontrh).ToList();
            //    }
            //    else
            //    {
            //        table.ToList();
            //    }
            //}
            //else
            //{
            //    if (ilktrh != null && sontrh != null)
            //    {
            //        table = table.Where(m => m.GÖREV_GİDİŞ_TARİHİ >= ilktrh && m.BÖLGE == bid.Bölge && m.GÖREV_GİDİŞ_TARİHİ <= sontrh).ToList();
            //    }
            //    else if (ilktrh == null || sontrh == null)
            //    {
            //        table = table.Where(m => m.BÖLGE == bid.Bölge).ToList();
            //    }
            //}
            var grid = new GridView();
            grid.DataSource = table;
            grid.DataBind();
            Response.ClearContent();
            if (ilktrh != null && sontrh != null)
            {
                Response.AddHeader("content-disposition", "attachment; filename=" + dosyaAdi + "(" + ilktrh + "-" + sontrh + ").xls");
            }
            else
            {
                Response.AddHeader("content-disposition", "attachment; filename=" + dosyaAdi + "(" + tarih + ").xls");
            }
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1254");
            Response.ContentEncoding = System.Text.Encoding.Unicode;   //türkçe kaarakter sorunu çözümü
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());  //türkçe karakter sorunu çözümü
            Response.Charset = "windows-1254";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = "application/excel";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Write(sw.ToString());
            Response.End();
        }
        //public ActionResult Test()                               //rol yetki atama tablosunda kayıları ekleme methodu
        //{
        //    RolYetkiAtama atama = new RolYetkiAtama();
        //    var bul = db.KullaniciTablosu.ToList();
        //    foreach (var item in bul)
        //    {
        //        var find = db.RolYetkiAtama.Where(m => m.Kullanici_Id == item.Kullanici_id).FirstOrDefault();
        //        if (find == null)
        //        {
        //            if (item.RolTipi == 4)
        //            {
        //                atama.Kullanici_Id = item.Kullanici_id;
        //                atama.Yetki_Id = item.YetkiTipi;
        //                atama.Rol_Id = 1;
        //                db.RolYetkiAtama.Add(atama);
        //                db.SaveChanges();
        //                atama.Rol_Id = 2;
        //                db.RolYetkiAtama.Add(atama);
        //                db.SaveChanges();
        //            }
        //            else if (item.RolTipi == 5)
        //            {
        //                atama.Kullanici_Id = item.Kullanici_id;
        //                atama.Yetki_Id = item.YetkiTipi;
        //                atama.Rol_Id = 1;
        //                db.RolYetkiAtama.Add(atama);
        //                db.SaveChanges();
        //                atama.Rol_Id = 3;
        //                db.RolYetkiAtama.Add(atama);
        //                db.SaveChanges();
        //            }
        //            else if (item.RolTipi == 6)
        //            {
        //                atama.Kullanici_Id = item.Kullanici_id;
        //                atama.Yetki_Id = item.YetkiTipi;
        //                atama.Rol_Id = 2;
        //                db.RolYetkiAtama.Add(atama);
        //                db.SaveChanges();
        //                atama.Rol_Id = 3;
        //                db.RolYetkiAtama.Add(atama);
        //                db.SaveChanges();
        //            }
        //            else if (item.RolTipi == 7)
        //            {
        //                atama.Kullanici_Id = item.Kullanici_id;
        //                atama.Yetki_Id = item.YetkiTipi;
        //                atama.Rol_Id = 1;
        //                db.RolYetkiAtama.Add(atama);
        //                db.SaveChanges();
        //                atama.Rol_Id = 2;
        //                db.RolYetkiAtama.Add(atama);
        //                db.SaveChanges();
        //                atama.Rol_Id = 3;
        //                db.RolYetkiAtama.Add(atama);
        //                db.SaveChanges();
        //            }
        //            else
        //            {
        //                atama.Kullanici_Id = item.Kullanici_id;
        //                atama.Yetki_Id = item.YetkiTipi;
        //                atama.Rol_Id = item.RolTipi;
        //                db.RolYetkiAtama.Add(atama);
        //                db.SaveChanges();
        //            }
        //        }
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("SirketIndex", "Sirket");
        //}
    }
}