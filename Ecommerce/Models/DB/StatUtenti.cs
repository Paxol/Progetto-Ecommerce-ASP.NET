using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models.DB
{
    public class StatUtenti
    {
        public int IDUtente { get; set; }
        public string Email { get; set; }
        public int ProdottiComprati { get; set; }
    }
}