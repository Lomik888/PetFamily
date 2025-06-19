using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFemily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class AdminAccountConfiguration : IEntityTypeConfiguration<AdminAccount>
{
    public void Configure(EntityTypeBuilder<AdminAccount> builder)
    {
        builder.ToTable("admins_accounts", schema: "Accounts");
        builder.Property(x => x.Id).IsRequired();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).IsRequired().HasColumnName("user_id");
    }
}