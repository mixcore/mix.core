using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Mix.Heart.Extensions;
using Mix.Lib.Attributes;
using System.Linq;
using System.Reflection;


namespace Mix.Lib.Conventions
{
    public class GenericControllerRouteConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.IsGenericType)
            {
                var genericType = controller.ControllerType.GenericTypeArguments[0];
                var customNameAttribute = genericType.GetCustomAttribute<GeneratedControllerAttribute>();

                if (customNameAttribute?.Route != null)
                {
                    if (!controller.Selectors.Any(s => s.AttributeRouteModel?.Template == customNameAttribute.Route))
                    {
                        controller.Selectors.Add(new SelectorModel
                        {
                            AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(customNameAttribute.Route)),
                        });
                        controller.ControllerName = customNameAttribute.Name.ToHypenCase('_');
                    }
                }
                else
                {
                    controller.ControllerName = genericType.Name.ToHypenCase();
                }
            }
        }
    }
}
