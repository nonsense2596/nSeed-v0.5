using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using nSeed.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Linq;

namespace nSeed.Global.Utils
{
    public class TorrentPartsReader
    {
        public static IConfiguration _configuration;
        public static void InitConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static List<TorrentCommentData> readComments(string res)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(res);

            List<TorrentCommentData> tcd = new List<TorrentCommentData>();

            var comment_container = doc.DocumentNode.SelectNodes("//div[@class='hsz_block']");
            foreach (var comment in comment_container)
            {
                var author = "";
                try
                {
                    author = comment.SelectSingleNode(".//div[@class='hsz_jobb_felso_txt']/a[contains(@href,'profile.php')]").InnerText;
                }
                catch (Exception e)
                {
                    author = "Torolt felhasznalo";
                }

                var date_outer = comment.SelectSingleNode(".//div[@class='hsz_jobb_felso_date']").InnerText;
                var regex = new Regex(@"\;(.*)\|");
                Match match = regex.Match(date_outer);
                var date = match.Groups[1].Value;

                var text = comment.SelectSingleNode(".//div[@class='hsz_jobb_comment']").InnerHtml;
                tcd.Add(new TorrentCommentData(author, DateTime.Parse(date), text));
            }


            return tcd;
        }

        public static TorrentDetailData readDetails(string res)
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

        public static TorrentDropdownData readDropdownInfo(string res)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(res);

            var outercontainer = doc.DocumentNode.SelectSingleNode("//div[@class='torrent_lenyilo_tartalom']");

            try
            {
                outercontainer.SelectSingleNode("//table[@class='torrent_kep_ico']").Remove();
                foreach (var node in outercontainer.SelectNodes("//div[@class='hr_stuff']"))
                {
                    node.Remove();
                }
                outercontainer.SelectSingleNode("//div[@class='banner']").Remove();
            }
            catch (Exception e)
            {
                // let it fail silently
            }

            var innerHTML = outercontainer.InnerHtml;
            // TODO change címkék to nseed címkék link (eg torrents.php?tags=misztikus)
            // TODO eventually add pictures
            // TODO proper error handling
            return null;
        }

        public static int readCommentCount(string res)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(res);

            var commentNumberContainer = doc.DocumentNode.SelectSingleNode("//div[@class='t_comments_txt']//a").InnerText;
            var regex = new Regex(@"\((.*)\)");
            var commentNumber = int.Parse(regex.Match(commentNumberContainer).Groups[1].Value);

            return commentNumber;
        }

        public static List<TorrentFileData> readFileList(string res)
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

        public static TorrentNFOData readNFO(string res)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(res);

            var nfoData = doc.DocumentNode.SelectNodes("//pre").First().InnerHtml;

            return new TorrentNFOData(nfoData);
        }

        public static List<TorrentSearchResultData> readSearchResults(string res)
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
                var downloadurl = string.Format(_configuration["nseed:torrentdownloadurltemplate"], id, _configuration["nseed:key"]);

                result.Add(new TorrentSearchResultData(name, categ, uploaddate, download, seed, leech, torrentsize, detail, id, downloadurl));
            }

            return result;
        }
    }
}
