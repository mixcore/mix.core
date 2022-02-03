using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Models.Common;
using Mix.Cms.Lib.Services;
using Mix.Infrastructure.Repositories;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Mix.Cms.Lib.Helpers
{
    public class EdmHelper
    {
        public static void SendEdm(EdmInfoModel info)
        {
            var edm = MixFileRepository.Instance.GetFile(info.Name, MixFileExtensions.CsHtml, MixTemplateFolders.Edms);
            if (edm != null && info.Recipients != null && info.Recipients.Count() > 0)
            {

                string senderBody = GetEdmBody(edm.Content, info.Data);
                Send(
                    info.Title,
                    senderBody,
                    string.Join(',', info.Recipients),
                    info.From
                    );
            }
        }

        public static string GetEdmBody(string template, JObject data)
        {
            string regex = @"(\[\[)(\w+)(\]\])";
            Regex rgx = new Regex(regex, RegexOptions.IgnoreCase);
            var matches = rgx.Matches(template);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    var colName = m.Groups[2].Value;
                    JToken val = GetJToken(m.Groups[2].Value, data);
                    template = template.Replace($"[[{colName}]]", val?.Value<string>() ?? m.Groups[0].Value);
                }
            }
            template = template.Replace($"[[createdDateTime]]", DateTime.Now.ToLongDateString());
            return template;
        }

        public static JToken GetJToken(string path, JObject data)
        {
            JToken result = data;
            string[] names = path.Split('.');
            foreach (var name in names)
            {
                result = result[name];
                if (result is null)
                {
                    break;
                }
            }
            return result;
        }

        public static void Send(string subject, string message, string to, string from = null)
        {
            MailMessage mailMessage = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(from ?? MixService.GetSmtpConfig<string>("From"))
            };
            mailMessage.To.Add(to);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            try
            {
                SmtpClient client = new SmtpClient(MixService.GetSmtpConfig<string>("Server"))
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(
                        MixService.GetSmtpConfig<string>("User"), MixService.GetSmtpConfig<string>("Password")
                        ),
                    Port = MixService.GetSmtpConfig<int>("Port"),
                    EnableSsl = MixService.GetSmtpConfig<bool>("SSL")
                };

                client.SendAsync(mailMessage, Guid.NewGuid().ToString());
            }
            catch (Exception e)
            {
                MixService.LogException(e);
            }
        }
    }
}
