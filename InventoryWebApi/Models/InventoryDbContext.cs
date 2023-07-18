

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace InventoryAPI.Models
{
    public class InventoryDbContext: DbContext
    {
        public InventoryDbContext(DbContextOptions options): base(options)
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

        public DbSet<Group> Inventorygroups { get; set; }

        public DbSet<Category> categories { get; set; }

        public DbSet<Equipment> equipments { get; set; }

        public DbSet<UnitOfMeasurement> unitOfMeasurements { get; set; }

        public DbSet<Issuence> issuences { get; set; }

        public DbSet<Purchase> purchases { get; set; }    

        public DbSet<Brand> brands { get; set; }

        public DbSet<IssuenceandEquipment> issuenceandEquipment { get; set; }

        public DbSet<PurchaseandEquipment> purchaseandEquipment { get; set; }

    }
}
