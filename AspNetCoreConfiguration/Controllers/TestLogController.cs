using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCoreConfiguration.Controllers
{
    public class TestLogController : Controller
    {
        private readonly ILogger<TestLogController> _logger;

        public TestLogController(ILogger<TestLogController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogError("This error occurs.");
            return View();
        }
    }
}
