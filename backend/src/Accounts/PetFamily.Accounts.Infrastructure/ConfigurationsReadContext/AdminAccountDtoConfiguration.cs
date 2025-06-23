using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFemily.Accounts.Application.Dto;

namespace PetFamily.Accounts.Infrastructure.ConfigurationsReadContext;

public class AdminAccountDtoConfiguration : IEntityTypeConfiguration<AdminAccountDto>
{
    public void Configure(EntityTypeBuilder<AdminAccountDto> builder)
    {
        builder.ToView("admins_accounts", schema: "Accounts");
        builder.Property(x => x.UserId).IsRequired().HasColumnName("user_id");
    }
}