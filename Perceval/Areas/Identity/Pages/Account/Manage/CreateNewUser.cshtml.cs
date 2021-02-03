using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Perceval.Areas.Identity.Pages.Account.Manage
{
    public class CreateNewUserModel : PageModel
    {
        private readonly ILogger<CreateNewUserModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateNewUserModel(ILogger<CreateNewUserModel> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }


        [TempData] public string StatusMessage { get; set; }

       
        public ActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public string Username { get; set; }
        
        [BindProperty]
        public string Password { get; set; }
        
        public async void OnPostAsync()
        {
            var user = new IdentityUser(Username);
            var result = await _userManager.CreateAsync(user, Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("New user created.");
                StatusMessage = "User created !";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.Description);
                }
                StatusMessage = "Error, user could not be created...";
            }
        }
    }
}