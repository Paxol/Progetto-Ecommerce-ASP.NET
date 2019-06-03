using Ecommerce.Interfaces;
using Ecommerce.Models.DB;
using System.Linq;
using System.Collections.Generic;
using Ecommerce.Models;

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

        public int AddAdmin(int uid)
        {
            throw new System.NotImplementedException();
        }

        public int AggiornaQuantitaCarrello(int idcarrello, int q)
        {
            throw new System.NotImplementedException();
        }

        public int AggiungiCarrello(int idcorso, int idutente)
        {
            throw new System.NotImplementedException();
        }

        public int CreaOrdine(int uid)
        {
            throw new System.NotImplementedException();
        }

        public List<User> GetAdmins()
        {
            throw new System.NotImplementedException();
        }

        public List<ItemCarrello> GetCarrello(int uid)
        {
            throw new System.NotImplementedException();
        }

        public CartaCredito GetCartaCredito(int userid)
        {
            throw new System.NotImplementedException();
        }

        public List<Categoria> GetCategories()
        {
            throw new System.NotImplementedException();
        }

        public Corso GetCorsoByID(int id)
        {
            throw new System.NotImplementedException();
        }

        public object GetCorsoByID(ModCorso a)
        {
            throw new System.NotImplementedException();
        }

        public List<Corso> GetMiglioriCorsi()
        {
            throw new System.NotImplementedException();
        }

        public Models.DB.Ordine GetOrdineByID(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<Models.DB.Ordine> GetOrdini(int uid)
        {
            throw new System.NotImplementedException();
        }

        public List<StatCorso> GetProdottiPiuVenduti(int limit, int page, out int tot)
        {
            throw new System.NotImplementedException();
        }

        public Recensione GetRecensioneUtente(int idcorso, int idutente)
        {
            throw new System.NotImplementedException();
        }

        public List<Recensione> GetRecensioni(int id, int idutente)
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

        public List<StatUtenti> GetUtentiPiuAttivi(int limit, int page, out int tot)
        {
            throw new System.NotImplementedException();
        }

        public int InsertCartaCredito(CartaCredito cc, int userid)
        {
            throw new System.NotImplementedException();
        }

        public int InsertCorso(Corso corso)
        {
            throw new System.NotImplementedException();
        }

        public int InsertRecensione(int v, int idc, string recensione, int valutazione)
        {
            throw new System.NotImplementedException();
        }

        public int RegisterUser(User user)
        {
            user.Roles = new List<string>() { "Utente" };
            users.Add(user);
            return 0;
        }

        public int RevokeAdmin(int uid)
        {
            throw new System.NotImplementedException();
        }

        public List<Corso> Ricerca(string testo)
        {
            throw new System.NotImplementedException();
        }

        public List<Corso> RicercaConFiltri(int idcategoria, decimal prezzomin, decimal prezzomax, string testo)
        {
            throw new System.NotImplementedException();
        }

        public int UpdateStatoOrdine(int id, string stato)
        {
            throw new System.NotImplementedException();
        }
    }
}