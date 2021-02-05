using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using HardwareInformation;
using static Perceval.Services.IInformationsService;

namespace Perceval.Services
{
    public class WindowsInformationsService : IInformationsService
    {
        private IInformationsService AsIInformationsService => this;
        private readonly MachineInformation _machineInformation;

        public WindowsInformationsService() => _machineInformation = MachineInformationGatherer.GatherInformation();

        public MachineInformation GetMachineInformation() => _machineInformation;

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
            return AsIInformationsService.GetTotalRam() - availableBytes / BytesToGigabytes;
        }

        public ulong GetUsedDiskSpace()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            return drives.Aggregate<DriveInfo, ulong>(0,
                       (current, driveInfo) =>
                           current + (ulong) (driveInfo.TotalSize - driveInfo.TotalFreeSpace)) /
                   BytesToGigabytes;
        }

        public List<(string, ulong, ulong)> GetDisksUsage()
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

        public TimeSpan GetUptime()
        {
            var uptime = new PerformanceCounter("System", "System Up Time");
            uptime.NextValue();
            Thread.Sleep(500);
            return TimeSpan.FromSeconds(uptime.NextValue());
        }
    }
}