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
    public class ZiyaretciController : Controller
    {
        // GET: Ziyaretci
        TYHTALEPEntities db = new TYHTALEPEntities();
        DateTime sinirlandirma = DateTime.Now.AddDays(-15);
        public void method()
        {
            List<SelectListItem> departman = (from i in db.DepartmanlarTablosu.ToList()
                                              select new SelectListItem
                                              {
                                                  Text = i.Ad,
                                                  Value = i.Departman_id.ToString()
                                              }).OrderBy(x => x.Text).ToList();
            departman.Insert(0, new SelectListItem { Value = "", Text = "Tüm Birimler"/*,Selected=true */});
            ViewBag.dept = departman;
        }
        public ActionResult ZiyaretciIndex()
        {
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];
            List<RolYetkiAtama> yetki = (List<RolYetkiAtama>)Session["yetki"];
            ViewBag.bslk = "ZİYARETÇİLER LİSTESİ";
            //List<SelectListItem> deger = (from i in db.DepartmanlarTablosu.ToList()
            //                              select new SelectListItem
            //                              {
            //                                  Text = i.Ad,
            //                                  Value = i.Departman_id.ToString()
            //                              }).OrderBy(x => x.Text).ToList();
            //deger.Add(new SelectListItem
            //{
            //    Text = "Tümü",
            //    Value = "0",
            //});
            //ViewBag.dgr1 = deger;
            method();
            var zyrt = db.Ziyaretci.Where(m => m.z_giristarih > sinirlandirma).OrderByDescending(m => m.ziyaretciid).ToList();
            foreach (var item in yetki)
            {
                if (item.Rol_Id == 3 && item.Yetki_Id != 1)
                {
                    zyrt = zyrt.Where(m => m.bolgeid == bid.Bölge_id).ToList();
                    break;
                }
            }
            //if (kid.YetkiTipi == 1)
            //{
            //    zyrt = zyrt.ToList();
            //}
            //else
            //{
            //    zyrt = zyrt.Where(m => m.bolgeid == bid.Bölge_id).ToList();
            //}
            return View(zyrt.ToList());
        }
        public ActionResult EskiKayitlar()
        {
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];
            List<RolYetkiAtama> yetki = (List<RolYetkiAtama>)Session["yetki"];
            ViewBag.bslk = "ZİYARETÇİLER LİSTESİ";
            //List<SelectListItem> deger = (from i in db.DepartmanlarTablosu.ToList()
            //                              select new SelectListItem
            //                              {
            //                                  Text = i.Ad,
            //                                  Value = i.Departman_id.ToString()
            //                              }).OrderBy(x => x.Text).ToList();
            //deger.Add(new SelectListItem
            //{
            //    Text = "Tümü",
            //    Value = "0",
            //});
            //ViewBag.dgr1 = deger;
            method();
            var zyrt = db.Ziyaretci.Where(m => m.z_giristarih <= sinirlandirma).OrderByDescending(m => m.ziyaretciid).ToList();
            foreach (var item in yetki)
            {
                if (item.Rol_Id == 3 && item.Yetki_Id != 1)
                {
                    zyrt = zyrt.Where(m => m.bolgeid == bid.Bölge_id).ToList();
                    break;
                }
            }
            //if (kid.YetkiTipi == 1)
            //{
            //    zyrt = zyrt.ToList();
            //}
            //else
            //{
            //    zyrt = zyrt.Where(m => m.bolgeid == bid.Bölge_id).ToList();
            //}
            return View("ZiyaretciIndex", zyrt.ToList());
        }
        public ActionResult Filtre()
        {
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];
            List<RolYetkiAtama> yetki = (List<RolYetkiAtama>)Session["yetki"];
            ViewBag.bslk = "ÇIKIŞ YAPMAYAN ZİYARETÇİLER LİSTESİ";
            //List<SelectListItem> deger = (from i in db.DepartmanlarTablosu.ToList()
            //                              select new SelectListItem
            //                              {
            //                                  Text = i.Ad,
            //                                  Value = i.Departman_id.ToString()
            //                              }).OrderBy(x => x.Text).ToList();
            //deger.Add(new SelectListItem
            //{
            //    Text = "Tümü",
            //    Value = "0",
            //});
            //ViewBag.dgr1 = deger;
            method();
            var zyrt = db.Ziyaretci.Where(m => m.z_cikistarih == null).OrderByDescending(m => m.ziyaretciid).ToList();
            foreach (var item in yetki)
            {
                if (item.Rol_Id == 3 && item.Yetki_Id != 1)
                {
                    zyrt = zyrt.Where(m => m.bolgeid == bid.Bölge_id).ToList();
                    break;
                }
            }
            //if (kid.YetkiTipi == 1)
            //{
            //    zyrt = zyrt.ToList();
            //}
            //else
            //{
            //    zyrt = zyrt.Where(m => m.bolgeid == bid.Bölge_id).ToList();
            //}
            return View("ZiyaretciIndex", zyrt.ToList());
        }
        public ActionResult BirimFiltre(int? id)
        {
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];
            List<RolYetkiAtama> yetki = (List<RolYetkiAtama>)Session["yetki"];
            ViewBag.bslk = "ZİYARETÇİLER LİSTESİ";
            //List<SelectListItem> deger = (from i in db.DepartmanlarTablosu.ToList()
            //                              select new SelectListItem
            //                              {
            //                                  Text = i.Ad,
            //                                  Value = i.Departman_id.ToString(),
            //                                  Selected=true
            //                              }).OrderBy(x => x.Text).ToList();
            //deger.Add(new SelectListItem
            //{
            //    Text = "Tüm Birimler",
            //    Value = "",
            //});
            //ViewBag.dept = deger;
            method();
            if (id == 0 || id == null)
            {
                return RedirectToAction("ZiyaretciIndex");
            }
            else
            {
                var zyrt = db.Ziyaretci.Where(m => m.birimid == id).OrderByDescending(m => m.ziyaretciid).ToList();
                foreach (var item in yetki)
                {
                    if (item.Rol_Id == 3 && item.Yetki_Id != 1)
                    {
                        zyrt = zyrt.Where(m => m.bolgeid == bid.Bölge_id).ToList();
                        break;
                    }
                }
                //if (kid.YetkiTipi == 1)
                //{
                //    zyrt = zyrt.ToList();
                //}
                //else
                //{
                //    zyrt = zyrt.Where(m => m.bolgeid == bid.Bölge_id).ToList();
                //}
                return View("ZiyaretciIndex", zyrt.ToList());
            }
        }
        public ActionResult Sil(int id)
        {
            var zyrtc = db.Ziyaretci.Find(id);
            db.Ziyaretci.Remove(zyrtc);
            db.SaveChanges();
            return RedirectToAction("ZiyaretciIndex");
        }
        [HttpGet]
        public ActionResult ZKayıt()
        {
            //List<SelectListItem> deger = (from i in db.DepartmanlarTablosu.ToList()
            //                              select new SelectListItem
            //                              {
            //                                  Text = i.Ad,
            //                                  Value = i.Departman_id.ToString()
            //                              }).OrderBy(x => x.Text).ToList();
            //ViewBag.dgr = deger;
            method();
            return View("ZKayıt", new Ziyaretci());
        }

        [HttpPost]
        public ActionResult ZKayıt(Ziyaretci zyrtc)
        {
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];
            List<RolYetkiAtama> yetki = (List<RolYetkiAtama>)Session["yetki"];
            if (zyrtc.ziyaretciid == 0)
            {
                zyrtc.z_giristarih = Convert.ToDateTime(DateTime.Now);
                zyrtc.kullaniciid = kid.Kullanici_id;
                zyrtc.bolgeid = bid.Bölge_id;
                db.Ziyaretci.Add(zyrtc);
            }
            else
            {
                var deger = db.Ziyaretci.Find(zyrtc.ziyaretciid);
                if (deger == null)
                {
                    return HttpNotFound();
                }
                foreach (var item in yetki)
                {
                    if (item.Rol_Id == 3 && (item.Yetki_Id == 1 || item.Yetki_Id == 3))
                    {
                        deger.z_adsoyad = zyrtc.z_adsoyad;
                        deger.z_tcno = zyrtc.z_tcno;
                        deger.birimid = zyrtc.birimid;
                        deger.ziyaretsebebi = zyrtc.ziyaretsebebi;
                        deger.z_edilenadsoyad = zyrtc.z_edilenadsoyad;
                        deger.kartno = zyrtc.kartno;
                        deger.z_plaka = zyrtc.z_plaka;
                        deger.z_giristarih = zyrtc.z_giristarih;
                        deger.z_cikistarih = zyrtc.z_cikistarih;
                        break;
                    }
                }
                //if (kid.YetkiTipi == 1 || kid.YetkiTipi == 3)
                //{
                //    deger.z_adsoyad = zyrtc.z_adsoyad;
                //    deger.z_tcno = zyrtc.z_tcno;
                //    deger.birimid = zyrtc.birimid;
                //    deger.ziyaretsebebi = zyrtc.ziyaretsebebi;
                //    deger.z_edilenadsoyad = zyrtc.z_edilenadsoyad;
                //    deger.kartno = zyrtc.kartno;
                //    deger.z_plaka = zyrtc.z_plaka;
                //    deger.z_giristarih = zyrtc.z_giristarih;
                //    deger.z_cikistarih = zyrtc.z_cikistarih;
                //}
            }
            db.SaveChanges();
            return RedirectToAction("ZiyaretciIndex", "Ziyaretci");
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var model = db.Ziyaretci.Find(id);
            if (model == null)
            { return HttpNotFound(); }
            method();
            return View("ZKayıt", model);
        }
        public ActionResult Yeni(int id)
        {
            var model = db.Ziyaretci.Find(id);
            model.ziyaretciid = 0;
            if (model == null)
            { return HttpNotFound(); }
            //List<SelectListItem> deger = (from i in db.DepartmanlarTablosu
            //                              select new SelectListItem
            //                              {
            //                                  Selected = true,
            //                                  Text = i.Ad,
            //                                  Value = i.Departman_id.ToString()
            //                              }).OrderBy(x => x.Text).ToList();
            //ViewBag.dept = deger;
            method();
            return View("ZKayıt", model);
        }
        public ActionResult cikis(int id)
        {
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            var model = db.Ziyaretci.Find(id);
            if (model.z_cikistarih == null)
            {
                model.z_cikistarih = Convert.ToDateTime(DateTime.Now);
                model.ikincikullanici = kid.Ad + " " + kid.Soyad;
            }

            db.SaveChanges();
            return RedirectToAction("ZiyaretciIndex");
        }
        public void GridExportToExcel(DateTime? ilktrh, DateTime? sontrh)
        {
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];
            List<RolYetkiAtama> yetki = (List<RolYetkiAtama>)Session["yetki"];
            string tarih = DateTime.Now.ToString("dd-MM-yyyy");
            string dosyaAdi = "Ziyaretci Kayıtları";
            var table = (from i in db.Ziyaretci
                         select new
                         {
                             GİRİŞ_TARİHİ = i.z_giristarih,   //alias tanımlama
                             ADI_SOYADI = i.z_adsoyad,
                             TC_KİMLİK_NO = i.z_tcno,
                             ZİYARET_SEBEBİ = i.ziyaretsebebi,
                             ZİYARET_EDİLEN_KİŞİ = i.z_edilenadsoyad,
                             ZİYARET_EDİLEN_BİRİM = i.DepartmanlarTablosu.Ad,
                             ARAÇ_PLAKASI = i.z_plaka,
                             KART_NUMARASI = i.kartno,
                             ÇIKIŞ_TARİHİ = i.z_cikistarih,
                             BÖLGE = i.BölgelerTablosu.Bölge,
                             KULLANICI = i.KullaniciTablosu.Ad + " " + i.KullaniciTablosu.Soyad
                         }).ToList();
            foreach (var item in yetki)
            {
                if (item.Rol_Id == 3 && item.Yetki_Id != 1)
                {
                    if (ilktrh != null && sontrh != null)
                    {
                        table = table.Where(m => m.BÖLGE == bid.Bölge && m.GİRİŞ_TARİHİ >= ilktrh && m.GİRİŞ_TARİHİ <= sontrh).ToList();
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
                        table = table.Where(m => m.GİRİŞ_TARİHİ >= ilktrh && m.GİRİŞ_TARİHİ <= sontrh).ToList();
                    }
                }
            }
            //if (kid.YetkiTipi == 1)
            //{
            //    if (ilktrh != null && sontrh != null)
            //    {
            //        table = table.Where(m => m.GİRİŞ_TARİHİ >= ilktrh && m.GİRİŞ_TARİHİ <= sontrh).ToList();
            //    }
            //    else if (ilktrh == null || sontrh == null)
            //    {
            //        table.ToList();
            //    }
            //}
            //else
            //{
            //    if (ilktrh != null && sontrh != null)
            //    {
            //        table = table.Where(m => m.GİRİŞ_TARİHİ >= ilktrh && m.BÖLGE == bid.Bölge && m.GİRİŞ_TARİHİ <= sontrh).ToList();
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
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Write(sw.ToString());
            Response.End();
        }
    }
}