using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class SubDepartment
    {
        [Key]
        public int subDeptAutoId { get; set; }
        public string subDeptId { get; set; }

        [Required]
        public string subDeptName { get; set; }
        public string status { get; set; }

        [NotMapped]
        public string deptSingleId { get; set; }
        public int deptAutoId { get; set; }
        [ForeignKey("deptAutoId")]
        public virtual Department department { get; set; }

        public string companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company companies { get; set; }
    }
}
