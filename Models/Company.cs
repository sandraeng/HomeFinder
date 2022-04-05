using System.ComponentModel.DataAnnotations;

namespace HomeFinder.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Display(Name ="*Company name")]
        [Required]
        public string Name { get; set; }

        [Display(Name ="*Organization number")]
        [Required]
        public string OrgNumber { get; set; }
    }
}
