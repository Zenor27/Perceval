﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perceval.Models.Dashboard;
using Perceval.Services;

namespace Perceval.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private readonly BaseInformationsService _informationsService;

        public DashboardController(BaseInformationsService informationsService)
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
                CpuClockSpeed = _informationsService.GetCpuClockSpeed(),
                CpuUsage = _informationsService.GetCpuUsage()
            };

            RamViewModel ramViewModel = new RamViewModel
            {
                UsedRam = _informationsService.GetUsedRam(),
                TotalRam = _informationsService.GetTotalRam(),
                NamesRam = _informationsService.GetNamesRam()
            };

            DiskViewModel diskViewModel = new DiskViewModel
            {
                TotalDiskSpace = _informationsService.GetTotalDiskSpace(),
                UsedDiskSpace = _informationsService.GetUsedDiskSpace(),
                NamesDisk = _informationsService.GetNamesDisk(),
                DisksUsage = _informationsService.GetDisksUsage()
            };

            UptimeViewModel uptimeViewModel = new UptimeViewModel
            {
                Uptime = _informationsService.GetUptime(),
                OS = _informationsService.GetOs()
            };
            return new DashboardViewModel
            {
                CpuViewModel = cpuViewModel,
                RamViewModel = ramViewModel,
                DiskViewModel = diskViewModel,
                UptimeViewModel = uptimeViewModel
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