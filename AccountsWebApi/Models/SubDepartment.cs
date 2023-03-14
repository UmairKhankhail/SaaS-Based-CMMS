using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class SubDepartment
    {
        [Key]
        public int subdeptautoid { get; set; }
        public string subdeptid { get; set; }

        [Required]
        public string subdeptname { get; set; }
        public string status { get; set; }

        [NotMapped]
        public string deptsingleid { get; set; }
        public int deptautoid { get; set; }
        [ForeignKey("deptautoid")]
        public virtual Department department { get; set; }

        public string companyid { get; set; }
        [ForeignKey("companyid")]
        public virtual Company companies { get; set; }
    }
}
