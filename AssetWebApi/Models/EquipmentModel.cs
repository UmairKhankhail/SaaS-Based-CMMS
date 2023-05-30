using System.ComponentModel.DataAnnotations;

namespace AssetWebApi.Models
{
    public class EquipmentModel
    {
        [Key]
        public int  eAutoId { get; set; }

        public string eId { get; set; }


        public string eName { get; set; }


        public string status { get; set; }

        public string companyId { get; set; }
    }
}
