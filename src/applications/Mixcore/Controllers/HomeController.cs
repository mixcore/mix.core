using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Lib.Services;
using Mixcore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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
            _translator.Translate<string>("Ok", "en-us");
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
