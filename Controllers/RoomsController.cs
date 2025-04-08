using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using OniroHotel.Models;

namespace OniroHotel.Controllers
{
    [Authorize]
    public class RoomsController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        // GET: Rooms
        [AllowAnonymous]
        public ActionResult Index()
        {
            DateTime inDate;
            DateTime outDate;

            if (Session["InDate"] != null && Session["OutDate"] != null)
            {
                inDate = (DateTime)Session["InDate"];
                outDate = (DateTime)Session["OutDate"];
            }
            else if (Session["inDate"] != null && Session["outDate"] != null)
            {
                inDate = (DateTime)Session["inDate"];
                outDate = (DateTime)Session["outDate"];
            }
            else
            {
                inDate = DateTime.Now;
                outDate = DateTime.Now.AddDays(1);
                Session["InDate"] = inDate;
                Session["OutDate"] = outDate;
            }

            ViewBag.inDate = inDate;
            ViewBag.outDate = outDate;

            // Recupera le camere disponibili e passale alla View
            var availableRooms = db.Rooms.Where(room => room.IsAvailable);

            return View(availableRooms);
        }
    

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rooms rooms = db.Rooms.Find(id);
            if (rooms == null)
            {
                return HttpNotFound();
            }
            return View(rooms);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rooms/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding.
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "RoomID,RoomNumber,RoomName,RoomImg,RoomDesc,RoomPrice,IsAvailable")]
                Rooms rooms
        )
        {
            if (ModelState.IsValid)
            {
                db.Rooms.Add(rooms);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rooms);
        }

        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rooms rooms = db.Rooms.Find(id);
            if (rooms == null)
            {
                return HttpNotFound();
            }
            return View(rooms);
        }

        // POST: Rooms/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding.
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "RoomID,RoomNumber,RoomName,RoomImg,RoomDesc,RoomPrice,IsAvailable")]
                Rooms rooms
        )
        {
            if (ModelState.IsValid)
            {
                db.Entry(rooms).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rooms);
        }

        // GET: Rooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rooms rooms = db.Rooms.Find(id);
            if (rooms == null)
            {
                return HttpNotFound();
            }
            return View(rooms);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rooms rooms = db.Rooms.Find(id);
            if (rooms == null)
            {
                return HttpNotFound();
            }
            db.Rooms.Remove(rooms);
            db.SaveChanges();
            return RedirectToAction("GetRooms");
        }

        public ActionResult GetRooms()
        {
            var rooms = db.Rooms.ToList();
            return View(rooms);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult AvailableDateCheck(DateTime checkInDate, DateTime checkOutDate)
        {
            /*controlla se le camere sono disponibili*/
            var camereDisponibili = db.Rooms
               .GroupBy(room => room.RoomName)
               .Where(group =>
                   group.All(room =>
                       room.IsAvailable
                       && !db.ReservationDetails.Any(rd =>
                           rd.RoomID == room.RoomID
                           && (
                               (
                                   checkInDate >= rd.Reservations.InDate
                                   && checkInDate <= rd.Reservations.OutDate
                               )
                               || (
                                   checkOutDate >= rd.Reservations.InDate
                                   && checkOutDate <= rd.Reservations.OutDate
                               )
                           )
                       )
                   )
               )
               .SelectMany(group => group);
            /*controlla se esiste una prenotazione che si sovrappone alle date scelte*/
            var sovrapposizione = db.Rooms.All(room =>
                db.ReservationDetails.Any(rd =>
                    rd.RoomID == room.RoomID
                    && (
                        (
                            checkInDate >= rd.Reservations.InDate
                            && checkInDate <= rd.Reservations.OutDate
                        )
                        || (
                            checkOutDate >= rd.Reservations.InDate
                            && checkOutDate <= rd.Reservations.OutDate
                        )
                    )
                )
            );
            /*fa un confronto tra le date scelte e le date di prenotazione*/
            if (sovrapposizione)
            {
                TempData["Error"] = "Le date selezionate sono già state prenotate, prova a cambiare il periodo di prenotazione.";
            }
            else
            {
                TempData["AvailableRooms"] = camereDisponibili;
                Session["inDate"] = checkInDate;
                Session["outDate"] = checkOutDate;
            }

            return RedirectToAction("Index");
        }
    }
}
