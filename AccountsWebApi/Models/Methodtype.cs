using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Methodtype
    {
        [Key]
        public int mtautoid { get; set; }

        public string mtid { get; set; }

        [Required]
        public string mtname { get; set; }
        public string description { get; set; }
        public string status { get; set; }

        public string companyid { get; set; }
        [ForeignKey("companyid")]
        public virtual Company companies { get; set; }
    }
}
