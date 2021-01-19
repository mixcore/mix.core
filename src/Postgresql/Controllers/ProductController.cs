using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Postgresql.Infrastructures;
using Postgresql.Infrastructures.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Postgresql.Controllers
{
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        [HttpPost]
        public async Task Create()
        {
            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = "1234567890",
                CreatedDate = DateTime.Now
            };

            using (var dbContext = new PostgresDbContext())
            {
                dbContext.Add<Product>(newProduct);
                await dbContext.SaveChangesAsync();
            }
        }

        [HttpGet]
        public async Task<List<Product>> Get()
        {
            var products = new List<Product>();

            using (var dbContext = new PostgresDbContext())
            {
                products.AddRange(await dbContext.Products.ToListAsync());
            }

            return products;
        }
    }
}
