using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Employee
    {
        [Key]
        public int employeeautoid { get; set; }
        public string employeeid { get; set; }

        [Required]
        public string employeename { get; set; }
        public string employeefathername { get; set; }
        public string employeedesignation { get; set; }
        public string employeecontactno { get; set; }
        public string employeeemail { get; set; }
        public string status { get; set; }

        public int deptautoid { get; set; }
        [ForeignKey("deptautoid")]
        public virtual Department department { get; set; }

        public string companyid { get; set; }
        [ForeignKey("companyid")]
        public virtual Company companies { get; set; }
    }
}
