using Ktu_Mobile_Docs.Controllers;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AdminController.ConfirmEmail),
                controller: "Admin",
                values: new { userId, code },
                protocol: scheme);
        }
    }
}