using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using HardwareInformation;

namespace Perceval.Services
{
    public interface IInformationsService
    {
        protected const ulong BytesToGigabytes = 1073741824;

        public MachineInformation GetMachineInformation();

        public int GetLogicalCpuCores() => GetMachineInformation().Cpu.Cores.Count;

        public string GetCpuName() => GetMachineInformation().Cpu.Name;

        public int GetPhysicalCpuCores() => Convert.ToInt32(GetMachineInformation().Cpu.PhysicalCores);

        public double GetCpuClockSpeed() => (double)GetMachineInformation().Cpu.MaxClockSpeed / 1000;

        public float GetCpuUsage();

        public double GetUsedRam();

        public ulong GetTotalRam();

        public List<string> GetNamesRam()
        {
            var names = GetMachineInformation().RAMSticks
                .Select(ram => $"{ram.Manufacturer} {ram.CapacityHRF} @ {ram.Speed}Mhz")
                .ToList();
            var groupedNames = names.GroupBy(s => s);

            return groupedNames.Select(g => $"{g.Count()} x {g.Key}").ToList();
        }

        public ulong GetTotalDiskSpace()
        {
            return GetMachineInformation().Disks
                .Aggregate((ulong)0, (acc, d) => d.Capacity / BytesToGigabytes + acc);
        }

        public ulong GetUsedDiskSpace();

        public List<string> GetNamesDisk()
        {
            return GetMachineInformation().Disks
                .Select(d => $"{d.Vendor ?? d.Model} {d.CapacityHRF}")
                .ToList();
        }

        public List<(string, ulong, ulong)> GetDisksUsage();

        public TimeSpan GetUptime();

        public string GetOs() => GetMachineInformation().OperatingSystem.ToString();
    }
}