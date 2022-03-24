using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeFinder.Models
{
    public class PropertyObject
    {
        public int Id { get; set; }

        public int PropertyTypeId { get; set; }

        [Required]
        [ForeignKey("PropertyTypeId")]
        [Display(Name = "Property Type")]
        public PropertyType PropertyType { get; set; }

        public string RealtorId { get; set; }

        [ForeignKey("RealtorId")]
        public HomeFinderUser Realtor { get; set; }

        [Required]
        public PropertyObjectStatus Status { get; set; }

        [Required]
        [Display(Name = "List price")]
        public decimal ListPrice { get; set; }

        [Required]
        [Display(Name = "Number of rooms")]
        public decimal NumberOfRooms { get; set; }

        [Required]
        [Display(Name = "Living area (sqm)")]
        public decimal Area { get; set; } // Net internal area/useable floor area. Svenska: boyta
        [Required]
        [Display(Name = "Non-living area (sqm)")]
        public decimal NonLivingArea { get; set; } // Gross internal area minus Net internal area. Svenska: biyta
        [Required]
        [Display(Name = "Lot area (sqm)")]
        public decimal LotArea { get; set; } // Tomtyta

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Uploaded date")]
        public DateTime UploadedDate { get; set; }

        [Display(Name = "Date of next showing")]
        public DateTime NextShowingDateTime { get; set; }

        public int AddressId { get; set; }

        [ForeignKey("AddressId")]
        public Address Address { get; set; }

        [Required]
        public List<HomeFinderImages> Images { get; set; } = new List<HomeFinderImages>();

        public string Description { get; set; }

        public int LeaseTypeId { get; set; }
        [Required]
        [ForeignKey("LeaseTypeId")]
        [Display(Name = "Lease type")]
        public LeaseType LeaseType { get; set; }

        [Display(Name = "Year built")]
        public int YearBuilt { get; set; }
    }
}