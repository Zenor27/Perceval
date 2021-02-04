using HardwareInformation;

namespace Perceval.Services
{
    public class LinuxInformationsService : IInformationsService
    {
        private readonly MachineInformation _machineInformation;

        public LinuxInformationsService()
        {
            _machineInformation = MachineInformationGatherer.GatherInformation();
        }

        MachineInformation IInformationsService.GetMachineInformation()
        {
            return _machineInformation;
        }
    }
}