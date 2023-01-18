using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTProje.Controllers
{
    public class HomePageController : Controller
    {
        // GET: Deneme

        [Authorize]

        public ActionResult Index()
        {
            return View();
        }
    }
}