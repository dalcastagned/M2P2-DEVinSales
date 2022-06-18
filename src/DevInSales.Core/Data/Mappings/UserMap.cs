using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DevInSales.Core.Entities;

namespace DevInSales.Core.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User2>
    {
        public void Configure(EntityTypeBuilder<User2> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnType("varchar(150)")
                .IsUnicode(false)
                .IsRequired();

            builder.Property(u => u.Password)
                .HasColumnType("varchar(50)")
                .IsUnicode(false)
                .IsRequired();
            builder.Property(u => u.BirthDate)
                .HasColumnType("date")
                .IsRequired();

            builder.HasData(
                new User2(1, "Allie.Spencer@manuel.us", "661845", "Allie Spencer", new DateTime(1980, 10, 11)),
                new User2(2, "Earnest@kari.biz", "800631", "Lemuel Witting", new DateTime(1980, 10, 11)),
                new User2(3, "Adella_Shanahan@kenneth.biz", "661342", "Kari Olson I", new DateTime(1980, 10, 11)),
                new User2(4, "Americo.Strosin@kale.tv", "661964", "Marion Nolan DDS", new DateTime(1980, 10, 11))
            );
        }
    }

}
