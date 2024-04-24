using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OniroHotel.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        public ActionResult Restaurant()
        {
            return View();
        }
        public ActionResult Spa()
        {
            return View();
        }

        public ActionResult Contacts()
        {
            return View();
        }
    }
}