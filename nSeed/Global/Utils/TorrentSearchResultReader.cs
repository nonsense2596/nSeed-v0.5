using nSeed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace nSeed.Global.Utils
{
    public static class TorrentSearchResultReader
    {
        public static IConfiguration _configuration;
        public static void InitConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static List<TorrentSearchResultData> read(string res)
        {
            List<TorrentSearchResultData> result = new List<TorrentSearchResultData>();

            var doc = new HtmlDocument();
            doc.LoadHtml(res);
            var innercontainers = doc.DocumentNode.SelectNodes("//div[@class='box_torrent']");
            foreach (var (item, index) in innercontainers.Select((value, i) => (value, i)))
            {
                var doc2 = new HtmlDocument();
                doc2.OptionEmptyCollection = true;
                doc2.LoadHtml(item.InnerHtml);

                string name = doc2.DocumentNode.SelectNodes("//nobr").First().InnerText;
                string categ = doc2.DocumentNode.SelectNodes("//img[@class='categ_link']").First().GetAttributeValue("alt", "-");
                string uploaddate = doc2.DocumentNode.SelectNodes("//div[@class='box_feltoltve2']").First().InnerText;
                string download = doc2.DocumentNode.SelectNodes("//div[@class='box_d2']").First().InnerText;
                string seed = doc2.DocumentNode.SelectNodes("//div[@class='box_s2']").First().InnerText;
                string leech = doc2.DocumentNode.SelectNodes("//div[@class='box_l2']").First().InnerText;
                string torrentsize = doc2.DocumentNode.SelectNodes("//div[@class='box_meret2']").First().InnerText;
                string? detail1 = doc2.DocumentNode.SelectNodes("//div[@class='torrent_txt']/a[@href]").FirstOrDefault()?.GetAttributeValue("href", "-");
                string? detail2 = doc2.DocumentNode.SelectNodes("//div[@class='torrent_txt2']/a[@href]").FirstOrDefault()?.GetAttributeValue("href", "-");
                string detail = detail1 == null ? detail2 : detail1;
                var id1 = doc2.DocumentNode.SelectNodes("//div[@class='torrent_txt']/a[@onclick]").FirstOrDefault()?.GetAttributeValue("onclick", "-");
                var id2 = doc2.DocumentNode.SelectNodes("//div[@class='torrent_txt2']/a[@onclick]").FirstOrDefault()?.GetAttributeValue("onclick", "-");
                string id = "";
                var regex = new Regex(@"\(([0-9]*)\)");
                if (id1 != null)
                {
                    Match match = regex.Match(id1);
                    id = match.Groups[1].Value;
                }
                if (id2 != null)
                {
                    Match match = regex.Match(id2);
                    id = match.Groups[1].Value;
                }
                var downloadurl = "torrents.php?action=download&id=" + id + "&key=" + _configuration["nseed:key"];

                result.Add(new TorrentSearchResultData(name, categ, uploaddate, download, seed, leech, torrentsize, detail, id, downloadurl));
            }

            return result;
        }
    }
}
