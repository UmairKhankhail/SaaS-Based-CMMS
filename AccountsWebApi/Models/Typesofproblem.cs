using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Typesofproblem
    {
        [Key]
        public int topAutoId { get; set; }

        public string topId { get; set; }

        [Required]
        public string topName { get; set; }
        public string description { get; set; }
        public string status { get; set; }

        public string companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company companies { get; set; }
    }
}
