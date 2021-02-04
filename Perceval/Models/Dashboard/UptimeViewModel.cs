using System;

namespace Perceval.Models.Dashboard
{
    public class UptimeViewModel
    {
        public TimeSpan Uptime { get; set; }

        public string OS { get; set; }
    }
}