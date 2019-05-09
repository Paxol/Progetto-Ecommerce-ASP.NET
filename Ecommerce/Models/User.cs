using System.Collections.Generic;

namespace Ecommerce.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
    }
}