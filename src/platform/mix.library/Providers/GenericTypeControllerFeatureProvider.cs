using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
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
        public Type BaseType { get; set; }

        public GenericTypeControllerFeatureProvider(List<Type> candidates, Type baseType = null)
        {
            Candidates = candidates;
            BaseType = baseType != null ? baseType : typeof(MixAutoGenerateRestApiController<,,,>);
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            foreach (var candidate in Candidates)
            {
                if (candidate.BaseType.IsGenericType)
                {
                    Type[] types = candidate.BaseType.GenericTypeArguments.Prepend(candidate).ToArray();
                    feature.Controllers.Add(
                        BaseType.MakeGenericType(types)
                            .GetTypeInfo()
                    );
                }
            }
        }
    }
}
