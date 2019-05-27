using System.Collections.Generic;
using System.Web.Mvc;
using Ecommerce.Models;
using Ecommerce.Models.DB;
using Ecommerce.Utils;

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
            if (a.IDcategoria == 0)
            {
                a = new Filtri();

                ViewData.Add("migliori_corsi", Components.DataLayer.Ricerca(testo));
               


            }
            else
                ViewData.Add("migliori_corsi", Components.DataLayer.RicercaConFiltri(a.IDcategoria, a.prezzoInizio, a.prezzoFine, testo));
           
            LoadCategoriesFiltri(a);
            ViewData.Add("testo", testo);

            return View(a);
        }
        
        private void LoadCategoriesFiltri(Filtri model)
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
    }
}