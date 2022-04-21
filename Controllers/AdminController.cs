using HomeFinder.Data;
using HomeFinder.Models;
using HomeFinder.RoleModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MimeDetective;
using System;

namespace HomeFinder.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<HomeFinderUser> userManager;
        private readonly HomeFinderContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AdminController(
            RoleManager<IdentityRole> roleManager,
            UserManager<HomeFinderUser> userManager,
            HomeFinderContext context,
            IWebHostEnvironment hostingEnvironment)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this._context = context;
            this._hostingEnvironment = hostingEnvironment;
        }


        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new ArgumentException($"User with id: {id} could not be found.");
            }

            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUser
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AddressId = user.AddressId,
                CompanyId = user.CompanyId,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles.ToList()
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRole model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Admin");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if(role == null)
            {
                throw new ArgumentException($"Role with id: {id} could not be found.");
            }

            var model = new EditRole
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach(var user in userManager.Users)
            {
                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRole model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                throw new ArgumentException($"Role with id: {model.Id} could not be found.");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);

            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync(roleId);

            if(role == null)
            {
                throw new ArgumentException($"Role with id: {roleId} could not be found.");
            }

            var model = new List<UserRole>();

            foreach(var user in userManager.Users)
            {
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRole.IsChecked = true;
                }
                else
                {
                    userRole.IsChecked = false;
                }
                model.Add(userRole);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRole> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                throw new ArgumentException($"Role with id: {roleId} could not be found.");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsChecked && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsChecked && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);

                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1)) 
                        continue;
                    else 
                        return RedirectToAction("EditRole", new {Id = roleId});
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });

        }

        // GET: Admin/DeleteUser/id
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Include(u => u.Address)
                .Include(u => u.Company)
                .FirstOrDefaultAsync();

            if (user is null)
            {
                throw new ArgumentException($"User with id: {id} could not be found.");
            }

            // Get list of files for this user. Filename contains user id.
            string proofFolder = "realtorProof";
            string webPath = $"/{proofFolder}";
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, proofFolder);
            List<string> fileUrls = new List<string>();
            List<string> imageUrls = new List<string>();

            DirectoryInfo di = new DirectoryInfo(uploadsFolder);

            foreach (FileInfo fi in di.GetFiles())
            {
                if (fi.Name.Contains(user.Id))
                {
                    if (IsImageFile(fi.FullName))
                    {
                        imageUrls.Add($"{webPath}/{fi.Name}");
                    }
                    else
                    {
                        fileUrls.Add($"{webPath}/{fi.Name}");
                    }

                }

            }
            ViewBag.UserFiles = fileUrls;
            ViewBag.UserImages = imageUrls;
            return View(user);
        }

        // POST: Admin/DeleteUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyDeleteUser(string id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Include(u => u.Address)
                .Include(u => u.Company)
                .FirstOrDefaultAsync();
            var userNoIs = await _context.NoticeOfInterests.Where(n => n.UserId == user.Id).ToListAsync();
            var userSaved = await _context.PropertyFavorited.Where(n => n.UserId == user.Id).ToListAsync();

            if (user is null)
            {
                throw new ArgumentException($"User with id: {id} could not be found.");
            }

            // Delete files for this user. Filename contains user id.
            string proofFolder = "realtorProof";
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, proofFolder);

            DirectoryInfo di = new DirectoryInfo(uploadsFolder);
            foreach (FileInfo fi in di.GetFiles())
            {
                if (fi.Name.Contains(user.Id))
                {
                    System.IO.File.Delete(fi.FullName);
                }
            }

            _context.NoticeOfInterests.RemoveRange(userNoIs);
            _context.PropertyFavorited.RemoveRange(userSaved);
            await _context.SaveChangesAsync();

            var userRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, userRoles);

            var result = await userManager.DeleteAsync(user);
            var userId = await userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            return RedirectToAction("ListUsers","Admin");
        }

        // GET: Admin/UnverifiedRealtors
        [HttpGet]
        public async Task<IActionResult> UnverifiedRealtors()
        {
            var users = await userManager.GetUsersInRoleAsync("UnverifiedRealtor");
            return View(users);
        }

        // GET: Admin/ViewUnverifiedRealtors/id
        [HttpGet]
        public async Task<IActionResult> ViewUnverifiedRealtor(string id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Include(u => u.Address)
                .Include(u => u.Company)
                .FirstOrDefaultAsync();
            if (user is null)
            {
                throw new ArgumentException($"User with id: {id} could not be found.");
            }
            if (!(await userManager.IsInRoleAsync(user,"UnverifiedRealtor")))
            {
                throw new ArgumentException($"User with id: {id} is not in role 'UnverifiedRealtor'.");
            }

            // Get list of files for this user. Filename contains user id.
            string proofFolder = "realtorProof";
            string webPath = $"/{proofFolder}";
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, proofFolder);
            List<string> fileUrls = new List<string>();
            List<string> imageUrls = new List<string>();

            DirectoryInfo di = new DirectoryInfo(uploadsFolder);

            foreach (FileInfo fi in di.GetFiles())
            {
                if (fi.Name.Contains(user.Id))
                {
                    if (IsImageFile(fi.FullName))
                    {
                        imageUrls.Add($"{webPath}/{fi.Name}");
                    } else
                    {
                        fileUrls.Add($"{webPath}/{fi.Name}");
                    }

                }

            }
            ViewBag.UserFiles = fileUrls;
            ViewBag.UserImages = imageUrls;
            return View(user);
        }

        // Post: Admin/VerifyRealtor
        [HttpPost]
        public async Task<IActionResult> VerifyRealtor(string id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Include(u => u.Address)
                .Include(u => u.Company)
                .FirstOrDefaultAsync();
            if (user is null)
            {
                throw new ArgumentException($"User with id: {id} could not be found.");
            }
            if (!(await userManager.IsInRoleAsync(user, "UnverifiedRealtor")))
            {
                throw new ArgumentException($"User with id: {id} is not in role 'UnverifiedRealtor'.");
            }

            return View(user);
        }
        // Post: Admin/RealtorVerified
        [HttpPost]
        public async Task<IActionResult> RealtorVerified(string id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Include(u => u.Address)
                .Include(u => u.Company)
                .FirstOrDefaultAsync();
            if (user is null)
            {
                throw new ArgumentException($"User with id: {id} could not be found.");
            }
            if (!(await userManager.IsInRoleAsync(user, "UnverifiedRealtor")))
            {
                throw new ArgumentException($"User with id: {id} is not in role 'UnverifiedRealtor'.");
            }

            await userManager.AddToRoleAsync(user, "Realtor");
            await userManager.RemoveFromRoleAsync(user, "UnverifiedRealtor");
            user.EmailConfirmed = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("UnverifiedRealtors","Admin");
        }
        /// <summary>
        /// Returns true if file exists and has mimetype image.
        /// Returns false otherwise.
        /// Throws ArgumentException if filePath is null.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private bool IsImageFile(string filePath)
        {
            if (filePath is null)
            {
                throw new ArgumentException($"filePath must be a non null string");
            }
            if (System.IO.File.Exists(filePath))
            {
                // This could be optimized if checking IsImageFile many times in a row.
                var Inspector = new ContentInspectorBuilder()
                {
                    Definitions = MimeDetective.Definitions.Default.All()
                }.Build();

                var content = ContentReader.Default.ReadFromFile(filePath);
                var results = Inspector.Inspect(content);
                var isImage = results.ByMimeType().Any(r => r.MimeType.Contains("image/"));

                return isImage;
            }
            return false;
        }
    }
}
