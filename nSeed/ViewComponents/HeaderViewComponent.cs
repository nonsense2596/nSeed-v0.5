using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace nSeed.ViewComponents
{
    public class HeaderViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewData["StorageInfo"] = "";

            // asd+= String.Format("{0}Gb/{1}Gb ({2}% free)", totalBytesGb, freeBytesGb, freePercent);
            List<Tuple<long,long,int>> diskSpaces = Global.Utils.SystemInformation.PrintDiskSpace();
            foreach (var diskSpace in diskSpaces)
            {
                ViewData["StorageInfo"] += String.Format("{0}Gb/{1}Gb ({2}% free), ", diskSpace.Item1, diskSpace.Item2, diskSpace.Item3);
            }
            return View("Default");
        }
    }
}


// https://docs.microsoft.com/en-us/answers/questions/343958/how-to-check-avilable-free-disk-space-drive-on-lin.html
// https://stackoverflow.com/questions/51390486/net-core-find-free-disk-space-on-different-os