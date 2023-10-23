using AI_Diet.Models.UserModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AI_Diet.Context
{
    public class ApplicationContext: IdentityDbContext<User>
    {
        public DbSet<DietData> DietData { get; set; }
        public DbSet<FoodDetails> FoodDetails { get; set; }
        public ApplicationContext(DbContextOptions options): base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.DietData)
                .WithOne(cd => cd.User)
                .HasForeignKey<DietData>(cd => cd.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.FoodDetails)
                .WithOne(dd => dd.User)
                .HasForeignKey<FoodDetails>(dd => dd.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
