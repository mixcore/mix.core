using System;

namespace Mix.Lib.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GeneratedControllerAttribute: Attribute
    {
        public GeneratedControllerAttribute(string route)
        {
            Route = route;
        }

        public string Route { get; set; }
    }
}
