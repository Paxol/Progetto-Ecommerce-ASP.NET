using System.Collections.Generic;
using System.Web.Mvc;
using Ecommerce.Models;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData.Add("migliori_corsi", new List<Corso>()
            {
                new Corso
                {
                    ID = 1,
                    Titolo = "Diventare Web Designer",
                    NomeAutore = "Simona Tocci",
                    Immagine = "https://i.udemycdn.com/course/240x135/1237748_f821.jpg",
                    Valutazione = 4.3f,
                    Prezzo = 149.99m
                },
                new Corso
                {
                    ID = 2,
                    Titolo = "BootCamp Python",
                    NomeAutore = "Jose Portilla",
                    Immagine = "https://i.udemycdn.com/course/240x135/567828_67d0.jpg",
                    Valutazione = 4.5f,
                    Prezzo = 149.99m
                },
                new Corso
                {
                    ID = 3,
                    Titolo = "Bootcamp sviluppatori web",
                    NomeAutore = "Colt Steele",
                    Immagine = "https://i.udemycdn.com/course/240x135/625204_436a_2.jpg",
                    Valutazione = 3.3f,
                    Prezzo = 199.99m
                },
                new Corso
                {
                    ID = 4,
                    Titolo = "Diventare Web Designer",
                    NomeAutore = "Simona Tocci",
                    Immagine = "https://i.udemycdn.com/course/240x135/1237748_f821.jpg",
                    Valutazione = 4.3f,
                    Prezzo = 149.99m
                },
                new Corso
                {
                    ID = 5,
                    Titolo = "BootCamp Python",
                    NomeAutore = "Jose Portilla",
                    Immagine = "https://i.udemycdn.com/course/240x135/567828_67d0.jpg",
                    Valutazione = 4.5f,
                    Prezzo = 149.99m
                },
                new Corso
                {
                    ID = 6,
                    Titolo = "Bootcamp sviluppatori web",
                    NomeAutore = "Colt Steele",
                    Immagine = "https://i.udemycdn.com/course/240x135/625204_436a_2.jpg",
                    Valutazione = 3.3f,
                    Prezzo = 199.99m
                },
            });
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Search(string id)
        {
            return View();
        }
    }
}