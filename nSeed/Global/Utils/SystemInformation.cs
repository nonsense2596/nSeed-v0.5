using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace nSeed.Global.Utils
{
    public class SystemInformation
    {
        public static string PrintDiskSpace(string driveLetter = "C:\\")
        {
            DriveInfo drive = new DriveInfo(driveLetter);

            var totalBytes = drive.TotalSize;
            var freeBytes = drive.AvailableFreeSpace;

            var freePercent = (int)((100 * freeBytes) / totalBytes);

            long totalBytesGb = (long)(totalBytes / Math.Pow(1024, 3));
            long freeBytesGb = (long)(freeBytes / Math.Pow(1024, 3));

            return String.Format("{0}Gb/{1}Gb ({2}% free)", totalBytesGb, freeBytesGb, freePercent);
        }
    }
}
