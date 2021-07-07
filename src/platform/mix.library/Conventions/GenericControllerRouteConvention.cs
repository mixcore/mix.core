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
                var customNameAttribute = genericType.GetCustomAttribute<GenerateRestApiControllerAttribute>();

                if (customNameAttribute != null)
                {
                    string moduleName = genericType.Assembly.GetName().Name.Replace(".", "-");
                    string modelName = genericType.Name.Replace("ViewModel", string.Empty);
                    string route = customNameAttribute.Route ?? $"api/v2/rest/{moduleName}/{modelName.ToHypenCase()[1..]}";
                    string name = customNameAttribute.Name ?? modelName.ToHypenCase(' ', false);
                    if (!controller.Selectors.Any(s => s.AttributeRouteModel?.Template == route))
                    {
                        controller.Selectors.Add(new SelectorModel
                        {
                            AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(route)),
                        });
                        controller.ControllerName = $"{moduleName.ToTitleCase()} - {name} (Auto Generate)";
                    }
                }
                else
                {
                    controller.ControllerName = genericType.Name;
                }
            }
        }
    }
}
