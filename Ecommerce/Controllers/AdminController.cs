using Ecommerce.Attributes;
using Ecommerce.Utils;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    public class AdminController : Controller
    {
        [SetPermissions(Permissions = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [SetPermissions(Permissions = "Admin")]
        public ActionResult AggCorso()
        {
            return View();
        }
    }
}