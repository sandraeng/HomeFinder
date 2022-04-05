using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeFinder.Data;
using HomeFinder.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace HomeFinder.Controllers
{
    public class PropertyObjectsController : Controller
    {
        private readonly HomeFinderContext _context;
        private readonly UserManager<HomeFinderUser> _userManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IConfiguration Configuration;

        public PropertyObjectsController(HomeFinderContext context, UserManager<HomeFinderUser> userManager, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _context = context;
            this._userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
            this.Configuration = configuration;
        }

        // GET: PropertyObjects
        public async Task<IActionResult> Index()
        {
            var homeFinderContext = _context.PropertyObjects.Include(p => p.Address).Include(p => p.Realtor);
            return View(await homeFinderContext.ToListAsync());
        }

        // GET: PropertyObjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyObject = await _context.PropertyObjects
                .Include(p => p.Address)
                .Include(p => p.Realtor)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (propertyObject == null)
            {
                return NotFound();
            }

            string apiKey = Configuration.GetValue<string>("GoogleMapsAPIKey");
            ViewBag.GoogleMapsURL = GoogleMapsURL(apiKey, propertyObject.Address.FullAddress);
            return View(propertyObject);
        }

        // GET: PropertyObjects/Create
        public IActionResult Create()
        {
            // Get a list of values in enum and add to viewbag, so it can be used to populate a dropdown in View.
            ViewBag.PropertyObjectStatuses = new SelectList(Enum.GetNames(typeof(PropertyObjectStatus)));
            ViewBag.PropertyTypes = _context.PropertyTypes.ToList();
            ViewBag.LeaseTypes = _context.LeaseTypes.ToList();

            ViewData["RealtorId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: PropertyObjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RealtorId,PropertyTypeId,Status,ListPrice,NumberOfRooms,Area,NextShowingDateTime,Address,LeaseTypeId,NonLivingArea,LotArea,YearBuilt,Description,Images")] PropertyObject propertyObject, List<IFormFile> files)
        {
            // Update PropertyType with value from DB.
            propertyObject.PropertyType = _context.PropertyTypes.Where(x => x.Id == propertyObject.PropertyTypeId).FirstOrDefault();
            // Update LeaseType with value from DB.
            propertyObject.LeaseType = _context.LeaseTypes.Where(x => x.Id == propertyObject.LeaseTypeId).FirstOrDefault();
            // Set uploaded date to today.
            propertyObject.UploadedDate = DateTime.Now;
            // Set AddressId to 0
            propertyObject.AddressId = 0;

            // Check if Property type and LeaseType was set, if not then reload Create View.
            if ((propertyObject.PropertyType is null) || (propertyObject.LeaseType is null))
            {
                // Get a list of values in enum and add to viewbag, so it can be used to populate a dropdown in View.
                ViewBag.PropertyObjectStatuses = new SelectList(Enum.GetNames(typeof(PropertyObjectStatus)));
                ViewBag.PropertyTypes = _context.PropertyTypes.ToList();
                ViewBag.LeaseTypes = _context.LeaseTypes.ToList();

                ViewData["RealtorId"] = new SelectList(_context.Users, "Id", "Id");
                return View(propertyObject);
            }
            ModelState.Remove("PropertyType"); // We manually update PropertyType above, ignore it in ModelState.
            ModelState.Remove("LeaseType"); // We manually update LeaseType above, ignore it in ModelState.
            ModelState.Remove("Address.Id");

            if (ModelState.IsValid)
            {
                _context.Add(propertyObject);
                await _context.SaveChangesAsync();

                foreach (var file in files)
                {
                    var image = new HomeFinderImages();
                    string path = UploadFile(file);
                    image.Path = "~/Images/" + path;
                    image.PropertyObjectId = propertyObject.Id;
                    _context.Add(image);
                    await _context.SaveChangesAsync();
                    propertyObject.Images.Add(image);
                }
                return RedirectToAction("Edit", "HomeFinderImages", new {id = propertyObject.Id });
            }


            // Get a list of values in enum and add to viewbag, so it can be used to populate a dropdown in View.
            ViewBag.PropertyObjectStatuses = new SelectList(Enum.GetNames(typeof(PropertyObjectStatus)));
            ViewBag.PropertyTypes = _context.PropertyTypes.ToList();
            ViewBag.LeaseTypes = _context.LeaseTypes.ToList();
            ViewData["RealtorId"] = new SelectList(_context.Users, "Id", "Id", propertyObject.RealtorId);
            return View(propertyObject);
        }

        private string UploadFile(IFormFile file)
        {
            string fileName = null;
            if (file != null)
            {
                string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return fileName;
        }

        // GET: PropertyObjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyObject = await _context.PropertyObjects.FindAsync(id);
            if (propertyObject == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", propertyObject.AddressId);
            ViewData["RealtorId"] = new SelectList(_context.Users, "Id", "Id", propertyObject.RealtorId);
            return View(propertyObject);
        }

        // POST: PropertyObjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RealtorId,PropertyTypeId,Status,ListPrice,NumberOfRooms,Area,NonLivingArea,LotArea,UploadedDate,NextShowingDateTime,AddressId,Description,LeaseTypeId,YearBuilt")] PropertyObject propertyObject)
        {
            if (id != propertyObject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(propertyObject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyObjectExists(propertyObject.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", propertyObject.AddressId);
            ViewData["RealtorId"] = new SelectList(_context.Users, "Id", "Id", propertyObject.RealtorId);
            return View(propertyObject);
        }

        // GET: PropertyObjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyObject = await _context.PropertyObjects
                .Include(p => p.Address)
                .Include(p => p.Realtor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (propertyObject == null)
            {
                return NotFound();
            }

            return View(propertyObject);
        }

        // POST: PropertyObjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var propertyObject = await _context.PropertyObjects.FindAsync(id);
            _context.PropertyObjects.Remove(propertyObject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: PropertyObjects/NoticeOfInterest/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> NoticeOfInterest(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Update noticeOfInterest with PropertyObject from db.
            var propertyObject = await _context.PropertyObjects
                .Where(po => po.Id == id)
                .Include(po => po.Address)
                .Include(po => po.Realtor)
                .ThenInclude(r => r.Company)
                .FirstOrDefaultAsync();

            if (propertyObject is not null)
            {
                var user = await _userManager.GetUserAsync(User);
                var oldNoticeOfInterest = await _context.NoticeOfInterests.Where(n => (n.PropertyObjectId == propertyObject.Id) && (n.UserId == user.Id)).FirstOrDefaultAsync();
                ViewBag.OldNoticeExists = false;
                if (oldNoticeOfInterest is not null)
                {
                    ViewBag.OldNoticeExists = true;
                }
                return View(propertyObject);
            }

            // Invalid data.
            return NotFound();
        }

        // POST: PropertyObjects/AddFavoriteObject
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> SaveFavoriteObject(int id)
        {
            // Update property object with correct object from db.
            var propertyObject = await _context.PropertyObjects
                .Where(po => po.Id == id)
                .FirstOrDefaultAsync();

            var user = await _userManager.GetUserAsync(User);

            if ((user is not null) && (propertyObject is not null))
            {
                // Verify that a PropertyFavoritedByUser for this PropertyObject and user doesn't exist.
                var oldFavorite = await _context.PropertyFavorited.Where(pf => (pf.PropertyObjectId == propertyObject.Id) && (pf.UserId == user.Id)).FirstOrDefaultAsync();
                if (oldFavorite is null)
                {
                    var favorite = new PropertyFavoritedByUser();
                    favorite.PropertyObject = propertyObject;
                    favorite.PropertyObjectId = propertyObject.Id;
                    favorite.User = user;
                    favorite.UserId = user.Id;

                    _context.Add(favorite);
                    await _context.SaveChangesAsync();
                }
                return Json(true);
            }
            return Json(false);
        }


        // POST: PropertyObjects/VerifyNoticeOfInterest
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> NoticeOfInterest(PropertyObject propertyObject)
        {
            if ((propertyObject.Id > 0))
            {
                // Update property object with correct object from db.
                propertyObject = await _context.PropertyObjects
                    .Where(po => po.Id == propertyObject.Id)
                    .FirstOrDefaultAsync();
            }
            var user = await _userManager.GetUserAsync(User);

            if ((user is not null) && (propertyObject is not null))
            {
                // Verify that a notice of interest for this PropertyObject and user doesn't exist.
                var oldNoticeOfInterest = await _context.NoticeOfInterests.Where(n => (n.PropertyObjectId == propertyObject.Id) && (n.UserId == user.Id)).FirstOrDefaultAsync();
                if (oldNoticeOfInterest is null)
                {
                    var notice = new NoticeOfInterest();
                    notice.PropertyObject = propertyObject;
                    notice.PropertyObjectId = propertyObject.Id;
                    notice.User = user;
                    notice.UserId = user.Id;

                    _context.Add(notice);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", propertyObject.Id);
                }
            }
            return NotFound();
        }



        [Authorize]
        public async Task<IActionResult> SavedObjects()
        {
            var user = await _userManager.GetUserAsync(User);

            return View(user);
        }
        [Authorize]
        public async Task<IActionResult> RemoveLikedObject(int id)
        {
            var user = await _userManager.GetUserAsync(User); // Need to check for liked object for this specific user!
            var objToRemove = await _context.PropertyFavorited.FirstOrDefaultAsync(lP => lP.PropertyObject.Id == id && lP.User.Id == user.Id);
            if (objToRemove == null)
            {
                return NotFound();
            }
            _context.PropertyFavorited.Remove(objToRemove);
            _context.SaveChanges();

            return RedirectToAction("SavedObjects");
        }
        [Authorize]
        public async Task<IActionResult> RemoveObjectOfInterest(int id)
        {
            var user = await _userManager.GetUserAsync(User); // Need to check for notice if interest-object for this specific user!
            var objToRemove = await _context.NoticeOfInterests.FirstOrDefaultAsync(nI => nI.PropertyObject.Id == id && nI.User.Id == user.Id);
            if (objToRemove == null)
            {
                return NotFound();
            }
            _context.NoticeOfInterests.Remove(objToRemove);
            _context.SaveChanges();

            return RedirectToAction("SavedObjects");
        }

        private bool PropertyObjectExists(int id)
        {
            return _context.PropertyObjects.Any(e => e.Id == id);
        }

        private string GoogleMapsURL(string apiKey, string searchQuery)
        {
            // URL-encode search Query.
            string q = HttpUtility.UrlPathEncode(searchQuery);
            string src = $"https://www.google.com/maps/embed/v1/place?key={apiKey}&q={q}";

            return src;
        }
    }
}
