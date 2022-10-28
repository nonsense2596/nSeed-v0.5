using Microsoft.AspNetCore.Mvc;
using nSeed.Models;
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
            List<DiskInformation> diskInformationList = Global.Utils.SystemInformation.DiskInformation();
            foreach (var diskInformation in diskInformationList)
            {
                ViewData["StorageInfo"] += String.Format("{0}Mb/{1}Mb ({2}% free), ", diskInformation.DiskSize, diskInformation.FreeSize, diskInformation.FreePercentage());
            }
            return View("Default");
        }
    }
}


// https://docs.microsoft.com/en-us/answers/questions/343958/how-to-check-avilable-free-disk-space-drive-on-lin.html
// https://stackoverflow.com/questions/51390486/net-core-find-free-disk-space-on-different-os