
using AssetWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Models
{
    public class AssetDbContext: DbContext
    {
        public AssetDbContext(DbContextOptions options): base(options)
        {
        }

        public DbSet<LinearAssetModel>  linearAssetModels{ get; set; }

        public DbSet<LinearSubItem> linearSubItems{ get; set; }

        public DbSet<LinearAsset> linearAssets { get; set; }

        public DbSet<EquipmentModel> equipmentModels{ get; set; }

        public DbSet<EquipmentSubItem> equipmentSubItems { get; set; }

        public DbSet<EquipmentAsset> equipmentAssets { get; set; }    


    }
}
