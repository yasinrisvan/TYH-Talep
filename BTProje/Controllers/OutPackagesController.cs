using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTProje.Models.EntityFramework;

namespace BTProje.Controllers
{

    public class OutPackagesController : Controller
    {
        TYHTALEPEntities db = new TYHTALEPEntities();
        // GET: OutPackages
        public ActionResult Index()
        {
            var packages = db.FollowPackages.ToList();

            return View(packages);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var packages = new FollowPackages()
                {
                    Date = DateTime.Now,
                    CompanyInformation = form["CompanyInformation"].ToString().ToUpper(),
                    ShippingCompany = form["ShippingCompany"].ToString().ToUpper(),
                    BuyerCompany = form["BuyerCompany"].ToString().ToUpper(),
                    PickUperPersonNameSurname = form["PickUperPersonNameSurname"].ToString().ToUpper(),


                };
                db.FollowPackages.Add(packages);
                db.SaveChanges();
                TempData["MessageSuccess"] = "KISISEL KARGO KAYDI OLUŞTURULDU ";
                //return View();

                return RedirectToRoute(new
                {
                    controller = "OutPackages",
                    action = "Index"
                });
            }
            else
            {
                TempData["Message"] = "KISISEL KARGO KAYDI OLUSTURULAMADI..!";
                return RedirectToAction("Index");
            }
        }
        public ActionResult Update(int id)
        {
            var packages = db.FollowPackages.Find(id);

            return View("Update", packages);
        }
        [HttpPost]
        public ActionResult Update(FormCollection form)
        {
            var id = Convert.ToInt32(form["Id"]);
            if (id == 0 || id == null)
            {
                TempData["Message"] = "KISISEL KARGO KAYDI GUNCELLENEMEDI..!";
                return RedirectToAction("Index");
            }
            else
            {
                var packages = db.FollowPackages.Find(id);
                if (packages == null)
                {
                    TempData["Message"] = "KISISEL KARGO KAYDI GUNCELLENEMEDI..!";
                    return RedirectToAction("Index");
                }
                else
                {
                    packages.CompanyInformation = form["CompanyInformation"].ToUpper();
                    packages.ShippingCompany = form["ShippingCompany"].ToUpper();
                    packages.BuyerCompany = form["BuyerCompany"].ToUpper();
                    packages.PickUperPersonNameSurname = form["PickUperPersonNameSurname"].ToUpper();
                    db.SaveChanges();
                    TempData["MessageSuccess"] = "KISISEL KARGO KAYDI GUNCELLENDI ";
                    //return View();

                    return RedirectToRoute(new
                    {
                        controller = "OutPackages",
                        action = "Index"
                    });
                }
            }

        }

        public ActionResult Delete(int id)
        {
            var package = db.FollowPackages.Find(id);
            if (package == null)
            {
                TempData["Message"] = "KISISEL KARGO KAYDI SILINEMEDI..!";
                return RedirectToAction("Index");
            }
            else
            {
                package.Situation = false;
                db.SaveChanges();
                TempData["MessageSuccess"] = "KISISEL KARGO KAYDI SILINDI ";
                return RedirectToRoute(new
                {
                    controller = "OutPackages",
                    action = "Index"
                });
            }
        }
    }
}