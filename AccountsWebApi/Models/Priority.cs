using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Priority
    {
        [Key]
        public int priorityautoid { get; set; }

        public string priorityid { get; set; }

        [Required]
        public string priorityname { get; set; }
        public string description { get; set; }
        public string status { get; set; }

        public string companyid { get; set; }
        [ForeignKey("companyid")]
        public virtual Company companies { get; set; }
    }
}
