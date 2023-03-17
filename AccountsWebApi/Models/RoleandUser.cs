using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class RoleandUser
    {
        [Key]
        public int roleUserId { get; set; }

        public int roleAutoId { get; set; }
        [ForeignKey("roleAutoId")]
        public virtual Role role { get; set; }

        public int userAutoId { get; set; }
        [ForeignKey("userAutoId")]
        public virtual User user { get; set; }

        public string companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company companies { get; set; }

    }
}
