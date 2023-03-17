using System.ComponentModel.DataAnnotations;

namespace AccountsWebApi.Models
{
    public class Permission
    {
        [Key]
        public string permissionId { get; set; }

        [Required]
        public string permissionName { get; set; }
        public string status { get; set; }
    }
}
