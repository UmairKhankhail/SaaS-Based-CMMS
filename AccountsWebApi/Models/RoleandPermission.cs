using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class RoleandPermission
    {
        [Key]
        public int rolePermissionId { get; set; }

        public int roleAutoId { get; set; }
        [ForeignKey("roleAutoId")]
        public virtual Role role { get; set; }

        public string permissionId { get; set; }
        [ForeignKey("permissionId")]
        public virtual Permission permission { get; set; }


        public string companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company companies { get; set; }

    }
}
