﻿using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.ViewModels;
using Mix.Common.Helper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Microsoft.EntityFrameworkCore.Storage;

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

        private static volatile MixService defaultInstance;

        private List<string> Cultures { get; set; }
        private JObject MixConfigurations { get; set; }
        private JObject GlobalSettings { get; set; }
        private JObject ConnectionStrings { get; set; }
        private JObject LocalSettings { get; set; }
        private JObject Translator { get; set; }
        private JObject Authentication { get; set; }
        private JObject IpSecuritySettings { get; set; }
        private JObject Smtp { get; set; }
        private readonly FileSystemWatcher watcher = new FileSystemWatcher();

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

        public static MixService DefaultInstance
        {
            get
            {
                if (defaultInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (defaultInstance == null)
                        {
                            defaultInstance = new MixService();
                            defaultInstance.LoadDefaultConfiggurations();
                        }
                    }
                }

                return defaultInstance;
            }
        }

        private void LoadConfiggurations()
        {
            // Load configurations from appSettings.json
            JObject jsonSettings = new JObject();
            var settings = FileRepository.Instance.GetFile(MixConstants.CONST_FILE_APPSETTING, string.Empty, true);

            if (string.IsNullOrEmpty(settings.Content))
            {
                settings = FileRepository.Instance.GetFile(MixConstants.CONST_DEFAULT_FILE_APPSETTING, string.Empty, true, "{}");
            }

            string content = string.IsNullOrWhiteSpace(settings.Content) ? "{}" : settings.Content;
            jsonSettings = JObject.Parse(content);
            //var cultures = CommonRepository.Instance.LoadCultures();

            instance.ConnectionStrings = JObject.FromObject(jsonSettings["ConnectionStrings"]);
            instance.MixConfigurations = JObject.FromObject(jsonSettings["MixConfigurations"]);
            instance.Authentication = JObject.FromObject(jsonSettings["Authentication"]);
            instance.IpSecuritySettings = JObject.FromObject(jsonSettings["IpSecuritySettings"]);
            instance.Smtp = JObject.FromObject(jsonSettings["Smtp"] ?? new JObject());
            instance.Translator = JObject.FromObject(jsonSettings["Translator"]);
            instance.GlobalSettings = JObject.FromObject(jsonSettings["GlobalSettings"]);
            instance.LocalSettings = JObject.FromObject(jsonSettings["LocalSettings"]);
            //instance.Cultures = cultures.Select(c=>c.Specificulture).ToList();
            CommonHelper.WebConfigInstance = jsonSettings;
        }

        private void LoadDefaultConfiggurations()
        {
            // Load configurations from appSettings.json
            JObject jsonSettings = new JObject();
            var settings = FileRepository.Instance.GetFile(MixConstants.CONST_DEFAULT_FILE_APPSETTING, string.Empty, true);

            string content = string.IsNullOrWhiteSpace(settings.Content) ? "{}" : settings.Content;
            jsonSettings = JObject.Parse(content);

            defaultInstance.ConnectionStrings = JObject.FromObject(jsonSettings["ConnectionStrings"]);
            defaultInstance.MixConfigurations = JObject.FromObject(jsonSettings["MixConfigurations"]);
            defaultInstance.Authentication = JObject.FromObject(jsonSettings["Authentication"]);
            defaultInstance.IpSecuritySettings = JObject.FromObject(jsonSettings["IpSecuritySettings"]);
            defaultInstance.Smtp = JObject.FromObject(jsonSettings["Smtp"] ?? new JObject());
            defaultInstance.Translator = JObject.FromObject(jsonSettings["Translator"]);
            defaultInstance.GlobalSettings = JObject.FromObject(jsonSettings["GlobalSettings"]);
            defaultInstance.LocalSettings = JObject.FromObject(jsonSettings["LocalSettings"]);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(500);
            Instance.LoadConfiggurations();
        }

        public static string GetConnectionString(string name)
        {
            // Enhance: Add Encrypt / Decrypt appconnection string
            return Instance.ConnectionStrings?[name].Value<string>();
        }

        public static void SetConnectionString(string name, string value)
        {
            // Enhance: Add Encrypt / Decrypt appconnection string
            Instance.ConnectionStrings[name] = value;
        }

        public bool CheckValidCulture(string specificulture)
        {
            if (Instance.Cultures == null)
            {
                var cultures = ViewModels.MixCultures.UpdateViewModel.Repository.GetModelList().Data;
                Instance.Cultures = cultures?.Select(c => c.Specificulture).ToList() ?? new List<string>();
            }
            return Instance.Cultures.Any(c => c == specificulture);
        }

        public static T GetAuthConfig<T>(string name)
        {
            var result = Instance.Authentication[name];
            if (result == null)
            {
                result = DefaultInstance.Authentication[name];
            }
            return result != null ? result.Value<T>() : default;
        }

        public static void SetAuthConfig<T>(string name, T value)
        {
            Instance.Authentication[name] = value.ToString();
        }

        public static T GetIpConfig<T>(string name)
        {
            var result = Instance.IpSecuritySettings[name];
            if (result == null)
            {
                result = DefaultInstance.IpSecuritySettings[name];
            }
            return result != null ? result.Value<T>() : default;
        }

        public static void SetIpConfig<T>(string name, T value)
        {
            Instance.IpSecuritySettings[name] = value.ToString();
        }

        public static T GetMixConfig<T>(string name)
        {
            var result = Instance.MixConfigurations[name];
            if (result == null)
            {
                result = DefaultInstance.MixConfigurations[name];
            }
            return result != null ? result.Value<T>() : default;
        }

        public static void SetMixConfig<T>(string name, T value)
        {
            Instance.MixConfigurations[name] = value != null ? JToken.FromObject(value) : null;
        }

        public static T GetConfig<T>(string name)
        {
            var result = Instance.GlobalSettings[name];
            if (result == null)
            {
                result = DefaultInstance.GlobalSettings[name];
            }
            return result != null ? result.Value<T>() : default;
        }

        public static T GetEnumConfig<T>(string name)
        {
            Enum.TryParse(typeof(T), Instance.GlobalSettings[name]?.Value<string>(), true, out object result);
            return result != null ? (T)result : default;
        }

        public static void SetConfig<T>(string name, T value)
        {
            Instance.GlobalSettings[name] = value != null ? JToken.FromObject(value) : null;
        }

        public static T GetConfig<T>(string name, string culture)
        {
            JToken result = null;
            if (!string.IsNullOrEmpty(culture) && Instance.LocalSettings[culture] != null)
            {
                result = Instance.LocalSettings[culture][name];
            }
            return result != null ? result.Value<T>() : default;
        }

        public static void SetConfig<T>(string name, string culture, T value)
        {
            Instance.LocalSettings[culture][name] = value.ToString();
        }

        public static T Translate<T>(string name, string culture)
        {
            var result = Instance.Translator[culture][name];
            //if (result == null)
            //{
            //    result = DefaultInstance.Translator[culture][name];
            //}
            return result != null ? result.Value<T>() : default;
        }

        public static string TranslateString(string name, string culture)
        {
            var result = Instance.Translator[culture][name];
            return result != null ? result.Value<string>() : name;
        }

        public static JObject GetTranslator(string culture)
        {
            return JObject.FromObject(Instance.Translator[culture] ?? new JObject());
        }

        public static JObject GetLocalSettings(string culture)
        {
            return JObject.FromObject(Instance.LocalSettings[culture] ?? new JObject());
        }

        public static JObject GetGlobalSetting()
        {
            return JObject.FromObject(Instance.GlobalSettings);
        }

        public static bool SaveSettings()
        {
            var settings = FileRepository.Instance.GetFile(MixConstants.CONST_FILE_APPSETTING, string.Empty, true, "{}");
            if (settings != null)
            {
                if (string.IsNullOrWhiteSpace(settings.Content))
                {
                    var defaultSettings = FileRepository.Instance.GetFile(MixConstants.CONST_DEFAULT_FILE_APPSETTING, string.Empty, true, "{}");
                    settings = new FileViewModel()
                    {
                        Filename = "appsettings",
                        Extension = ".json",
                        Content = defaultSettings.Content
                    };
                    return FileRepository.Instance.SaveFile(settings);
                }
                else
                {
                    JObject jsonSettings = JObject.Parse(settings.Content);

                    jsonSettings["ConnectionStrings"] = instance.ConnectionStrings;
                    jsonSettings["GlobalSettings"] = instance.GlobalSettings;
                    jsonSettings["GlobalSettings"]["LastUpdateConfiguration"] = DateTime.UtcNow;
                    jsonSettings["Translator"] = instance.Translator;
                    jsonSettings["LocalSettings"] = instance.LocalSettings;
                    jsonSettings["Authentication"] = instance.Authentication;
                    jsonSettings["IpSecuritySettings"] = instance.IpSecuritySettings;
                    jsonSettings["Smtp"] = instance.Smtp;
                    settings.Content = jsonSettings.ToString();
                    return FileRepository.Instance.SaveFile(settings);
                }
            }
            else
            {
                return false;
            }
        }

        public static bool SaveSettings(string content)
        {
            var settings = FileRepository.Instance.GetFile(MixConstants.CONST_FILE_APPSETTING, string.Empty, true, "{}");

            settings.Content = content;
            return FileRepository.Instance.SaveFile(settings);
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
                var ListLanguage = context.MixLanguage.ToList();
                var cultures = context.MixCulture.ToList();
                foreach (var culture in cultures)
                {
                    JObject arr = new JObject();
                    foreach (var lang in ListLanguage.Where(l => l.Specificulture == culture.Specificulture).ToList())
                    {
                        JProperty l = new JProperty(lang.Keyword, lang.Value ?? lang.DefaultValue);
                        arr.Add(l);
                    }
                    Instance.Translator.Add(new JProperty(culture.Specificulture, arr));
                }

                Instance.LocalSettings = new JObject();
                var listLocalSettings = context.MixConfiguration.ToList();
                foreach (var culture in cultures)
                {
                    JObject arr = new JObject();
                    foreach (var cnf in listLocalSettings.Where(l => l.Specificulture == culture.Specificulture).ToList())
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
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        public static Task SendEdm(string culture, string template, JObject data, string subject, string from)
        {
            return Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(data["email"].Value<string>()))
                {
                    string to = data["email"].Value<string>();
                    var getEdm = ViewModels.MixTemplates.UpdateViewModel.GetTemplateByPath(template, culture);
                    if (getEdm.IsSucceed && !string.IsNullOrEmpty(getEdm.Data.Content))
                    {
                        string body = getEdm.Data.Content;
                        foreach (var prop in data.Properties())
                        {
                            body = body.Replace($"[[{prop.Name}]]", data[prop.Name].Value<string>());
                        }
                        MixService.SendMail(subject, body, to, from);
                    }
                }
            });
        }

        public static void SendMail(string subject, string message, string to, string from = null)
        {
            MailMessage mailMessage = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(from ?? instance.Smtp.Value<string>("From"))
            };
            mailMessage.To.Add(to);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            try
            {
                SmtpClient client = new SmtpClient(instance.Smtp.Value<string>("Server"))
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(instance.Smtp.Value<string>("User"), instance.Smtp.Value<string>("Password")),
                    Port = instance.Smtp.Value<int>("Port"),
                    EnableSsl = instance.Smtp.Value<bool>("SSL")
                };

                client.Send(mailMessage);
            }
            catch
            {
                try
                {
                    SmtpClient smtpClient = new SmtpClient
                    {
                        UseDefaultCredentials = true
                    };
                    smtpClient.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    MixService.LogException(ex);
                    // ToDo: cannot send mail
                }
            }
        }

        public static void LogException(Exception ex)
        {
            string fullPath = $"{Environment.CurrentDirectory}/logs/{DateTime.Now.ToString("dd-MM-yyyy")}";
            if (!string.IsNullOrEmpty(fullPath) && !Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            string filePath = $"{fullPath}/log_exceptions.json";

            try
            {
                FileInfo file = new FileInfo(filePath);
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
                JObject jex = new JObject
                {
                    new JProperty("CreatedDateTime", DateTime.UtcNow),
                    new JProperty("Details", JObject.FromObject(ex))
                };
                arrExceptions.Add(jex);
                content = arrExceptions.ToString();

                using (var writer = File.CreateText(filePath))
                {
                    writer.WriteLine(content);
                }
            }
            catch
            {
                Console.Write($"Cannot write log file {filePath}");
                // File invalid
            }
        }

        public static string GetTemplateFolder(string culture)
        {
            return $"content/templates/{Instance.LocalSettings[culture][AppSettingKeywords.ThemeFolder]}";
        }

        public static string GetTemplateUploadFolder(string culture)
        {
            return $"content/templates/{Instance.LocalSettings[culture][AppSettingKeywords.ThemeFolder]}/uploads";
        }

        public static MixCmsContext GetDbContext()
        {
            var provider = Enum.Parse<MixDatabaseProvider>(MixService.GetConfig<string>(MixConstants.CONST_SETTING_DATABASE_PROVIDER));
            switch (provider)
            {
                case MixDatabaseProvider.MSSQL:
                    return new MsSqlMixCmsContext();
                case MixDatabaseProvider.MySQL:
                    return new MySqlMixCmsContext();
                case MixDatabaseProvider.PostgreSQL:
                default:
                    // TODO: Add PostgreSQL db context
                    return null;
            }

        }
    }
}