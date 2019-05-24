using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models.DB
{
    public class ItemCarrello
    {
        public int ID { get; set; }
        public int Quantita { get; set; }
        public decimal Prezzo { get; set; }
        public Corso Corso { get; set; }
    }
}