using System;

namespace Mix.Lib.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GenerateRestApiControllerAttribute: Attribute
    {
        public GenerateRestApiControllerAttribute(string route = null, string name = null, bool isRestful = true, bool isMultiLanguage = false)
        {
            Route = route;
            Name = name;
            IsRestful = isRestful;
            IsMultiLanguage = IsMultiLanguage;
        }

        public string Route { get; set; }

        public string Name { get; set; }

        public bool IsRestful{ get; set; }

        public bool IsMultiLanguage { get; set; }
    }
}
