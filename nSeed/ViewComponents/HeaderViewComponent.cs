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
        private readonly long kex;

        public HeaderViewComponent()
        {
            this.kex = new DriveInfo("C:\\").AvailableFreeSpace;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewData["StorageInfo"] = Global.Utils.SystemInformation.PrintDiskSpace();
            return View("Default");
            //return await Task.FromResult((IViewComponentResult)View("/Views/Shared/Components/Test/Default"));
        }
    }
}


// https://docs.microsoft.com/en-us/answers/questions/343958/how-to-check-avilable-free-disk-space-drive-on-lin.html
// https://stackoverflow.com/questions/51390486/net-core-find-free-disk-space-on-different-os