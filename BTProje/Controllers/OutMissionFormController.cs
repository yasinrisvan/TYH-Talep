using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using BTProje.Models.EntityFramework;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity.Validation;

namespace BTProje.Controllers
{
    public class OutMissionFormController : Controller
    {
        TYHTALEPEntities db = new TYHTALEPEntities();

        SqlConnection dbf = new SqlConnection("Server=192.168.100.12;Database=TYH802;Trusted_Connection=True;");

        // GET: OutMissionForm
        public ActionResult Index()
        {
            KullaniciTablosu kullanici = (KullaniciTablosu)Session["kullanici"];
            List<SelectListItem> user = (from i in db.KullaniciTablosu.Where(m => m.MaliyetMNo == 3511501.ToString() && m.BölgeID == kullanici.BölgeID).ToList()
                                         select new SelectListItem
                                         {
                                             Text = i.Ad + " " + i.Soyad,
                                             Value = i.Kullanici_id.ToString()
                                         }).OrderBy(x => x.Text).ToList();
            ViewBag.User = user;

            List<SelectListItem> unit = (from i in db.Unit.ToList()
                                         select new SelectListItem
                                         {
                                             Text = i.NameOfUnit,
                                             Value = i.Id.ToString()
                                         }).OrderBy(x => x.Text).ToList();
            ViewBag.Unit = unit;

            List<SelectListItem> mission = (from i in db.Mission.ToList()
                                            select new SelectListItem
                                            {
                                                Text = i.MissionName,
                                                Value = i.MissionId.ToString()
                                            }).OrderBy(x => x.Text).ToList();
            ViewBag.Mission = mission;


            return View();
        }
        //[HttpPost]
        //public ActionResult Index(DgsForm dgsForm)
        //{
        //    if (ModelState.IsValid)
        //    { // code of project
        //        DgsForm newForm = new DgsForm();
        //        newForm.UserId = dgsForm.UserId;
        //        newForm.StartDate = dgsForm.StartDate;
        //        newForm.Description = dgsForm.Description.ToUpper();
        //        if (dgsForm.CodeOfProject == null || dgsForm.CodeOfProject == "")
        //        {
        //            newForm.CodeOfProject = "";
        //        }
        //        else
        //        {
        //            newForm.CodeOfProject = dgsForm.CodeOfProject.ToUpper();

        //        }
        //        if (dgsForm.UnitOfDepartment == null)
        //        {
        //            newForm.UnitOfDepartment = 11;
        //        }
        //        else
        //        {
        //            newForm.UnitOfDepartment = dgsForm.UnitOfDepartment;
        //        }
        //        if (dgsForm.MissionId == null)
        //        {
        //            newForm.MissionId = 11;
        //        }
        //        else
        //        {
        //            newForm.MissionId = dgsForm.MissionId;
        //        }
        //        db.DgsForm.Add(newForm);
        //        db.SaveChanges();
        //        TempData["MessageSuccess"] = "GOREVLI PERSONEL EKLENDI ";
        //    }

        //    else
        //    {
        //        @TempData["MessageError"] = "KAYDEDİLEMEDİ !";

        //    }
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public ActionResult Index(FormCollection form, string Unit)
        {
            if (form["CodeOfProject"] == null)
                form["CodeOfProject"] = "";

            //if (form["Unit"] == null)
            //    form["Unit"] = "";

            if (form["MissionId"] == null)
                form["MissionId"] = 11.ToString();

            if (form["Description"] == null)
                form["Description"] = "";

            DgsForm dgsForm = new DgsForm
            {
                UserId = Convert.ToInt32(form["UserId"]),
                //Unit = form["Unit"].ToString(),
                Unit = Unit,
                CodeOfProject = form["CodeOfProject"].ToUpper(),
                StartDate = Convert.ToDateTime(form["StartDate"]),
                MissionId = Convert.ToInt16(form["MissionId"]),
                Description = form["Description"].ToUpper(),
                UnitOfDepartment = 11,


            };

            db.DgsForm.Add(dgsForm);
            db.SaveChanges();


            TempData["MessageSuccess"] = "GOREVLI PERSONEL EKLENDI ";
            return RedirectToAction("Index");
        }

