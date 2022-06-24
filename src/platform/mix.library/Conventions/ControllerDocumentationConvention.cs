using Microsoft.AspNetCore.Mvc.ApplicationModels;

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
                string name = controller.ControllerName.ToHypenCase(' ', false);
                controller.ControllerName = $"{moduleName} | {name}";
            }
        }
    }
}
