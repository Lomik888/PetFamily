using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFemily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class RefreshSessionsConfiguration : IEntityTypeConfiguration<RefreshSessions>
{
    public void Configure(EntityTypeBuilder<RefreshSessions> builder)
    {
        builder.ToTable("refresh_sessions", schema: "Accounts");

        builder.Property(x => x.Id).IsRequired().HasColumnName("id");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Jti).IsRequired().HasColumnName("jti");
        builder.Property(x => x.UserId).IsRequired().HasColumnName("user_id");
        builder.Property(x => x.CreatedAt).IsRequired().HasColumnName("created_at");
        builder.Property(x => x.ExpireAt).IsRequired().HasColumnName("expire_at");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}