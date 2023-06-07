using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceWebApi.Models
{
    public class ProcedureHealthAndSafety
    {
        [Key]
        public int phsAutoId { get; set; }

        public  int pAutoId { get; set; }
        [ForeignKey("pAutoId")]
        public virtual Procedure Procedure { get; set; }

        public string cpName { get; set; }

        public string description { get; set; }

        public string companyId { get; set; }

    }
}
