using Ecommerce.Attributes;
using Ecommerce.Models;
using Ecommerce.Models.DB;
using Ecommerce.Utils;
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
            ViewData["carrello"] = Components.DataLayer.GetCarrello(SessionContext.GetUserID()); 

            return View();
        }

        public ActionResult Ordina()
        {
            var dal = Components.DataLayer;

            var carrello = dal.GetCarrello(SessionContext.GetUserID());
            Ordine ordine = new Ordine { items = carrello };

            return View(ordine);
        }

        public ActionResult DoOrdina(Ordine ordine)
        {
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

        [SetPermissions(Permissions = "Admin,Utente")]
        public ActionResult AggiungiCarrello(int id)
        {
            Components.DataLayer.AggiungiCarrello(id, SessionContext.GetUserID());

            return View(Components.DataLayer.GetCorsoByID(id));
        }
    }
}