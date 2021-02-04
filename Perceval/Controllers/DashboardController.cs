using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perceval.Models.Dashboard;
using Perceval.Services;

namespace Perceval.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private readonly IInformationsService _informationsService;

        public DashboardController(IInformationsService informationsService)
        {
            _informationsService = informationsService;
        }

        private DashboardViewModel GetDashboardViewModel()
        {
            CpuViewModel cpuViewModel = new CpuViewModel
            {
                LogicalCpuCores = _informationsService.GetLogicalCpuCores(),
                PhysicalCpuCores = _informationsService.GetPhysicalCpuCores(),
                CpuName = _informationsService.GetCpuName(),
                CpuClockSpeed = (double) _informationsService.GetCpuClockSpeed() / 1000,
                CpuUsage = _informationsService.GetCpuUsage()
            };

            RamViewModel ramViewModel = new RamViewModel
            {
                UsedRam = _informationsService.GetUsedRam(),
                TotalRam = _informationsService.GetTotalRam(),
                NamesRam = _informationsService.GetNamesRam()
            };
            return new DashboardViewModel
            {
                CpuViewModel = cpuViewModel,
                RamViewModel = ramViewModel
            };
        }

        public ActionResult GetInformations()
        {
            return PartialView("Informations", GetDashboardViewModel());
        }

        public ViewResult Index()
        {
            return View(GetDashboardViewModel());
        }
    }
}