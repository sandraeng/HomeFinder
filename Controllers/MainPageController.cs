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
                .Include(p => p.PropertyType);
                
                
            return View(await propertyobjects.ToListAsync());
        }
    }
}
