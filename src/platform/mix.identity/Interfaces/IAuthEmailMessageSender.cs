using System.Threading.Tasks;

namespace Mix.Identity.Interfaces
{
    public interface IAuthEmailMessageSender
    {
        public Task SendEmailAsync(string email, string subject, string message);
    }
}
