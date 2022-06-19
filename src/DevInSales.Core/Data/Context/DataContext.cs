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
            
            this.SeedRoles(modelBuilder);
            this.SeedUsers(modelBuilder);
            this.SeedUserRoles(modelBuilder);

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
        public DbSet<Product> Products { get; set; }
        public DbSet<Address> Addresses{ get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<SaleProduct> SaleProducts { get; set; }

        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Administrador", NormalizedName = "ADMINISTRADOR" },
                new Role { Id = 2, Name = "Gerente", NormalizedName = "GERENTE" },
                new Role { Id = 3, Name = "Usuario", NormalizedName = "USUARIO" }
            );
        }
        private void SeedUsers(ModelBuilder modelBuilder)
        {
            User user = new User()
            {
                Id = 1,
                Name = "Suporte",
                UserName = "suporte",
                NormalizedUserName = "SUPORTE",
                Email = "suporte@suporte.com",
                NormalizedEmail = "SUPORTE@SUPORTE.COM",
                EmailConfirmed = true,
                BirthDate = DateTime.Now,
                SecurityStamp = "YYGXBMRWXF6A3J5PEYA3EVNXG6Y4YBTC"
            };
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, "suporte");
            user.PasswordExpired = DateTime.Now.AddDays(-1).ToShortDateString();

            modelBuilder.Entity<User>().HasData(user);
        }
        private void SeedUserRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { UserId = 1, RoleId = 1 },
                new UserRole { UserId = 1, RoleId = 2 },
                new UserRole { UserId = 1, RoleId = 3 }
            );
        }

    }
}
        
  
