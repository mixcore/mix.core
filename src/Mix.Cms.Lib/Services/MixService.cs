using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.ViewModels;
using Mix.Common.Helper;
using Newtonsoft.Json.Linq;

namespace Mix.Cms.Lib.Services
{
    public class MixService
    {
        /// <summary>
        /// The synchronize root
        /// </summary>
        private static readonly object syncRoot = new Object();
        /// <summary>
        /// The instance
        /// </summary>
        private static volatile MixService instance;

        private List<string> Cultures { get; set; }
        private JObject GlobalSettings { get; set; }
        private JObject ConnectionStrings { get; set; }
        private JObject LocalSettings { get; set; }
        private JObject Translator { get; set; }
        private JObject Authentication { get; set; }
        private JObject Smtp { get; set; }
        readonly FileSystemWatcher watcher = new FileSystemWatcher();

        public MixService()
        {
            watcher.Path = System.IO.Directory.GetCurrentDirectory();
            watcher.Filter = "";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        public static MixService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new MixService();
                            instance.LoadConfiggurations();
                        }
                    }
                }

                return instance;
            }
        }

        private void LoadConfiggurations()
        {
            // Load configurations from appSettings.json
            JObject jsonSettings = new JObject();
            var settings = FileRepository.Instance.GetFile(MixConstants.CONST_FILE_APPSETTING, ".json", string.Empty, true);
            
            if (string.IsNullOrEmpty(settings.Content))
            {
                settings = FileRepository.Instance.GetFile(MixConstants.CONST_DEFAULT_FILE_APPSETTING, ".json", string.Empty, true, "{}");
            }

            string content = string.IsNullOrWhiteSpace(settings.Content) ? "{}" : settings.Content;
            jsonSettings = JObject.Parse(content);


            instance.ConnectionStrings = JObject.FromObject(jsonSettings["ConnectionStrings"]);
            instance.Authentication = JObject.FromObject(jsonSettings["Authentication"]);
            instance.Smtp = JObject.FromObject(jsonSettings["Smtp"] ?? new JObject());
            instance.Translator = JObject.FromObject(jsonSettings["Translator"]);
            instance.GlobalSettings = JObject.FromObject(jsonSettings["GlobalSettings"]);
            instance.LocalSettings = JObject.FromObject(jsonSettings["LocalSettings"]);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(500);
            Instance.LoadConfiggurations();
        }


        public static string GetConnectionString(string name)
        {
            return Instance.ConnectionStrings?[name].Value<string>();
        }

        public static void SetConnectionString(string name, string value)
        {
            Instance.ConnectionStrings[name] = value;
        }

        public static T GetAuthConfig<T>(string name)
        {
            var result = Instance.Authentication[name];
            return result != null ? result.Value<T>() : default(T);
        }

        public static void SetAuthConfig<T>(string name, T value)
        {
            Instance.Authentication[name] = value.ToString();
        }

        public static T GetConfig<T>(string name)
        {
            var result = Instance.GlobalSettings[name];
            return result != null ? result.Value<T>() : default(T);
        }

        public static void SetConfig<T>(string name, T value)
        {
            Instance.GlobalSettings[name] = value != null ? JToken.FromObject(value) : null;
        }


        public static T GetConfig<T>(string name, string culture)
        {
            var result = !string.IsNullOrEmpty(culture) ? Instance.LocalSettings[culture][name] : null;
            return result != null ? result.Value<T>() : default(T);
        }

        public static void SetConfig<T>(string name, string culture, T value)
        {
            Instance.LocalSettings[culture][name] = value.ToString();
        }

        public static T Translate<T>(string name, string culture)
        {
            var result = Instance.Translator[culture][name];
            return result != null ? result.Value<T>() : default(T);
        }

        public static JObject GetTranslator(string culture)
        {
            return JObject.FromObject(Instance.Translator[culture]);
        }

        public static JObject GetLocalSettings(string culture)
        {
            return JObject.FromObject(Instance.LocalSettings[culture]);
        }

        public static JObject GetGlobalSetting()
        {
            return JObject.FromObject(Instance.GlobalSettings);
        }

        public static bool Save()
        {
            var settings = FileRepository.Instance.GetFile(MixConstants.CONST_FILE_APPSETTING, ".json", string.Empty, true, "{}");
            if (settings != null)
            {
                JObject jsonSettings = !string.IsNullOrWhiteSpace(settings.Content) ? JObject.Parse(settings.Content) : new JObject();

                jsonSettings["ConnectionStrings"] = instance.ConnectionStrings;
                jsonSettings["GlobalSettings"] = instance.GlobalSettings;
                jsonSettings["GlobalSettings"]["LastUpdateConfiguration"] = DateTime.UtcNow;
                jsonSettings["Translator"] = instance.Translator;
                jsonSettings["LocalSettings"] = instance.LocalSettings;
                jsonSettings["Authentication"] = instance.Authentication;
                jsonSettings["Smtp"] = instance.Smtp;
                settings.Content = jsonSettings.ToString();
                return FileRepository.Instance.SaveFile(settings);
            }
            else
            {
                return false;
            }

        }
        public static void Reload()
        {
            Instance.LoadConfiggurations();

        }
        public static void LoadFromDatabase(MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction
                , out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                Instance.Translator = new JObject();
                var ListLanguage = context.MixLanguage;
                foreach (var culture in context.MixCulture)
                {
                    JObject arr = new JObject();
                    foreach (var lang in ListLanguage.Where(l => l.Specificulture == culture.Specificulture))
                    {
                        JProperty l = new JProperty(lang.Keyword, lang.Value ?? lang.DefaultValue);
                        arr.Add(l);
                    }
                    Instance.Translator.Add(new JProperty(culture.Specificulture, arr));
                }

                Instance.LocalSettings = new JObject();
                var listLocalSettings = context.MixConfiguration;
                foreach (var culture in context.MixCulture)
                {
                    JObject arr = new JObject();
                    foreach (var cnf in listLocalSettings.Where(l => l.Specificulture == culture.Specificulture))
                    {
                        JProperty l = new JProperty(cnf.Keyword, cnf.Value);
                        arr.Add(l);
                    }
                    Instance.LocalSettings.Add(new JProperty(culture.Specificulture, arr));
                }
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(true, isRoot, transaction);
            }
            catch (Exception ex) // TODO: Add more specific exeption types instead of Exception only
            {

                var error = UnitOfWorkHelper<MixCmsContext>.HandleException<MixLanguage>(ex, isRoot, transaction);
            }
            finally
            {
                //if current Context is Root
                if (isRoot)
                {
                    context?.Dispose();
                }

            }
        }

        public static CryptoViewModel<string> EncryptStringToBytes_Aes(JObject plainText)
        {
            var result = new CryptoViewModel<string>();
            //var Key = Convert.FromBase64String(keyString);
            //var IV = Convert.FromBase64String(iv);
            // Check arguments.
            if (plainText == null)
                throw new ArgumentNullException("plainText");
            //if (Key == null || Key.Length <= 0)
            //    throw new ArgumentNullException("Key");
            //if (IV == null || IV.Length <= 0)
            //    throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;
                //aesAlg.Key = Key;
                //aesAlg.IV = IV;

                result.Base64Key = Convert.ToBase64String(aesAlg.Key);
                result.Base64IV = Convert.ToBase64String(aesAlg.IV);

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            result.Data = Convert.ToBase64String(encrypted);
            return result;
        }

        public static CryptoViewModel<JObject> DecryptStringFromBytes_Aes(string cipherText, string keyString, string iv)
        {
            CryptoViewModel<JObject> result = new CryptoViewModel<JObject>()
            {
                Base64Key = keyString,
                Base64IV = iv
            };
            var Key = Convert.FromBase64String(keyString);
            var IV = Convert.FromBase64String(iv);
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;
            var fullCipher = Convert.FromBase64String(cipherText);
            // Create an Aes object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(fullCipher))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }
            result.Data = JObject.Parse(plaintext);
            return result;

        }

        public static string EncryptString(string text, string keyString, string strIV)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public static string DecryptString(string cipherText, string keyString, string strIV)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
        public static string EncryptString(string text, string keyString)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public static string DecryptString(string cipherText, string keyString)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }

        public static void SendMail(string subject, string message, string to)
        {
            SmtpClient client = new SmtpClient(instance.Smtp.Value<string>("Server"));
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(instance.Smtp.Value<string>("User"), instance.Smtp.Value<string>("Password"));
            client.Port = instance.Smtp.Value<int>("Port");
            client.EnableSsl = instance.Smtp.Value<bool>("SSL");
            MailMessage mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = true;
            mailMessage.From = new MailAddress(instance.Smtp.Value<string>("From"));
            mailMessage.To.Add(to);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            client.Send(mailMessage);

        }
    }
}
