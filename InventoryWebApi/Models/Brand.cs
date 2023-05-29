using System.ComponentModel.DataAnnotations;

namespace InventoryAPI.Models
{
    public class Brand
    {
        [Key]
        public int brandAutoId { get; set; }

        public string brandId { get; set; }

        [Required]
        public string brandName { get; set; }

        public string status { get; set; }

        public string companyId { get; set; }   
    }
}
