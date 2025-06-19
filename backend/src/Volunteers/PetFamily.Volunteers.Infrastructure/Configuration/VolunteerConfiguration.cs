using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Volunteers.Domain;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using PetFamily.Volunteers.Domain.ValueObjects.SharedVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;
using File = PetFamily.SharedKernel.ValueObjects.File;

namespace PetFamily.Volunteers.Infrastructure.Configuration;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers", schema: "Volunteers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .HasColumnName("id")
            .ValueGeneratedNever()
            .HasConversion(
                volunteerId => volunteerId.Value,
                value => VolunteerId.Create(value).Value);

        builder.ComplexProperty(x => x.Name, xb =>
        {
            xb.Property(x => x.FirstName)
                .HasMaxLength(Name.FIRST_NAME_LENGTH)
                .HasColumnName("first_name")
                .IsRequired();

            xb.Property(x => x.LastName)
                .HasMaxLength(Name.LAST_NAME_LENGTH)
                .HasColumnName("last_name")
                .IsRequired();

            xb.Property(x => x.Surname)
                .HasMaxLength(Name.SURNAME_LENGTH)
                .HasColumnName("surname")
                .IsRequired();
        });

        builder.Property(x => x.Description)
            .IsRequired()
            .HasColumnName("description")
            .HasMaxLength(Description.VALUE_MAX_LENGHT)
            .ValueGeneratedNever()
            .HasConversion(
                description => description.Value,
                value => Description.Create(value).Value);

        builder.ComplexProperty(x => x.PhoneNumber, xb =>
        {
            xb.Property(x => x.RegionCode)
                .HasColumnName("region_code")
                .HasMaxLength(PhoneNumber.PHONENUMBER_MIN_LENGTH)
                .IsRequired();

            xb.Property(x => x.Number)
                .HasColumnName("number")
                .HasMaxLength(PhoneNumber.NUMBER_MAX_LENGTH)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Files, xb =>
        {
            xb.Property(x => x.Items)
                .HasConversion(
                    value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
                    value => JsonSerializer.Deserialize<IReadOnlyList<File>>(value, JsonSerializerOptions.Default)!,
                    new ValueComparer<IReadOnlyList<File>>(
                        (c1, c2) => c1!.SequenceEqual(c2!),
                        c => c.Aggregate(0, (current, value) => HashCode.Combine(current, value.GetHashCode())),
                        c => c.ToList()
                    ))
                .HasColumnName("files")
                .HasColumnType("jsonb");
        });

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.DeletedAt)
            .HasColumnName("deleted_at")
            .IsRequired(false);

        builder.HasMany(x => x.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}