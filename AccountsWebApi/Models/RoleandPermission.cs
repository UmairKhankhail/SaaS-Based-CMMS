using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class RoleandPermission
    {
        [Key]
        public int rolepermissionid { get; set; }

        public int roleautoid { get; set; }
        [ForeignKey("roleautoid")]
        public virtual Role role { get; set; }

        public string permissionid { get; set; }
        [ForeignKey("permissionid")]
        public virtual Permission permission { get; set; }


        public string companyid { get; set; }
        [ForeignKey("companyid")]
        public virtual Company companies { get; set; }

    }
}
