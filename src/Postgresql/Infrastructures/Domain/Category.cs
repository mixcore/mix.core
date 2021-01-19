using System;
using System.Collections.Generic;

namespace Postgresql.Infrastructures.Domain
{
    public class Category
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsPublic { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
