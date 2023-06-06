using System.ComponentModel.DataAnnotations;

namespace PreventiveMaintenanceWebApi.Models
{
    public class MeterReading
    {
        [Key]
        public int mrAutoId { get; set; }
        public int assetModelId { get; set; }
        public string assetId { get; set; }
        public string paramType { get; set; }
        public string paramName { get; set; }
        public double minValue { get; set; }
        public double maxValue { get; set; }
        public DateTime initialDate { get; set; }
        public string eventIdCalendar { get; set; }
        public int frequencyDays { get; set; }
        public string companyId { get; set; }
    }
}
