using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using nSeed.Global.Utils;
using nSeed.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace nSeed.Controllers
{
    public class TorrentsController : Controller
    {
        private readonly ILogger<TorrentsController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        private HttpClient httpClient;

        public TorrentsController(ILogger<TorrentsController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            httpClient = _httpClientFactory.CreateClient("myhttpclient");
            _configuration = configuration;
        }

        // displays the torrent search interface
        [HttpGet("torrents")]
        public IActionResult Torrents()
        {
            return View("TorrentSearch");
        }

        // queries the seedy torrent site with the entered search options and returns the table containing the results
        [HttpPost("TorrentSearch")]
        public async Task<IActionResult> TorrentSearch([FromForm] TorrentSearchPostData tspd)
        {


            var dictt = tspd.GetPostParametersDict();

            Uri uri4 = new Uri(_configuration["nseed:baseurl"] + _configuration["nseed:searchpath"]);
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



        // returns details data about a specific torrent
        [HttpGet("torrentdetails/{id}")]
        public async Task<IActionResult> torrentdetails(int id)
        {

            try
            {
                Uri uri = new Uri(_configuration["nseed:baseurl"] + _configuration["nseed:detailspath"] + id);
                var response = await httpClient.GetAsync(uri);
                var content = await response.Content.ReadAsStringAsync();
                return Content(content);
            }
            catch (HttpRequestException) { }

            return Content("exception occured");

        }

        // downloads a specific torrent to the server
        [HttpPost("torrentdownload/{id}")]
        public async Task<IActionResult> torrentdownload(int id)
        {

            try
            {

                Uri uri = new Uri(_configuration["nseed:baseurl"] + _configuration["nseed:torrentspath"] + "?action=download&id=" + id + "&key=" + _configuration["nseed:key"]);
                var response3 = await httpClient.GetAsync(uri);

                using (var fs = new FileStream(_configuration["nseed:downloadpath"] + "torrent" + DateTime.Now.Ticks + ".torrent", FileMode.CreateNew))
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
