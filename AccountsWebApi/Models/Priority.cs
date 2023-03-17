using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Priority
    {
        [Key]
        public int priorityAutoId { get; set; }

        public string priorityId { get; set; }

        [Required]
        public string priorityName { get; set; }
        public string description { get; set; }
        public string status { get; set; }

        public string companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company companies { get; set; }
    }
}
