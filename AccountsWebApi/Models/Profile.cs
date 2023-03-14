using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Profile
    {
        [Key]
        public int profileautoid { get; set; }

        public string profileid { get; set; }

        [Required]
        public string profilename { get; set; }
        public string status { get; set; }

        public string companyid { get; set; }
        [ForeignKey("companyid")]
        public virtual Company companies { get; set; }

        [NotMapped]
        public List<string> list_username { get; set; }
    }
}
