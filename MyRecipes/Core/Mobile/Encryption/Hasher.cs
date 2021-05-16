using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core.Mobile.Encryption
{
    static class Hasher
    {
        public static string HashPassword(string decryptedPassword, string salt)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(decryptedPassword + salt));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public static bool CheckPassword(string input, string encryptedPassword, string salt)
        {
            return encryptedPassword == HashPassword(salt, input);
        }

        public static string EncryptString(string decryptedString, string salt)
        {
            return StringCiphering.Encrypt(decryptedString, salt);
        }

        public static string DecryptString(string encryptedString, string salt)
        {
            return StringCiphering.Decrypt(encryptedString, salt);
        }
    }
}
