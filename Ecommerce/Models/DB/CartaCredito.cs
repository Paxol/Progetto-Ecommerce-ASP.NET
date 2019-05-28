using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models.DB
{
    public class CartaCredito
    {
        public string Numero { get; set; }
        public string Scadenza { get; set; }
        public string Proprietario { get; set; }
        public string CVV { get; set; }
    }
}