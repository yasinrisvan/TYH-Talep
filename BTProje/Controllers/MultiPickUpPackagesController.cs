using BTProje.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTProje.Controllers
{
    public class MultiPickUpPackagesController : Controller
    {
        TYHTALEPEntities db = new TYHTALEPEntities();
        // GET: MultiPickUpPackages
        public ActionResult Index()
        {
            KargoModel model = new KargoModel();
            model.Kargos = db.Kargo.Where
   (x => x.Durum == "Yolda" || x.Durum == "Gönderime Hazır")
   .OrderByDescending(x => x.Id).ToList<Kargo>();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(KargoModel p)
        {
            if (ModelState.IsValid == true)
            {

                for (int i = 0; i < p.Kargos.Count(); i++)
                {
                    if (p.Kargos[i].DurumCheck == true)
                    {
                        var list = db.Kargo.Find(p.Kargos[i].Id);
                        list.Durum = "Teslim Alındı";
                        p.Kargos[i].DurumCheck = false;

                        db.SaveChanges();
                        TempData["MessageSuccess"] = "TOPLU TESLIM ALIM YAPILDI";
                    }
                }
            }
            else
            {
                TempData["Message"] = "KARGOLAR TESLIM ALINMADI..!";
            }
            return RedirectToAction("Index");
        }
    }
}