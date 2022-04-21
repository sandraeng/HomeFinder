using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeFinder.Data;
using HomeFinder.Models;

namespace HomeFinder.Controllers
{
    [Route("api/HomeFinderImages")]
    [ApiController]
    public class HomeFinderImagesControllerAPI : ControllerBase
    {
        private readonly HomeFinderContext _context;

        public HomeFinderImagesControllerAPI(HomeFinderContext context)
        {
            _context = context;
        }

        // GET: api/HomeFinderImagesControllerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HomeFinderImages>>> GetImages()
        {
            return await _context.Images.ToListAsync();
        }

        // GET: api/HomeFinderImagesControllerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HomeFinderImages>> GetHomeFinderImages(int id)
        {
            var homeFinderImages = await _context.Images.FindAsync(id);

            if (homeFinderImages == null)
            {
                return NotFound();
            }

            return homeFinderImages;
        }

        // PUT: api/HomeFinderImagesControllerAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHomeFinderImages(int id, List<HomeFinderImages> homeFinderImages)
        {
            //if (id != homeFinderImages.Id)
            //{
            //    return BadRequest();
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

            return NoContent();
        }

        // POST: api/HomeFinderImagesControllerAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HomeFinderImages>> PostHomeFinderImages(HomeFinderImages homeFinderImages)
        {
            _context.Images.Add(homeFinderImages);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHomeFinderImages", new { id = homeFinderImages.Id }, homeFinderImages);
        }

        // DELETE: api/HomeFinderImagesControllerAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHomeFinderImages(int id)
        {
            var homeFinderImages = await _context.Images.FindAsync(id);
            if (homeFinderImages == null)
            {
                return NotFound();
            }

            _context.Images.Remove(homeFinderImages);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HomeFinderImagesExists(int id)
        {
            return _context.Images.Any(e => e.Id == id);
        }
    }
}
