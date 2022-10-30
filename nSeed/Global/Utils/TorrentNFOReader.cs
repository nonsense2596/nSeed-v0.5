using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using nSeed.Models;
using System.Linq;

namespace nSeed.Global.Utils
{
    public class TorrentNFOReader
    {
        public static IConfiguration _configuration;
        public static void InitConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static TorrentNFOData read(string res)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(res);
            
            var nfoData = doc.DocumentNode.SelectNodes("//pre").First().InnerHtml;

            return new TorrentNFOData(nfoData);
        }
    }
}
