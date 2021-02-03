using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Perceval.Areas.Identity
{
    public class DisabledIdentityPage : PageModel
    {
        public ActionResult OnGet()
        {
            return RedirectToAction("Index", "AccessDenied");
        }
    }
}