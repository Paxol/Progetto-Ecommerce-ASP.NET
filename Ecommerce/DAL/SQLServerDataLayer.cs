using Ecommerce.Interfaces;
using Ecommerce.Models.DB;
using Ecommerce.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Ecommerce.DAL
{
    public class SQLServerDataLayer : IDataLayer
    {
        private readonly string conn_string;
        public SQLServerDataLayer()
        {
            conn_string = ConfigurationManager.ConnectionStrings["Edunet"].ConnectionString;
        }

        public User GetUserByEmail(string mail)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd = new SqlCommand("GetUserAndRolesByMail", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@mail", mail);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            User user = null;
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                user = new User();
                user.UserID = (int)dr["IDUtente"];
                user.Email = (string)dr["Email"];
                user.Name = (string)dr["Nome"];
                user.Roles = new List<string>(((string)dr["Ruoli"]).Split(','));
            }

            conn.Close();
            return user;
        }

        public User GetUserByEmailAndPassword(string mail, string password)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            // Recupero la password dal DB
            SqlCommand cmd1 = new SqlCommand("GetUserPasswordByMail", conn);
            cmd1.CommandType = CommandType.StoredProcedure;

            cmd1.Parameters.AddWithValue("@mail", mail);
            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);

            User user = null;

            if (dt1.Rows.Count > 0) // Se la mail è registrata
            {
                DataRow dr1 = dt1.Rows[0];
                string passwordHashDB = (string)dr1["PasswordHash"];

                byte[] salt = Convert.FromBase64String(passwordHashDB.Substring(0, 4)); // Estraggo il sale (primi 4 caratteri)

                byte[] passwordHash = GenerateSaltedHash(Encoding.UTF8.GetBytes(password), salt); // Genero l'hash della password inserita dall'utente

                bool passwordok = CompareByteArrays(passwordHash, Convert.FromBase64String(passwordHashDB.Substring(4, passwordHashDB.Length - 4))); // Verifico i due hash

                if (passwordok)
                {
                    // Gli hash corrispondono, utente loggato, recupero i dati dell'utente
                    user = GetUserByEmail(mail);
                }
            }

            conn.Close();

            return user;
        }

        private byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        private bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Registra un utente
        /// </summary>
        /// <param name="user">Utente da registrare</param>
        /// <returns>Intero che rappresenta lo stato dell'operazione
        /// 0:      Successo
        /// -1:     Errore sconosciuto
        /// -10:    Email già registrata
        /// </returns>
        public int RegisterUser(User user)
        {
            Random rnd = Components.Random;
            byte[] salt = new byte[] {
                (byte)rnd.Next(byte.MaxValue),
                (byte)rnd.Next(byte.MaxValue),
                (byte)rnd.Next(byte.MaxValue),
            };

            byte[] passwordHash = GenerateSaltedHash(Encoding.UTF8.GetBytes(user.Password), salt); // Genero l'hash della password inserita dall'utente

            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd = new SqlCommand("RegisterUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@mail", user.Email);
            cmd.Parameters.AddWithValue("@password", Convert.ToBase64String(salt) + Convert.ToBase64String(passwordHash));
            cmd.Parameters.AddWithValue("@name", user.Name);

            var returnParam = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParam.Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();
            conn.Close();

            switch (returnParam.Value)
            {
                case 2627:
                    return -10;
                case 0:
                    return 0;
                default:
                    return -1;
            }
        }

        public User GetUserByID(int id)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd = new SqlCommand("GetUserAndRolesByID", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@uid", id);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            User user = null;

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                user = new User();
                user.UserID = (int)dr["IDUtente"];
                user.Email = (string)dr["Email"];
                user.Name = (string)dr["Nome"];
                user.Roles = new List<string>(((string)dr["Ruoli"]).Split(','));
            }

            conn.Close();

            return user;
        }

        public List<Categoria> GetCategories()
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("GetCategories", conn);
            cmd1.CommandType = CommandType.StoredProcedure;

            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);

            List<Categoria> categorie = new List<Categoria>();

            foreach (DataRow row in dt1.Rows)
            {
                categorie.Add(new Categoria
                {
                    ID = (int)row["IDCategoria"],
                    Nome = (string)row["Nome"]
                });
            }

            conn.Close();
            return categorie;
        }

        public int InsertCorso(Corso corso)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd = new SqlCommand("InsertCorso", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@titolo", corso.Titolo);
            cmd.Parameters.AddWithValue("@autore", corso.Autore);
            cmd.Parameters.AddWithValue("@prezzo", corso.Prezzo);
            cmd.Parameters.AddWithValue("@descrizione", corso.Descrizione);
            cmd.Parameters.AddWithValue("@categoria", corso.Categoria.ID);
            cmd.Parameters.AddWithValue("@immagine", corso.Immagine);

            int a = cmd.ExecuteNonQuery();
            conn.Close();

            if (a > 0)
                return 0;
            else
                return a;
        }

        public Corso GetCorsoByID(int id)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd = new SqlCommand("GetCorsoByID", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id", id);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            conn.Close();

            Corso corso = null;

            if (dt.Rows.Count > 0)
            {
                corso = new Corso();
                DataRow dr = dt.Rows[0];
                corso.ID = (int)dr["IDCorso"];
                corso.Autore = (string)dr["Autore"];
                corso.Titolo = (string)dr["Titolo"];
                corso.Immagine = (string)dr["Immagine"];
                corso.Descrizione = (string)dr["Descrizione"];
                corso.Prezzo = Convert.ToDecimal(dr["Prezzo"].ToString());
                corso.Categoria = new Categoria
                {
                    ID = (int)dr["IDCategoria"],
                    Nome = (string)dr["Categoria"]
                };
            }

            return corso;
        }

        public List<Corso> GetMiglioriCorsi()
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("GetMiglioriCorsi", conn);
            cmd1.CommandType = CommandType.StoredProcedure;

            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);

            List<Corso> corsi = new List<Corso>();

            foreach (DataRow dr in dt1.Rows)
            {
                Corso corso = new Corso();
                corso.ID = (int)dr["IDCorso"];
                corso.Autore = (string)dr["Autore"];
                corso.Titolo = (string)dr["Titolo"];
                corso.Immagine = (string)dr["Immagine"];
                corso.Descrizione = (string)dr["Descrizione"];
                corso.Valutazione = float.Parse(dr["MediaVoto"].ToString());
                corso.Prezzo = Convert.ToDecimal(dr["Prezzo"].ToString());
                corso.Categoria = new Categoria
                {
                    ID = (int)dr["IDCategoria"],
                    Nome = (string)dr["Categoria"]
                };

                corsi.Add(corso);
            }

            conn.Close();
            return corsi;
        }

        public List<StatCorso> GetProdottiPiuVenduti(int limit, int page, out int tot)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("GetProdottiPiuVenduti", conn);
            cmd1.CommandType = CommandType.StoredProcedure;

            cmd1.Parameters.AddWithValue("@limit", limit);
            cmd1.Parameters.AddWithValue("@page", page);
            var returnParam = cmd1.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParam.Direction = ParameterDirection.ReturnValue;

            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);

            tot = (int)returnParam.Value;

            List<StatCorso> corsi = new List<StatCorso>();

            foreach (DataRow dr in dt1.Rows)
            {
                StatCorso corso = new StatCorso();
                corso.IDCorso = (int)dr["IDCorso"];
                corso.Titolo = (string)dr["Titolo"];
                corso.Prezzo = (decimal)dr["Prezzo"];
                corso.Vendite = (int)dr["Vendite"];

                corsi.Add(corso);
            }

            conn.Close();
            return corsi;
        }

        public List<StatUtenti> GetUtentiPiuAttivi(int limit, int page, out int tot)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("GetUtentiPiuAttivi", conn);
            cmd1.CommandType = CommandType.StoredProcedure;

            cmd1.Parameters.AddWithValue("@limit", limit);
            cmd1.Parameters.AddWithValue("@page", page);
            var returnParam = cmd1.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParam.Direction = ParameterDirection.ReturnValue;

            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);

            tot = (int)returnParam.Value;

            List<StatUtenti> utenti = new List<StatUtenti>();

            foreach (DataRow dr in dt1.Rows)
            {
                StatUtenti utente = new StatUtenti()
                {
                    IDUtente = (int)dr["IDUtente"],
                    Email = (string)dr["Email"],
                    ProdottiComprati = (int)dr["ProdottiComprati"],
                };

                utenti.Add(utente);
            }

            conn.Close();
            return utenti;
        }

        public List<ItemCarrello> GetCarrello(int uid)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("GetCarrello", conn);
            cmd1.CommandType = CommandType.StoredProcedure;

            cmd1.Parameters.AddWithValue("@uid", uid);

            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);

            List<ItemCarrello> carrello = new List<ItemCarrello>();

            foreach (DataRow dr in dt1.Rows)
            {
                carrello.Add(new ItemCarrello
                {
                    ID = (int)dr["IDCarrello"],
                    Quantita = (int)dr["Quantita"],
                    Prezzo = (decimal)dr["Prezzo"],
                    Corso = new Corso
                    {
                        ID = (int)dr["IDCorso"],
                        Titolo = (string)dr["Titolo"],
                        Descrizione = (string)dr["Descrizione"],
                        Immagine = (string)dr["Immagine"],
                    }
                });
            }

            conn.Close();
            return carrello;
        }

        public int AggiornaQuantitaCarrello(int idcarrello, int q)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("AggiornaQuantitaCarrello", conn);
            cmd1.CommandType = CommandType.StoredProcedure;

            cmd1.Parameters.AddWithValue("@id", idcarrello);
            cmd1.Parameters.AddWithValue("@q", q);

            int a = cmd1.ExecuteNonQuery();

            conn.Close();
            return a;
        }

        public List<Corso> RicercaConFiltri(int idcategoria, decimal prezzomin, decimal prezzomax, string testo)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("RicercaConFiltri", conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@IDcategoria", idcategoria);
            cmd1.Parameters.AddWithValue("@prezzomin", prezzomin);
            cmd1.Parameters.AddWithValue("@prezzomax", prezzomax);
            cmd1.Parameters.AddWithValue("@testo", testo == null ? "" : testo);  //se è nulla segnala cosi "" se no metti testo

            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);

            List<Corso> corsi = new List<Corso>();

            foreach (DataRow dr in dt1.Rows)
            {
                Corso corso = new Corso();
                corso.ID = (int)dr["IDCorso"];
                corso.Autore = (string)dr["Autore"];
                corso.Titolo = (string)dr["Titolo"];
                corso.Immagine = (string)dr["Immagine"];
                corso.Descrizione = (string)dr["Descrizione"];
                corso.Prezzo = Convert.ToDecimal(dr["Prezzo"].ToString());

                corsi.Add(corso);
            }

            conn.Close();
            return corsi;

        }
        public List<Corso> Ricerca(string testo)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("Ricerca", conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@testo", testo);

            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);

            List<Corso> corsi = new List<Corso>();

            foreach (DataRow dr in dt1.Rows)
            {
                Corso corso = new Corso();
                corso.ID = (int)dr["IDCorso"];
                corso.Autore = (string)dr["Autore"];
                corso.Titolo = (string)dr["Titolo"];
                corso.Immagine = (string)dr["Immagine"];
                corso.Descrizione = (string)dr["Descrizione"];
                corso.Prezzo = Convert.ToDecimal(dr["Prezzo"].ToString());

                corsi.Add(corso);
            }

            conn.Close();
            return corsi;
        }

        public int AggiungiCarrello(int idcorso, int idutente)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("AggiungiCarrello", conn);
            cmd1.CommandType = CommandType.StoredProcedure;

            cmd1.Parameters.AddWithValue("@id", idcorso);
            cmd1.Parameters.AddWithValue("@uid", idutente);

            int a = cmd1.ExecuteNonQuery();

            conn.Close();
            return a;
        }
        
        public List<Recensione> GetRecensioni(int idcorso, int idutente)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("GetRecensioniById", conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@id", idcorso);
            cmd1.Parameters.AddWithValue("@uid", idutente);

            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);

            List<Recensione> rec = new List<Recensione>();

            foreach (DataRow dr in dt1.Rows)
            {
                Recensione recensione = new Recensione();
                recensione.NomeUtente = (string)dr["Nome"];
                recensione.Valutazione = (int)dr["Voto"];
                recensione.Descrizione = (string)dr["Descrizione"];
                recensione.Data = (DateTime)dr["Data"];
                rec.Add(recensione);
            }

            conn.Close();
            return rec;
        }

        public CartaCredito GetCartaCredito(int userid)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("GetCartaCredito", conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@uid", userid);

            string s = (string)cmd1.ExecuteScalar();

            conn.Close();
            if (s == null)
                return null;
            else
                return CartaCredito.FromCSV(Crypto.Decrypt(Convert.FromBase64String(s)));
        }

        public int InsertCartaCredito(CartaCredito cc, int userid)
        {
            string ccc = Convert.ToBase64String(Crypto.Encrypt(cc.ToCSV()));

            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("InsertCartaCredito", conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@cc", ccc);
            cmd1.Parameters.AddWithValue("@uid", userid);

            int a = cmd1.ExecuteNonQuery();

            conn.Close();

            return a;
        }

        public int CreaOrdine(int uid)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("CreaOrdine", conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@uid", uid);

            var returnParam = cmd1.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParam.Direction = ParameterDirection.ReturnValue;

            int a = cmd1.ExecuteNonQuery();

            conn.Close();

            return (int)returnParam.Value;
        }

        public List<Ordine> GetOrdiniByUserID(int uid)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("GetOrdini", conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@uid", uid);

            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);

            conn.Close();

            List<Ordine> ordini = new List<Ordine>();

            foreach (DataRow dr in dt1.Rows)
            {
                Ordine ordine = ordini.FirstOrDefault((ord) => ord.ID == (int)dr["IDOrdine"]);
                if (ordine == null)
                {
                    ordine = new Ordine
                    {
                        ID = (int)dr["IDOrdine"],
                        Data = DateTime.Parse(dr["Data"].ToString()),
                        Stato = dr["Stato"].ToString()
                    };

                    ordini.Add(ordine);
                }

                ordine.Items.Add(new ItemCarrello
                {
                    ID = (int)dr["IDItemOrdine"],
                    Quantita = (int)dr["Quantita"],
                    Prezzo = (decimal)dr["Prezzo"],
                    Corso = new Corso
                    {
                        ID = (int)dr["IDCorso"],
                        Titolo = (string)dr["Titolo"],
                        Autore = (string)dr["Autore"],
                        Descrizione = (string)dr["Descrizione"],
                        Immagine = (string)dr["Immagine"],
                    }
                });
            }

            return ordini;
        }

        public List<User> GetAdmins()
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("GetAdmins", conn);
            cmd1.CommandType = CommandType.StoredProcedure;

            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);

            List<User> admins = new List<User>();

            foreach (DataRow dr in dt1.Rows)
            {
                admins.Add(new User { UserID = (int)dr["IDUtente"], Email = dr["Email"].ToString() });
            }

            conn.Close();
            return admins;
        }

        public int AddAdmin(int uid)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("AddAdmin", conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@uid", uid);

            int a = cmd1.ExecuteNonQuery();

            conn.Close();

            return a;
        }

        public int RevokeAdmin(int uid)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("RevokeAdmin", conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@uid", uid);

            int a = cmd1.ExecuteNonQuery();

            conn.Close();

            return a;
        }

        public Ordine GetOrdineByID(int id)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("GetOrdineByID", conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@id", id);

            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);

            conn.Close();

            DataRow dr = dt1.Rows[0];
            Ordine ordine = new Ordine
            {
                ID = (int)dr["IDOrdine"],
                Data = DateTime.Parse(dr["Data"].ToString()),
                Stato = dr["Stato"].ToString()
            };

            ordine.Items.Add(new ItemCarrello
            {
                ID = (int)dr["IDItemOrdine"],
                Quantita = (int)dr["Quantita"],
                Prezzo = (decimal)dr["Prezzo"],
                Corso = new Corso
                {
                    ID = (int)dr["IDCorso"],
                    Titolo = (string)dr["Titolo"],
                    Autore = (string)dr["Autore"],
                    Descrizione = (string)dr["Descrizione"],
                    Immagine = (string)dr["Immagine"],
                }
            });

            return ordine;
        }

        public int UpdateStatoOrdine(int id, string stato)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("UpdateStatoOrdine", conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@id", id);
            cmd1.Parameters.AddWithValue("@stato", stato);

            int a = cmd1.ExecuteNonQuery();

            conn.Close();

            return a;
        }

        public int InsertRecensione(int idu, int idc, string recensione, int valutazione)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("InsertRecensioni", conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@idUtente", idu);
            cmd1.Parameters.AddWithValue("@idCorso", idc);
            cmd1.Parameters.AddWithValue("@descrizione", recensione);
            cmd1.Parameters.AddWithValue("@voto", valutazione);

            int a = cmd1.ExecuteNonQuery();

            conn.Close();

            return a;
        }

        public Recensione GetRecensioneUtente(int idcorso, int idutente)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("GetRecensioneUtente", conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@idc", idcorso);
            cmd1.Parameters.AddWithValue("@idu", idutente);

            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);
            conn.Close();
            Recensione recensione = null;
            if (dt1.Rows.Count > 0)
            {
                DataRow dr = dt1.Rows[0];
                recensione = new Recensione
                {
                    ID = (int)dr["IDValutazione"],
                    Data = (DateTime)dr["Data"],
                    Descrizione = (string)dr["Descrizione"],
                    Valutazione = (int)dr["Voto"],
                    IDCorso = (int)dr["FK_IDCorso"]
                };
            }

            return recensione;
        }

        public int UpdateRecensione(string id, string recensione, int valutazione)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("UpdateRecensione", conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@id", id);
            cmd1.Parameters.AddWithValue("@descrizione", recensione);
            cmd1.Parameters.AddWithValue("@voto", valutazione);

            int a = cmd1.ExecuteNonQuery();

            conn.Close();

            return a;
        }

        public Corso GetCorsoRandom()
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("GetCorsoRandom", conn);
            cmd1.CommandType = CommandType.StoredProcedure;

            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);
            conn.Close();

            Corso corso = null;
            if (dt1.Rows.Count > 0)
            {
                DataRow dr = dt1.Rows[0];
                corso = new Corso();
                corso.ID = (int)dr["IDCorso"];
                corso.Autore = (string)dr["Autore"];
                corso.Titolo = (string)dr["Titolo"];
                corso.Immagine = (string)dr["Immagine"];
                corso.Descrizione = (string)dr["Descrizione"];
                corso.Valutazione = float.Parse(dr["MediaVoto"].ToString());
                corso.Prezzo = Convert.ToDecimal(dr["Prezzo"].ToString());
                corso.Categoria = new Categoria
                {
                    ID = (int)dr["IDCategoria"],
                    Nome = (string)dr["Categoria"]
                };

            }
            
            return corso;
        }

        public List<Ordine> GetAllOrdini(int limit, int page, out int tot)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("GetAllOrdini", conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@limit", limit);
            cmd1.Parameters.AddWithValue("@page", page);

            var returnParam = cmd1.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParam.Direction = ParameterDirection.ReturnValue;

            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);
            tot = (int)returnParam.Value;

            conn.Close();

            List<Ordine> ordini = new List<Ordine>();

            foreach (DataRow dr in dt1.Rows)
            {
                Ordine ordine = ordini.FirstOrDefault((ord) => ord.ID == (int)dr["IDOrdine"]);
                if (ordine == null)
                {
                    ordine = new Ordine
                    {
                        ID = (int)dr["IDOrdine"],
                        Data = DateTime.Parse(dr["Data"].ToString()),
                        Stato = dr["Stato"].ToString(),
                        Prodotti = (int)dr["Prodotti"]
                    };

                    ordini.Add(ordine);
                }
            }

            return ordini;
        }

        public int AddCategoria(string nome)
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            SqlCommand cmd1 = new SqlCommand("AddCategoria", conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@nome", nome);

            int a = cmd1.ExecuteNonQuery();

            conn.Close();

            return a;
        }
    }
}