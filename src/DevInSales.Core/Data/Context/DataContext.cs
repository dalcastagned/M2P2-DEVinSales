using System.Reflection;
using DevInSales.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Core.Data.Context
{
    public class DataContext : IdentityDbContext<
              User,
              Role,
              int,
              IdentityUserClaim<int>,
              UserRole,
              IdentityUserLogin<int>,
              IdentityRoleClaim<int>,
              IdentityUserToken<int>
          >
    {
        public 
            DataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<UserRole>(
                userRole =>
                {
                    userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                    userRole
                        .HasOne(ur => ur.Role)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.RoleId)
                        .IsRequired();

                    userRole
                        .HasOne(ur => ur.User)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.UserId)
                        .IsRequired();
                }
            );
        }
        public DbSet<User2> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Address> Addresses{ get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<SaleProduct> SaleProducts { get; set; }

    }
}
        
  
