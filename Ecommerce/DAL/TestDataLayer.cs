﻿using Ecommerce.Interfaces;
using Ecommerce.Models;
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

        public bool ChangePassword(int userid, string newpassword)
        {
            User user = users.FirstOrDefault((u) => u.UserID == userid);
            if (user != null)
            {
                user.Password = newpassword;
                return true;
            }
            else
            {
                return false;
            }
        }

        public User GetUserByEmail(string mail)
        {
            return users.FirstOrDefault((u) => u.Email == mail);
        }

        public User GetUserByEmailAndPassword(string mail, string password)
        {
            return users.FirstOrDefault((u) => u.Email == mail && u.Password == password);
        }

        public bool RegisterUser(User user)
        {
            users.Add(user);
            return true;
        }
    }
}