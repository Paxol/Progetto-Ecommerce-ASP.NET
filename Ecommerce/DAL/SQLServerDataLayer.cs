using Ecommerce.Interfaces;
using Ecommerce.Models;
using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Ecommerce.Utils;

namespace Ecommerce.DAL
{
    public class SQLServerDataLayer : IDataLayer
    {
        string conn_string;
        public SQLServerDataLayer()
        {
            conn_string = ConfigurationManager.ConnectionStrings["Edunet"].ConnectionString;
        }

        public bool ChangePassword(int userid, string newpassword)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string mail)
        {
            throw new NotImplementedException();
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

                    SqlCommand cmd = new SqlCommand("GetUserAndRolesByMail", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@mail", mail);

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        user = new User();
                        user.UserID = (int)dr["IDUtente"];
                        user.Email = (string)dr["Email"];
                        user.Name = (string)dr["Nome"];
                        user.Roles = new List<string>(((string)dr["Ruoli"]).Split(','));
                    }
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

            int ret = cmd.ExecuteNonQuery();
            conn.Close();

            return ret;
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
    }
}