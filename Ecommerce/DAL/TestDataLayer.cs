using Ecommerce.Interfaces;
using Ecommerce.Models.DB;
using System.Linq;
using System.Collections.Generic;

namespace Ecommerce.DAL
{
    public class TestDataLayer : IDataLayer
    {
        private readonly List<User> users;

        public TestDataLayer()
        {
            users = new List<User>() {
                new User
                {
                    UserID = 1,
                    Email = "user@test.com",
                    Name = "User 1",
                    Password = "password.123",
                    Roles = new List<string>() { "Utente" }
                },
                new User
                {
                    UserID = 2,
                    Email = "admin@test.com",
                    Name = "Admin 1",
                    Password = "password.123",
                    Roles = new List<string>() { "Admin" }
                }
            };
        }
        
        public List<Categoria> GetCategories()
        {
            throw new System.NotImplementedException();
        }

        public Corso GetCorsoByID(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<Corso> GetMiglioriCorsi()
        {
            throw new System.NotImplementedException();
        }

        public User GetUserByEmail(string mail)
        {
            return users.FirstOrDefault((u) => u.Email == mail);
        }

        public User GetUserByEmailAndPassword(string mail, string password)
        {
            return users.FirstOrDefault((u) => u.Email == mail && u.Password == password);
        }

        public User GetUserByID(int id)
        {
            return users.FirstOrDefault((u) => u.UserID == id);
        }

        public int InsertCorso(Corso corso)
        {
            throw new System.NotImplementedException();
        }

        public int RegisterUser(User user)
        {
            user.Roles = new List<string>() { "Utente" };
            users.Add(user);
            return 0;
        }
    }
}