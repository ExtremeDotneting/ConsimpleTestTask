using Microsoft.EntityFrameworkCore;

namespace ConsimpleTestTask.WebApp.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }

        public DbSet<ProductModel> Products { get; set; }

        public DbSet<ProductCategoryModel> ProductsCategories { get; set; }

        public DbSet<PurchaseModel> Purchases { get; set; }

        public DbSet<PurchasePositionModel> PurchasePositions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
           
            optionsBuilder.UseSqlServer((opt) =>
            {
                optionsBuilder.UseSqlServer(AppSettings.DB_CONNECTION);
            });
        }
    }
}