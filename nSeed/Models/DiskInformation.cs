namespace nSeed.Models
{
    public class DiskInformation
    {
        public string DiskName { get; set; }

        public long DiskSize { get; set; }
        
        public long FreeSize { get; set; }

        public DiskInformation(string diskName, long diskSize, long freeSize)
        {
            DiskName = diskName;
            DiskSize = diskSize;
            FreeSize = freeSize;
        }

        public float FreePercentage()
        {
            return (100 * FreeSize) / DiskSize;
        }

        public float UsedPercentage()
        {
            return 100 - FreePercentage();
        }

        public long UsedSize()
        {
            return DiskSize - FreeSize;
        }
    }
}
