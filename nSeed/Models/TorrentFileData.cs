namespace nSeed.Models
{
    public class TorrentFileData
    {
        public TorrentFileData(string name, string size)
        {
            Name = name;
            Size = size;
        }

        public string Name { get; set; }
        public string Size { get; set; }
    }
}
