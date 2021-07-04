using Microsoft.Extensions.Configuration;
using Mix.Identity.Abstracts;

namespace Mix.Common
{
    public class Startup : IdentityStartupModule
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
