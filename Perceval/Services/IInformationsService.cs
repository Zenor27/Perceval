using System;
using System.Diagnostics;
using System.Threading;

namespace Perceval.Services
{
    public interface IInformationsService
    {
        public int GetLogicalCpuCores()
        {
            return Environment.ProcessorCount;
        }

        public string GetCpuName();

        public int GetPhysicalCpuCores();

        public uint GetCpuClockSpeed();

        public float GetCpuUsage()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();
            Thread.Sleep(500);
            return cpuCounter.NextValue() / 100;
        }
    }
}