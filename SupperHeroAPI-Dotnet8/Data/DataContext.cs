using Microsoft.EntityFrameworkCore;
using SupperHeroAPI_Dotnet8.Entities;

namespace SupperHeroAPI_Dotnet8.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options) { }
        public DbSet<SupperHero> SuperHeroes { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SupperHero>().HasKey();
        }
    }
}
