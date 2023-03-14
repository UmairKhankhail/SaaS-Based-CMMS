using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Functionallocation
    {
        [Key]
        public int flautoid { get; set; }

        public string flid { get; set; }

        [Required]
        public string flname { get; set; }

        [NotMapped]
        public string facilitysingleid { get; set; }

        [NotMapped]
        public string floorsingleid { get; set; }

        [NotMapped]
        public string subdeptsingleid { get; set; }

        public string status { get; set; }
        public string description { get; set; }

        public int facilityautoid { get; set; }
        [ForeignKey("facilityautoid")]
        public virtual Facility facilities { get; set; }

        public int floorautoid { get; set; }
        [ForeignKey("floorautoid")]
        public virtual Floor floors { get; set; }

        public int subdeptautoid { get; set; }
        [ForeignKey("subdeptautoid")]
        public virtual SubDepartment subDepartment { get; set; }

        public string companyid { get; set; }
        [ForeignKey("companyid")]
        public virtual Company companies { get; set; }
    }
}
