using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryAPI.Models
{
    public class Equipment
    {
        [Key]
        public int equipAutoId { get; set; }

        public string equipId { get; set; }

        [Required]
        public string equipName { get; set; }

        public string equipCost { get; set; }

        public string equipLeadTime { get; set; }

        public string status { get; set;}

        public int catAutoId { get; set; }
        [ForeignKey("catAutoId")]
        public virtual Category categories { get; set; }

        public int groupAutoId { get; set; }
        [ForeignKey("groupAutoId")]
        public virtual Group groups { get; set; }

        public string companyId { get; set; }
    }
}
