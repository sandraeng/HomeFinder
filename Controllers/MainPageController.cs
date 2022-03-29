using HomeFinder.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HomeFinder.Controllers
{
    public class MainPageController : Controller
    {
        private readonly HomeFinderContext _context;

        public MainPageController(HomeFinderContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchString, string minNumRooms, string maxNumRooms, string minPrice, string maxPrice)
        {
            var propertyobjects = _context.PropertyObjects
                .Include(p => p.Address)
                .Include(p => p.PropertyType)
                .Include(p => p.Images)
                .Select(p => p);

            if (!string.IsNullOrEmpty(searchString))
            {
                propertyobjects = propertyobjects.Where(p => p.Address.City.Contains(searchString) || p.Address.StreetAddress.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(minNumRooms))
            {
                var min = int.Parse(minNumRooms);
                propertyobjects = propertyobjects.Where(p => p.NumberOfRooms >= min);

            }
            if (!string.IsNullOrEmpty(maxNumRooms))
            {
                var max = int.Parse(maxNumRooms);
                propertyobjects = propertyobjects.Where(p => p.NumberOfRooms <= max);

            }
            if (!string.IsNullOrEmpty(minPrice))
            {
                var min = int.Parse(minPrice);
                propertyobjects = propertyobjects.Where(p => p.ListPrice >= min);

            }
            if (!string.IsNullOrEmpty(maxPrice))
            {
                var max = int.Parse(maxPrice);
                propertyobjects = propertyobjects.Where(p => p.ListPrice <= max);

            }

            return View(await propertyobjects.ToListAsync());
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

            return View(propertyObject);
        }
    }
}
