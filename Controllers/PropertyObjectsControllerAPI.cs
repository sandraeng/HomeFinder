using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeFinder.Data;
using HomeFinder.Models;
using Microsoft.Extensions.Configuration;
using System.Web;

namespace HomeFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyObjectsControllerAPI : ControllerBase
    {
        private readonly HomeFinderContext _context;
        private readonly IConfiguration configuration;

        public PropertyObjectsControllerAPI(HomeFinderContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        // GET: api/PropertyObjectsControllerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyObject>>> GetPropertyObjects()
        {
            return await _context.PropertyObjects.ToListAsync();
        }

        // GET: api/PropertyObjectsControllerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyObjectDTO>> GetPropertyObject(int id)
        {
            var propertyObject = await _context.PropertyObjects
                .Include(p => p.Address)
                .Include(p => p.Realtor)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(m => m.Id == id);
            string apiKey = configuration.GetValue<string>("GoogleMapsAPIKey");
            var propertyObjectDTO = new PropertyObjectDTO
            {
                Id = propertyObject.Id,
                AddressId = propertyObject.AddressId,
                PropertyTypeId = propertyObject.PropertyTypeId,
                RealtorId = propertyObject.RealtorId,
                Status = propertyObject.Status,
                ListPrice = propertyObject.ListPrice,
                NumberOfRooms = propertyObject.NumberOfRooms,
                Area = propertyObject.Area,
                NonLivingArea = propertyObject.NonLivingArea,
                LotArea = propertyObject.LotArea,
                UploadedDate = propertyObject.UploadedDate,
                NextShowingDateTime = propertyObject.NextShowingDateTime,
                Images = propertyObject.Images,
                Description = propertyObject.Description,
                LeaseTypeId = propertyObject.LeaseTypeId,
                YearBuilt = propertyObject.YearBuilt,
                GoogleMapsURL = GoogleMapsURL(apiKey, propertyObject.Address.FullAddress)

            };
            if (propertyObject == null)
            {
                return NotFound();
            }


            return propertyObjectDTO;
        }

        // PUT: api/PropertyObjectsControllerAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPropertyObject(int id, PropertyObject propertyObject)
        {
            if (id != propertyObject.Id)
            {
                return BadRequest();
            }

            _context.Entry(propertyObject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyObjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PropertyObjectsControllerAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PropertyObject>> PostPropertyObject(PropertyObject propertyObject)
        {
            // Update PropertyType with value from DB.
            propertyObject.PropertyType = _context.PropertyTypes.Where(x => x.Id == propertyObject.PropertyTypeId).FirstOrDefault();
            // Update LeaseType with value from DB.
            propertyObject.LeaseType = _context.LeaseTypes.Where(x => x.Id == propertyObject.LeaseTypeId).FirstOrDefault();
            // Set uploaded date to today.
            propertyObject.UploadedDate = DateTime.Now;
            // Set AddressId to 0
            propertyObject.AddressId = 0;
            _context.PropertyObjects.Add(propertyObject);
            await _context.SaveChangesAsync();
            return NoContent();

            //return CreatedAtAction("GetPropertyObject", new { id = propertyObject.Id }, propertyObject);
        }

        // DELETE: api/PropertyObjectsControllerAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePropertyObject(int id)
        {
            var propertyObject = await _context.PropertyObjects.FindAsync(id);
            if (propertyObject == null)
            {
                return NotFound();
            }

            _context.PropertyObjects.Remove(propertyObject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PropertyObjectExists(int id)
        {
            return _context.PropertyObjects.Any(e => e.Id == id);
        }
        private string GoogleMapsURL(string apiKey, string searchQuery)
        {
            // URL-encode search Query.
            string q = HttpUtility.UrlPathEncode(searchQuery);
            string src = $"https://www.google.com/maps/embed/v1/place?key={apiKey}&q={q}";

            return src;
        }
    }
}
