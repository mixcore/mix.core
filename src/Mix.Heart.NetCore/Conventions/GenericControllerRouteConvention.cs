using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Mix.Heart.Extensions;
using Mix.Heart.NetCore.Attributes;
using System.Reflection;

namespace Mix.Heart.RestFul.Conventions
{
    public class GenericControllerRouteConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.IsGenericType)
            {
                // BaseController Generic Type <TDbContext, TModel, TView>
                var genericType = controller.ControllerType.GenericTypeArguments[2];
                var customNameAttribute = genericType.GetCustomAttribute<GeneratedControllerAttribute>();

                if (customNameAttribute?.Route != null)
                {
                    controller.Selectors.Add(new SelectorModel
                    {
                        AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(customNameAttribute.Route)),
                    });
                }
                else
                {
                    controller.ControllerName = genericType.Name.ToHypenCase();
                }
            }
        }
    }
}