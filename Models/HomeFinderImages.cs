using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeFinder.Models
{
    public class HomeFinderImages
    {
        public int Id { get; set; }

        [Required]
        public string Path { get; set; }

        [MaxLength(30)]
        public string AltText { get; set; }

        public int PropertyObjectId { get; set; }
        [ForeignKey("PropertyObjectId")]
        public PropertyObject PropertyObject { get; set; }
    }
}
