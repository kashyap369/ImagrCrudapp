using ImageCloude.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageCloude.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ImageModel> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ImageModel>().HasKey(id =>id.ImageId);
            modelBuilder.Entity<ImageModel>().Property(id => id.ImageId).ValueGeneratedOnAdd();

        }
    }
}
