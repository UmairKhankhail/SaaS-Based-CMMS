
using AccountsWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Models
{
    public class InventoryDbContext: DbContext
    {
        public InventoryDbContext(DbContextOptions options): base(options)
        {
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
