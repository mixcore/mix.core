using Mix.Lib.Abstracts;
using System.Reflection;

namespace Mix.Theme.Domain
{
    public class StartupService : StartupApi
    {
        public StartupService(Assembly assembly) : base(assembly)
        {
        }
    }
}
