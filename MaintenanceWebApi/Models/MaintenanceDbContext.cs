
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

   

    }
}
