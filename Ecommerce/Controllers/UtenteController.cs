using Ecommerce.Attributes;
using Ecommerce.Models.DB;
using Ecommerce.Utils;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    public class UtenteController : Controller
    {
        [SetPermissions(Permissions = "Admin,Utente")]
        public ActionResult Index()
        {
            ViewBag.User = SessionContext.GetUserData().Name;

            return View();
        }

        [SetPermissions(Permissions = "Admin,Utente")]
        public ActionResult Carrello(int limit = 5, int page = 1)
        {
            ViewData["carrello"] = Components.DataLayer.GetCarrello(SessionContext.GetUserData().UserID); 

            return View();
        }

        [SetPermissions(Permissions = "Admin,Utente")]
        [HttpPost]
        public ActionResult CartUpdate(FormCollection form)
        {
            int cartid = int.Parse(form["cartid"]);
            int quantita = int.Parse(form["quantita"]);

            if (quantita > 0)
            {
                //update quantita
            } else
            {
                //remove item
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [SetPermissions(Permissions = "Admin,Utente")]
        public ActionResult Ordini()
        {
            return View();
        }

        [SetPermissions(Permissions = "Admin,Utente")]
        public ActionResult InfoPagamento()
        {
            return View();
        }
    }
}