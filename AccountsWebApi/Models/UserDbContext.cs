using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using AccountsWebApi.Models;
using Microsoft.Build.Evaluation;

namespace AccountsWebApi.Models
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<Department> departments { get; set; }
        public DbSet<SubDepartment> subDepartments { get; set; }
        public DbSet<Company> companies { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Permission> permissions { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<RoleandPermission> roleAndPermissions { get; set; }
        public DbSet<RoleandDepartment> roleAndDepartments { get; set; }
        public DbSet<RoleandUser> userAndRoles { get; set; }
        public DbSet<Facility> facilities { get; set; }
        public DbSet<Floor> floors { get; set; }
        public DbSet<Functionallocation> functionalLocations { get; set; }
        public DbSet<Position> positions { get; set; }
        public DbSet<Tool> tools { get; set; }
        public DbSet<Methodtype> methodTypes { get; set; }
        public DbSet<Typeofmaintenance> typeOfMaintenances { get; set; }
        public DbSet<Typesofproblem> typesOfProblems { get; set; }
        public DbSet<Priority> priorities { get; set; }
        public DbSet<Profile> profiles { get; set; }
        public DbSet<Userandprofile> userAndProfiles { get; set; }
    }
}
