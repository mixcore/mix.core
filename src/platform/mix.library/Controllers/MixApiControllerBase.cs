using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Heart.Models;
using Mix.Lib.Services;

namespace Mix.Lib.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class MixApiControllerBase: ControllerBase
    {
        protected readonly ILogger<MixApiControllerBase> _logger;
        private readonly TranslatorService _translator;
        public MixApiControllerBase(
            ILogger<MixApiControllerBase> logger,
            MixService mixService,
            TranslatorService translator) : base()
        {
            _logger = logger;
            _translator = translator;
        }

        protected virtual ActionResult<T> GetResponse<T>(RepositoryResponse<T> result)
        {
            if (result.IsSucceed)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}
