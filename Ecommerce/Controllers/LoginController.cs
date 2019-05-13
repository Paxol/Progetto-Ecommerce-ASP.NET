using Ecommerce.Interfaces;
using Ecommerce.Models;
using Ecommerce.Models.DB;
using Ecommerce.Utils;
using System.Web.Mvc;
using System.Web.Security;

namespace Ecommerce.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Login login)
        {
            IDataLayer dataLayer = Components.DataLayer;

            var authUser = dataLayer.GetUserByEmailAndPassword(login.UserName, login.Password);
            if (authUser != null)
            {
                SessionContext.SetAuthenticationToken(login.Ricorda, authUser);
                var returnUrl = Request.Params["ReturnUrl"];
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Utente");
                }
            }
            else
            {
                ViewBag.NotValidUser = "Email o password errati";
                return View();
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(Register register)
        {
            IDataLayer dataLayer = Components.DataLayer;

            if (register.IsPasswordValid)
            {
                //registra l'utente
                var result = dataLayer.RegisterUser(new User
                {
                    Email = register.Email,
                    Name = register.Nome,
                    Password = register.Password
                });

                switch (result)
                {
                    case -10:   // Email già registrata
                        ViewBag.NotValid = "Email già registrata";
                        break;
                    case 0:     // Inserimento corretto
                        return RedirectToAction("Index");
                    default:
                        ViewBag.NotValid = "Si è verificato un'errore, riprova più tardi";
                        break;
                }
            }
            else
            {
                ViewBag.NotValid = "Le password non corrispondono";
            }

            return View();
        }
    }
}