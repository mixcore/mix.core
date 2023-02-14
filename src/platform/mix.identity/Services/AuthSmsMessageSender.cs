using Mix.Identity.Interfaces;
using System.Threading.Tasks;

namespace Mix.Identity.Services
{
    public class AuthSmsMessageSender : ISmsSender
    {
        /// <summary>
        /// Sends the SMS asynchronous.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
