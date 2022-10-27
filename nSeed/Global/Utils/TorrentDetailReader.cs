using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using nSeed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nSeed.Global.Utils
{
    public static class TorrentDetailReader
    {
        public static IConfiguration _configuration;
        public static void InitConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static TorrentDetailData read(string res)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(res);

            return null;
        }
    }
}
