using HomeFinder.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeFinder.Models
{
    public class PropertySearchModel
    {
        public PropertySearchModel()
        {
            Results = new List<PropertyObject>();
            Searchstring = "";
        }
        
        public string Searchstring { get; set; }
        public int MinNumRooms { get; set; }
        public int MaxNumRooms { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int MinArea { get; set; }
        public int MaxArea { get; set; }
        public bool IsHouse { get; set; }
        public bool IsApartment { get; set; }
        public bool IsTownhouse { get; set; }
        public bool IsFarm { get; set; }
        public bool IsLot { get; set; }

        public List<PropertyObject> Results { get; set; }
    }

    
}
