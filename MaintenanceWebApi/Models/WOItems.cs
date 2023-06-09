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

        public int itemAutoId { get; set; }
        public string itemName { get; set; }
        public int quantity { get; set; }

        public int stock { get; set; }

        public int userAutoId { get; set; }
        public int cost { get; set; }
        public string requestStatus { get; set; }

        public string itemDescp { get; set; }
        public string companyId { get; set; }
    }
}
