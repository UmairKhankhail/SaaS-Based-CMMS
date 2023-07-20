
using AssetWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace AssetWebApi.Models
{
    public class AssetDbContext: DbContext
    {
        public AssetDbContext(DbContextOptions options): base(options)
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

        public DbSet<LinearAssetModel>  linearAssetModels{ get; set; }

        public DbSet<LinearSubItem> linearSubItems{ get; set; }

        public DbSet<LinearAsset> linearAssets { get; set; }

        public DbSet<EquipmentModel> equipmentModels{ get; set; }

        public DbSet<EquipmentSubItem> equipmentSubItems { get; set; }

        public DbSet<EquipmentAsset> equipmentAssets { get; set; }    


    }
}
