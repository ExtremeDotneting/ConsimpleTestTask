using System;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ConsimpleTestTask.WebApp.Utils
{
    public static class HashExtensions
    {
        public static string HashString(string str)
        {
            var saltStr = "MY SUPER SALT";
            byte[] salt = Encoding.ASCII.GetBytes(saltStr);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: str,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));
            return hashed;
        }

        public static bool Compare(string str, string hash)
        {
            return HashString(str) == hash;
        }
    }
}

