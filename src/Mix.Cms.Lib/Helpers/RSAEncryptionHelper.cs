using System;
using System.Security.Cryptography;
using System.Text;

namespace Mix.Cms.Lib.Helpers
{
    public class RSAEncryptionHelper
    {
        static UnicodeEncoding ByteConverter = new UnicodeEncoding();
        static RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

        public static string GetEncryptedText(string text)
        {
            byte[] arrPlaintext = ByteConverter.GetBytes(text);
            byte[] arrEncryptedtext = Encryption(arrPlaintext, RSA.ExportParameters(false), false);
            return ByteConverter.GetString(arrEncryptedtext);
        }

        public static string GetDecryptedText(string encryptedtext)
        {
            byte[] arrEncryptedtext = ByteConverter.GetBytes(encryptedtext);
            byte[] arrDecryptedtex = Decryption(arrEncryptedtext, RSA.ExportParameters(false), false);
            return ByteConverter.GetString(arrDecryptedtex);
        }



        static public byte[] Encryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey); encryptedData = RSA.Encrypt(Data, DoOAEPPadding);
                }
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        static public byte[] Decryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    decryptedData = RSA.Decrypt(Data, DoOAEPPadding);
                }
                return decryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
