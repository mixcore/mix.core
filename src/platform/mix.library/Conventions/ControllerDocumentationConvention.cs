using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Mix.Heart.Extensions;

namespace Mix.Lib.Conventions
{
    public class ControllerDocumentationConvention : IControllerModelConvention
    {
        void IControllerModelConvention.Apply(ControllerModel controller)
        {
            if (controller == null)
                return;

            if (controller.Attributes.Any(m => m.GetType().Name == "ApiControllerAttribute"))
            {
                string moduleName = controller.ControllerType.Assembly.GetName().Name;
                string name = controller.ControllerName.ToHyphenCase(' ', false);
                controller.ControllerName = $"{moduleName.Replace("mix.", "").ToTitleCase()} - {name.Replace("Mix ", "")}";
            }
        }
    }
}
