using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Position
    {
        [Key]
        public int positionAutoId { get; set; }

        public string positionId { get; set; }

        [Required]
        public string positionName { get; set; }
        public string status { get; set; }

        public string companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company companies { get; set; }
    }
}
