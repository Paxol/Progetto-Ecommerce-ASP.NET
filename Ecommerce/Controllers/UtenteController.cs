using Ecommerce.Attributes;
using Ecommerce.Models;
using Ecommerce.Models.DB;
using Ecommerce.Utils;
using System;
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

        [SetPermissions(Permissions = "Admin,Utente")]
        public ActionResult Ordina()
        {
            Models.Ordine ordine = new Models.Ordine();

            var dal = Components.DataLayer;

            var carrello = dal.GetCarrello(SessionContext.GetUserID());

            if (carrello.Count == 0)
                return RedirectToAction("Carrello");

            ViewData.Add("Items", carrello);

            ordine.Carta = dal.GetCartaCredito(SessionContext.GetUserID());
            if (ordine.Carta != null) ordine.SalvaCarta = true;

            ordine.NumOrig = Convert.ToBase64String(Crypto.Encrypt(ordine.Carta.Numero));
            ordine.Carta.Numero = ordine.Carta.NumeroAst;

            return View(ordine);
        }

        [SetPermissions(Permissions = "Admin,Utente")]
        [HttpPost]
        public ActionResult Ordina(Models.Ordine ordine)
        {
            try
            {
                if (ordine.Carta.Numero == ordine.Carta.NumeroAst)
                {
                    ordine.Carta.Numero = Crypto.Decrypt(Convert.FromBase64String(ordine.NumOrig));
                }

                var dal = Components.DataLayer;
                int a = CartaCredito.IsValid(ordine.Carta);
                if (a == 0)
                {
                    if (ordine.SalvaCarta) dal.InsertCartaCredito(ordine.Carta, SessionContext.GetUserID());

                    var carrello = dal.GetCarrello(SessionContext.GetUserID());
                    int b = dal.CreaOrdine(SessionContext.GetUserID());
                    if (b == 0)
                    {
                        ViewData.Add("Items", carrello);
                        return View(new Models.Ordine { StatoOrdine = Models.Ordine.Stato.Validato });
                    }
                    else
                    {
                        return View(new Models.Ordine { Errore = "Errore del server, riprova più tardi. Codice errore: " + b, StatoOrdine = Models.Ordine.Stato.Errore });
                    }
                }
                else
                {
                    switch (a)
                    {
                        case -1:
                            ordine.Errore = "Numero errato";
                            break;
                        case -2:
                            ordine.Errore = "Proprietario non valido";
                            break;
                        case -3:
                            ordine.Errore = "Scadenza errata";
                            break;
                        case -4:
                            ordine.Errore = "CVV errato";
                            break;
                        default:
                            break;
                    }

                    ordine.NumOrig = Convert.ToBase64String(Crypto.Encrypt(ordine.Carta.Numero));
                    ordine.Carta.Numero = ordine.Carta.NumeroAst;
                    return View(ordine);
                }
            }
            catch (Exception)
            {
                return View(new Models.Ordine { Errore = "Errore del server, riprova più tardi", StatoOrdine = Models.Ordine.Stato.Errore });
            }
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
            var ordini = Components.DataLayer.GetOrdini(SessionContext.GetUserID());
            ViewData.Add("Ordini", ordini);
            return View();
        }

        [SetPermissions(Permissions = "Admin,Utente")]
        public ActionResult InfoPagamento()
        {
            var carta = Components.DataLayer.GetCartaCredito(SessionContext.GetUserID());
            if (carta == null) carta = new CartaCredito();
            return View(carta);
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

            Components.DataLayer.InsertCartaCredito(carta, SessionContext.GetUserID());

            return RedirectToAction("InfoPagamento");
        }

        [SetPermissions(Permissions = "Admin,Utente")]
        public ActionResult AggiungiCarrello(int id)
        {
            Components.DataLayer.AggiungiCarrello(id, SessionContext.GetUserID());

            return View(Components.DataLayer.GetCorsoByID(id));
        }

        [HttpPost]
        [SetPermissions(Permissions = "Admin,Utente")]
        public ActionResult AggRecensione(FormCollection form)
        {
            var idr = form["idr"];
            int idc = int.Parse(form["idc"]);
            string recensione = form["recensione"];
            int valutazione = int.Parse(form["valutazione"]);

            if (idr == null)
                Components.DataLayer.InsertRecensione(SessionContext.GetUserID(), idc, recensione, valutazione);
            else
                Components.DataLayer.UpdateRecensione(idr, recensione, valutazione);
            
            return Redirect("/Corsi/" + idc);
        }
    }
}