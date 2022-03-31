using System.ComponentModel.DataAnnotations;

namespace HomeFinder.RoleModels
{
    public class CreateRole
    {
        [Required]
        public string RoleName { get; set; }
    }
}
