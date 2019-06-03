using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models.DB
{
    public class Ordine
    {
        public int ID { get; set; }
        public string Stato { get; set; }
        public DateTime Data { get; set; }
        public List<ItemCarrello> Items { get; set; } = new List<ItemCarrello>();
        public int Prodotti { get; set; }
    }
}