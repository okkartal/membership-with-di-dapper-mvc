using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Security
{
    public class SaltedHash : IHashProvider
    {
        readonly HashAlgorithm _hashProvider;
        readonly int _salthLength;

        /// <summary>
        /// Default salt length is 16 characters
        /// </summary>
        public SaltedHash()
        {
            _hashProvider = new SHA256Managed();
            _salthLength = 16;
        }

        private byte[] ComputeHash(byte[] data, byte[] salt)
        {
            var dataAndSalt = new byte[data.Length + _salthLength];
            Array.Copy(data, dataAndSalt, data.Length);
            Array.Copy(salt, 0, dataAndSalt, data.Length, _salthLength);

            return _hashProvider.ComputeHash(dataAndSalt);
        }

        public void GetHashAndSalt(byte[] data, out byte[] hash, out byte[] salt)
        {
            salt = new byte[_salthLength];

            var random = new RNGCryptoServiceProvider();
            random.GetNonZeroBytes(salt);

            hash = ComputeHash(data, salt);
        }

        public void GetHashAndSaltString(string data, out string hash, out string salt)
        {
            byte[] hashOut;
            byte[] saltOut;

            GetHashAndSalt(Encoding.UTF8.GetBytes(data), out hashOut, out saltOut);

            hash = Convert.ToBase64String(hashOut);
            salt = Convert.ToBase64String(saltOut);
        }

        public bool VerifyHash(byte[] data, byte[] hash, byte[] salt)
        {
            var newHash = ComputeHash(data, salt);

            if (newHash.Length != hash.Length) return false;

            return !hash.Where((t, lp) => !t.Equals(newHash[lp])).Any();
        }

         
        public bool VerifyHashString(string data, string hash, string salt)
        {
            var dataToVerify = Encoding.UTF8.GetBytes(data);
            var hashToVerify = Convert.FromBase64String(hash);
            var saltToVerify = Convert.FromBase64String(salt);
            return VerifyHash(dataToVerify, hashToVerify, saltToVerify);
        }
    }

    public interface IHashProvider
    {
        void GetHashAndSalt(byte[] data, out byte[] hash, out byte[] salt);
        void GetHashAndSaltString(string data, out string hash, out string salt);
        bool VerifyHash(byte[] data, byte[] hash, byte[] salt);
        bool VerifyHashString(string data, string hash, string salt);
    }
}
