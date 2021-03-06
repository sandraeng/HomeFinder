using System.ComponentModel.DataAnnotations.Schema;

namespace HomeFinder.Models
{
    public class NoticeOfInterest
    {
        public string UserId { get; set; }
        public int PropertyObjectId { get; set; }
        public bool HandledByRealtor { get; set; } = false;

        [ForeignKey("UserId")]
        public HomeFinderUser User { get; set; }

        [ForeignKey("PropertyObjectId")]
        public PropertyObject PropertyObject { get; set; }
    }
}
