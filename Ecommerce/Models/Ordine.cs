using Ecommerce.Models.DB;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class Ordine
    {
        public List<ItemCarrello> items { get; set; } = new List<ItemCarrello>();
    }
}