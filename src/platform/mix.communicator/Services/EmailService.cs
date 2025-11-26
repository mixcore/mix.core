using Microsoft.Extensions.Configuration;
using Mix.Communicator.Models;
using Mix.Heart.Exceptions;
using System.Net;
using System.Net.Mail;

namespace Mix.Communicator.Services
{
    public class EmailService
    {
        private EmailSettingModel Settings { get; set; } = new EmailSettingModel();

        public EmailService(IConfiguration configuration)
        {

            var session = configuration.GetSection(MixAppSettingsSection.Smtp);
            session.Bind(Settings);
        }

        public async Task SendMail(EmailMessageModel msg)
        {
            if (string.IsNullOrEmpty(msg.Subject) || string.IsNullOrEmpty(msg.To))
            {
                throw new MixException(Heart.Enums.MixErrorStatus.Badrequest, "Invalid Mail message");
            }
            MailMessage mailMessage = new MailMessage
            {

                IsBodyHtml = true,
                From = new MailAddress(msg.From ?? Settings.From, msg.FromName ?? Settings.FromName)
            };
            foreach (var receipient in msg.To.Split(','))
            {
                mailMessage.To.Add(receipient);
            }
            if (!string.IsNullOrEmpty(msg.CC))
            {
                foreach (var cc in msg.CC.Split(','))
                {
                    mailMessage.CC.Add(cc);
                }
            }
            mailMessage.Body = msg.Message;
            mailMessage.Subject = msg.Subject;
            try
            {
                var client = new SmtpClient(Settings.Server)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(
                         Settings.User, Settings.Password
                        ),
                    Port = Settings.Port,
                    EnableSsl = Settings.SSL
                };

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw new MixException(Heart.Enums.MixErrorStatus.ServerError, ex);
            }
        }

    }
}
