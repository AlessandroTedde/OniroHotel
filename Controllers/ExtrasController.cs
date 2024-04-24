using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OniroHotel.Models;

namespace OniroHotel.Controllers
{
    [Authorize]
    public class ExtrasController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        // GET: Extras
        public ActionResult Index()
        {
            return View(db.Extras.ToList());
        }

        // GET: Extras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Extras extras = db.Extras.Find(id);
            if (extras == null)
            {
                return HttpNotFound();
            }
            return View(extras);
        }

        // GET: Extras/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Extras/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExtraID,ExtraName,ExtraImg,ExtraPrice")] Extras extras)
        {
            if (ModelState.IsValid)
            {
                db.Extras.Add(extras);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(extras);
        }

        // GET: Extras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Extras extras = db.Extras.Find(id);
            if (extras == null)
            {
                return HttpNotFound();
            }
            return View(extras);
        }

        // POST: Extras/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExtraID,ExtraName,ExtraImg,ExtraPrice")] Extras extras)
        {
            if (ModelState.IsValid)
            {
                db.Entry(extras).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(extras);
        }

        // GET: Extras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Extras extras = db.Extras.Find(id);
            if (extras == null)
            {
                return HttpNotFound();
            }
            return View(extras);
        }

        // POST: Extras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Extras extras = db.Extras.Find(id);
            db.Extras.Remove(extras);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
