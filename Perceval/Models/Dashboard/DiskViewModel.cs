using System.Collections.Generic;

namespace Perceval.Models.Dashboard
{
    public class DiskViewModel
    {
        public ulong TotalDiskSpace { get; set; }

        public ulong UsedDiskSpace { get; set; }

        public double DiskUsage => (double)UsedDiskSpace / TotalDiskSpace;

        public List<string> NamesDisk { get; set; }

        public List<(string, ulong, ulong)> DisksUsage { get; set; }
    }
}