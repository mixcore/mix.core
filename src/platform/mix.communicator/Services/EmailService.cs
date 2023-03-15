using Microsoft.Extensions.Configuration;
using Mix.Communicator.Models;
using Mix.Heart.Exceptions;
using Mix.Service.Services;
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
            MailMessage mailMessage = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(msg.From ?? Settings.From)
            };
            mailMessage.To.Add(msg.To);
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
