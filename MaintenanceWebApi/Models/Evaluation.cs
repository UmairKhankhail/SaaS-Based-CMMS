using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceWebApi.Models
{
    public class Evaluation
    {
        [Key]
        public int evaluationAutoId { get; set; }

        public int woAutoId { get; set; }
        [ForeignKey("woAutoId")]
        public virtual WorkOrder WorkOrder { get; set; }

        public string evaluationId { get; set; }

        public string userName { get; set; }

        public string topName { get; set; }

        public DateTime startTime { get; set; }

        public DateTime endTime { get; set; }

        public string remarks { get; set; }


        public string companyId { get; set; }

    }
}
