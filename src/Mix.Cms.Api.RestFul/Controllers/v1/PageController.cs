using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Controllers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.MixPages;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Route("api/v1/rest/{culture}/page/portal")]
    [ApiController]
    public class PageController : BaseRestApiController<MixCmsContext, MixPage, ReadListItemViewModel>
    {
    }
}