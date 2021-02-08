using Microsoft.EntityFrameworkCore;

namespace ConsimpleTestTask.WebApp.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<TestModel> TestModels { get; set; }

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