using Microsoft.Extensions.Configuration;
using nSeed.Models;
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
        
        public static List<DiskInformation> DiskInformation()
        {
            List<DiskInformation> diskInformationList = new List<DiskInformation>();
            
            IEnumerable<IConfigurationSection> DownloadRoots = _configuration.GetSection("nseed:downloadroots").GetChildren();
            
            foreach (var downloadRoot in DownloadRoots)
            {
                DriveInfo drive = new DriveInfo(downloadRoot.Value);
                long totalBytes = drive.TotalSize;
                long freeBytes = drive.AvailableFreeSpace;
                
                long totalBytesMb = (long)(totalBytes / Math.Pow(1024, 2));
                long freeBytesMb = (long)(freeBytes / Math.Pow(1024, 2));

                diskInformationList.Add(new DiskInformation(downloadRoot.Value, totalBytesMb, freeBytesMb));
            }

            return diskInformationList;
        }
    }
}
