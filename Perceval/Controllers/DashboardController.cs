using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Perceval.Models.Dashboard;
using Perceval.Services;

namespace Perceval.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IInformationsService _informationsService;

        public DashboardController(ILogger<DashboardController> logger, IInformationsService informationsService)
        {
            _logger = logger;
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
                UsedRam = 8,
                TotalRam = 16
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