using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Typeofmaintenance
    {
        [Key]
        public int tomautoid { get; set; }

        public string tomid { get; set; }

        [Required]
        public string tomname { get; set; }
        public string description { get; set; }
        public string status { get; set; }

        public string companyid { get; set; }
        [ForeignKey("companyid")]
        public virtual Company companies { get; set; }
    }
}
