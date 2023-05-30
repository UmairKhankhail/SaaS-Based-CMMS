using Microsoft.CodeAnalysis.Options;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetWebApi.Models
{
    public class EquipmentModel
    {
        [Key]
        public int  eAutoId { get; set; }

        public string eId { get; set; }

        [NotMapped]
        public int validityCheck { get; set; }
        public string eName { get; set; }

        [NotMapped]
        public List<EquipmentSubItemsList> listSubItems { get; set; }

        public string status { get; set; }

        public string companyId { get; set; }
    }
}
