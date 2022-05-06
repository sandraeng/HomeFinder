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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace HomeFinder.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PropertyTypesController : Controller
    {
        private readonly HomeFinderContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public PropertyTypesController(HomeFinderContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: PropertyTypes
        public async Task<IActionResult> Index()
        {
            // API: ListPropertyTypes()
            // ViewModel: PropertyTypes

            return View(await _context.PropertyTypes.ToListAsync());
        }

        // GET: PropertyTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // API: GetPropertyTypeById()
            // ViewModel: PropertyType

            var propertyType = await _context.PropertyTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (propertyType == null)
            {
                return NotFound();
            }

            return View(propertyType);
        }

        // GET: PropertyTypes/Create
        public IActionResult Create()
        {
            // API: ListPropertyTypes()
            // ViewModel: PropertyType

            // Get a list of values in enum and add to viewbag, so it can be used to populate a dropdown in View.
            ViewBag.PropertyTypeNames = new SelectList(Enum.GetNames(typeof(PropertyTypeName)));

            return View();
        }

        // POST: PropertyTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IconUrl,PropertyTypeName")] PropertyType propertyType, IFormFile file)
        {
            // API: AddPropertyType()
            // POST ViewModel: PropertyType
            // API: UploadFile() ?

            string path = UploadFile(file);
            propertyType.IconUrl = "~/Images/" + path;
            ModelState.Remove("IconUrl");
            if (ModelState.IsValid)
            {
                _context.Add(propertyType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(propertyType);
        }
        private string UploadFile(IFormFile file)
        {
            // API: UploadFile()
            // POST ViewModel: File/Image?

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
        // GET: PropertyTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // API: ListPropertyTypes()
            // API: GetPropertyTypeById()
            // ViewModel: PropertyType

            if (id == null)
            {
                return NotFound();
            }
            var propertyType = await _context.PropertyTypes.FindAsync(id);
            if (propertyType == null)
            {
                return NotFound();
            }
            ViewBag.PropertyTypeNames = new SelectList(Enum.GetNames(typeof(PropertyTypeName)));

            return View(propertyType);
        }

        // POST: PropertyTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IconUrl,PropertyTypeName")] PropertyType propertyType, IFormFile file)
        {
            // API: ListPropertyTypes()
            // API: GetPropertyTypeById()
            // API: UpdatePropertyType()
            // ViewModel: PropertyType
            // POST ViewModel: PropertyType

            if (id != propertyType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(file != null)
                    {
                        string path = UploadFile(file);
                        propertyType.IconUrl = "~/Images/" + path;
                    }
                    _context.Update(propertyType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyTypeExists(propertyType.Id))
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
            return View(propertyType);
        }

        // GET: PropertyTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // API: GetPropertyTypeById()

            if (id == null)
            {
                return NotFound();
            }

            var propertyType = await _context.PropertyTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (propertyType == null)
            {
                return NotFound();
            }

            return View(propertyType);
        }

        // POST: PropertyTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // API: RemovePropertyType()

            var propertyType = await _context.PropertyTypes.FindAsync(id);
            _context.PropertyTypes.Remove(propertyType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyTypeExists(int id)
        {
            return _context.PropertyTypes.Any(e => e.Id == id);
        }
    }
}
