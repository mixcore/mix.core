using Mix.Heart.Helpers;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Mix.Lib.Services
{
    public class MixService
    {
        public static void InitAppSettings()
        {
            if (!File.Exists($"{MixConstants.CONST_FILE_APPSETTING}"))
            {
                File.Copy($"{MixConstants.CONST_DEFAULT_FILE_APPSETTING}", $"{MixConstants.CONST_FILE_APPSETTING}");
                var aesKey = AesEncryptionHelper.GenerateCombinedKeys(256);
                MixAppSettingService.SetConfig(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.ApiEncryptKey, aesKey);
                MixAppSettingService.SetConfig(MixAppSettingsSection.Authentication, MixAuthConfigurations.SecretKey, Guid.NewGuid().ToString("N"));
                MixAppSettingService.SaveSettings();
            }
        }

        public static string GetConnectionString(string connectionName)
        {
            return MixAppSettingService.GetConfig<string>(MixAppSettingsSection.ConnectionStrings, connectionName);
        }

        public static void SendMail(string subject, string message, string to, string from = null)
        {
            MailMessage mailMessage = new()
            {
                IsBodyHtml = true,
                From = new MailAddress(from)
            };
            mailMessage.To.Add(to);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            try
            {
                string server = MixAppSettingService.GetConfig<string>(MixAppSettingsSection.Smtp, "Server");
                string user = MixAppSettingService.GetConfig<string>(MixAppSettingsSection.Smtp, "User");
                string pwd = MixAppSettingService.GetConfig<string>(MixAppSettingsSection.Smtp, "Password");
                int port = MixAppSettingService.GetConfig<int>(MixAppSettingsSection.Smtp, "Port");
                bool ssl = MixAppSettingService.GetConfig<bool>(MixAppSettingsSection.Smtp, "SSL");
                SmtpClient client = new(server)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(user, pwd),
                    Port = port,
                    EnableSsl = ssl
                };

                client.Send(mailMessage);
            }
            catch
            {
                try
                {
                    SmtpClient smtpClient = new()
                    {
                        UseDefaultCredentials = true
                    };
                    smtpClient.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    LogException(ex);
                    // ToDo: cannot send mail
                }
            }
        }

        public static void LogException(Exception ex)
        {
            string fullPath = $"{Environment.CurrentDirectory}/logs/{DateTime.Now:dd-MM-yyyy}";
            if (!string.IsNullOrEmpty(fullPath) && !Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            string filePath = $"{fullPath}/log_exceptions.json";

            try
            {
                FileInfo file = new(filePath);
                string content = "[]";
                if (file.Exists)
                {
                    using (StreamReader s = file.OpenText())
                    {
                        content = s.ReadToEnd();
                    }
                    File.Delete(filePath);
                }

                JArray arrExceptions = JArray.Parse(content);
                JObject jex = new()
                {
                    new JProperty("CreatedDateTime", DateTime.UtcNow),
                    new JProperty("Details", JObject.FromObject(ex))
                };
                arrExceptions.Add(jex);
                content = arrExceptions.ToString();

                using var writer = File.CreateText(filePath);
                writer.WriteLine(content);
            }
            catch
            {
                Console.Write($"Cannot write log file {filePath}");
                // File invalid
            }
        }
    }
}
