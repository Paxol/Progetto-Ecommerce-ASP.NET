using Ecommerce.Interfaces;
using Ecommerce.Models;
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

        public ActionResult ResetPassword()
        {
            return View();
        }
    }
}