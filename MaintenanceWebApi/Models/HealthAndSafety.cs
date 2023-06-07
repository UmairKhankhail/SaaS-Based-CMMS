using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceWebApi.Models
{
    public class HealthAndSafety
    {
        [Key]
        public int hsAutoId { get; set; }

        public int woAutoId { get; set; }
        [ForeignKey("woAutoId")]
        public virtual WorkOrder WorkOrder { get; set; }


        public string userName { get; set; }

        public string remarks { get; set; }
        public string companyId { get; set; }

        [NotMapped]
        public List<string> hsCheckList { get; set; }
    }

}
