using Microsoft.AspNetCore.Http;
using Mix.Database.Entities.Cms;
using Mix.Heart.UnitOfWork;
using Mix.Services.Databases.Lib.Abtracts;
using Mix.Services.Databases.Lib.Services;
using Mix.Services.Ecommerce.Lib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Services.Ecommerce.Lib.Services
{
    public sealed class ProductService : MixPostServiceBase<ProductViewModel>
    {
        public ProductService(UnitOfWorkInfo<MixCmsContext> uow, MixMetadataService metadataService, IHttpContextAccessor httpContextAccessor) : base(uow, metadataService, httpContextAccessor)
        {
        }
    }
}
