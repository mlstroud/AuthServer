using System;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace AuthServer.Library.Services
{
    public class CryptographyService : ICryptographyService
    {
        public string GetSalt()
        {
            var salt = new byte[16];

            using var rngProvider = new RNGCryptoServiceProvider();
            
            rngProvider.GetNonZeroBytes(salt);

            return Convert.ToBase64String(salt);
        }

        public string GetHash(string password, string salt) =>
            Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: GetSaltBytes(salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32));

        private byte[] GetSaltBytes(string salt) =>
            Encoding.ASCII.GetBytes(salt);
    }
}
