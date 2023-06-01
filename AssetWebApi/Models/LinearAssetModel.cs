using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetWebApi.Models
{
    public class LinearAssetModel
    {
        [Key]
        public int laAutoID { get; set; }

        public string laID { get; set; }

        public string laName { get; set; }


        public string status { get; set; }

        public string companyId { get; set; }

        [NotMapped]
        public List<LinearSubItemList> listSubItems { get; set; }

        [NotMapped]
        public int validityCheck { get; set; }

    }
}
