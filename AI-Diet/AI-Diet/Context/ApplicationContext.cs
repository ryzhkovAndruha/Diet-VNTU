using AI_Diet.Models.UserModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AI_Diet.Context
{
    public class ApplicationContext: IdentityDbContext<User>
    {
        public DbSet<CalorieCalculatorData> CalorieCalculatorDatas { get; set; }
        public DbSet<DietData> DietData { get; set; }
        public ApplicationContext(DbContextOptions options): base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.CalorieCalculatorData)
                .WithOne(cd => cd.User)
                .HasForeignKey<CalorieCalculatorData>(cd => cd.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.DietData)
                .WithOne(dd => dd.User)
                .HasForeignKey<DietData>(dd => dd.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
