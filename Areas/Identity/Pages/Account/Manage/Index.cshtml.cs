using HomeFinder.Data;
using HomeFinder.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeFinder.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly HomeFinderContext _context;
        private readonly UserManager<HomeFinderUser> _userManager;
        private readonly SignInManager<HomeFinderUser> _signInManager;
        private readonly ILogger<RegisterModel> _logger;

        public IndexModel(
            HomeFinderContext context,
            UserManager<HomeFinderUser> userManager,
            SignInManager<HomeFinderUser> signInManager,
            ILogger<RegisterModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }


        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            public string Roles { get; set; }
            public HomeFinderUser User { get; set; }
            public Address Address { get; set; }
            public Company Company { get; set; }
        }

        // Updates InputModel Input with data from db.
        private async Task LoadAsync(HomeFinderUser user)
        {
            var address = await _context.Addresses.Where(a => a.Id == user.AddressId).FirstOrDefaultAsync();
            var company = await _context.Companies.Where(c => c.Id == user.CompanyId).FirstOrDefaultAsync();
            var roles = await _userManager.GetRolesAsync(user);

            Input = new InputModel
            {
                User = user,
                Address = address,
                Company = company,
                Roles = string.Join(", ", roles),
            };
        }

        // Loads page if a user exists.
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        // Handles posted data.
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }


            var dbUser = await _context.Users.Where(u => u.Id == user.Id)
                .Include(u => u.Address)
                .Include(u => u.Company)
                .FirstOrDefaultAsync();
            
            if (dbUser == null)
            {
                StatusMessage = "Error: Unable to find this user in context.";
                return RedirectToPage();
            }
            if (dbUser.Id != Input.User.Id)
            {
                StatusMessage = "Error: Id for submitted user data is not the same as currently logged in user!";
                return RedirectToPage();
            }
            if (dbUser.Address.Id != Input.Address.Id)
            {
                StatusMessage = "Error: Id for submitted address data is not the same as address id for currently logged in user!";
                return RedirectToPage();
            }

            // Set any changed data from input to context. Done per property to prevent unwanted changes.
            dbUser.FirstName = Input.User.FirstName;
            dbUser.LastName = Input.User.LastName;
            dbUser.PhoneNumber = Input.User.PhoneNumber;

            dbUser.Address.StreetAddress = Input.Address.StreetAddress;
            dbUser.Address.PostalCode = Input.Address.PostalCode;
            dbUser.Address.City = Input.Address.City;
            dbUser.Address.Country = Input.Address.Country;

            // Currently not allowing user to edit own company info.

            await _context.SaveChangesAsync();

            await LoadAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();

        }

    }
}
