using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class AllOrdini
    {
        public List<DB.Ordine> Ordini { get; set; } = new List<DB.Ordine>();
        public int Limit { get; set; } = 25;
        public int Page { get; set; } = 1;
        public int Total { get; set; }
    }
}