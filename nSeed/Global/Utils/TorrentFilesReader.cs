using HtmlAgilityPack;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Configuration;
using nSeed.Models;
using System.Collections.Generic;
using System.Linq;

namespace nSeed.Global.Utils
{
    public class TorrentFilesReader
    {
        public static IConfiguration _configuration;
        public static void InitConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static List<TorrentFileData> read(string res)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(res);

            List<TorrentFileData> tfd = new List<TorrentFileData>();
            
            var listrow_odd = doc.DocumentNode.SelectNodes("//tr[@class='listrow_odd']");
            var listrow_even = doc.DocumentNode.SelectNodes("//tr[@class='listrow_even']");

            var listrow_odd_filenames = listrow_odd.Select(el => el.SelectNodes("td[@width='780']/div"));
            var listrow_odd_filesizes = listrow_odd.Select(el => el.SelectNodes("td[@width='71']"));
            var listrow_odd_filenames_and_filesizes = listrow_odd_filenames.Zip(listrow_odd_filesizes, (name, size) => new { name, size });
            foreach (var item in listrow_odd_filenames_and_filesizes)
            {
                tfd.Add(new TorrentFileData(item.name.First().InnerText, item.size.First().InnerText));
            }


            var listrow_even_filenames = listrow_even.Select(el => el.SelectNodes("td[@width='780']/div"));
            var listrow_even_filesizes = listrow_even.Select(el => el.SelectNodes("td[@width='71']"));
            var listrow_even_filenames_and_filesizes = listrow_even_filenames.Zip(listrow_even_filesizes, (name, size) => new { name, size });
            foreach (var item in listrow_even_filenames_and_filesizes)
            {
                tfd.Add(new TorrentFileData(item.name.First().InnerText, item.size.First().InnerText));
            }

            return tfd;
        }
    }
}
