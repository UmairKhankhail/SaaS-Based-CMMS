using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using PreventiveMaintenanceWebApi.Models;
namespace PreventiveMaintenanceWebApi.Models
{
    public class PreventiveMaintenanceDbContext: DbContext
    {
        public PreventiveMaintenanceDbContext(DbContextOptions options) : base(options)
        {
            try
            {
                var dbcreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (dbcreator != null)
                {
                    if (!dbcreator.CanConnect()) dbcreator.Create();
                    if (!dbcreator.HasTables()) dbcreator.CreateTables();
                }
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
            }
        }

        public DbSet<ScheduledWorkRequest> scheduledWorkRequests { get; set; }
        public DbSet<MeterReading> meterReadings { get; set; }
        public DbSet<MeterReadingEntry> meterReadingEntries { get; set; }
        public DbSet<Inspection> inspections { get; set; }
        public DbSet<InspectionEntry> inspectionEntries { get; set; }
        public DbSet<GoogleCalendarRecord> googleCalendarRecords { get; set; }
    }
}
