using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeFinder.Models
{
    public class PropertyObjectDTO
    {
        public int Id { get; set; }

        public int PropertyTypeId { get; set; }


        public PropertyType PropertyType { get; set; }

        public string RealtorId { get; set; }

        public HomeFinderUser Realtor { get; set; }

        public PropertyObjectStatus Status { get; set; }

        public decimal ListPrice { get; set; }

  
        public decimal NumberOfRooms { get; set; }

        public decimal Area { get; set; } // Net internal area/useable floor area. Svenska: boyta
 
        public decimal NonLivingArea { get; set; } // Gross internal area minus Net internal area. Svenska: biyta

        public decimal LotArea { get; set; } // Tomtyta

      
        public DateTime UploadedDate { get; set; }

        public DateTime NextShowingDateTime { get; set; }

        public int AddressId { get; set; }

        public Address Address { get; set; }

        public List<HomeFinderImages> Images { get; set; } = new List<HomeFinderImages>();

        public string Description { get; set; }

        public int LeaseTypeId { get; set; }

        public LeaseType LeaseType { get; set; }

        public int YearBuilt { get; set; }

        public string GoogleMapsURL { get; set; }
    }
}