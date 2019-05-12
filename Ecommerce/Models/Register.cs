using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Register
    {
        [Required(ErrorMessage = "Indirizzo email richiesto")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nome richiesto")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Password richiesta")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password richiesta")]
        [DataType(DataType.Password)]
        public string RipetiPassword { get; set; }

        public bool IsPasswordValid { get => Password.Equals(RipetiPassword); }
    }
}