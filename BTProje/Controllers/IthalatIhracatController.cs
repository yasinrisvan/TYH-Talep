using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using BTProje.Models.EntityFramework;

namespace BTProje.Controllers
{
    public class IthalatIhracatController : Controller
    {
        TYHTALEPEntities db = new TYHTALEPEntities();
        // GET: IthalatIhracat
        DateTime sinirlandirma = DateTime.Now.AddDays(-15);
        public ActionResult Index()
        {
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];
            List<RolYetkiAtama> yetki = (List<RolYetkiAtama>)Session["yetki"];
            ViewBag.excl = 0;
            ViewBag.bslk = "İTHALAT/İHRACAT ARAÇ KAYIT LİSTESİ";
            var ihrct = db.Ithalat_Ihracat_Arac.Where(m => m.giristarih > sinirlandirma).OrderByDescending(m => m.ihracataracid).ToList();
            foreach (var item in yetki)
            {
                //if (kid.YetkiTipi == 1)
                if (item.Rol_Id == 3 && item.Yetki_Id != 1)
                {
                    ihrct = ihrct.Where(m => m.bolgeid == bid.Bölge_id).ToList();
                    break;
                }
            }
            return View(ihrct.ToList());
        }
        public ActionResult EskiKayitlar()
        {
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];
            List<RolYetkiAtama> yetki = (List<RolYetkiAtama>)Session["yetki"];
            ViewBag.excl = 0;
            ViewBag.bslk = "İTHALAT/İHRACAT ARAÇ KAYIT LİSTESİ";
            var ihrct = db.Ithalat_Ihracat_Arac.Where(m => m.giristarih <= sinirlandirma).OrderByDescending(m => m.ihracataracid).ToList();
            foreach (var item in yetki)
            {
                //if (kid.YetkiTipi == 1)
                if (item.Rol_Id == 3 && item.Yetki_Id != 1)
                {
                    ihrct = ihrct.Where(m => m.bolgeid == bid.Bölge_id).ToList();
                    break;
                }
            }
            return View("Index", ihrct.ToList());
        }
        public ActionResult Filtre(int id)
        {
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];
            List<RolYetkiAtama> yetki = (List<RolYetkiAtama>)Session["yetki"];
            var ihrct = db.Ithalat_Ihracat_Arac.OrderByDescending(m => m.ihracataracid).ToList();
            foreach (var item in yetki)
            {
                //if (kid.YetkiTipi == 1)
                if (item.Rol_Id == 3 && item.Yetki_Id != 1)
                {
                    ihrct = ihrct.Where(m => m.bolgeid == bid.Bölge_id).ToList();
                    break;
                }
            }
            if (id == 1)
            {
                ViewBag.excl = 0;
                ViewBag.bslk = "ÇIKIŞ İŞLEMİ YAPMAYAN ARAÇLAR LİSTESİ";
                ihrct = ihrct.Where(m => m.cikistarih == null).ToList();
            }
            else if (id == 2)
            {
                ViewBag.excl = 1;
                ViewBag.bslk = "İTHALAT ARAÇLARI KAYIT LİSTESİ";
                ihrct = ihrct.Where(m => m.nakliyetur == "İthalat").ToList();
            }
            else if (id == 3)
            {
                ViewBag.excl = 2;
                ViewBag.bslk = "İHRACAT ARAÇ KAYIT LİSTESİ";
                ihrct = ihrct.Where(m => m.nakliyetur == "İhracat").ToList();
            }
            return View("Index", ihrct.ToList());
        }
        [HttpGet]
        public ActionResult IthalatKayıt()
        {
            TempData["tur"] = "İthalat";
            ViewBag.bsl = "İTHALAT ARAÇ KAYIT EKLEME SAYFASI";
            TempData["tarih"] = DateTime.Now;
            TempData["ctarih"] = null;
            return View("IhracatKayıt", new Ithalat_Ihracat_Arac());
        }
        [HttpGet]
        public ActionResult IhracatKayıt()
        {
            TempData["tur"] = "İhracat";
            ViewBag.bsl = "İHRACAT ARAÇ KAYIT EKLEME SAYFASI";
            TempData["tarih"] = DateTime.Now;
            TempData["ctarih"] = null;
            return View("IhracatKayıt", new Ithalat_Ihracat_Arac());
        }
        [HttpPost]
        public ActionResult IhracatKayıt(Ithalat_Ihracat_Arac ihrct)
        {
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];
            List<RolYetkiAtama> yetki = (List<RolYetkiAtama>)Session["yetki"];
            if (ModelState.IsValid)
            {
                if (ihrct.ihracataracid == 0)
                {
                    ihrct.giristarih = Convert.ToDateTime(TempData["tarih"]);
                    if (TempData["ctarih"] != null)
                    {
                        ihrct.cikistarih = Convert.ToDateTime(TempData["ctarih"]);
                    }
                    ihrct.kullaniciid = kid.Kullanici_id;
                    ihrct.bolgeid = bid.Bölge_id;
                    ihrct.nakliyetur = (string)TempData["tur"];
                    db.Ithalat_Ihracat_Arac.Add(ihrct);
                }
                else
                {
                    var deger = db.Ithalat_Ihracat_Arac.Find(ihrct.ihracataracid);
                    if (deger == null)
                    {
                        return HttpNotFound();
                    }
                    deger.sevkyeri = ihrct.sevkyeri;
                    deger.irsaliyeno = ihrct.irsaliyeno;
                    deger.muhurno = ihrct.muhurno;
                    deger.irsaliyetarih = ihrct.irsaliyetarih;
                    deger.koliadet = ihrct.koliadet;
                    deger.ikincikullanici = kid.Ad + " " + kid.Soyad;
                    foreach (var item in yetki)
                    {
                        //if (kid.YetkiTipi == 1 || kid.YetkiTipi == 3)
                        if (item.Rol_Id == 3 && (item.Yetki_Id == 1 || item.Yetki_Id == 3))
                        {
                            deger.tasiyicifirmaad = ihrct.tasiyicifirmaad;
                            deger.tasiyiciplaka = ihrct.tasiyiciplaka;
                            deger.soforadsoyad = ihrct.soforadsoyad;
                            deger.sofortc = ihrct.sofortc;
                            deger.sofortel = ihrct.sofortel;
                            deger.giristarih = ihrct.giristarih;
                            deger.cikistarih = ihrct.cikistarih;
                            //deger.sevkyeri = ihrct.sevkyeri;
                            //deger.irsaliyeno = ihrct.irsaliyeno;
                            //deger.muhurno = ihrct.muhurno;
                            //deger.irsaliyetarih = ihrct.irsaliyetarih;
                            //deger.koliadet = ihrct.koliadet;
                            //deger.ikincikullanici = kid.Ad + " " + kid.Soyad;                            
                            break;
                        }
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index", "IthalatIhracat");
            }
            return View("IhracatKayıt");
        }
        public ActionResult Sil(int id)
        {
            var ihrct = db.Ithalat_Ihracat_Arac.Find(id);
            db.Ithalat_Ihracat_Arac.Remove(ihrct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult cikis(int id)
        {
            var model = db.Ithalat_Ihracat_Arac.Find(id);
            model.cikistarih = Convert.ToDateTime(DateTime.Now);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var model = db.Ithalat_Ihracat_Arac.Find(id);
            if (model.nakliyetur == "İthalat")
            {
                ViewBag.bsl = "İTHALAT ARAÇ KAYIT EKLEME SAYFASI";
            }
            else
            {
                ViewBag.bsl = "İHRACAT ARAÇ KAYIT EKLEME SAYFASI";
            }
            if (model == null)
            { return HttpNotFound(); }
            return View("IhracatKayıt", model);
        }
        public ActionResult TekrarKayit(int id)
        {
            var model = db.Ithalat_Ihracat_Arac.Find(id);
            if (model.nakliyetur == "İthalat")
            {
                TempData["tur"] = "İthalat";
                ViewBag.bsl = "İTHALAT ARAÇ KAYIT EKLEME SAYFASI";
            }
            else
            {
                ViewBag.bsl = "İHRACAT ARAÇ KAYIT EKLEME SAYFASI";
                TempData["tur"] = "İhracat";
            }
            model.ihracataracid = 0;
            TempData["tarih"] = model.giristarih;
            if (model.cikistarih != null)
            {
                TempData["ctarih"] = model.cikistarih;
            }
            else
            {
                TempData["ctarih"] = null;
            }
            if (model == null)
            { return HttpNotFound(); }
            return View("IhracatKayıt", model);
        }
        public ActionResult YeniKayit(int id)
        {
            var model = db.Ithalat_Ihracat_Arac.Find(id);
            Ithalat_Ihracat_Arac bul = new Ithalat_Ihracat_Arac();
            if (model.nakliyetur == "İthalat")
            {
                TempData["tur"] = "İthalat";
                ViewBag.bsl = "İTHALAT ARAÇ KAYIT EKLEME SAYFASI";
            }
            else
            {
                ViewBag.bsl = "İHRACAT ARAÇ KAYIT EKLEME SAYFASI";
                TempData["tur"] = "İhracat";
            }
            //model.ihracataracid = 0;
            bul.tasiyicifirmaad = model.tasiyicifirmaad;
            bul.soforadsoyad=model.soforadsoyad;
            bul.sofortel=model.sofortel;
            bul.sofortc=model.sofortc;
            TempData["tarih"] = DateTime.Now;
            TempData["ctarih"] = null;
            if (model == null)
            { return HttpNotFound(); }
            return View("IhracatKayıt", bul);
        }
        public void GridExportToExcel(int? id, DateTime? ilktrh, DateTime? sontrh)
        {
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];
            List<RolYetkiAtama> yetki = (List<RolYetkiAtama>)Session["yetki"];
            string tarih = DateTime.Now.ToString("dd-MM-yyyy");
            string dosyaAdi = "İthalat-İhracat Araç Kayıtları";
            var table = (from i in db.Ithalat_Ihracat_Arac
                             //where i.bolgeid==bid.Bölge_id
                         select new
                         {
                             NAKLİYE_ARAÇ_TÜRÜ = i.nakliyetur,
                             GİRİŞ_TARİHİ = i.giristarih,   //alias tanımlama
                             TAŞIYICI_FİRMA_ADI = i.tasiyicifirmaad,
                             ARAÇ_PLAKASI = i.tasiyiciplaka,
                             ŞOFÖR_AD_SOYAD = i.soforadsoyad,
                             ŞOFÖR_TC = i.sofortc,
                             ŞOFÖR_TELEFON = i.sofortel,
                             SEVKİYATIN_GİTTİĞİ_YER = i.sevkyeri,
                             İRSALİYE_NO = i.irsaliyeno,
                             MÜHÜR_NO = i.muhurno,
                             İRSALİYE_TARİHİ = i.irsaliyetarih,
                             KOLİ_ADET = i.koliadet,
                             ÇIKIŞ_TARİHİ = i.cikistarih,
                             BÖLGE = i.BölgelerTablosu.Bölge,
                             KULLANICI = i.KullaniciTablosu.Ad + " " + i.KullaniciTablosu.Soyad
                         }).ToList();
            foreach (var item in yetki)
            {
                //if (kid.YetkiTipi == 1)
                if (item.Rol_Id == 3 && item.Yetki_Id != 1)
                {
                    if (id == 1)
                    {
                        if (ilktrh != null && sontrh != null)
                        {
                            table = table.Where(m => m.NAKLİYE_ARAÇ_TÜRÜ == "İthalat" && m.GİRİŞ_TARİHİ >= ilktrh && m.BÖLGE == bid.Bölge && m.GİRİŞ_TARİHİ <= sontrh).ToList();
                        }
                        else
                        {
                            table = table.Where(m => m.BÖLGE == bid.Bölge && m.NAKLİYE_ARAÇ_TÜRÜ == "İthalat").ToList();
                        }
                        dosyaAdi = "İthalat Araç Kayıtları";
                    }
                    else if (id == 2)
                    {
                        if (ilktrh != null && sontrh != null)
                        {
                            table = table.Where(m => m.NAKLİYE_ARAÇ_TÜRÜ == "İhracat" && m.BÖLGE == bid.Bölge && m.GİRİŞ_TARİHİ >= ilktrh && m.GİRİŞ_TARİHİ <= sontrh).ToList();
                        }
                        else
                        {
                            table = table.Where(m => m.BÖLGE == bid.Bölge && m.NAKLİYE_ARAÇ_TÜRÜ == "İhracat").ToList();
                        }
                        dosyaAdi = "İhracat Araç Kayıtları";
                    }
                    else if (id == 0)
                    {
                        if (ilktrh != null && sontrh != null)
                        {
                            table = table.Where(m => m.BÖLGE == bid.Bölge && m.GİRİŞ_TARİHİ >= ilktrh && m.GİRİŞ_TARİHİ <= sontrh).ToList();
                        }
                        else
                        {
                            table = table.Where(m => m.BÖLGE == bid.Bölge).ToList();
                        }
                    }
                    break;
                }
                else
                {
                    if (id == 1)
                    {
                        if (ilktrh != null && sontrh != null)
                        {
                            table = table.Where(m => m.NAKLİYE_ARAÇ_TÜRÜ == "İthalat" && m.GİRİŞ_TARİHİ >= ilktrh && m.GİRİŞ_TARİHİ <= sontrh).ToList();
                        }
                        else
                        {
                            table = table.Where(m => m.NAKLİYE_ARAÇ_TÜRÜ == "İthalat").ToList();
                        }
                        dosyaAdi = "İthalat Araç Kayıtları";
                    }
                    else if (id == 2)
                    {
                        if (ilktrh != null && sontrh != null)
                        {
                            table = table.Where(m => m.NAKLİYE_ARAÇ_TÜRÜ == "İhracat" && m.GİRİŞ_TARİHİ >= ilktrh && m.GİRİŞ_TARİHİ <= sontrh).ToList();
                        }
                        else
                        {
                            table = table.Where(m => m.NAKLİYE_ARAÇ_TÜRÜ == "İhracat").ToList();
                        }
                        dosyaAdi = "İhracat Araç Kayıtları";
                    }
                    else if (id == 0)
                    {
                        if (ilktrh != null && sontrh != null)
                        {
                            table = table.Where(m => m.GİRİŞ_TARİHİ >= ilktrh && m.GİRİŞ_TARİHİ <= sontrh).ToList();
                        }
                        else
                        {
                            table.ToList();
                        }
                    }
                }
            }
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
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Write(sw.ToString());
            Response.End();
        }
    }
}