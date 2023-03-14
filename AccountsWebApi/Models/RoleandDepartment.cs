using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class RoleandDepartment
    {
        [Key]
        public int roledeptid { get; set; }

        public int roleautoid { get; set; }
        [ForeignKey("roleautoid")]
        public virtual Role role { get; set; }

        public int deptautoid { get; set; }
        [ForeignKey("deptautoid")]
        public virtual Department department { get; set; }

        public string companyid { get; set; }
        [ForeignKey("companyid")]
        public virtual Company companies { get; set; }

    }
}
