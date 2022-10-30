using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

            var outercontainer = doc.DocumentNode.SelectNodes("//div[@class='torrent_reszletek']").First();
            var col1 = outercontainer.SelectNodes("//div[@class='dd']");

            var typePlain = col1.Nodes().ElementAt(0).InnerHtml; // tipus magyar
            var type = col1.Nodes().ElementAt(2).InnerHtml; // tipus tipus
            var uploadDate = col1.Nodes().ElementAt(3).InnerHtml; // datum
            var uploader = col1.Nodes().ElementAt(5).InnerHtml; // feltolto
            var commentNum = col1.Nodes().ElementAt(7).InnerHtml; // hozzaszolasok
            var seeders = col1.Nodes().ElementAt(8).InnerHtml; // seederek
            var leechers = col1.Nodes().ElementAt(9).InnerHtml;  // leecherek
            var downloads = col1.Nodes().ElementAt(10).InnerHtml;  // letoltesek +++
            var downloadSpeed = col1.Nodes().ElementAt(11).InnerHtml;   // sebesseg becsult xmbps
            var size = col1.Nodes().ElementAt(12).InnerHtml;   // meret


            return new TorrentDetailData(
                typePlain, 
                type, 
                DateTime.Parse(uploadDate), 
                uploader, 
                commentNum,
                int.Parse(seeders),
                int.Parse(leechers), 
                downloads, 
                downloadSpeed, 
                size
            );
        }
    }
}
