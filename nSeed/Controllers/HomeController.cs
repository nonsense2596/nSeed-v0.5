using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using nSeed.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Collections.Specialized;
using nSeed.Global.Utils;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace nSeed.Controllers
{

    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        private HttpClient httpClient;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            httpClient = _httpClientFactory.CreateClient("myhttpclient");
            _configuration = configuration;
        }

        // landing page of the web application, displays the nseed login button
        public IActionResult Index()
        {
            ViewData["StorageInfo"] = SystemInformation.DiskInformation();

            return View();
        }

        // admin-only test page
        [Authorize(Policy = "RequireAdministratorRole")]
        public IActionResult Test()
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
