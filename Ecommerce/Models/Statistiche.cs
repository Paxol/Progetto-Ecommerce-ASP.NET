namespace Ecommerce.Models
{
    public class Statistiche
    {
        public struct Pagination
        {
            public int Limit { get; set; }
            public int Page { get; set; }
            public int Total { get; set; }
        }

        public Pagination CorsiPiuVenduti { get; set; }
        public Pagination UtentiPiuAttivi { get; set; }
    }
}