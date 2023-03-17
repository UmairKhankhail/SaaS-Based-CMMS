using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Role
    {
        [Key]
        public int roleAutoId { get; set; }
        public string roleId { get; set; }

        [Required]
        public string roleName { get; set; }
        public string status { get; set; }

        public string companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company companies { get; set; }

        [NotMapped]
        public List<string> listDepartments { get; set; }

        [NotMapped]
        public List<string> listPermissions { get; set; }
    }
}
