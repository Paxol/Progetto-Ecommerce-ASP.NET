using Ecommerce.Attributes;
using Ecommerce.Models;
using Ecommerce.Models.DB;
using Ecommerce.Utils;
using System.Web.Mvc;
using System.Collections.Generic;
using System.IO;
using System;

namespace Ecommerce.Controllers
{
    public class AdminController : Controller
    {
        [SetPermissions(Permissions = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        private void LoadCategories(ModCorso model)
        {
            var cat = Components.DataLayer.GetCategories();
            foreach (var item in cat)
            {
                model.Categorie.Add(new SelectListItem
                {
                    Value = item.ID.ToString(),
                    Text = item.Nome
                });
            }
        }

        [SetPermissions(Permissions = "Admin")]
        public ActionResult EditCorso(int id = 0)
        {
            ViewBag.Title = "Aggiungi corso";
            ModCorso model = null;
            Corso corso = null;

            if (id != 0)
            {
                var dal = Components.DataLayer;
                corso = dal.GetCorsoByID(id);
            }

            if (corso != null)
            {
                ViewBag.Title = "Modifica corso";
                model = new ModCorso
                {
                    IsModifica = true,
                    Autore = corso.Autore,
                    Descrizione = corso.Descrizione,
                    IDCategoria = corso.Categoria.ID,
                    Prezzo = corso.Prezzo.ToString("#.##"),
                    Titolo = corso.Titolo,
                    Immagine = corso.Immagine
                };
            }
            else
            {
                if (id != 0)
                {
                    ViewBag.NotValid = true;
                    ViewBag.Title = "Modifica corso";
                }

                model = new ModCorso() { IsModifica = false };
            }

            model.Categorie = new List<SelectListItem>();
            LoadCategories(model);

            return View(model);
        }

        [SetPermissions(Permissions = "Admin")]
        [HttpPost]
        public ActionResult UploadCorso(ModCorso corso)
        {
            ViewBag.Message = "Caricamento fallito, tra 3 secondi verrai rimandato alla pagina precedente";

            if (corso.File != null && corso.File.ContentLength > 0)
            {
                var s = corso.File.FileName.Split('.');
                string a = "";

                for (int i = 0; i < s.Length - 1; i++)
                    a += s[i];
                a += DateTimeOffset.UtcNow.ToUnixTimeSeconds() + "." + s[s.Length - 1];

                var fileName = Path.GetFileName(a);
                var path = Path.Combine(Server.MapPath("~/Content/img"), fileName);
                corso.File.SaveAs(path);

                var dal = Components.DataLayer;
                int val = dal.InsertCorso(new Corso
                {
                    Autore = corso.Autore,
                    Categoria = new Categoria { ID = corso.IDCategoria },
                    Descrizione = corso.Descrizione,
                    Immagine = fileName,
                    Prezzo = Convert.ToDecimal(corso.Prezzo.Replace('.', ',')),
                    Titolo = corso.Titolo
                });

                if (val == 0)
                    ViewBag.Message = "Caricamento completato, tra 3 secondi verrai rimandato alla pagina precedente";
                else
                    ViewBag.Message = "Errore " + val + ", tra 3 secondi verrai rimandato alla pagina precedente";
            }


            return View();
        }

        [SetPermissions(Permissions ="Admin")]
        public ActionResult Statistiche(Statistiche statistiche)
        {
            if (statistiche.CorsiPiuVenduti.Limit == 0 || statistiche.CorsiPiuVenduti.Page == 0)
                statistiche.CorsiPiuVenduti = new Statistiche.Pagination { Limit = Math.Max(1, statistiche.CorsiPiuVenduti.Limit), Page = Math.Max(1, statistiche.CorsiPiuVenduti.Page) };

            ViewData["corsi_piu_venduti"] = Components.DataLayer.GetProdottiPiuVenduti(statistiche.CorsiPiuVenduti.Limit, statistiche.CorsiPiuVenduti.Page, out int tot);

            statistiche.CorsiPiuVenduti = new Statistiche.Pagination { Limit = statistiche.CorsiPiuVenduti.Limit, Page = statistiche.CorsiPiuVenduti.Page, Total = tot };

            return View(statistiche);
        }
    }
}