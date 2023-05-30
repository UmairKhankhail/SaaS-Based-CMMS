using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetWebApi.Models
{
    public class LinearAsset
    {
        [Key]
        public int lAssetAuotId { get; set; }

        public string lAssetId { get; set; }
        public int laAutoId { get; set; }
        [ForeignKey("laAutoId")]
        public virtual LinearAssetModel linearAssetModel { get; set; }

        public string laAssetName { get; set; }

        public string code { get; set; }

        public string deptId { get; set; }

        public string subDeptId { get; set; }

        public string flId { get; set; }

        public string description { get; set; }

        public string companyId { get; set; }

        [NotMapped]
        public  List<String> linearSubItems { get; set; }
    }
}
