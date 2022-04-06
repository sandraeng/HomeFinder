using HomeFinder.Data;
using HomeFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeFinder.Controllers
{
    public class MainPageController : Controller
    {
        private readonly HomeFinderContext _context;
        private readonly PropertySearchModel searchModel;

        public MainPageController(HomeFinderContext context, PropertySearchModel searchModel)
        {
            _context = context;
            this.searchModel = searchModel;

            



        }
        public IActionResult Index(int? page)
        {
            searchModel.Results = GetAllProps();

            searchModel.MaxPrice = (int)searchModel.Results.Max(p => p.ListPrice);
            searchModel.MinPrice = (int)searchModel.Results.Min(p => p.ListPrice);

            searchModel.MaxArea = (int)searchModel.Results.Max(p => p.Area);
            searchModel.MinArea = 0;

            searchModel.MaxNumRooms = (int)searchModel.Results.Max(p => p.NumberOfRooms);
            var pager = new Pager(searchModel.Results.Count, page);
            var model = new PropertySearchModel
            {
                Results = searchModel.Results.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList(),
                Pager = pager
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Index(PropertySearchModel searchModel, int? page)
        {
            if (searchModel.MinNumRooms > searchModel.MaxNumRooms)
            {
                ModelState.AddModelError("MaxNumRooms", "Maximum rooms must be greater or equal to minimum rooms");
            }

            if (searchModel.MinPrice > searchModel.MaxPrice)
            {
                ModelState.AddModelError("MaxPrice", "Maximum price must be greater or equal to minimum price");
            }

            if (searchModel.MinArea > searchModel.MaxArea)
            {
                ModelState.AddModelError("MaxArea", "Maximum area must be greater or equal to minimum area");
            }

            if (ModelState.IsValid)
            {
                var props = GetAllProps();
                


                int numberBool = 0;
                

                if (searchModel.IsHouse)
                {
                    searchModel.Results.AddRange(props.Where(p => p.PropertyType.PropertyTypeName == PropertyTypeName.House));
                    numberBool++;
                }
                
                if (searchModel.IsApartment)
                {
                    searchModel.Results.AddRange(props.Where(p => p.PropertyType.PropertyTypeName == PropertyTypeName.Apartment));
                    numberBool++;
                }
               
                if (searchModel.IsTownhouse)
                {
                    searchModel.Results.AddRange(props.Where(p => p.PropertyType.PropertyTypeName == PropertyTypeName.Townhouse));
                    numberBool++;
                }
                
                if (searchModel.IsFarm)
                {
                    searchModel.Results.AddRange(props.Where(p => p.PropertyType.PropertyTypeName == PropertyTypeName.Farm));
                    numberBool++;
                }
                
                if (searchModel.IsLot)
                {
                    searchModel.Results.AddRange(props.Where(p => p.PropertyType.PropertyTypeName == PropertyTypeName.Lot));
                    numberBool++;
                }

                if (numberBool > 0)
                {
                    searchModel.Results = searchModel.Results.Where(p => p.ListPrice >= searchModel.MinPrice && p.ListPrice <= searchModel.MaxPrice).ToList();
                    searchModel.Results = searchModel.Results.Where(p => p.Area >= searchModel.MinArea && p.Area <= searchModel.MaxArea).ToList();
                }
                else
                {
                    searchModel.Results = props.Where(p => p.ListPrice >= searchModel.MinPrice && p.ListPrice <= searchModel.MaxPrice).ToList();
                    searchModel.Results = searchModel.Results.Where(p => p.Area >= searchModel.MinArea && p.Area <= searchModel.MaxArea).ToList();
                }

                if (!string.IsNullOrEmpty(searchModel.Searchstring))
                {
                    searchModel.Searchstring = searchModel.Searchstring.ToLower().Trim();
                     searchModel.Results = searchModel.Results.Where(p => p.Address.City.ToLower().Trim().Contains(searchModel.Searchstring) || p.Address.StreetAddress.ToLower().Contains(searchModel.Searchstring)).ToList();
                }




            }
            var pager = new Pager(searchModel.Results.Count, page);
                searchModel.Results.Where(p => p.NumberOfRooms >= searchModel.MinNumRooms && p.NumberOfRooms <= searchModel.MaxNumRooms).Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();
                searchModel.Pager = pager;
            

            return View(searchModel);
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

        private List<PropertyObject>GetAllProps(){

            return _context.PropertyObjects
                .Include(p => p.Address)
                .Include(p => p.PropertyType)
                .Include(p => p.Images).ToList();

        }
    }
}
