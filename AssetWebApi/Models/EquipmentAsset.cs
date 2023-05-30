using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetWebApi.Models
{
    public class EquipmentAsset
    {
        [Key]
        public int eAssetAuotoId { get; set; }

        public string eAssetId { get; set; }
        public int eAutoId { get; set; }
        [ForeignKey("eAutoId")]
        public virtual EquipmentModel equipmentModel{ get; set; }

        public string eAssetName { get; set; }

        public string code { get; set; }

        public string brandNmae { get; set; }

        public string deptId { get; set; }

        public string subDeptId  { get; set; }

        public string flId { get; set; }

        public  string description { get; set; }

        public string companyId { get; set; }
    }
}
