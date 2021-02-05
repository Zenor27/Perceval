using System;
using System.Collections.Generic;
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

        public double GetUsedRam()
        {
            return 0;
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
            return new TimeSpan();
        }
    }
}