using System.Collections.Generic;
using System.Web.Mvc;
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