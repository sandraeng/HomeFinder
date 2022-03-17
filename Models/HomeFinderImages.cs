using System.ComponentModel.DataAnnotations;

namespace HomeFinder.Models
{
    public class HomeFinderImages
    {
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        [MaxLength(30)]
        public string AltText { get; set; }
    }
}
