using Ecommerce.Models;

namespace Ecommerce.Interfaces
{
    public interface IDataLayer
    {
        User GetUserByEmail(string mail);
        User GetUserByEmailAndPassword(string mail, string password);

        bool ChangePassword(int userid, string newpassword);

        bool RegisterUser(User user);
    }
}
