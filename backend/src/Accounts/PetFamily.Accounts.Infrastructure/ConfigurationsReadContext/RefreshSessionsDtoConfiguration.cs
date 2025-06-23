using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFemily.Accounts.Application.Dto;

namespace PetFamily.Accounts.Infrastructure.ConfigurationsReadContext;

public class RefreshSessionsDtoConfiguration : IEntityTypeConfiguration<RefreshSessionsDto>
{
    public void Configure(EntityTypeBuilder<RefreshSessionsDto> builder)
    {
        builder.ToView("refresh_sessions", schema: "Accounts");
        builder.Property(x => x.Id).IsRequired().HasColumnName("id");
        builder.Property(x => x.Jti).IsRequired().HasColumnName("jti");
        builder.Property(x => x.UserId).IsRequired().HasColumnName("user_id");
        builder.Property(x => x.CreatedAt).IsRequired().HasColumnName("created_at");
        builder.Property(x => x.ExpireAt).IsRequired().HasColumnName("expire_at");
        builder.HasOne<UserDto>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}