using Ecommerce.Models.DB;
using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class Ordine
    {
        public enum Stato { ConfermaUtente, Validato, Errore }

        public CartaCredito Carta { get; set; }
        public bool SalvaCarta { get; set; }
        public string Errore { get; set; }
        public Stato StatoOrdine { get; set; } = Stato.ConfermaUtente;
        public string NumOrig { get; set; }
    }
}