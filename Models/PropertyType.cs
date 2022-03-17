using System.ComponentModel.DataAnnotations;

namespace HomeFinder.Models
{
    public class PropertyType
    {
        public int Id { get; set; }

        [Required]
        public string IconUrl { get; set; }

        [Required]
        public PropertyTypeName PropertyTypeName { get; set; }
    }
}
