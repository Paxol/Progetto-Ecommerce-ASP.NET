namespace Ecommerce.Models
{
    public class Corso
    {
        public Corso() { }
        public Corso(int id, string titolo, string nomeAutore, string image, float valutazione, decimal prezzo)
        {
            ID = id;
            Titolo = titolo;
            NomeAutore = nomeAutore;
            Immagine = image;
            Valutazione = valutazione;
            Prezzo = prezzo;
        }

        public int ID { get; set; }
        public string Titolo { get; set; }
        public string NomeAutore { get; set; }
        public string Immagine { get; set; }
        public float Valutazione { get; set; }
        public decimal Prezzo { get; set; }        
    }
}