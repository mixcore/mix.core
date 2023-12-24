using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mix.Heart.Extensions;

namespace Mix.Services.Graphql.Lib.Resolvers
{
    public class NameFieldResolver : IFieldResolver
    {
        private static object? GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName)?.GetValue(src, null);
        }

        public async ValueTask<object?> ResolveAsync(IResolveFieldContext context)
        {
            var source = context.Source;
            if (source == null)
            {
                return default;
            }
            var name = context.FieldAst.Name.StringValue.ToTitleCase();
            var value = GetPropValue(source, name);
            //if (value == null)
            //{
            //    throw new InvalidOperationException($"Expected to find property {context.FieldAst.Name} on {context.Source.GetType().Name} but it does not exist.");
            //}
            return await Task.FromResult(value);
        }
    }
}
