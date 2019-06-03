using System;

namespace Ecommerce.Models.DB
{
    public class Recensione
    {
        public int ID { get; set; }
        public int IDCorso { get; set; }
        public string NomeUtente { get; set; }
        public int Valutazione { get; set; }
        public DateTime Data { get; set; }
        public string Descrizione { get; set; }
    }
}