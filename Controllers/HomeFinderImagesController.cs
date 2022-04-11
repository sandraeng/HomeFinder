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
    public class HomeFinderImagesController : Controller
    {
        private readonly HomeFinderContext _context;

        public HomeFinderImagesController(HomeFinderContext context)
        {
            _context = context;
        }

        // GET: HomeFinderImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Id = id;

            var homeFinderImages = await _context.Images.Where(i => i.PropertyObjectId == id).ToListAsync();

            if (homeFinderImages == null)
            {
                return NotFound();
            }
            //ViewData["PropertyObjectId"] = new SelectList(_context.PropertyObjects, "Id", "Id", homeFinderImages.PropertyObjectId);
            return View(homeFinderImages);
        }

        // POST: HomeFinderImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Path,AltText,PropertyObjectId")] List<HomeFinderImages> homeFinderImages)
        {
            //if (id != homeFinderImages.Id)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                foreach (var image in homeFinderImages)
                {
                    try
                    {
                        _context.Update(image);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!HomeFinderImagesExists(image.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return RedirectToAction("Index", "MainPage");
            }
          
            //ViewData["PropertyObjectId"] = new SelectList(_context.PropertyObjects, "Id", "Id", homeFinderImages.PropertyObjectId);
            return View(homeFinderImages);
        }

        // GET: HomeFinderImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeFinderImages = await _context.Images
                .Include(h => h.PropertyObject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeFinderImages == null)
            {
                return NotFound();
            }

            return View(homeFinderImages);
        }

        // POST: HomeFinderImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homeFinderImages = await _context.Images.FindAsync(id);
            _context.Images.Remove(homeFinderImages);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeFinderImagesExists(int id)
        {
            return _context.Images.Any(e => e.Id == id);
        }
    }
}
