using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Ecommerce.Utils
{
    public class Crypto
    {
        public static byte[] Encrypt(string plainText)
        {
            byte[] encrypted;
            // Create a new TripleDESCryptoServiceProvider.  
            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            {
                //get keys
                byte[] key = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/App_Data/key"));// @"C:\Users\Mirko\source\Progetto-Ecommerce-ASP.NET\Ecommerce\App_Data\key");
                byte[] iv = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/App_Data/iv"));// @"C:\Users\Mirko\source\Progetto-Ecommerce-ASP.NET\Ecommerce\App_Data\iv");

                // Create encryptor  
                ICryptoTransform encryptor = tdes.CreateEncryptor(key, iv);
                // Create MemoryStream  
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption  
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream  
                    // to encrypt  
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream  
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            // Return encrypted data  
            return encrypted;
        }

        public static string Decrypt(byte[] cipherText)
        {
            string plaintext = null;

            //get keys
            byte[] key = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/App_Data/key"));
            byte[] iv = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/App_Data/iv"));

            // Create TripleDESCryptoServiceProvider  
            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            {
                // Create a decryptor  
                ICryptoTransform decryptor = tdes.CreateDecryptor(key, iv);
                // Create the streams used for decryption.  
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream  
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream  
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }
    }
}