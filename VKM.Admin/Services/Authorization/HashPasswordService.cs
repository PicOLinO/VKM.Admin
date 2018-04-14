using System;
using System.Security.Cryptography;

namespace VKM.Admin.Services.Authorization
{
    public static class HashPasswordService
    {
        public static string Hash(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            var hash = pbkdf2.GetBytes(20);
            var hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            var passwordHash = Convert.ToBase64String(hashBytes);
            return passwordHash;
        }

        public static bool IsEqual(string hashPassword, string password)
        {
            byte[] hashBytes = Convert.FromBase64String(hashPassword);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (var i=0; i < 20; i++)
                if (hashBytes[i+16] != hash[i])
                    throw new UnauthorizedAccessException();
            return true;
        }
    }
}