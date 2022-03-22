using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeFinder.Data;
using HomeFinder.Models;

namespace HomeFinder.Controllers
{
    public class PropertyObjectsController : Controller
    {
        private readonly HomeFinderContext _context;

        public PropertyObjectsController(HomeFinderContext context)
        {
            _context = context;
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (propertyObject == null)
            {
                return NotFound();
            }

            return View(propertyObject);
        }

        // GET: PropertyObjects/Create
        public IActionResult Create()
        {
            // Get a list of values in enum and add to viewbag, so it can be used to populate a dropdown in View.
            ViewBag.PropertyObjectStatuses = new SelectList(Enum.GetNames(typeof(PropertyObjectStatus)));
            ViewBag.PropertyTypes = _context.PropertyTypes.ToList();

            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id");
            ViewData["RealtorId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: PropertyObjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RealtorId,PropertyTypeId,Status,ListPrice,NumberOfRooms,Area,UploadedDate,NextShowingDateTime,AddressId,Address")] PropertyObject propertyObject)
        {
            // Update PropertyType with value from DB.
            propertyObject.PropertyType = _context.PropertyTypes.Where(x => x.Id == propertyObject.PropertyTypeId).FirstOrDefault();


            if (propertyObject.PropertyType is null)
            {
                // Get a list of values in enum and add to viewbag, so it can be used to populate a dropdown in View.
                ViewBag.PropertyObjectStatuses = new SelectList(Enum.GetNames(typeof(PropertyObjectStatus)));
                ViewBag.PropertyTypes = _context.PropertyTypes.ToList();

                ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id");
                ViewData["RealtorId"] = new SelectList(_context.Users, "Id", "Id");
                return View(propertyObject);
            }
            ModelState.Remove("PropertyType"); // We manually update PropertyType above, ignore it in ModelState.

            if (ModelState.IsValid)
            {
                _context.Add(propertyObject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Get a list of values in enum and add to viewbag, so it can be used to populate a dropdown in View.
            ViewBag.PropertyObjectStatuses = new SelectList(Enum.GetNames(typeof(PropertyObjectStatus)));
            ViewBag.PropertyTypes = _context.PropertyTypes.ToList();
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", propertyObject.AddressId);
            ViewData["RealtorId"] = new SelectList(_context.Users, "Id", "Id", propertyObject.RealtorId);
            return View(propertyObject);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,RealtorId,Status,ListPrice,NumberOfRooms,Area,UploadedDate,NextShowingDateTime,AddressId")] PropertyObject propertyObject)
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

        private bool PropertyObjectExists(int id)
        {
            return _context.PropertyObjects.Any(e => e.Id == id);
        }
    }
}
