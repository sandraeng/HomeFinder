using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace HomeFinder.Models
{
    public class Address
    {
        [HiddenInput(DisplayValue = false)]
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

        [NotMapped]
        public string FullAddress
        {
            get
            {
                string fa = $"{StreetAddress}, {PostalCode} {City}, {Country}";
                // Remove whitespaces immediately before ','.
                fa = Regex.Replace(fa, @"\s+,+", ",");
                // Remove multiple consecutive ','.
                fa = Regex.Replace(fa, @",+", ",");
                // In case some values were null, remove excess whitespaces.
                fa = Regex.Replace(fa, @"(\s)\s+", "$1");
                // Trim whitespaces and ','.
                fa = Regex.Replace(fa, @"^[\s,]+|[\s,]+$", "");
                return fa;
            }
        }
    }
}
