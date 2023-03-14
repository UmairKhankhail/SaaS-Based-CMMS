using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Position
    {
        [Key]
        public int positionautoid { get; set; }

        public string positionid { get; set; }

        [Required]
        public string positionname { get; set; }
        public string status { get; set; }

        public string companyid { get; set; }
        [ForeignKey("companyid")]
        public virtual Company companies { get; set; }
    }
}
