using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Typesofproblem
    {
        [Key]
        public int topautoid { get; set; }

        public string topid { get; set; }

        [Required]
        public string topname { get; set; }
        public string description { get; set; }
        public string status { get; set; }

        public string companyid { get; set; }
        [ForeignKey("companyid")]
        public virtual Company companies { get; set; }
    }
}
