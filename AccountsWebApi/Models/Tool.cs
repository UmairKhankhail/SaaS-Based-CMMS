using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Tool
    {
        [Key]
        public int toolautoid { get; set; }

        public string toolid { get; set; }

        [Required]
        public string toolname { get; set; }
        public string status { get; set; }

        public string companyid { get; set; }
        [ForeignKey("companyid")]
        public virtual Company companies { get; set; }
    }
}
