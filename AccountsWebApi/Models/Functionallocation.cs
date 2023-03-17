using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountsWebApi.Models
{
    public class Functionallocation
    {
        [Key]
        public int flAutoId { get; set; }

        public string flId { get; set; }

        [Required]
        public string flName { get; set; }

        [NotMapped]
        public string facilitySingleId { get; set; }

        [NotMapped]
        public string floorSingleId { get; set; }

        [NotMapped]
        public string subDeptSingleId { get; set; }

        public string status { get; set; }
        public string description { get; set; }

        public int facilityAutoId { get; set; }
        [ForeignKey("facilityAutoId")]
        public virtual Facility facilities { get; set; }

        public int floorAutoId { get; set; }
        [ForeignKey("floorAutoId")]
        public virtual Floor floors { get; set; }

        public int subDeptAutoId { get; set; }
        [ForeignKey("subDeptAutoId")]
        public virtual SubDepartment subDepartment { get; set; }

        public string companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company companies { get; set; }
    }
}
