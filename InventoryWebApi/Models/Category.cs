using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryAPI.Models
{
    public class Category
    {
        [Key]

        public int catAutoId { get; set; }

        public string catId { get; set; }

        [Required]
        public string catName { get; set; }

        public int groupAutoId { get; set; }
        [ForeignKey("groupAutoId")]
        public virtual Group groups { get; set; }

        public string status { get; set; }

        public string companyId { get; set; }

       
    }
}
