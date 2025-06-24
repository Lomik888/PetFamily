using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFemily.Accounts.Application.Dto;

namespace PetFamily.Accounts.Infrastructure.ConfigurationsReadContext;

public class PermissionDtoConfiguration : IEntityTypeConfiguration<PermissionDto>
{
    public void Configure(EntityTypeBuilder<PermissionDto> builder)
    {
        builder.ToView("permission", schema: "Accounts");
        builder.Property(x => x.Id).IsRequired().HasColumnName("id");
        builder.Property(x => x.Code).IsRequired().HasColumnName("code");
    }
}