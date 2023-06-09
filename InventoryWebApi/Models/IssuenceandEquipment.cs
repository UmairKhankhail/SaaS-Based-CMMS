using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryAPI.Models
{
    public class IssuenceandEquipment
    {
        [Key]
        public int issueEquipId { get; set; }

        public int issuenceAutoId { get; set; }
        [ForeignKey("issuenceAutoId")]
        public virtual Issuence Issuence { get; set; }

        public int equipAutoId { get; set; }
        [ForeignKey("equipAutoId")]
        public virtual Equipment Equipment{ get; set; }
        
        public string equipName { get; set; }
        public int quantity { get; set; }
        public string companyId { get; set; }
    }
}
