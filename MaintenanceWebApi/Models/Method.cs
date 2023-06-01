

using System.ComponentModel.DataAnnotations;

namespace MaintenanceWebApi.Models
{
    public class Method
    {
        [Key]
        public int mtAutoId { get; set; }

        public string mtName { get; set; }

        public string assetName { get; set; }

        public string downTimeValue { get; set; }

        public string companyId { get; set; }
    }
}
