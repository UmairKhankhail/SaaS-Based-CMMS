using AccountsWebApi.Models;
using InventoryWebApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryAPI.Models
{
    public class Purchase
    {
        [Key]
        public int purchaseAutoId { get; set; }  

        public string purchaseId { get; set; }

        public int userAutoId { get; set; }

        public string status { get; set; }

        public string purchasesDescp { get; set; }

        public string companyId { get; set; }

        [NotMapped]
        public int validityCheck { get; set; }

        [NotMapped]
        public List<PurchaseList> equipList { get; set; }
    }
}
