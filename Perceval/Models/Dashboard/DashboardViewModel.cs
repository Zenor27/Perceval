namespace Perceval.Models.Dashboard
{
    public class DashboardViewModel
    {
        public CpuViewModel CpuViewModel { get; set; }
        
        public RamViewModel RamViewModel { get; set; }
        
        public DiskViewModel DiskViewModel { get; set; }
    }
}