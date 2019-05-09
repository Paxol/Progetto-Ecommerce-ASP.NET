using Ecommerce.Models;
using System.Web.Mvc;

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
            if (login.UserName == "mirko" && login.Password == "pass") // Verifica login
            {
                return Redirect(Url.Action("Index", "Utente"));
            }
            else
            {
                ViewBag.NotValidUser = "Nome utente o password errati";
                return View();
            }
        }

        public ActionResult ResetPassword()
        {
            return View();
        }
    }
}