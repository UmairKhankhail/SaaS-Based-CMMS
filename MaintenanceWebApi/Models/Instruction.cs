using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceWebApi.Models
{
    public class Instruction
    {
        [Key]
        public int insAutoId { get; set; }

        public int pAutoId { get; set; }
        [ForeignKey("pAutoId")]
        public virtual Procedure Procedure { get; set; }

        public int woAutoId { get; set; }
        [ForeignKey("woAutoId")]
        public virtual WorkOrder WorkOrder { get; set; }


        public string companyId { get; set; }


    }
}
