using System.ComponentModel.DataAnnotations;

namespace HomeFinder.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string OrgNumber { get; set; }
    }
}
