using System.Security.Cryptography;

namespace BankingControlPanel.Utility
{
    public static class KeyGeneration
    {
        public static string GenerateSecureKey(int keySize = 256)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var key = new byte[keySize / 8];
                rng.GetBytes(key);
                return Convert.ToBase64String(key);
            }
        }
    }
}