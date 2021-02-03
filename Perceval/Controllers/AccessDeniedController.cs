using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Perceval.Controllers
{
    public class AccessDeniedController : BaseController
    {
        private readonly ILogger<AccessDeniedController> _logger;

        public AccessDeniedController(ILogger<AccessDeniedController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}