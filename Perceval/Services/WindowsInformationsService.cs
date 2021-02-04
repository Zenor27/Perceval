using HardwareInformation;

namespace Perceval.Services
{
    public class WindowsInformationsService : IInformationsService
    {
        private readonly MachineInformation _machineInformation;

        public WindowsInformationsService()
        {
            _machineInformation = MachineInformationGatherer.GatherInformation();
        }

        MachineInformation IInformationsService.GetMachineInformation()
        {
            return _machineInformation;
        }
    }
}