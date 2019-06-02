using System.Collections.Generic;
using System.Web.Mvc;

namespace Ecommerce.Models
{
    public class Filtri
    {
        public int IDcategoria { get; set; }
        public string prezzoInizio { get; set; }    //problemi di conversione decimal => string
        public string prezzoFine { get; set; }      //problemi di conversione decimal => string
        public List<SelectListItem> Categorie { get; set; } = new List<SelectListItem>();
    }
}