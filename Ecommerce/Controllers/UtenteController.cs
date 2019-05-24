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

        public ActionResult Carrello()
        {
            ViewData["carrello"] = new List<ItemCarrello>() {
                new ItemCarrello
                {
                    ID = 1,
                    Prezzo = 50,
                    Quantita = 2,
                    Corso = new Corso()
                    {
                        Titolo = "Corso 1",
                        Descrizione = "Descrizione 1",
                        Immagine = "culo.jpg"
                    }
                },
                new ItemCarrello
                {
                    ID = 2,
                    Prezzo = 80.99m,
                    Quantita = 1,
                    Corso = new Corso()
                    {
                        Titolo = "Corso 2",
                        Descrizione = "Descrizione 2",
                        Immagine = "culo.jpg"
                    }
                },
            };

            return View();
        }

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

        public ActionResult Ordini()
        {
            return View();
        }

        public ActionResult InfoPagamento()
        {
            return View();
        }
    }
}