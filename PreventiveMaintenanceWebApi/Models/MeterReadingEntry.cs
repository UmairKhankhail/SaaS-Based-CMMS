using System.ComponentModel.DataAnnotations;

namespace PreventiveMaintenanceWebApi.Models
{
    public class MeterReadingEntry
    {
        [Key]
        public int mreAutoId { get; set; }
        public int assetModelId { get; set; }
        public string assetId { get; set; }
        public string paramName { get; set; }
        public double value { get; set; }
        public string remarks { get; set; }
        public string companyId { get; set; }
    }
}
