using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nSeed.Models
{
    public class TorrentSearchResultData
    {
        public TorrentSearchResultData(string name, string categ, string uploadDate, string download, string seed, string leech, string torrentSize, string detail, string id, string downloadUrl)
        {
            Name = name;
            Categ = categ;
            UploadDate = uploadDate;
            Download = download;
            Seed = seed;
            Leech = leech;
            TorrentSize = torrentSize;
            Detail = detail;
            Id = id;
            DownloadUrl = downloadUrl;
        }
        public string Name { get; set; }
        public string Categ { get; set; }
        public string UploadDate { get; set; }
        public string Download { get; set; }
        public string Seed { get; set; }
        public string Leech { get; set; }
        public string TorrentSize { get; set; }
        public string Detail { get; set; }
        public string Id { get; set; }
        public string DownloadUrl { get; set; }

    }
}
