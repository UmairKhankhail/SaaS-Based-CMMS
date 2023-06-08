using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaintenanceWebApi.Models
{
    public class woAssetItem
    {
        [Key]
        public int woAssetItemAutoId { get; set; }

        public int woAutoId { get; set; }
        [ForeignKey("woAutoId")]
        public virtual WorkOrder WorkOrder { get; set; }

        public string woAssetItemName { get; set; }

        public string woAssetItemType { get; set; }

        public string woAssetItemsApproval { get; set; }

        public string companyId { get; set; }
    }
}
