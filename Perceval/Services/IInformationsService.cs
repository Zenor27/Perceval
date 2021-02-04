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

        public double GetCpuClockSpeed()
        {
            return (double) GetMachineInformation().Cpu.NormalClockSpeed / 1000;
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
            return GetTotalRam() - availableBytes / BytesToGigabytes;
        }

        public ulong GetTotalRam()
        {
            var totalRam = GetMachineInformation().RAMSticks.Aggregate((ulong) 0, (acc, r) => r.Capacity + acc);
            return totalRam / 1073741824;
        }

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
                .Aggregate((ulong) 0, (acc, d) => d.Capacity / BytesToGigabytes + acc);
        }

        public ulong GetUsedDiskSpace()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            return drives.Aggregate<DriveInfo, ulong>(0,
                (current, driveInfo) =>
                    current + (ulong) (driveInfo.TotalSize - driveInfo.TotalFreeSpace)) / BytesToGigabytes;
        }

        public List<string> GetNamesDisk()
        {
            return GetMachineInformation().Disks
                .Select(d => $"{d.Vendor ?? d.Model} {d.CapacityHRF}")
                .ToList();
        }
    }
}