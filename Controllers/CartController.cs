using OniroHotel.Models;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace OniroHotel.Controllers
{
    public class CartController : Controller
    {
        private ModelDBContext db = new ModelDBContext();
        // GET: Cart
        public ActionResult Cart()
        {
            if (Session["LogedUserID"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var userId = (int)Session["LogedUserID"];
            var user = db.Users.Find(userId);

            var cart = Session["Cart"] as CartViewModel ?? new CartViewModel();
            cart.UserName = user.FullName;
            cart.UserEmail = user.Email;
            cart.UserPhone = user.Telephone;

            cart.Extras = db.Extras.ToList();
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRoom(int id)
        {
            var room = db.Rooms.Find(id);
            if (room != null)
            {
                var cart = Session["Cart"] as CartViewModel ?? new CartViewModel();
                cart.Room = room;
                Session["Cart"] = cart;
            }
            return RedirectToAction("Cart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddExtra(int id)
        {
            var extra = db.Extras.Find(id);
            if (extra != null)
            {
                var cart = Session["Cart"] as CartViewModel ?? new CartViewModel();
                if (cart.Extras == null)
                {
                    cart.Extras = new List<Extras>();
                }
                cart.Extras.Add(extra);
                Session["Cart"] = cart;
            }
            return RedirectToAction("Cart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveExtra(int id)
        {
            var extra = db.Extras.Find(id);
            if (extra != null)
            {
                var cart = Session["Cart"] as CartViewModel;
                if (cart != null && cart.Extras != null)
                {
                    cart.Extras.Remove(extra);
                    Session["Cart"] = cart;
                }
            }
            return RedirectToAction("Cart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(DateTime inDate, DateTime outDate, string treatment, int guests)
        {
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

            var cart = Session["Cart"] as CartViewModel;
            if (cart != null && cart.Room != null)
            {
                int nights = (outDate - inDate).Days;

                // Calcola il costo totale della stanza
                int roomTotal = cart.Room.RoomPrice * nights;

                // Calcola il costo totale degli extra solo se ci sono extra nel carrello
                int extrasTotal = 0;
                if (cart.Extras != null && cart.Extras.Any())
                {
                    extrasTotal = cart.Extras.Sum(e => e.ExtraPrice);
                }

                int treatmentPrice = 0;
                switch (treatment)
                {
                    case "BB":
                        treatmentPrice = 30;
                        break;
                    case "HB":
                        treatmentPrice = 55;
                        break;
                    case "FB":
                        treatmentPrice = 75;
                        break;
                }
                // Calcola il costo totale del trattamento
                int treatmentTotal = treatmentPrice * guests * nights;

                // Calcola il costo totale
                int total = roomTotal + extrasTotal + treatmentTotal;

                var userId = (int)Session["LogedUserID"];
                var user = db.Users.Find(userId);

                // Applica lo sconto in base al campo Fidelity dell'utente
                if (user.Fidelity == 1)
                {
                    total = total * 90 / 100; // Sconto del 10%
                }
                else if (user.Fidelity == 2)
                {
                    total = total * 85 / 100; // Sconto del 15%
                }
                else if (user.Fidelity == 3)
                {
                    total = total * 75 / 100; // Sconto del 25%
                }

                var reservation = new Reservations
                {
                    // Calcola il numero di notti
                    UserID = (int)Session["LogedUserID"],
                    InDate = inDate,
                    OutDate = outDate,
                    Treatment = treatment,
                    Total = total,
                    Guests = guests
                };

                // Associa la stanza alla prenotazione
                var reservationDetail = new ReservationDetails
                {
                    RoomID = cart.Room.RoomID,
                    ResID = reservation.ResID
                };
                db.ReservationDetails.Add(reservationDetail);

                // Associa gli extra alla prenotazione
                if (cart.Extras != null && cart.Extras.Any())
                {
                    foreach (var extra in cart.Extras)
                    {
                        var reservationDetailExtra = new ReservationDetails
                        {
                            ExtraID = extra.ExtraID,
                            ResID = reservation.ResID
                        };
                        db.ReservationDetails.Add(reservationDetailExtra);
                    }
                }

                string stripePublishableKey = ConfigurationManager.AppSettings["Stripe:PublishableKey"];
                string stripeSecretKey = ConfigurationManager.AppSettings["Stripe:SecretKey"];

                StripeConfiguration.ApiKey = stripeSecretKey;
                var domain = "https://localhost:44330/";
                var Options = new SessionCreateOptions
                {
                    SuccessUrl = domain + "Home/Index",
                    CancelUrl = domain + "Home/Index",
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    CustomerEmail = "alessandrotedde98@gmail.com"
                };

                string description = cart.Room.RoomName;
                if (cart.Extras != null && cart.Extras.Count > 0)
                {
                    description += " + " + string.Join(", ", cart.Extras.Select(e => e.ExtraName));
                }
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)total * 100,
                        Currency = "eur",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = cart.Room.RoomName,
                            Description = description,
                            Images = new List<string> { cart.Room.RoomImg }
                        }
                    },
                    Quantity = 1
                };

                Options.LineItems.Add(sessionLineItem);

                // creo la sessione di Stripe e la invio al client
                var service = new SessionService();
                Session session = service.Create(Options);
                Response.Redirect(session.Url);

                db.Reservations.Add(reservation);
                db.SaveChanges();
                // svuoto il carrello
                Session.Remove("Cart");

                // reindirizzo alla pagina di successo
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
