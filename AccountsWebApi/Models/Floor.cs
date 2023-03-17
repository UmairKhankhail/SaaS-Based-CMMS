using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Floor
    {
        [Key]
        public int floorAutoId { get; set; }

        public string floorId { get; set; }

        [Required]
        public string floorName { get; set; }

        [NotMapped]
        public string facilitySingleId { get; set; }
        public string status { get; set; }

        public int facilityAutoId { get; set; }
        [ForeignKey("facilityAutoId")]
        public virtual Facility facilities { get; set; }

        public string companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company companies { get; set; }
    }
}
