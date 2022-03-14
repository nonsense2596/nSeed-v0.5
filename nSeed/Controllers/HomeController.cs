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

        // after we log in, we get redirected to this, which is just a static page with the search functionality
        [HttpGet("Torrents")]
        public IActionResult Torrents()
        {
            return View("TorrentSearch");
        }

        // the Torrents() function posts to this function, this gets its params, and proxies them to nseed, getting the resulting page from there
        [HttpPost("TorrentSearch")]
        public async Task<IActionResult> TorrentSearch([FromForm] TorrentSearchPostData tspd)
        {


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

            List<TorrentSearchResultData> torrentsearchresultdata = TorrentSearchResultReader.read(res3);
            ViewData["torrentdata"] = torrentsearchresultdata;
            return View();

        }



        // CURRENT WRITING FRONT HERE

        [HttpGet("torrentdetails/{id}")]
        public async Task<IActionResult> torrentdetails(int id)
        {
            // MAKE A REQUEST HERE TO NSEED AND RETURN A CONTENT WITH TEXT/HTML WITH THE PARSING OF IT
            return View("kek");
        }
        [HttpPost("torrentdownload/{id}")]
        public async Task<IActionResult> torrentdownload(int id)
        {

            try
            {

                Uri uri = new Uri(_configuration["nseed:baseurl"] + _configuration["nseed:torrentspath"] + "?action=download&id=" + id + "&key=" + _configuration["nseed:key"]);
                var response3 = await httpClient.GetAsync(uri);

                using (var fs = new FileStream(_configuration["nseed:downloadpath"], FileMode.CreateNew))
                {
                    await response3.Content.CopyToAsync(fs);
                }
                var response2 = await httpClient.GetAsync(_configuration["nseed:baseurl"] + _configuration["nseed:indexpath"]);
                string res2 = await response2.Content.ReadAsStringAsync();
                return Content(res2);
            }
            catch (HttpRequestException) { }


            return Content("success");
        }

    }
}
