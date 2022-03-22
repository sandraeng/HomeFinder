using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HomeFinder.Models
{
    // Add profile data for application users by adding properties to the HomeFinderUser class
    public class HomeFinderUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int? AddressId { get; set; }

        [ForeignKey("AddressId")]
        public Address Address { get; set; }

    }
}
