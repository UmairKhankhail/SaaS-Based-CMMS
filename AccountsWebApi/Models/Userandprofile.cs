using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Userandprofile
    {
        [Key]
        public int userandprofileautoid { get; set; }

        public int profileautoid { get; set; }
        [ForeignKey("profileautoid")]
        public virtual Profile profiles { get; set; }

        public int userautoid { get; set; }
        [ForeignKey("userautoid")]
        public virtual User users { get; set; }

        public string companyid { get; set; }
        [ForeignKey("companyid")]
        public virtual Company companies { get; set; }
    }
}
