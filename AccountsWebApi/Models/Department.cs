using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Department
    {
        [Key]
        public int deptAutoId { get; set; }

        public string deptId { get; set; }

        [Required]
        public string deptName { get; set; }
        public string status { get; set; }

        public string companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company companies { get; set; }
    }
}
