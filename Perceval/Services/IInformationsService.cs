using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using HardwareInformation;

namespace Perceval.Services
{
    public interface IInformationsService
    {
        protected MachineInformation GetMachineInformation();

        public int GetLogicalCpuCores()
        {
            return GetMachineInformation().Cpu.Cores.Count;
        }

        public string GetCpuName()
        {
            return GetMachineInformation().Cpu.Name;
        }

        public int GetPhysicalCpuCores()
        {
            return Convert.ToInt32(GetMachineInformation().Cpu.PhysicalCores);
        }

        public uint GetCpuClockSpeed()
        {
            return GetMachineInformation().Cpu.NormalClockSpeed;
        }

        public float GetCpuUsage()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();
            Thread.Sleep(500);
            return cpuCounter.NextValue() / 100;
        }

        public double GetUsedRam()
        {
            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available Bytes", null);
            ramCounter.NextValue();
            Thread.Sleep(500);
            var availableBytes = ramCounter.NextValue();
            return GetTotalRam() - availableBytes / 1073741824;
        }

        public ulong GetTotalRam()
        {
            var totalRam = GetMachineInformation().RAMSticks.Aggregate((ulong) 0, (acc, r) => r.Capacity + acc);
            return totalRam / 1073741824;
        }

        public List<string> GetNamesRam()
        {
            return GetMachineInformation().RAMSticks
                .Select(ram => $"{ram.Manufacturer} {ram.CapacityHRF} @ {ram.Speed}Mhz")
                .ToList();
        }
    }
}