using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Typeofmaintenance
    {
        [Key]
        public int tomAutoId { get; set; }

        public string tomId { get; set; }

        [Required]
        public string tomName { get; set; }
        public string description { get; set; }
        public string status { get; set; }

        public string companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company companies { get; set; }
    }
}
