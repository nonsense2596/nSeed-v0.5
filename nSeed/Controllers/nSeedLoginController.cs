using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        // constructor
        public nSeedLoginController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            httpClient = _httpClientFactory.CreateClient("myhttpclient");
            _configuration = configuration;
        }

        // checks whether a session is already available and usable
        // does this by trying to reach the main index url of the site and checks for redirects
        // if it gets redirected to login.php, then no sessions are available
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

        // returns a new session
        // if there are no sessions available, logs in and creates a new, otherwise returns the active one
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
                                                                                  // TODO DO SMTHING WITH THIS, MIGHT NOT BE NECESSARY THO
                }
                catch (HttpRequestException) { return false; }
            }
            return true;

        }


        // invokes the login sequence and redirects to the search page if successful
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            if (await DoLogin()) return RedirectToAction("Torrents", "Torrents");
            return Content("failed login");

        }



        //[HttpGet("Index2")]
        //public async Task<IActionResult> Index2()
        //{
            
        //    try
        //    {

        //        var response3 = await httpClient.GetAsync(new Uri("https://ncore.pro/torrents.php?action=download&id=3307590&key=56a267f2b0d7f1e2f6d3cc52d7890bdc"));
                
        //        using (var fs = new FileStream(_configuration["nseed:downloadpath"], FileMode.CreateNew))
        //        {
        //            await response3.Content.CopyToAsync(fs);
        //        }
        //        var response2 = await httpClient.GetAsync(_configuration["nseed:baseurl"] + _configuration["nseed:indexpath"]);
        //        string res2 = await response2.Content.ReadAsStringAsync();
        //        return Content(res2);
        //    }
        //    catch (HttpRequestException) { }


        //    return Content("succ2");

        }


}
