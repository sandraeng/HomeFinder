using HomeFinder.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index()
        {
            var propertyobjects = _context.PropertyObjects
                .Include(p => p.Address)
                .Include(p => p.PropertyType)
                .Include(p => p.Images);
                
                
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
