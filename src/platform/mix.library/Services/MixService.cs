using Mix.Heart.Services;
using Mix.Shared.Constants;
using Mix.Shared.Services;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Mix.Lib.Services
{
    public class MixService
    {
        public readonly GlobalConfigService _globalConfigService;
        public readonly SmtpConfigService _smtpConfigService;
        public readonly MixConfigurationService _configService;
        public readonly MixFileService _fileService;

        public MixService(
            GlobalConfigService globalConfigService,
            MixConfigurationService configService,
            MixFileService fileService, 
            SmtpConfigService smtpConfigService)
        {
            _globalConfigService = globalConfigService;
            _configService = configService;
            _fileService = fileService;
            _smtpConfigService = smtpConfigService;
        }

        public string GetAssetFolder(string culture = null)
        {
            culture ??= _globalConfigService.AppSettings.DefaultCulture;
            return $"{_globalConfigService.AppSettings.Domain}/" +
                $"{MixFolders.SiteContentAssetsFolder}/" +
                $"{_configService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, culture)}/assets";
        }

        public string GetUploadFolder(string culture = null)
        {
            culture ??= _globalConfigService.AppSettings.DefaultCulture;
            return $"{MixFolders.SiteContentAssetsFolder}/" +
                $"{_configService.GetConfig<string>(MixAppSettingKeywords.ThemeFolder, culture)}/uploads/" +
                $"{DateTime.UtcNow.ToString(MixConstants.CONST_UPLOAD_FOLDER_DATE_FORMAT)}";
        }

        public void SendMail(string subject, string message, string to, string from = null)
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
                string server = _smtpConfigService.GetConfig<string>("Server");
                string user = _smtpConfigService.GetConfig<string>("User");
                string pwd = _smtpConfigService.GetConfig<string>("Password");
                int port = _smtpConfigService.GetConfig<int>("Port");
                bool ssl = _smtpConfigService.GetConfig<bool>("SSL");
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
