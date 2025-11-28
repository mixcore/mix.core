using System.Reflection;

namespace Mix.Shared
{
    public class MixAssemblyFinder
    {
        public static List<Assembly> GetAssembliesByPrefix(string prefix)
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(IsSelectedModule(prefix)).ToList();
        }

        private static Func<Assembly, bool> IsSelectedModule(string prefix)
        {
            return p => p.FullName.StartsWith(prefix);
        }
    }
}
