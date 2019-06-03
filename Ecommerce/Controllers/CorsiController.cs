using Ecommerce.Utils;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    public class CorsiController : Controller
    {
        public ActionResult View(int id)
        {
            //   ViewData.Add("ID", id);
            ViewData.Add("ID", Components.DataLayer.GetCorsoByID(id));
            ViewData.Add("recensioni", Components.DataLayer.GetRecensioni(id, SessionContext.GetUserID()));
            ViewData.Add("recensione", Components.DataLayer.GetRecensioneUtente(id, SessionContext.GetUserID()));

            return View();

            //    ViewData.Add("migliori_corsi", Components.DataLayer.GetCorsoByID(id));
        }

    }
}