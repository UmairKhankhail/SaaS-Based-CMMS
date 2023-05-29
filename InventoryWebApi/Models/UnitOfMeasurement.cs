using System.ComponentModel.DataAnnotations;

namespace InventoryAPI.Models
{
    public class UnitOfMeasurement
    {
        [Key]
        public int uomAutoId { get; set; }

        public string uomId { get; set; }

        public string uomName { get; set; }

        public string status { get; set;}

        public string companyId { get; set; }
    }
}
