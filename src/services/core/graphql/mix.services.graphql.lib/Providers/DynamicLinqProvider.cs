using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Reflection;

namespace Mix.Services.Graphql.Lib.Providers
{
    public class DynamicLinqProvider : IDynamicLinkCustomTypeProvider
    {
        public HashSet<Type> GetCustomTypes()
        {
            HashSet<Type> types = new HashSet<Type>();
            types.Add(typeof(EF));
            return types;
        }

        public Dictionary<Type, List<MethodInfo>> GetExtensionMethods()
        {
            throw new NotImplementedException();
        }

        public Type ResolveType(string typeName)
        {
            throw new NotImplementedException();
        }

        public Type ResolveTypeBySimpleName(string simpleTypeName)
        {
            throw new NotImplementedException();
        }
    }
}
