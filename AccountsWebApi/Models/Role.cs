using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Role
    {
        [Key]
        public int roleautoid { get; set; }
        public string roleid { get; set; }

        [Required]
        public string rolename { get; set; }
        public string status { get; set; }

        public string companyid { get; set; }
        [ForeignKey("companyid")]
        public virtual Company companies { get; set; }

        [NotMapped]
        public List<string> list_Departments { get; set; }

        [NotMapped]
        public List<string> list_permissions { get; set; }
    }
}
