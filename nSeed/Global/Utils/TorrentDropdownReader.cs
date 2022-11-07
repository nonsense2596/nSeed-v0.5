using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using nSeed.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace nSeed.Global.Utils
{
    public class TorrentDropdownReader
    {
        public static IConfiguration _configuration;
        public static void InitConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static TorrentDropdownData read(string res)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(res);

            var outercontainer = doc.DocumentNode.SelectSingleNode("//div[@class='torrent_lenyilo_tartalom']");

            try
            {
                outercontainer.SelectSingleNode("//table[@class='torrent_kep_ico']").Remove();
                foreach(var node in outercontainer.SelectNodes("//div[@class='hr_stuff']"))
                {
                    node.Remove();
                }
                outercontainer.SelectSingleNode("//div[@class='banner']").Remove();
            } catch (Exception e) { 
                // let it fail silently
            }

            var innerHTML = outercontainer.InnerHtml;
            // TODO change címkék to nseed címkék link (eg torrents.php?tags=misztikus)
            // TODO eventually add pictures
            // TODO proper error handling
            return null;
        }

        public static int commentCount(string res)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(res);

            var commentNumberContainer = doc.DocumentNode.SelectSingleNode("//div[@class='t_comments_txt']//a").InnerText;
            var regex = new Regex(@"\((.*)\)");
            var commentNumber = int.Parse(regex.Match(commentNumberContainer).Groups[1].Value);

            return commentNumber;
        }
    }
}
