using System.Collections.Generic;

namespace Perceval.Models.Dashboard
{
    public class RamViewModel
    {
        public double UsedRam { get; set; }
        public ulong TotalRam { get; set; }
        
        public List<string> NamesRam { get; set; }

        public double RamUsage => UsedRam / TotalRam;
    }
}