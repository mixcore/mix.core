using System;

namespace Mix.Lib.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GeneratedControllerAttribute: Attribute
    {
        public GeneratedControllerAttribute(string route, string name)
        {
            Route = route;
            Name = name;
        }

        public string Route { get; set; }

        public string Name { get; set; }
    }
}
