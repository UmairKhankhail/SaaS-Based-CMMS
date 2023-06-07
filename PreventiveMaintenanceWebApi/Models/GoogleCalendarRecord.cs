using System.ComponentModel.DataAnnotations;

namespace PreventiveMaintenanceWebApi.Models
{
    public class GoogleCalendarRecord
    {
        [Key]
        public int googleCalendarAutoId { get; set; }
        public string googleCalendarId { get; set; }
        public string calendarSummary { get; set; }
        public string calendarDescription { get; set; }
        public string timeZoneGMT { get; set; }
        public string timeZoneWord { get; set; }
        public string timeZoneRegional { get; set; }
        public string iFrame { get; set; }
        public string companyId { get; set; }
    }
}
