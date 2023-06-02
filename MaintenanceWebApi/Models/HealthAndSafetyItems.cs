using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace MaintenanceWebApi.Models
{
    public class HealthAndSafetyItems
    {
        [Key]
        public int hsiAutoId { get; set; }

        public int hsAutoId { get; set; }
        [ForeignKey("hsAutoId")]
        public virtual HealthAndSafety HealthAndSafety { get; set; }

        public int phsAutoId { get; set; }
        [ForeignKey("phsAutoId")]
        public virtual ProcedureHealthAndSafety ProcedureHealthAndSafety { get; set; }


        public int woAutoId { get; set; }
        [ForeignKey("woAutoId")]
        public virtual WorkOrder WorkOrder { get; set; }
        public string companyId { get; set; }

    }
}
