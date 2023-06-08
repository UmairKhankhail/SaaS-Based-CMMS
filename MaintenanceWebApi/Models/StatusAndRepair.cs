using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceWebApi.Models
{
    public class StatusAndRepair
    {
        [Key]
        public int srAutoId { get; set; }

        public string srId { get; set; }

        public int woAutoId { get; set; }
        [ForeignKey("woAutoId")]
        public virtual WorkOrder WorkOrder { get; set; }
        public string username { get; set; }
        
        public string itemName { get; set; }

        public string faultyNotFaulty { get; set; }

        public string inhouseOrOutsource { get; set; }

        public string worker { get; set; }

        public string companyId { get; set; }
    }
}
