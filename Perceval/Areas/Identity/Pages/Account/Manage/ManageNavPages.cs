using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Perceval.Areas.Identity.Pages.Account.Manage
{
    public static class ManageNavPages
    {
        public static string ChangePassword => "ChangePassword";

        public static string CreateNewUser => "CreateNewUser";

        public static string ChangePasswordNavClass(ViewContext viewContext) =>
            PageNavClass(viewContext, ChangePassword);

        public static string CreateNewUserNavClass(ViewContext viewContext) => PageNavClass(viewContext, CreateNewUser);


        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                             ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}