using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Reflection;
using System.Threading.Tasks;

namespace Mix.Cms.Api.GraphQL.Infrastructure.Providers
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
