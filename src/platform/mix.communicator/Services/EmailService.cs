using Microsoft.Extensions.Configuration;
using Mix.Communicator.Models;
using Mix.Heart.Extensions;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Mail;

namespace Mix.Communicator.Services
{
    public class EmailService
    {
        private EmailSettingModel _settings { get; set; } = new EmailSettingModel();

        public EmailService(IConfiguration configuration)
        {

            var session = configuration.GetSection(MixAppSettingsSection.Smtp);
            session.Bind(_settings);
        }

        public void SendMail(string subject, string message, string to, string? from = null)
        {
            MailMessage mailMessage = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(from ?? _settings.From)
            };
            mailMessage.To.Add(to);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            try
            {
                SmtpClient client = new SmtpClient(_settings.Server)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(
                         _settings.User, _settings.Password
                        ),
                    Port = _settings.Port,
                    EnableSsl = _settings.SSL
                };

                client.Send(mailMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public Task SendMailWithEdmTemplate(string template, JObject data, string subject, string to, string? from = null)
        {
            return Task.Run(() =>
            {
                foreach (var prop in data.Properties())
                {
                    template = template.Replace($"[[{prop.Name.ToTitleCase()}]]", data.Value<string>(prop.Name));
                }
                SendMail(subject, template, to, from);
            });
        }
    }
}
