using System;

namespace Mix.Lib.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GeneratePublisherAttribute: Attribute
    {
        public string Route { get; set; }

        public string Name { get; set; }

        public bool IsAuthorized{ get; set; }
    }
}
