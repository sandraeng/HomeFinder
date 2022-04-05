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
    public class RegisterRelatorModel : PageModel
    {
        private readonly SignInManager<HomeFinderUser> _signInManager;
        private readonly UserManager<HomeFinderUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        //private readonly IEmailSender _emailSender;

        public RegisterRelatorModel(
            UserManager<HomeFinderUser> userManager,
            SignInManager<HomeFinderUser> signInManager,
            ILogger<RegisterModel> logger,
            /*IEmailSender emailSender*/
            IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

            [Display(Name = "*Relator proof")]
            [Required]
            public IFormFile RelatorProof { get; set; }
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
                

                var user = new HomeFinderUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    PhoneNumber = Input.PhoneNumber,
                    
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    string uniqueFilename = null;
                    if (Input.RelatorProof != null)
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "relatorProof");
                        uniqueFilename = Guid.NewGuid().ToString() + $"_{Input.FirstName}_{Input.LastName}_" + Input.RelatorProof.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFilename);

                        Input.RelatorProof.CopyTo(new FileStream(filePath, FileMode.Create));
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
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
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
