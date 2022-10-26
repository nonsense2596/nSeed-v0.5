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

            Uri uri = new Uri(_configuration["nseed:baseurl"] + _configuration["nseed:searchpath"]);
            var content = new FormUrlEncodedContent(tspd.GetPostParametersDict().ToEnumerable(true));
            var request = new HttpRequestMessage()
            {
                RequestUri = uri,
                Method = HttpMethod.Post,
                Content = content,
            };
            request.Headers.ExpectContinue = false;
            var response = await httpClient.SendAsync(request);
            string responseString = await response.Content.ReadAsStringAsync();

            List<TorrentSearchResultData> torrentsearchresultdata = TorrentSearchResultReader.read(responseString);
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
            catch (HttpRequestException e) 
            {

                return Content(e.Message);
            }

        }

        // downloads a specific torrent to the server
        [HttpPost("torrentdownload/{id}")]
        public async Task<StatusCodeResult> Torrentdownload(int id)
        {
            try
            {
                Uri uri = new Uri(_configuration["nseed:baseurl"] + _configuration["nseed:torrentspath"] + "?action=download&id=" + id + "&key=" + _configuration["nseed:key"]);
                var torrentToBeDownloaded = await httpClient.GetAsync(uri);

                using (var fs = new FileStream(_configuration["nseed:downloadpath"] + id + "_" + DateTime.Now.Ticks + ".torrent", FileMode.CreateNew))
                {
                    await torrentToBeDownloaded.Content.CopyToAsync(fs);
                }
                return StatusCode(201);
            }
            catch (HttpRequestException) {
                return StatusCode(500);
            }   
        }

    }
}
