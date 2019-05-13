namespace Ecommerce.Models.DB
{
    public class Corso
    {
        public int ID { get; set; }
        public string Titolo { get; set; }
        public string Autore { get; set; }
        public string Immagine { get; set; }
        public float Valutazione { get; set; }
        public string Prezzo { get; set; }
        public Categoria Categoria { get; internal set; }
        public string Descrizione { get; internal set; }
    }
}