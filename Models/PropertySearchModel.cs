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
            MinNumRooms = 0;
            MinArea = 0;
            
        }
        
        public string Searchstring { get; set; }
        [Required(ErrorMessage = "Minimum rooms requires a value")]
        [Range(0,int.MaxValue, ErrorMessage = "Minimum rooms cant be lower then 0") ]
        public int? MinNumRooms { get; set; }
        [Required(ErrorMessage = "Maximum rooms requires a value")]
        [Range(0, int.MaxValue, ErrorMessage = "Maximum rooms cant be lower then 0")]
        public int? MaxNumRooms { get; set; }
        [Required(ErrorMessage = "Minimum price requires a value")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum price cant be lower then 1")]
        public int? MinPrice { get; set; }
        [Required(ErrorMessage = "Maximum price requires a value")]
        [Range(1, int.MaxValue, ErrorMessage = "Maximum price cant be lower then 1")]
        public int? MaxPrice { get; set; }
        [Required(ErrorMessage = "Minimum area requires a value")]
        [Range(0, int.MaxValue, ErrorMessage = "Minimum area cant be lower then 0")]
        public int? MinArea { get; set; }
        [Required(ErrorMessage = "Maximum area requires a value")]
        [Range(0, int.MaxValue, ErrorMessage = "Maximum area cant be lower then 0")]
        public int? MaxArea { get; set; }
        public bool IsHouse { get; set; }
        public bool IsApartment { get; set; }
        public bool IsTownhouse { get; set; }
        public bool IsFarm { get; set; }
        public bool IsLot { get; set; }

        public List<PropertyObject> Results { get; set; }
        public Pager Pager { get; set; }
    }

    
}
