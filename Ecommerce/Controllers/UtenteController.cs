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

            int a = Components.DataLayer.AggiornaQuantitaCarrello(cartid, quantita);

            return new HttpStatusCodeResult(a > 0 ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
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

        [HttpPost]
        [SetPermissions(Permissions = "Admin,Utente")]
        public ActionResult SalvaInfoPagamento(FormCollection form)
        {
            var carta = new CartaCredito
            {
                Numero = form["cc_num"],
                Scadenza = form["cc_scadenza"],
                Proprietario = form["cc_proprietario"],
                CVV = form["cc_cvv"],
            };
            
            return RedirectToAction("InfoPagamento");
        }
    }
}