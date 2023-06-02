using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceWebApi.Models
{
    public class WOItems
    {
        [Key]
        public int woItemsAutoId { get; set; }

        public string woItemsId { get; set; }

        public int woAutoId { get; set; }
        [ForeignKey("woAutoId")]
        public virtual WorkOrder WorkOrder { get; set; }


        public string quantity { get; set; }

        public string stock { get; set; }

        public string cost { get; set; }
        public string requestStatus { get; set; }

        public string companyId { get; set; }
    }
}
