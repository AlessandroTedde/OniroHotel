using OniroHotel.Models;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Diagnostics;

namespace OniroHotel.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users u)
        {
            /*se email e password corrispondono a quelli nel database, reindirizza alla pagina home*/
                   using (ModelDBContext db = new ModelDBContext())
                {
                    var match = db.Users.Where(a => a.Email.Equals(u.Email) && a.Password.Equals(u.Password)).FirstOrDefault();
                    if (match != null)
                    {
                        u.Username = u.FullName;
                        Session["LogedUserID"] = match.UserID;
                        Session["LogedUserFullname"] = u.Username.ToString();
                        Session["UserRole"] = match.UserRole;
                    FormsAuthentication.SetAuthCookie(u.Email, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["Error"] = "Email o password errati.";
                        return RedirectToAction("Login", "Login");
                    }
                }
            
                //using (ModelDBContext db = new ModelDBContext())
                //{
                //    var match = db.Users.Where(a => a.Username.Equals(u.Username) && a.Password.Equals(u.Password)).FirstOrDefault();
                //    if (match != null)
                //    {
                //        Session["LogedUserID"] = match.UserID;
                //        Session["LogedUserFullname"] = match.Username.ToString();
                //    FormsAuthentication.SetAuthCookie(u.Username, false);
                //    return RedirectToAction("Index", "Home");
                //    }
                //    else
                //    {
                //        TempData["Error"] = "Username o password errati";
                //        return RedirectToAction("Login", "Login");
                //    }
                //}
        }

        public ActionResult Logout()
        {
            /*elimina la sessione e reindirizza alla home*/
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            Users u = new Users();
            u.UserRole = "HotelGuest";
            u.Fidelity = 0;
            return View(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Users u)
        {
            if (ModelState.IsValid)
            {
                using (ModelDBContext db = new ModelDBContext())
                {
                    var match = db.Users.Where(a => a.Email.Equals(u.Email)).FirstOrDefault();
                    if (match == null)
                    {
                        u.Username = u.FullName;
                        db.Users.Add(u);
                        Debug.WriteLine("UserRole: " + u.UserRole);
                        Debug.WriteLine("Fidelity: " + u.Fidelity);
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbEntityValidationException e)
                        {
                            Debug.WriteLine(e);
                        }

                        TempData["Success"] = "Registrazione avvenuta con successo";
                        return RedirectToAction("Login", "Login");
                    }
                    else
                    {
                        TempData["Error"] = "Utente già registrato";
                        return RedirectToAction("Register", "Login");
                    }
                }
            }
            else
            {
                return View(u);
            }
        }
    }
}