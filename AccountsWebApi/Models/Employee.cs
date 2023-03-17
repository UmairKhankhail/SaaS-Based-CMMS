using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Employee
    {
        [Key]
        public int employeeAutoId { get; set; }
        public string employeeId { get; set; }

        [Required]
        public string employeeName { get; set; }
        public string employeeFatherName { get; set; }
        public string employeeDesignation { get; set; }
        public string employeeContactNo { get; set; }
        public string employeeeMail { get; set; }
        public string status { get; set; }

        public int deptAutoId { get; set; }
        [ForeignKey("deptAutoId")]
        public virtual Department department { get; set; }

        public string companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company companies { get; set; }
    }
}
