
using MaintenanceWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MaintenanceWebApi.Models
{
    public class MaintenanceDbContext : DbContext
    {
        public MaintenanceDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Procedure> procedures { get; set; }

        public DbSet<ProcedureMethod> procedureMethods { get; set; }

        public DbSet<ProcedureHealthAndSafety> procedureHealthAndSafeties { get; set; }

        public DbSet<Method> methods { get; set; }

        public DbSet<MethodSteps> methodSteps { get; set; }

        public DbSet<WorkRequest> workRequests { get; set; }

        public DbSet<WorkOrder> workOrders { get; set; }
        public DbSet<woAssetItem> woAssetItems { get; set; }
        public DbSet<Instruction> instructions { get; set; }

        public DbSet<Execution> executions { get; set; }

        public DbSet<Evaluation> evaluations { get; set; }

        public DbSet<HealthAndSafety> healthAndSafeties { get; set; }

        public DbSet<HealthAndSafetyItems> HealthAndSafetyItems { get; set; }

        public DbSet<StatusAndRepair> statusAndRepairs { get; set; }

        public DbSet<WOItems> wOItems { get; set; }


    }
}
