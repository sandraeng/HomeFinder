using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeFinder.Models
{
    public class PropertyObject
    {
        public int Id { get; set; }

        [Required]
        public PropertyType PropertyType { get; set; }

        public string RealtorId { get; set; }
        [ForeignKey("RealtorId")]
        public HomeFinderUser Realtor { get; set; }

        [Required]
        public PropertyObjectStatus Status { get; set; }

        [Required]
        public decimal ListPrice { get; set; }

        [Required]
        public decimal NumberOfRooms { get; set; }

        [Required]
        public decimal Area { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime UploadedDate { get; set; }

        public DateTime NextShowingDateTime { get; set; }

        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public Address Address { get; set; }

        [Required]
        public List<HomeFinderImages> Images { get; set; } = new List<HomeFinderImages>();
    }
}
