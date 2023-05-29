using System.ComponentModel.DataAnnotations;

namespace InventoryAPI.Models
{
    public class Group
    {
        [Key]
        public int groupAutoID { get; set; }

        public string groupID { get; set; }

        [Required]
        public string groupName { get; set; }

        public string status { get; set; }
        
        public string companyId { get; set; }
    }
}
