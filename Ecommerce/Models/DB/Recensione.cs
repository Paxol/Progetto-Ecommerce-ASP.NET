using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models.DB
{
    public class Recensione
    {
        public int IDCorso { get; set; }
        public string NomeUtente { get; set; }
        public string descrizione { get; set; }
        public DateTime data { get; set; }
        public int Valutazione { get; set; }


    }
}