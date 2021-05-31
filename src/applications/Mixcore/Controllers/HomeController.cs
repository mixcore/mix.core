using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Lib.Services;
using Mixcore.Models;
using System.Diagnostics;

namespace Mixcore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TranslatorService _translator;
        public HomeController(ILogger<HomeController> logger, 
            TranslatorService translator)
        {
            _logger = logger;
            _translator = translator;
            TranslatorService.Translate<string>("Ok", "en-us");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
