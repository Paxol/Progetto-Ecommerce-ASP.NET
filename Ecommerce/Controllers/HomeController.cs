using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Ecommerce.Models;
using Ecommerce.Models.DB;
using Ecommerce.Utils;
using System;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData.Add("migliori_corsi", Components.DataLayer.GetMiglioriCorsi());
            return View();
        }

        public ActionResult Cerca(string testo = "", Filtri a = null)
        {
            List<Corso> corsi;

            if (a.IDcategoria == 0)
            {
                a = new Filtri();
                corsi = Components.DataLayer.Ricerca(testo);
            }
            else
                corsi = Components.DataLayer.RicercaConFiltri(a.IDcategoria, decimal.Parse(a.prezzoInizio), decimal.Parse(a.prezzoFine), testo);

            ViewData.Add("migliori_corsi", corsi);
            a.prezzoInizio = ((int)Math.Floor(corsi.Min((c) => c.Prezzo))).ToString();
            a.prezzoFine = ((int)Math.Ceiling(corsi.Max((c) => c.Prezzo))).ToString();

            LoadCategoriesFiltri(corsi, a);
            ViewData.Add("testo", testo);

            return View(a);
        }

        private void LoadCategoriesFiltri(List<Corso> corsi, Filtri model)
        {
            var cat = Components.DataLayer.GetCategories();
            foreach (var item in cat)
            {
                if (corsi.Count((c) => c.ID == item.ID) < 1)
                    continue;

                model.Categorie.Add(new SelectListItem
                {
                    Value = item.ID.ToString(),
                    Text = item.Nome
                });
            }
        }
    }
}