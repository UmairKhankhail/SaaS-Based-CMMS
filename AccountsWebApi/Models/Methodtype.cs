using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Methodtype
    {
        [Key]
        public int mtAutoId { get; set; }

        public string mtId { get; set; }

        [Required]
        public string mtName { get; set; }
        public string description { get; set; }
        public string status { get; set; }

        public string companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company companies { get; set; }
    }
}
