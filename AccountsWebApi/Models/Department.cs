using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Department
    {
        [Key]
        public int deptautoid { get; set; }

        public string deptid { get; set; }

        [Required]
        public string deptname { get; set; }
        public string status { get; set; }

        public string companyid { get; set; }
        [ForeignKey("companyid")]
        public virtual Company companies { get; set; }
    }
}
