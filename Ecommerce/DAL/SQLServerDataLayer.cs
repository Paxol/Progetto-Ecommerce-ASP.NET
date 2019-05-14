using Ecommerce.Interfaces;
using Ecommerce.Models.DB;
using Ecommerce.Utils;
using System;
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

            // Recupero la password dal DB
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

            var returnParam = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParam.Direction = ParameterDirection.ReturnValue;

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
                corso.Prezzo = dr["Prezzo"].ToString();
                corso.Categoria = new Categoria
                {
                    ID = (int)dr["FK_IDCategoria"],
                    Nome = (string)dr["Categoria"]
                };
            }

            return corso;
        }

        public List<Corso> GetMiglioriCorsi()
        {
            SqlConnection conn = new SqlConnection(conn_string);
            conn.Open();

            // Recupero la password dal DB
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
                corso.Prezzo = dr["Prezzo"].ToString();
                corso.Categoria = new Categoria
                {
                    ID = (int)dr["FK_IDCategoria"],
                    Nome = (string)dr["Categoria"]
                };

                corsi.Add(corso);
            }

            conn.Close();
            return corsi;
        }
    }
}