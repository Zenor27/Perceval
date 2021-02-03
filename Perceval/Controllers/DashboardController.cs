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
            return new DashboardViewModel
            {
                LogicalCpuCores = _informationsService.GetLogicalCpuCores(),
                PhysicalCpuCores = _informationsService.GetPhysicalCpuCores(),
                CpuName = _informationsService.GetCpuName(),
                CpuClockSpeed = (double) _informationsService.GetCpuClockSpeed() / 1000,
                CpuUsage = _informationsService.GetCpuUsage()
            };
        }

        public ViewResult Index()
        {
            return View(GetDashboardViewModel());
        }

        public IActionResult GetCpuInformations()
        {
            return PartialView("CpuCard", GetDashboardViewModel());
        }
    }
}