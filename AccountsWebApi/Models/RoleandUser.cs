using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class RoleandUser
    {
        [Key]
        public int roleuserid { get; set; }

        public int roleautoid { get; set; }
        [ForeignKey("roleautoid")]
        public virtual Role role { get; set; }

        public int userautoid { get; set; }
        [ForeignKey("userautoid")]
        public virtual User user { get; set; }

        public string companyid { get; set; }
        [ForeignKey("companyid")]
        public virtual Company companies { get; set; }

    }
}
