using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class RoleandDepartment
    {
        [Key]
        public int roleDeptId { get; set; }

        public int roleAutoId { get; set; }
        [ForeignKey("roleAutoId")]
        public virtual Role role { get; set; }

        public int deptAutoId { get; set; }
        [ForeignKey("deptAutoId")]
        public virtual Department department { get; set; }

        public string companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company companies { get; set; }

    }
}
