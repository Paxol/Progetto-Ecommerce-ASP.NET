using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models.DB
{
    public class StatCorso
    {
        public int IDCorso { get; set; }
        public string Titolo { get; set; }
        public decimal Prezzo { get; set; }
        public int Vendite { get; set; }
    }
}