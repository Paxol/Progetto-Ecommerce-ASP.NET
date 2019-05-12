using Ecommerce.Models;

namespace Ecommerce.Interfaces
{
    public interface IDataLayer
    {
        User GetUserByID(int id);
        User GetUserByEmail(string mail);
        User GetUserByEmailAndPassword(string mail, string password);

        bool ChangePassword(int userid, string newpassword);

        int RegisterUser(User user);
    }
}
