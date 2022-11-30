using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mix.services.ecommerce.Domain.Dtos;
using mix.services.ecommerce.Domain.Entities;
using mix.services.ecommerce.Domain.Services;
using mix.services.ecommerce.Domain.ViewModels;
using Mix.Database.Entities.Cms;
using Mix.Database.Services;
using Mix.Heart.UnitOfWork;
using Mix.Identity.Constants;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Lib.ViewModels;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Service.Services;

namespace mix.services.ecommerce.Controllers
{
    [Route("api/ecommerce")]
    [ApiController]
    public class EcommerceController : MixTenantApiControllerBase
    {
        private readonly EcommerceService _ecommerceService;
        protected UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        public EcommerceController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration, MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService, 
            EcommerceService ecommerceService, UnitOfWorkInfo<MixCmsContext> cmsUOW)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, queueService)
        {
            _ecommerceService = ecommerceService;
            _cmsUOW = cmsUOW;
        }

        #region Routes

        [MixAuthorize]
        [HttpGet]
        [Route("my-shopping-cart")]
        public async Task<ActionResult<OrderViewModel>> MyShoppingOrder()
        {
            var cart = await _ecommerceService.GetOrCreateShoppingOrder(User, CancellationToken);
            return Ok(cart);
        }
        
        [MixAuthorize]
        [HttpPost]
        [Route("add-to-cart")]
        public async Task<ActionResult<OrderViewModel>> AddToCart(CartItemDto item)
        {
            var cart = await _ecommerceService.AddToCart(User, item, CancellationToken);
            return Ok(cart);
        }
        
        [MixAuthorize]
        [HttpPost]
        [Route("remove-from-cart")]
        public async Task<ActionResult> RemoveFromCart(CartItemDto item)
        {
            var cart = await _ecommerceService.RemoveFromCart(User, item, CancellationToken);
            return Ok(cart);
        }

        #endregion
    }
}
