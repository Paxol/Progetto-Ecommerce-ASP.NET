using System.Collections.Generic;
using System.Web.Mvc;

namespace Ecommerce.Models
{
    public class Filtri
    {
        public int IDcategoria { get; set; }
        public decimal prezzoInizio { get; set; }
        public decimal prezzoFine { get; set; }
        public List<SelectListItem> Categorie { get; set; } = new List<SelectListItem>();
    }
}