using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Perceval.Services
{
    public class LinuxInformationsService : BaseInformationsService
    {
        // FIXME: Implement methods down here
        public override float GetCpuUsage()
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

        public override double GetUsedRam()
        {
            return GetMemory().Used;
        }

        public override ulong GetTotalRam()
        {
            return (ulong)GetMemory().Total;
        }

        public override ulong GetUsedDiskSpace()
        {
            return 0;
        }

        public override List<(string, ulong, ulong)> GetDisksUsage()
        {
            return new List<(string, ulong, ulong)>();
        }

        public override TimeSpan GetUptime()
        {
            var uptime = File.ReadAllText("/proc/uptime").Split(" ")[0];
            return TimeSpan.FromSeconds(Convert.ToDouble(uptime));
        }
    }
}