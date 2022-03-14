using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace nSeed
{
    [Route("api/[controller]")]
    [ApiController]
    public class nSeedLoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _configuration;

        private HttpClient httpClient;

        public nSeedLoginController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            httpClient = _httpClientFactory.CreateClient("myhttpclient");
            _configuration = configuration;
        }

        // KONNEKCIÓ AKKOR KELL AMIKOR BELÉPÜNK AZ OLDALRA ÉS KILISTÁZZUK A SHITEKET, A LETÖLTÉS UTÁNA BÁRMIKOR KELLHET MERT A LINKEK
        // /torrents.php?action=download&id=XYZ&key=XYZXYZXYZ &key=XYZ része konstans egy accounthoz

        public async Task<bool> IsSessionAvailable()
        {
            var cookies = HttpContext.RequestServices.GetService(typeof(CookieContainer)) as CookieContainer;
            Debug.WriteLine(cookies.GetCookieHeader(new Uri(_configuration["nseed:baseurl"])));
            Debug.WriteLine("cookie num: " + cookies.GetCookies(new Uri(_configuration["nseed:baseurl"])).Count);
            if (cookies.GetCookies(new Uri(_configuration["nseed:baseurl"])).Count == 0)
            {
                var response = await httpClient.GetAsync(_configuration["nseed:baseurl"]+ _configuration["nseed:indexpath"]);
                string res = await response.Content.ReadAsStringAsync();

                using (HttpContent contentt = response.Content)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Found)
                    {
                        HttpResponseHeaders headers = response.Headers;
                        if (headers != null && headers.Location != null)
                        {
                            string redirectedUrl = headers.Location.ToString();
                            Debug.WriteLine("redurl: " + redirectedUrl);
                            if (redirectedUrl.Contains("login.php"))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public async Task<bool> DoLogin()
        {
            if (! (await IsSessionAvailable()))
            {
                var loginlink = _configuration["nseed:baseurl"] + _configuration["nseed:loginpath"];

                var pairs = new Dictionary<string, string>
                {
                    {"nev", _configuration["nseed:nev"]},
                    {"pass",  _configuration["nseed:pass"]},
                    {"ne_leptessen_ki", "1"},
                    {"set_lang", "en"},
                    {"submitted", "1"},
                    {"submit", "Access!"}
                };
                Uri uri = new Uri(loginlink);
                try
                {
                    var content = new FormUrlEncodedContent(pairs);
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = uri,
                        Method = HttpMethod.Post,
                        Content = content,
                    };
                    request.Headers.ExpectContinue = false;
                    var response = await httpClient.SendAsync(request);           // DISABLE NSEED LOGIN
                    string res = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException) { return false; }
            }
            return true;

        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            if (await DoLogin()) return RedirectToAction("Torrents", "Home");
            return Content("failed login");
            //return RedirectToAction("Torrents", "Home");
            try
            {


                // TRY TO DOWNLOAD A FILE WITH THIS CODE
                //var response3 = await httpClient.GetAsync(new Uri(".....torrents.php?action=download&id=XYZXYZ&key=XYZXYZXYZ"));
                //using (var fs = new FileStream(@"D:\Downloads\das.torrent", FileMode.CreateNew))
                //{
                //    await response3.Content.CopyToAsync(fs);
                //}









                // TRY TO GET TORRENT SEARCH RESULTS
                // TODO INNEN KB. MINDENT ATDOLGOZNI
                // TODO /torrents.php?oldal=i.... a több oldalhoz
                // https://......./ajax.php?action=torrent_drop&id=XYZXYZ a torrent további infók begyűjtéséhez
                // maga a download link, ami a //torrents.php?action=download&id=XYZXYZ&key=ASDASD kikövetkeztethető a fő lista id-jeiből is, de így több infót kapunk
                //var pairs3 = new Dictionary<string, string>
                //{
                //    {"nyit_filmek_resz","true"},    // TODO
                //    {"kivalasztott_tipus[]","hd"},
                //    {"mire","frozen"},
                //    {"miben","name"},
                //    {"tipus","kivalasztottak_kozott"},
                //    {"submit.x", "1"},
                //    {"submit.y", "1"},
                //    {"tags",""}
                //};
                //Uri uri3 = new Uri("https://........./torrents.php");
                //var content3 = new FormUrlEncodedContent(pairs3);
                //var request3 = new HttpRequestMessage()
                //{
                //    RequestUri = uri3,
                //    Method = HttpMethod.Post,
                //    Content = content3,
                //};
                //request3.Headers.ExpectContinue = false;
                //var response3 = await httpClient.SendAsync(request3);
                //string res3 = await response3.Content.ReadAsStringAsync();
                //Debug.WriteLine(res3);
                //return Content(res3,"text/html");


                // TRY TO REDIRECT TO OTHER CONTROLLER AND METHOD AND SUCH...
                //return RedirectToAction("Torrents","Home");




                // to return webpage like stuff
                // public async Task<IActionResult> Index()
                // return Content(res2,"text/html");


                /*QBittorrentClientController qbcc = new QBittorrentClientController();
                await QBittorrentClientController.qbittorrentclient.LoginAsync("username", "pass");
                var ver = await QBittorrentClientController.qbittorrentclient.GetQBittorrentVersionAsync();
                Debug.WriteLine("keksz");
                yield return ver.ToString();*/
                //yield return "asd";

            }
            catch (HttpRequestException) { }


           


        }

        [HttpGet("Index2")]
        public async Task<IActionResult> Index2()
        {
            //var httpClient = _httpClientFactory.CreateClient("myhttpclient");


            try
            {


                //var response3 = await httpClient.GetAsync(new Uri("https://......../torrents.php?action=download&id=XYZXYZ&key=XYZXYZXYZ"));
                //using (var fs = new FileStream(@"D:\Downloads\dass2.torrent", FileMode.CreateNew))
                //{
                //    await response3.Content.CopyToAsync(fs);
                //}
                var response2 = await httpClient.GetAsync(_configuration["nseed:baseurl"] + _configuration["nseed:indexpath"]);
                string res2 = await response2.Content.ReadAsStringAsync();
                return Content(res2);
            }
            catch (HttpRequestException) { }


            return Content("succ2");

        }




    }
}
