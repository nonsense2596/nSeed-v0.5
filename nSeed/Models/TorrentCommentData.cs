using System;

namespace nSeed.Models
{
    public class TorrentCommentData
    {
        public TorrentCommentData(
            string author,
            DateTime date,
            string text)
        {
            Author = author;
            Date = date;
            Text = text;
        }

        public string Author { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
    }
}
