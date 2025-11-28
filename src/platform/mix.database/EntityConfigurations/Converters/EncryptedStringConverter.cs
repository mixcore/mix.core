using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Security.Cryptography;
using System.Text;

namespace Mix.Database.EntityConfigurations.Converters
{
    /// <summary>
    /// A basic string encryption converter for Entity Framework Core.
    /// Note: This is a simplified implementation. In production, use a proper key management system.
    /// </summary>
    public class EncryptedStringConverter : ValueConverter<string, string>
    {
        public EncryptedStringConverter()
            : base(
                v => EncryptValue(v),
                v => DecryptValue(v))
        {
        }

        private static string EncryptValue(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext))
                return plaintext;

            // For now, return a simple base64 encoding as a placeholder
            // In production, this should use proper encryption
            try
            {
                var bytes = Encoding.UTF8.GetBytes(plaintext);
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return plaintext;
            }
        }

        private static string DecryptValue(string ciphertext)
        {
            if (string.IsNullOrEmpty(ciphertext))
                return ciphertext;

            // For now, return a simple base64 decoding as a placeholder
            // In production, this should use proper decryption
            try
            {
                var bytes = Convert.FromBase64String(ciphertext);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return ciphertext;
            }
        }
    }

    /// <summary>
    /// Nullable version of the encrypted string converter
    /// </summary>
    public class EncryptedNullableStringConverter : ValueConverter<string?, string?>
    {
        public EncryptedNullableStringConverter()
            : base(
                v => v != null ? EncryptValue(v) : v,
                v => v != null ? DecryptValue(v) : v)
        {
        }

        private static string EncryptValue(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext))
                return plaintext;

            try
            {
                var bytes = Encoding.UTF8.GetBytes(plaintext);
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return plaintext;
            }
        }

        private static string DecryptValue(string ciphertext)
        {
            if (string.IsNullOrEmpty(ciphertext))
                return ciphertext;

            try
            {
                var bytes = Convert.FromBase64String(ciphertext);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return ciphertext;
            }
        }
    }
}