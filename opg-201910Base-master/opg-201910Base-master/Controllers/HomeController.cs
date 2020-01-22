using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using opg_201910_interview.Business.Interface;
using opg_201910_interview.Models;

namespace opg_201910_interview.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileProcess _fileProcess;

        public HomeController(ILogger<HomeController> logger, IFileProcess fileProcess)
        {
            _logger = logger;
            _fileProcess = fileProcess;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetFiles")]
        public IActionResult GetFiles() { 
            var result = _fileProcess.GetUploadFiles();
            if (!result.Any()) {
                return NotFound();
            }

            return Ok(result);
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
