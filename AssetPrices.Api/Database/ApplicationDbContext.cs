using AssetPrices.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssetPrices.Api.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) 
            : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>().HasKey(a => a.Symbol);
            modelBuilder.Entity<AssetPrice>().HasKey(ap => ap.Id); 
            
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetPrice> AssetPrices { get; set; }

    }
}
