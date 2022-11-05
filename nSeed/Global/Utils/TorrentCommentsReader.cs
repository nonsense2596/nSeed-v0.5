using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using nSeed.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace nSeed.Global.Utils
{
    public class TorrentCommentsReader
    {
        public static IConfiguration _configuration;
        public static void InitConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static List<TorrentCommentData> read(string res)
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
    }
}
