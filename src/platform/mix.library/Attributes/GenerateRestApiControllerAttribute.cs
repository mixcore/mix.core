namespace Mix.Lib.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GenerateRestApiControllerAttribute : Attribute
    {
        public string Route { get; set; }

        public string Name { get; set; }

        public bool QueryOnly { get; set; }

        public bool IsAuthorized { get; set; }
    }
}
