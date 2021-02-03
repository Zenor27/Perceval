using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Perceval.Models.Login;

namespace Perceval.Controllers
{
    public class LoginController : BaseController
    {
        private readonly ILogger<LoginController> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginController(ILogger<LoginController> logger, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "AccessDenied");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            var result =
                await _signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, true, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Login/Password incorrect");
                return View("Index");
            }

            _logger.LogInformation($"User {loginViewModel.Username} logged in.");

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}