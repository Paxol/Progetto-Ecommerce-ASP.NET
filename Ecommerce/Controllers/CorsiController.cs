using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    public class CorsiController : Controller
    {
        public ActionResult View(int id)
        {
            ViewData.Add("ID", id);
            return View();
        }
    }
}