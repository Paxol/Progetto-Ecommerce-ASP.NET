using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Indirizzo email richiesto")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password richiesta")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool Ricorda { get; set; }
    }
}