        public ActionResult GetForm()
        {
            var forms = db.DgsForm.OrderByDescending(x => x.Id).ToList();


            return View(forms);
        }
        public ActionResult Incumbent()
        {
            var forms = db.DgsForm.Where(x => x.EndDate == null).ToList();
            return View("GetForm", forms/*.Where(m => m.EndDate == null).ToList()*/);
        }
        public ActionResult Update(int id)
        {
            TempData["MessageSuccess"] = null;

            TempData["MessageError"] = null;
            //if(string.IsNullOrEmpty(id.ToString()))
            //{

            //}
            KullaniciTablosu kullanici = (KullaniciTablosu)Session["kullanici"];
            List<SelectListItem> user = (from i in db.KullaniciTablosu.Where(m => m.MaliyetMNo == 3511501.ToString() && m.BölgeID == kullanici.BölgeID).ToList()
                                         select new SelectListItem
                                         {
                                             Text = i.Ad + " " + i.Soyad,
                                             Value = i.Kullanici_id.ToString()
                                         }).OrderBy(x => x.Text).ToList();
            ViewBag.User = user;

            List<SelectListItem> unit = (from i in db.Unit.ToList()
                                         select new SelectListItem
                                         {
                                             Text = i.NameOfUnit,
                                             Value = i.Id.ToString()
                                         }).OrderBy(x => x.Text).ToList();
            ViewBag.Unit = unit;

            List<SelectListItem> mission = (from i in db.Mission.ToList()
                                            select new SelectListItem
                                            {
                                                Text = i.MissionName,
                                                Value = i.MissionId.ToString()
                                            }).OrderBy(x => x.Text).ToList();
            ViewBag.Mission = mission;
            var dgs = db.DgsForm.Find(id);
            return View(dgs);
        }
        [HttpPost]
        public ActionResult Update(DgsForm dgsForm)
        {

            //dgsForm.Description = dgsForm.Description.ToUpper();
            //dgsForm.CodeOfProject = dgsForm.CodeOfProject.ToUpper();


            if (ModelState.IsValid)
            {
                var old = db.DgsForm.Find(dgsForm.Id);

                string endDate = dgsForm.EndDate.ToString();
                old.UserId = dgsForm.UserId;
                old.StartDate = dgsForm.StartDate;
                old.Description = dgsForm.Description.ToUpper();
                old.EndDate = dgsForm.EndDate;
                old.Unit = dgsForm.Unit; 
                if (dgsForm.CodeOfProject == null)
                {
                    old.CodeOfProject = "";
                }
                else if (dgsForm.CodeOfProject != old.CodeOfProject)
                {
                    old.CodeOfProject = dgsForm.CodeOfProject.ToUpper().Trim();
                }

                if (dgsForm.UnitOfDepartment == null)
                {
                    old.UnitOfDepartment = 11;
                }
                else if (dgsForm.CodeOfProject != old.CodeOfProject)
                {
                    old.UnitOfDepartment = dgsForm.UnitOfDepartment;
                }

                if (dgsForm.MissionId == null)
                {
                    old.MissionId = 11;
                }
                else if (dgsForm.MissionId != old.MissionId)
                {
                    old.MissionId = dgsForm.MissionId;
                }

                //DateTime yasin = DateTime.Now.AddDays(-14);

                db.SaveChanges();

                TempData["MessageSuccess"] = "KAYIT GUNCELLENDI";
            }
            else
            {
                TempData["MessageError"] = "KAYIT GUNCELLENEMEDI";
            }

            return RedirectToAction("GetForm");
        }
        public ActionResult DateEnd(int id)
        {
            var old = db.DgsForm.Find(id);
            old.EndDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("GetForm");
        }
        public void GridExportToExcel(DateTime? ilktrh, DateTime? sontrh)
        {
            KullaniciTablosu kid = (KullaniciTablosu)Session["kullanici"];
            BölgelerTablosu bid = (BölgelerTablosu)Session["bolge"];
            string tarih = DateTime.Now.ToString("dd-MM-yyyy");
            string dosyaAdi = "DGSB KAYITLARI";
            var table = (from i in db.DgsForm
                         select new
                         {
                             SIRA_NO = i.Id,   //alias tanımlama
                             BÖLGE = i.KullaniciTablosu.BölgelerTablosu.Bölge,
                             ADI_SOYADI = i.KullaniciTablosu.Ad + " " + i.KullaniciTablosu.Soyad,
                             TC_KİMLİK_NO = i.KullaniciTablosu.TC,
                             BİRİM = i.Unit,
                             //BİRİM = i.Unit1.NameOfUnit,
                             PROJE_KODU = i.CodeOfProject,
                             BAŞLANGIÇ_TARİHİ = i.StartDate,
                             //BAŞLANGIÇ_SAATİ = i.StartDate.Value.ToShortTimeString(),
                             //asd="",
                             //ast="",
                             BİTİŞ_TARİHİ = i.EndDate,
                             GÖREVLİ_OLMA_NEDENİ = i.Mission.MissionName,
                             AÇIKLAMA = i.Description,
                         }).ToList();
            //if (kid.YetkiTipi == 1)
            //{
            //foreach (var item in table)
            //{
                
            //    //item.asd.Insert(0,item.BAŞLANGIÇ_TARİHİ.Value.ToShortDateString());
            //    item.asd.Insert(7, "testtt");
            //    //item.asd.Replace(item.asd, item.BAŞLANGIÇ_TARİHİ.Value.ToShortTimeString());
            //    item.ast.Insert(8, item.BAŞLANGIÇ_TARİHİ.Value.ToShortTimeString());
            //}
            if (ilktrh != null && sontrh != null)
            {
                table = table.Where(m =>m.BAŞLANGIÇ_TARİHİ >= ilktrh)
                .Where(m => m.BAŞLANGIÇ_TARİHİ <= sontrh).ToList();
            }
            else if (ilktrh == null || sontrh == null)
            {
                table.ToList();
            }
            //}
            //else
            //{
            if (ilktrh != null && sontrh != null)
            {
                table = table.Where(m =>m.BAŞLANGIÇ_TARİHİ >= ilktrh)
                             //.Where(m => m.BÖLGE == bid.Bölge)
                             .Where(m => m.BAŞLANGIÇ_TARİHİ <= sontrh).ToList();
            }
            else if (ilktrh == null || sontrh == null)
            {
                table = table.Where(m => m.BÖLGE == bid.Bölge).ToList();
            }
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
        public ActionResult Review(int id)
        {
            var person = db.DgsForm.Find(id);
            if (person == null)
            {
                return View("Error");
            }
            else
            {
                DgsForm form = new DgsForm();
                return View(person);
            }
        }
        public ActionResult Delete(int id)
        {
            var dgs = db.DgsForm.Find(id);
            db.DgsForm.Remove(dgs);
            db.SaveChanges();
            return RedirectToAction("GetForm");
        }


        public JsonResult Cascad(int p)      //cascading dropdownlist
        {

            string unit = "";
            var user = db.KullaniciTablosu.Find(p);
            int sNo = Convert.ToInt32(user.SicilNo);
            string sicilNo;
            if (sNo < 10000)
            {
                sicilNo = "0" + user.SicilNo;
            }
            else
            {
                sicilNo = user.SicilNo;
            }

            dbf.Open();
            string sql = "SELECT [RDDEPARTMENT] as unit FROM [dbo].[CANIAS_ARGE] WHERE [PERSID]='" + sicilNo + "'";
            SqlCommand cmd = new SqlCommand(sql, dbf);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            SqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                unit = dr["unit"].ToString();
            }
            dbf.Close();

            return Json(unit, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Example()
        {
            KullaniciTablosu kullanici = (KullaniciTablosu)Session["kullanici"];
            List<SelectListItem> user = (from i in db.KullaniciTablosu.Where(m => m.MaliyetMNo == 3511501.ToString() && m.BölgeID == kullanici.BölgeID).ToList()
                                         select new SelectListItem
                                         {
                                             Text = i.Ad + " " + i.Soyad,
                                             Value = i.Kullanici_id.ToString()
                                         }).OrderBy(x => x.Text).ToList();
            ViewBag.User = user;

            List<SelectListItem> unit = (from i in db.Unit.ToList()
                                         select new SelectListItem
                                         {
                                             Text = i.NameOfUnit,
                                             Value = i.Id.ToString()
                                         }).OrderBy(x => x.Text).ToList();
            ViewBag.Unit = unit;

            List<SelectListItem> mission = (from i in db.Mission.ToList()
                                            select new SelectListItem
                                            {
                                                Text = i.MissionName,
                                                Value = i.MissionId.ToString()
                                            }).OrderBy(x => x.Text).ToList();
            ViewBag.Mission = mission;


            return View();
        } ///gereksiz

    }
}