using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetWebApi.Models
{
    public class EquipmentSubItem
    {
        [Key]
        public int esAutoId { get; set; }

        public string esId { get; set; }

        public int eAutoId { get; set; }
        [ForeignKey("eAuotId")]
        public virtual EquipmentModel equipmentModel { get; set; }

        public string esName { get; set; }

        public string esDescription { get; set; }

        public string esPosition { get; set; }
        
        public int esParentId { get; set; }

        public string companyId { get; set; }


    }
}
