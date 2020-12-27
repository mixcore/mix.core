using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Api.GraphQL.Infrastructure
{
    public class NameFieldResolver : IFieldResolver
    {
        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName)?.GetValue(src, null);
        }

        public object Resolve(IResolveFieldContext context)
        {
            var source = context.Source;
            if (source == null)
            {
                return null;
            }
            var name = Char.ToUpperInvariant(context.FieldAst.Name[0]) + context.FieldAst.Name.Substring(1);
            var value = GetPropValue(source, name);
            //if (value == null)
            //{
            //    throw new InvalidOperationException($"Expected to find property {context.FieldAst.Name} on {context.Source.GetType().Name} but it does not exist.");
            //}
            return value;
        }
    }
}
