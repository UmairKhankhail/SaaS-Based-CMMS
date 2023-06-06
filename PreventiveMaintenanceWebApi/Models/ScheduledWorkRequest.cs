using System.ComponentModel.DataAnnotations;

namespace PreventiveMaintenanceWebApi.Models
{
    public class ScheduledWorkRequest
    {
        [Key]
        public int swrAutoId { get; set; }
        public int assetModelId { get; set; }
        public string assetId { get; set; }
        public string headOfProblem { get; set; }
        public string description { get; set; }
        public int frequencyDays { get; set; }
        public DateTime initialDateTime { get; set; }
        public string eventIdCalendar { get; set; }
        public string companyId { get; set; }

    }
}
