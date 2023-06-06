using Microsoft.EntityFrameworkCore;
using PreventiveMaintenanceWebApi.Models;
namespace PreventiveMaintenanceWebApi.Models
{
    public class PreventiveMaintenanceDbContext: DbContext
    {
        public PreventiveMaintenanceDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ScheduledWorkRequest> scheduledWorkRequests { get; set; }
        public DbSet<MeterReading> meterReadings { get; set; }
        public DbSet<MeterReadingEntry> meterReadingEntries { get; set; }
        public DbSet<Inspection> inspections { get; set; }
        public DbSet<InspectionEntry> inspectionEntries { get; set; }
        public DbSet<GoogleCalendarRecord> googleCalendarRecords { get; set; }
    }
}
