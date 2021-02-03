using System;
using System.Management;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Perceval.Services
{
    public class WindowsInformationsService : IInformationsService
    {
        public string GetCpuName()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0\");
            return key?.GetValue("ProcessorNameString").ToString() ?? "Unknown CPU";
        }

        public int GetPhysicalCpuCores()
        {
            int coreCount = 0;
            foreach (var item in new ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }

            return coreCount;
        }

        public uint GetCpuClockSpeed()
        {
            uint clockSpeed = 0;
            var searcher = new ManagementObjectSearcher("select MaxClockSpeed from Win32_Processor");
            foreach (var item in searcher.Get())
            {
                clockSpeed += (uint)item["MaxClockSpeed"];
            }

            return clockSpeed;
        }
    }
}