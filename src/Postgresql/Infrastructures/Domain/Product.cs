using System;

namespace Postgresql.Infrastructures.Domain
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid CategoryId { get; set; }

        public bool IsPublic { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Category Category { get; set; }
    }
}
