using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
            public double Free => Total - Used;
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
            return (ulong) GetMemory().Total;
        }

        private class Disk
        {
            public string Name { get; set; }
            public double Total { get; set; }
            public double Used { get; set; }
            public double Free => Total - Used;
        }

        private List<Disk> GetDisksInformations()
        {
            // var fileName = "lsblk --output NAME,FSSIZE,FSUSED -e 7 -i -n --raw --bytes";
            var fileName = "lsblk --output NAME,FSSIZE,FSUSED -e 7 -i -n --raw --bytes";
            var info = new ProcessStartInfo(fileName);
            info.FileName = "/bin/bash";
            info.Arguments = $"-c \"{fileName}\"";
            info.RedirectStandardOutput = true;

            var output = "";
            using (var proc = Process.Start(info))
            {
                output = proc?.StandardOutput.ReadToEnd();
            }

            var disksLines = output?.Split("\n");
            var regroupedByDisks = new Dictionary<string, Disk>();
            var currentDisk = disksLines[0].Replace(" ", "");
            for (var i = 1; i < disksLines.Length; i++)
            {
                var currentPartition = disksLines[i];
                // New disk
                if (!currentPartition.StartsWith(currentDisk))
                {
                    currentDisk = currentPartition.Replace(" ", "");
                    continue;
                }

                ;

                var partitionValues = currentPartition.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                // No informations on this partition
                if (partitionValues.Length == 1)
                {
                    continue;
                }

                var partitionSize = Convert.ToInt64(partitionValues[1]);
                var partitionsUsed = Convert.ToInt64(partitionValues[2]);
                if (regroupedByDisks.ContainsKey(currentDisk))
                {
                    var disk = regroupedByDisks[currentDisk];
                    disk.Used += (double) partitionsUsed / BytesToGigabytes;
                    disk.Total += (double) partitionSize / BytesToGigabytes;
                }
                else
                {
                    regroupedByDisks.Add(currentDisk, new Disk
                    {
                        Name = currentDisk,
                        Used = (double) partitionsUsed / BytesToGigabytes,
                        Total = (double) partitionSize / BytesToGigabytes
                    });
                }
            }

            return regroupedByDisks.Values.ToList();
        }


        public override ulong GetUsedDiskSpace()
        {
            return (ulong) GetDisksInformations().Select(x => x.Used).Sum(x => x);
        }

        public override ulong GetTotalDiskSpace()
        {
            return (ulong) GetDisksInformations().Select(x => x.Total).Sum(x => x);
        }

        public override List<(string, ulong, ulong)> GetDisksUsage()
        {
            return GetDisksInformations().Aggregate(new List<(string, ulong, ulong)>(), (acc, x) =>
            {
                acc.Add((x.Name, (ulong) x.Free, (ulong) x.Total));
                return acc;
            });
        }

        public override List<string> GetNamesDisk()
        {
            return GetDisksInformations().Select(x => x.Name).ToList();
        }

        public override TimeSpan GetUptime()
        {
            var uptime = File.ReadAllText("/proc/uptime").Split(" ")[0];
            return TimeSpan.FromSeconds(Convert.ToDouble(uptime));
        }
    }
}