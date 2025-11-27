using System.Security.Cryptography;
using System.Text;

namespace Mix.Lib.Services.Compliance
{
    public interface IFieldEncryptionService
    {
        string Encrypt(string plaintext);
        string Decrypt(string ciphertext);
        void RotateKeys();
        string GetCurrentKeyVersion();
    }

    public class FieldEncryptionService : IFieldEncryptionService
    {
        private readonly byte[] _key;
        private readonly string _keyVersion;

        public FieldEncryptionService(string encryptionKey = null)
        {
            // In production, this should come from a secure key management service
            // like Azure Key Vault, AWS KMS, etc.
            _key = !string.IsNullOrEmpty(encryptionKey) 
                ? Convert.FromBase64String(encryptionKey)
                : GenerateKey();
            _keyVersion = "v1.0";
        }

        public string Encrypt(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext))
                return plaintext;

            try
            {
                using (var aes = Aes.Create())
                {
                    aes.Key = _key;
                    aes.GenerateIV();

                    using (var encryptor = aes.CreateEncryptor())
                    using (var msEncrypt = new MemoryStream())
                    {
                        // Prepend IV to the encrypted data
                        msEncrypt.Write(aes.IV, 0, aes.IV.Length);
                        
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plaintext);
                        }

                        var result = msEncrypt.ToArray();
                        return Convert.ToBase64String(result);
                    }
                }
            }
            catch
            {
                // In production, log the error but don't expose sensitive information
                throw new InvalidOperationException("Encryption failed");
            }
        }

        public string Decrypt(string ciphertext)
        {
            if (string.IsNullOrEmpty(ciphertext))
                return ciphertext;

            try
            {
                var fullCipher = Convert.FromBase64String(ciphertext);

                using (var aes = Aes.Create())
                {
                    aes.Key = _key;
                    
                    // Extract IV from the beginning of the encrypted data
                    var iv = new byte[aes.BlockSize / 8];
                    var cipher = new byte[fullCipher.Length - iv.Length];
                    
                    Array.Copy(fullCipher, 0, iv, 0, iv.Length);
                    Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);
                    
                    aes.IV = iv;

                    using (var decryptor = aes.CreateDecryptor())
                    using (var msDecrypt = new MemoryStream(cipher))
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
            catch
            {
                // In production, log the error but don't expose sensitive information
                throw new InvalidOperationException("Decryption failed");
            }
        }

        public void RotateKeys()
        {
            // Key rotation implementation would:
            // 1. Generate a new key
            // 2. Update key management service
            // 3. Re-encrypt existing data with new key (gradually)
            // This is a complex operation that should be done carefully
            throw new NotImplementedException("Key rotation requires careful implementation with data migration");
        }

        public string GetCurrentKeyVersion()
        {
            return _keyVersion;
        }

        private static byte[] GenerateKey()
        {
            using (var aes = Aes.Create())
            {
                aes.GenerateKey();
                return aes.Key;
            }
        }
    }
}