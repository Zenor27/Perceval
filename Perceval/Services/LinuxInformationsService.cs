using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using HardwareInformation;

namespace Perceval.Services
{
    public class LinuxInformationsService : IInformationsService
    {
        private IInformationsService AsIInformationsService => this;
        private readonly MachineInformation _machineInformation;

        public LinuxInformationsService() => _machineInformation = MachineInformationGatherer.GatherInformation();

        public MachineInformation GetMachineInformation() => _machineInformation;

        // FIXME: Implement methods down here
        public float GetCpuUsage()
        {
            return 0;
        }

        private class Memory
        {
            public double Used;
            public double Total;
            public double Free => Total - Free;
        }

        private Memory GetMemory()
        {
            var fileName = "free -m";
            var info = new ProcessStartInfo(fileName);
            info.FileName = "/bin/bash";
            info.Arguments = $"-c \"{fileName}\"";
            info.RedirectStandardOutput = true;

            var output = "";
            using (var proc = Process.Start(info))
            {
                output = proc?.StandardOutput.ReadToEnd();
            }

            var lines = output?.Split("\n");
            var memory = lines?[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            return new Memory
            {
                Used = double.Parse(memory?[2] ?? "0") / 1000,
                Total = double.Parse(memory?[1] ?? "0") / 1000
            };
        }

        public double GetUsedRam()
        {
            return GetMemory().Used;
        }

        public ulong GetTotalRam()
        {
            return (ulong) GetMemory().Total;
        }

        public ulong GetUsedDiskSpace()
        {
            return 0;
        }

        public List<(string, ulong, ulong)> GetDisksUsage()
        {
            return new List<(string, ulong, ulong)>();
        }

        public TimeSpan GetUptime()
        {
            var uptime = File.ReadAllText("/proc/uptime").Split(" ")[0];
            return TimeSpan.FromSeconds(Convert.ToDouble(uptime));
        }
    }
}