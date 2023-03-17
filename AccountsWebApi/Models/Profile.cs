using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Profile
    {
        [Key]
        public int profileAutoId { get; set; }

        public string profileId { get; set; }

        [Required]
        public string profileName { get; set; }
        public string status { get; set; }

        public string companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company companies { get; set; }

        [NotMapped]
        public List<string> listUsername { get; set; }
    }
}
