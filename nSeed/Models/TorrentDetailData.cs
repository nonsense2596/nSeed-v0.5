using System;

namespace nSeed.Models
{
    public class TorrentDetailData
    {
        public TorrentDetailData(
            string typePlain, 
            string type, 
            DateTime uploadDate, 
            string uploader, 
            string commentNum, 
            int seeders, 
            int leechers, 
            string downloads, 
            string downloadSpeed, 
            string size)
        {
            TypePlain = typePlain;
            Type = type;
            UploadDate = uploadDate;
            Uploader = uploader;
            CommentNum = commentNum;
            Seeders = seeders;
            Leechers = leechers;
            Downloads = downloads;
            DownloadSpeed = downloadSpeed;
            Size = size;
        }

        public string TypePlain { get; set; }
        public string Type { get; set; }
        public DateTime UploadDate { get; set; }
        public string Uploader { get; set; }
        public string CommentNum { get; set; }
        public int Seeders { get; set; }
        public int Leechers { get; set; }
        public string Downloads { get; set; }
        public string DownloadSpeed { get; set; }
        public string Size { get; set; }

    }
}
