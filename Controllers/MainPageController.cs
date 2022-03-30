﻿using HomeFinder.Data;
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
            this.searchModel.Results = GetAllProps();
        }
        public async Task<IActionResult> Index()
        {

            return View(searchModel);
        }
        [HttpPost]
        public async Task<IActionResult> Index(PropertySearchModel searchModel)
        {
            if (searchModel.MinNumRooms > searchModel.MaxNumRooms)
            {
                ModelState.AddModelError("", "Maximumrooms must be greater or equal to Minimumrooms");
            }

            if (searchModel.MinPrice > searchModel.MaxPrice)
            {
                ModelState.AddModelError("", "MaxPrice must be greater or equal to MinPrice");
            }

            if (searchModel.MinArea > searchModel.MaxArea)
            {
                ModelState.AddModelError("", "MaxArea must be greater or equal to MinArea");
            }

            if (ModelState.IsValid)
            {
                var props = GetAllProps();

                var numberOfBool = 0;

               

                if (searchModel.IsHouse)
                {
                    searchModel.Results.AddRange(props.Where(p => p.PropertyType.PropertyTypeName == PropertyTypeName.House));
                    numberOfBool++;
                }
                if (searchModel.IsApartment)
                {
                    searchModel.Results.AddRange(props.Where(p => p.PropertyType.PropertyTypeName == PropertyTypeName.Apartment));
                    numberOfBool++;
                }
                if (searchModel.IsTownhouse)
                {
                    searchModel.Results.AddRange(props.Where(p => p.PropertyType.PropertyTypeName == PropertyTypeName.Townhouse));
                    numberOfBool++;
                }
                if (searchModel.IsFarm)
                {
                    searchModel.Results.AddRange(props.Where(p => p.PropertyType.PropertyTypeName == PropertyTypeName.Farm));
                    numberOfBool++;
                }
                if (searchModel.IsLot)
                {
                    searchModel.Results.AddRange(props.Where(p => p.PropertyType.PropertyTypeName == PropertyTypeName.Lot));
                    numberOfBool++;
                }


                if (numberOfBool > 0)
                {
                    if (!string.IsNullOrEmpty(searchModel.Searchstring))
                    {
                        searchModel.Results = searchModel.Results.Where(p => p.Address.City.Contains(searchModel.Searchstring) || p.Address.StreetAddress.Contains(searchModel.Searchstring)).ToList();
                    }

                    if (searchModel.MaxNumRooms > 0)
                    {
                        searchModel.Results = searchModel.Results.Where(p => p.NumberOfRooms >= searchModel.MinNumRooms && p.NumberOfRooms <= searchModel.MaxNumRooms).ToList();
                    }

                    searchModel.Results = searchModel.Results.Where(p => p.ListPrice >= searchModel.MinPrice && p.ListPrice <= searchModel.MaxPrice).ToList();

                    searchModel.Results = searchModel.Results.Where(p => p.ListPrice >= searchModel.MinArea && p.NumberOfRooms <= searchModel.MaxArea).ToList();
                }
                else
                {
                    if (!string.IsNullOrEmpty(searchModel.Searchstring))
                    {
                        searchModel.Results.AddRange(props.Where(p => p.Address.City.Contains(searchModel.Searchstring) || p.Address.StreetAddress.Contains(searchModel.Searchstring)));
                    }

                    if (searchModel.MaxNumRooms > 0)
                    {
                        searchModel.Results = searchModel.Results.Where(p => p.NumberOfRooms >= searchModel.MinNumRooms && p.NumberOfRooms <= searchModel.MaxNumRooms).ToList();
                    }

                    searchModel.Results = searchModel.Results.Where(p => p.ListPrice >= searchModel.MinPrice && p.ListPrice <= searchModel.MaxPrice).ToList();

                    searchModel.Results = searchModel.Results.Where(p => p.ListPrice >= searchModel.MinArea && p.NumberOfRooms <= searchModel.MaxArea).ToList();
                }



            }

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
