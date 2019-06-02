using System;
using System.Text.RegularExpressions;

namespace Ecommerce.Models.DB
{
    public class CartaCredito
    {
        public string Numero { get; set; }
        public string Scadenza { get; set; }
        public string Proprietario { get; set; }
        public string CVV { get; set; }
        public string NumeroAst { get => Numero == null ? null : "************" + Numero.Substring(Numero.Length - 4); }

        /// <summary>
        /// Verifica la validità della carta di credito
        /// </summary>
        /// <param name="carta"></param>
        /// <returns>
        /// 0 se valida, -1 se numero errato, -2 se proprietario mancante, -3 se scadenza errata, -4 se cvv errato
        /// </returns>
        public static int IsValid(CartaCredito carta)
        {
            if (string.IsNullOrWhiteSpace(carta.Proprietario))
                return -2;

            var cardCheck = new Regex(@"^([\-\s]?[0-9]{4}){4}$");
            var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            var yearCheck = new Regex(@"^[0-9]{2}$");
            var cvvCheck = new Regex(@"^\d{3}$");

            if (!cardCheck.IsMatch(carta.Numero)) // <1>check card number is valid
                return -1;
            if (!cvvCheck.IsMatch(carta.CVV)) // <2>check cvv is valid as "999"
                return -4;

            var dateParts = carta.Scadenza.Split('/'); //expiry date in from MM/yyyy            
            if (!monthCheck.IsMatch(dateParts[0]) || !yearCheck.IsMatch(dateParts[1])) // <3 - 6>
                return -3; // ^ check date format is valid as "MM/yyyy"

            var year = int.Parse(dateParts[1]);
            var month = int.Parse(dateParts[0]);
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month); //get actual expiry date
            var cardExpiry = new DateTime(int.Parse("20" + year), month, lastDateOfExpiryMonth, 23, 59, 59);

            //check expiry greater than today & within next 6 years <7, 8>>
            return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6)) ? 0 : -3;
        }

        public string ToCSV()
        {
            return $"{Numero};{Scadenza};{Proprietario};{CVV}";
        }

        public static CartaCredito FromCSV(string csv)
        {
            var a = csv.Split(';');
            return new CartaCredito { Numero = a[0], Scadenza = a[1], Proprietario = a[2], CVV = a[3] };
        }
    }
}