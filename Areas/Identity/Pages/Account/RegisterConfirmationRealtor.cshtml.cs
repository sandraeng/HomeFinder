using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HomeFinder.Areas.Identity.Pages.Account
{
    public class RegisterConfirmationRealtorModel : PageModel
    {
        public string ReturnUrl { get; set; }
        public string Email { get; set; }
        public void OnGet(string email, string returnUrl = null)
        {
            Email = email;
            ReturnUrl = returnUrl;
        }
    }
}
