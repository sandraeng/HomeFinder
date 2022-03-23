using System.ComponentModel.DataAnnotations;

namespace HomeFinder.Models
{
    public class LeaseType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
