using System.ComponentModel.DataAnnotations.Schema;

namespace HomeFinder.Models
{
    public class PropertyFavoritedByUser
    {
        public string UserId { get; set; }
        public int PropertyObjectId { get; set; }

        [ForeignKey("UserId")]
        public HomeFinderUser User { get; set; }
        [ForeignKey("PropertyObjectId")]
        public PropertyObject PropertyObject { get; set; }
    }
}
