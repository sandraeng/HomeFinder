using HomeFinder.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HomeFinder.Areas.Identity.Pages.Account
{
    public class RegisterRealtorModel : PageModel
    {
        private readonly SignInManager<HomeFinderUser> _signInManager;
        private readonly UserManager<HomeFinderUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        //private readonly IEmailSender _emailSender;

        public RegisterRealtorModel(
            UserManager<HomeFinderUser> userManager,
            SignInManager<HomeFinderUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger,
            /*IEmailSender emailSender*/
            IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
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

            [Display(Name = "*Realtor proof")]
            [Required]
            public IFormFile RealtorProof { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                // Check if there is a realtor-role, otherwise we can't create realtors.
                var unverifedRealtorRole = _roleManager.Roles.Where(r => r.Name == "UnverifiedRealtor").FirstOrDefault();
                if(unverifedRealtorRole == null)
                {
                    string errorMessage = "Unable to register new realtor. The user role 'UnverifiedRealtor' does not exist. Please notify site Administrators.";
                    ModelState.AddModelError(string.Empty, errorMessage);
                    return Page();
                }

                var user = new HomeFinderUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    PhoneNumber = Input.PhoneNumber,
                    Company = new Company { Name = Input.Company.Name, OrgNumber = Input.Company.OrgNumber }
                    
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    var addedUser = _userManager.Users.Where(u => u.UserName == user.UserName).FirstOrDefault();
                    await _userManager.AddToRoleAsync(addedUser, unverifedRealtorRole.Name);

                    string uniqueFilename = null;
                    if (Input.RealtorProof != null)
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "realtorProof");
                        uniqueFilename = user.Id.ToString() + $"_{Input.FirstName}_{Input.LastName}";
                        string filePath = Path.Combine(uploadsFolder, uniqueFilename);
                                                                                                            //Relator proof file gets uploaded to wwwroot/relatorProof
                        Input.RealtorProof.CopyTo(new FileStream(filePath, FileMode.Create));
                    }

                    _logger.LogInformation("User created a new account with password.");

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmationRealtor", new { Email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

    }
}
