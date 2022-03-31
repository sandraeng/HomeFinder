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
            
        }
        
        public string Searchstring { get; set; }
        [Range(0,int.MaxValue, ErrorMessage = "Minimumrooms cant be lower then 0") ]
        public int MinNumRooms { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Maximumrooms cant be lower then 0")]
        public int MaxNumRooms { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "MinPrice cant be lower then 0")]
        public int MinPrice { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "MaxPrice cant be lower then 0")]
        public int MaxPrice { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "MinArea cant be lower then 0")]
        public int MinArea { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "MaxArea cant be lower then 0")]
        public int MaxArea { get; set; }
        public bool IsHouse { get; set; }
        public bool IsApartment { get; set; }
        public bool IsTownhouse { get; set; }
        public bool IsFarm { get; set; }
        public bool IsLot { get; set; }

        public List<PropertyObject> Results { get; set; }
    }

    
}
