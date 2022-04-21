using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        [ForeignKey("PropertyObjectId")]
        public PropertyObject PropertyObject { get; set; }
    }
}
