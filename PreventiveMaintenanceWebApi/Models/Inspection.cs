using System.ComponentModel.DataAnnotations;

namespace PreventiveMaintenanceWebApi.Models
{
    public class Inspection
    {
        [Key]
        public int inspectionAutoId { get; set; }
        public int assetModelId { get; set; }
        public string assetId { get; set; }
        public string question { get; set; }
        public string options { get; set; }
        public string workRequestOfOptions { get; set; }
        public DateTime initialDate { get; set; }
        public string eventIdCalendar { get; set; }
        public int frequencyDays { get; set; }
        public string companyId { get; set; }
    }
}
