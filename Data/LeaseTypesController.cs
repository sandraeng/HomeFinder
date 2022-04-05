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
    public class LeaseTypesController : Controller
    {
        private readonly HomeFinderContext _context;

        public LeaseTypesController(HomeFinderContext context)
        {
            _context = context;
        }

        // GET: LeaseTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.LeaseTypes.ToListAsync());
        }

        // GET: LeaseTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaseType = await _context.LeaseTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaseType == null)
            {
                return NotFound();
            }

            return View(leaseType);
        }

        // GET: LeaseTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaseTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] LeaseType leaseType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leaseType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leaseType);
        }

        // GET: LeaseTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaseType = await _context.LeaseTypes.FindAsync(id);
            if (leaseType == null)
            {
                return NotFound();
            }
            return View(leaseType);
        }

        // POST: LeaseTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] LeaseType leaseType)
        {
            if (id != leaseType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaseType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaseTypeExists(leaseType.Id))
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
            return View(leaseType);
        }

        // GET: LeaseTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaseType = await _context.LeaseTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaseType == null)
            {
                return NotFound();
            }

            return View(leaseType);
        }

        // POST: LeaseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaseType = await _context.LeaseTypes.FindAsync(id);
            _context.LeaseTypes.Remove(leaseType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaseTypeExists(int id)
        {
            return _context.LeaseTypes.Any(e => e.Id == id);
        }
    }
}
