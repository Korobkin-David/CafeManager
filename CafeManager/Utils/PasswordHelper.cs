using System.Security.Cryptography;
using System.Text;

namespace CafeManager.Utils
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[16];
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            byte[] hashBytes = new byte[48]; // 16 salt + 32 hash
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string base64Hash)
        {
            byte[] hashBytes = Convert.FromBase64String(base64Hash);
            
            //vytáhneme salt (prvnich 16 bytů)
            byte[] salt = new Byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            
            //Vytvoříme hash ze zadaného hesla se stejným saltem
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);
            
            //Porovnáme výsledek s uloženým hashem
            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 16] != hash[i]) return false;
            }

            return true;
        }
    } 
}
