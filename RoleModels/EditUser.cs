using HomeFinder.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeFinder.RoleModels
{
    public class EditUser
    {
        public string Id { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public int? CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

        public int? AddressId { get; set; }

        [ForeignKey("AddressId")]
        public Address Address { get; set; }
        public List<string> Claims { get; set; } = new List<string>();
        public List<string> Roles { get; set; } = new List<string>();
    }
}
