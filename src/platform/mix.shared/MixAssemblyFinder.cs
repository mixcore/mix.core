using System.Reflection;

namespace Mix.Shared
{
    public class MixAssemblyFinder
    {
        public static List<Assembly> GetMixAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(IsMixModule()).ToList();
        }

        private static Func<Assembly, bool> IsMixModule()
        {
            return p => p.FullName.StartsWith("mix");
        }
    }
}
