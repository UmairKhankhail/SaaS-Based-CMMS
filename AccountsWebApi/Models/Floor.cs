using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Floor
    {
        [Key]
        public int floorautoid { get; set; }

        public string floorid { get; set; }

        [Required]
        public string floorname { get; set; }

        [NotMapped]
        public string facilitysingleid { get; set; }
        public string status { get; set; }

        public int facilityautoid { get; set; }
        [ForeignKey("facilityautoid")]
        public virtual Facility facilities { get; set; }

        public string companyid { get; set; }
        [ForeignKey("companyid")]
        public virtual Company companies { get; set; }
    }
}
