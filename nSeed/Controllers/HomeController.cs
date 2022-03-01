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

        public IActionResult Index()
        {
            return View();
        }
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

        [HttpGet("Torrents")]
        public IActionResult Torrents()
        {
            return View();
        }

        [HttpPost("Torrentss")]
        public async Task<IActionResult> Torrentss([FromForm] TorrentSearchPostData tspd)
        {

            //var pairs3 = new NameValueCollection
            //    {
            //        {"nyit_sorozat_resz","true"},    // TODO
            //        {"kivalasztott_tipus[]","hdser,hdser_hun"},
            //        {"mire","the+walking+dead"},
            //        {"miben","name"},
            //        {"tipus","kivalasztottak_kozott"},
            //        {"submit.x", "1"},
            //        {"submit.y", "1"},
            //        {"tags",""} // TODO future
            //    };

            var dictt = tspd.GetPostParametersDict();



            Uri uri4 = new Uri(_configuration["nseed:baseurl"]+_configuration["nseed:searchpath"]);
            var content4 = new FormUrlEncodedContent(tspd.GetPostParametersDict().ToEnumerable(true));
            var request4 = new HttpRequestMessage()
            {
                RequestUri = uri4,
                Method = HttpMethod.Post,
                Content = content4,
            };
            request4.Headers.ExpectContinue = false;
            var response3 = await httpClient.SendAsync(request4);
            string res3 = await response3.Content.ReadAsStringAsync();
            return Content(res3, "text/html");


        }


    }
}
