using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Services.Ecommerce.Lib.Dtos
{
    public sealed class UpdateWarehouseDto
    {
        public string Sku { get; set; }
        public int Inventory { get; set; }
    }
}
