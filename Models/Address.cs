using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HomeFinder.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Display(Name = "Street Address")]
        [MaxLength(50)]
        public string StreetAddress { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(8)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }
    }
}
