using System.Collections.Generic;

namespace Mix.Common.Domain.Models
{
    internal class RouteModel
    {
        public string Name { get; set; }
        public string Template { get; set; }
        public string Method { get; set; }
    }

    internal class RootResultModel
    {
        public List<RouteModel> Routes { get; set; }
        public int Total { get; set; }
    }
}
