using HomeFinder.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeFinder.Areas.Identity.Pages.Account
{
    public class RegisterRelatorModel : PageModel
    {
        private readonly SignInManager<HomeFinderUser> _signInManager;
        private readonly UserManager<HomeFinderUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        //private readonly IEmailSender _emailSender;

        public RegisterRelatorModel(
            UserManager<HomeFinderUser> userManager,
            SignInManager<HomeFinderUser> signInManager,
            ILogger<RegisterModel> logger
            /*IEmailSender emailSender*/)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            //_emailSender = emailSender;
        }

        [BindProperty]
        public InputModelRelator Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModelRelator
        {
            [Required]
            [EmailAddress]
            [Display(Name = "*Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "*Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "*Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [StringLength(60)]
            [Display(Name = "*First name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(60)]
            [Display(Name = "*Last name")]
            public string LastName { get; set; }
            
            [Required]
            [Display(Name = "*Phone number")]
            public string PhoneNumber { get; set; }

            public Company Company { get; set; }
            
            [Required]
            public IFormFile RelatorProof { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }
    }
}
