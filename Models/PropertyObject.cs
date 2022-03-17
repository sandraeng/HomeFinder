using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeFinder.Models
{
    public class PropertyObject
    {
        public int Id { get; set; }
        public PropertyType PropertyType { get; set; }
        public HomeFinderUser Realtor { get; set; }
        public PropertyObjectStatus Status { get; set; }
        public decimal ListPrice { get; set; }
        public decimal NumberOfRooms { get; set; }
        public decimal Area { get; set; }
        [DataType(DataType.Date)]
        public DateTime UploadedDate { get; set; }
        public DateTime NextShowingDateTime { get; set; }
        public Address Address { get; set; }
        public List<HomeFinderImages> Images { get; set; } = new List<HomeFinderImages>();
    }
}
