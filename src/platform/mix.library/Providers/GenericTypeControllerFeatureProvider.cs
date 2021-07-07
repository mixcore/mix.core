using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Mix.Lib.Attributes;
using Mix.Lib.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mix.Lib.Providers
{
    public class GenericTypeControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public List<Type> Candidates { get; set; }

        public GenericTypeControllerFeatureProvider(List<Type> candidates)
        {
            Candidates = candidates;
            
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            foreach (var candidate in Candidates)
            {
                if (candidate.BaseType.IsGenericType)
                {
                    var attr = candidate.GetCustomAttribute<GenerateRestApiControllerAttribute>();
                    var baseType = attr.IsRestful
                            ? typeof(MixAutoGenerateRestApiController<,,,>)
                            : typeof(MixAutoGenerateQueryApiController<,,,>);
                    Type[] types = candidate.BaseType.GenericTypeArguments.Prepend(candidate).ToArray();
                    feature.Controllers.Add(
                        baseType.MakeGenericType(types)
                            .GetTypeInfo()
                    );
                }
            }
        }
    }
}
