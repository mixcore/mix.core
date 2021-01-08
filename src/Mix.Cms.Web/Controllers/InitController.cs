﻿using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class InitController : BaseController
    {

        [HttpGet]
        [Route("init")]
        [Route("init/{page}")]
        public IActionResult Index(string page)
        {
            if (!MixService.GetConfig<bool>("IsInit"))
            {
                return Redirect("/");
            }
            else
            {
                page = page ?? "";
                var initStatus = MixService.GetConfig<int>("InitStatus");
                switch (initStatus)
                {
                    case 0:
                        if (page.ToLower() != "")
                        {
                            return Redirect($"/init");
                        }
                        break;

                    case 1:
                        if (page.ToLower() != "step2")
                        {
                            return Redirect($"/init/step2");
                        }
                        break;

                    case 2:
                        if (page.ToLower() != "step3")
                        {
                            return Redirect($"/init/step3");
                        }
                        break;

                    case 3:
                        if (page.ToLower() != "step4")
                        {
                            return Redirect($"/init/step4");
                        }
                        break;

                    case 4:
                        if (page.ToLower() != "step5")
                        {
                            return Redirect($"/init/step5");
                        }
                        break;
                }
                return View();
            }
        }
    }
}