using System;
using System.Collections.Generic;
using System.Linq;
using HardwareInformation;

namespace Perceval.Services
{
    public abstract class BaseInformationsService
    {
        protected readonly MachineInformation _machineInformation;
        protected const ulong BytesToGigabytes = 1073741824;

        protected BaseInformationsService()
        {
            _machineInformation = MachineInformationGatherer.GatherInformation();
        }

        public int GetLogicalCpuCores() => _machineInformation.Cpu.Cores.Count;

        public string GetCpuName() => _machineInformation.Cpu.Name;

        public int GetPhysicalCpuCores() => Convert.ToInt32(_machineInformation.Cpu.PhysicalCores);

        public double GetCpuClockSpeed() => (double) _machineInformation.Cpu.MaxClockSpeed / 1000;


        public List<string> GetNamesRam()
        {
            var names = _machineInformation.RAMSticks
                .Select(ram => $"{ram.Manufacturer} {ram.CapacityHRF} @ {ram.Speed}Mhz")
                .ToList();
            var groupedNames = names.GroupBy(s => s);

            return groupedNames.Select(g => $"{g.Count()} x {g.Key}").ToList();
        }


        public string GetOs() => _machineInformation.OperatingSystem.ToString();

        /*
         * Abstract methods
         * Depending on OS
         */

        public abstract ulong GetUsedDiskSpace();

        public abstract float GetCpuUsage();

        public abstract double GetUsedRam();

        public abstract ulong GetTotalRam();

        public abstract List<(string, ulong, ulong)> GetDisksUsage();

        public abstract ulong GetTotalDiskSpace();

        public abstract List<string> GetNamesDisk();

        public abstract TimeSpan GetUptime();
    }
}