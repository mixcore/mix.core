using Microsoft.Extensions.Configuration;
using Mix.Identity.Abstracts;

namespace Mix.Messenger
{
    public class Startup : IdentityStartupModule
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
