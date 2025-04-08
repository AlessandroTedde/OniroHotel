using System;
using System.Web.Mvc;

namespace OniroHotel.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveDates(DateTime inDate, DateTime outDate)
        {

            System.Diagnostics.Debug.WriteLine("inDate: " + inDate);
            System.Diagnostics.Debug.WriteLine("outDate: " + outDate);
            Session["InDate"] = inDate;
            Session["OutDate"] = outDate;

            return RedirectToAction("Index", "Rooms");
         }
    }
}