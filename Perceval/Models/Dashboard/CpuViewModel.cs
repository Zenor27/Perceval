namespace Perceval.Models.Dashboard
{
    public class CpuViewModel
    {
        public int LogicalCpuCores { get; set; }
        
        public int PhysicalCpuCores { get; set; }
        
        public string CpuName { get; set; }
        
        public double CpuClockSpeed { get; set; }
        
        public float CpuUsage { get; set; }    }
}