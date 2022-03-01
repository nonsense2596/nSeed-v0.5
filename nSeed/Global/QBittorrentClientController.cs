using QBittorrent.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace nSeed.Global
{
    public class QBittorrentClientController
    {
        public static readonly QBittorrentClient qbittorrentclient;

        static QBittorrentClientController()
        {
            qbittorrentclient = new QBittorrentClient(new Uri("http:/localhost.localdomain:port"));
        }
    }
}
