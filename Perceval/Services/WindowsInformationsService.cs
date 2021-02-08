using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Perceval.Services
{
    public class WindowsInformationsService : BaseInformationsService
    {
        public override float GetCpuUsage()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();
            Thread.Sleep(500);
            return cpuCounter.NextValue() / 100;
        }

        public override double GetUsedRam()
        {
            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available Bytes", null);
            ramCounter.NextValue();
            Thread.Sleep(500);
            var availableBytes = ramCounter.NextValue();
            return GetTotalRam() - availableBytes / BytesToGigabytes;
        }

        public override ulong GetTotalRam()
        {
            var totalRam = _machineInformation.RAMSticks.Aggregate((ulong) 0, (acc, r) => r.Capacity + acc);
            return totalRam / BytesToGigabytes;
        }

        public override ulong GetUsedDiskSpace()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            return drives.Aggregate<DriveInfo, ulong>(0,
                       (current, driveInfo) =>
                           current + (ulong) (driveInfo.TotalSize - driveInfo.TotalFreeSpace)) /
                   BytesToGigabytes;
        }

        public override List<(string, ulong, ulong)> GetDisksUsage()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            return drives.Aggregate(new List<(string, ulong, ulong)>(),
                (acc, driveInfo) =>
                {
                    acc.Add((driveInfo.Name, Convert.ToUInt64(driveInfo.TotalFreeSpace / (long) BytesToGigabytes),
                        Convert.ToUInt64(driveInfo.TotalSize / (long) BytesToGigabytes)));
                    return acc;
                });
        }

        public override ulong GetTotalDiskSpace()
        {
            return _machineInformation.Disks
                .Aggregate((ulong) 0, (acc, d) => d.Capacity / BytesToGigabytes + acc);
        }

        public override List<string> GetNamesDisk()
        {
            return _machineInformation.Disks
                .Select(d => $"{d.Vendor ?? d.Model} {d.CapacityHRF}")
                .ToList();
        }

        public override TimeSpan GetUptime()
        {
            var uptime = new PerformanceCounter("System", "System Up Time");
            uptime.NextValue();
            Thread.Sleep(500);
            return TimeSpan.FromSeconds(uptime.NextValue());
        }
    }
}