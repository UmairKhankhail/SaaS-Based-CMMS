using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryAPI.Models
{
    public class PurchaseandEquipment
    {
        [Key]
        public int purchaseEquipId { get; set; }
       
        public int purchaseAutoId { get; set; }
        [ForeignKey("purchaseAutoId")]
        public virtual Purchase Purchase { get; set; }

        public int equipAutoId { get; set; }
        [ForeignKey("equipAutoId")]
        public virtual Equipment Equipment { get; set; }    

        public string companyId { get; set; }
    }
}
