using System;

namespace Mix.Lib.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GenerateRestApiControllerAttribute: Attribute
    {
        public GenerateRestApiControllerAttribute(bool isRestful = true)
        {
            IsRestful = isRestful;
        }
        public GenerateRestApiControllerAttribute(string route, string name, bool isRestful = true)
        {
            Route = route;
            Name = name;
            IsRestful = isRestful;
        }

        public string Route { get; set; }

        public string Name { get; set; }

        public bool IsRestful{ get; set; }
    }
}
