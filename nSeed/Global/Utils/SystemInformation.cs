using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace nSeed.Global.Utils
{
    public class SystemInformation
    {
        public static IConfiguration _configuration;
        public static void InitConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public static List<Tuple<long, long, int>> PrintDiskSpace()
        {
            List<Tuple<long, long, int>> diskInfoTuples = new List<Tuple<long,long,int>>();
            IEnumerable<IConfigurationSection> DownloadRoots = _configuration.GetSection("nseed:downloadroots").GetChildren();
            foreach (var DownloadRoot in DownloadRoots)
            {
                DriveInfo drive = new DriveInfo(DownloadRoot.Value);
                long totalBytes = drive.TotalSize;
                long freeBytes = drive.AvailableFreeSpace;

                int freePercent = (int)((100 * freeBytes) / totalBytes);

                long totalBytesGb = (long)(totalBytes / Math.Pow(1024, 3));
                long freeBytesGb = (long)(freeBytes / Math.Pow(1024, 3));

                diskInfoTuples.Add(Tuple.Create(totalBytesGb, freeBytesGb, freePercent));
            }

            return diskInfoTuples;
        }
    }
}
