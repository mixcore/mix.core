using System.Threading.Tasks;

namespace Mix.Identity.Interfaces
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message);
    }
}
