using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Model.Security
{
    public class UserConfigurationMap : IEntityTypeConfiguration<UserConfiguration>
    {
        public void Configure(EntityTypeBuilder<UserConfiguration> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(u => u.Email)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(u => u.Name)
                   .HasMaxLength(100)
                   .IsRequired();
        }
    }
}
