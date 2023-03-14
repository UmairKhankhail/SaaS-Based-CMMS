using System.ComponentModel.DataAnnotations;

namespace AccountsWebApi.Models
{
    public class Permission
    {
        [Key]
        public string permissionid { get; set; }

        [Required]
        public string permissionname { get; set; }
        public string status { get; set; }
    }
}
