using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFemily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permission", schema: "Accounts");
        builder.Property(x => x.Id).IsRequired().HasColumnName("id");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).IsRequired().HasColumnName("code");
    }
}