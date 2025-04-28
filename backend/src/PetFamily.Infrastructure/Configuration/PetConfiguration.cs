using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Domain.VolunteerContext.PetsVO.Enums;
using PetFamily.Domain.VolunteerContext.SharedVO;
using File = PetFamily.Domain.VolunteerContext.SharedVO.File;

namespace PetFamily.Infrastructure.Configuration;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.HelpStatus)
            .IsRequired()
            .HasColumnName("status")
            .HasConversion<string>(
                helpStatus => helpStatus.Value.ToString(),
                value => HelpStatus.Create((HelpStatuses)Enum.Parse(typeof(HelpStatuses), value)).Value);

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at")
            .HasConversion(
                createdAt => createdAt.Value.ToUniversalTime(),
                value => CreatedAt.Create(DateTime.SpecifyKind(value, DateTimeKind.Utc)).Value);

        builder.Property(x => x.Vaccinated)
            .IsRequired()
            .HasColumnName("vaccinated");

        builder.Property(x => x.DateOfBirth)
            .IsRequired()
            .HasColumnName("date_of_birth")
            .HasConversion(
                dateOfBirth => dateOfBirth.Value.ToUniversalTime(),
                value => DateOfBirth.Create(DateTime.SpecifyKind(value, DateTimeKind.Utc)).Value);

        builder.Property(x => x.Sterilize)
            .IsRequired()
            .HasColumnName("sterilize");

        builder.ComplexProperty(x => x.PhoneNumber, xb =>
        {
            xb.Property(x => x.RegionCode)
                .IsRequired()
                .HasColumnName("region_code");

            xb.Property(x => x.Number)
                .IsRequired()
                .HasColumnName("number");
        });

        builder.Property(x => x.Weight)
            .IsRequired()
            .HasColumnName("weight")
            .HasConversion(
                weight => weight.Value,
                value => Weight.Create(value).Value);

        builder.Property(x => x.Height)
            .IsRequired()
            .HasColumnName("height")
            .HasConversion(
                height => height.Value,
                value => Height.Create(value).Value);

        builder.ComplexProperty(x => x.Address, xb =>
        {
            xb.Property(x => x.Country)
                .IsRequired()
                .HasColumnName("country")
                .HasMaxLength(Address.COUNTRY_MAX_LENGHT);

            xb.Property(x => x.City)
                .IsRequired()
                .HasColumnName("city")
                .HasMaxLength(Address.CITY_MAX_LENGHT);

            xb.Property(x => x.Street)
                .IsRequired()
                .HasColumnName("street")
                .HasMaxLength(Address.STREET_MAX_LENGHT);

            xb.Property(x => x.HouseNumber)
                .IsRequired()
                .HasColumnName("house_number")
                .HasMaxLength(Address.HOUSENUMBER_MAX_LENGHT);

            xb.Property(x => x.ApartmentNumber)
                .IsRequired(false)
                .HasColumnName("apartment_number")
                .HasMaxLength(Address.APARTMENTNUMBER_MAX_LENGHT);
        });

        builder.ComplexProperty(x => x.HealthDescription, xb =>
        {
            xb.Property(x => x.SharedHealthStatus)
                .IsRequired()
                .HasColumnName("shared_health_status")
                .HasMaxLength(HealthDescription.SHAREDHEALTHSTATUS_MAX_LENGHT);

            xb.Property(x => x.SkinCondition)
                .IsRequired()
                .HasColumnName("skin_condition")
                .HasMaxLength(HealthDescription.SKINCONDITION_MAX_LENGHT);

            xb.Property(x => x.MouthCondition)
                .IsRequired()
                .HasColumnName("mouth_condition")
                .HasMaxLength(HealthDescription.MOUTHCONDITION_MAX_LENGHT);

            xb.Property(x => x.DigestiveSystemCondition)
                .IsRequired()
                .HasColumnName("digestive_system_condition")
                .HasMaxLength(HealthDescription.DIGESTIVESYSTEMCONDITION_MAX_LENGHT);
        });

        builder.Property(x => x.Color)
            .IsRequired()
            .HasColumnName("color")
            .HasMaxLength(Color.VELUE_MAX_LENGHT)
            .HasConversion(
                color => color.Value,
                value => Color.Create(value).Value);

        builder.ComplexProperty(x => x.SpeciesBreedId, xb =>
        {
            xb.Property(x => x.SpeciesId)
                .IsRequired()
                .HasColumnName("species_id");

            xb.Property(x => x.BreedId)
                .IsRequired()
                .HasColumnName("breed_id");
        });

        builder.Property(x => x.Description)
            .IsRequired()
            .HasColumnName("description")
            .HasMaxLength(Description.VALUE_MAX_LENGHT)
            .HasConversion(
                description => description.Value,
                value => Description.Create(value).Value);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasMaxLength(Name.VELUE_MAX_LENGHT)
            .HasConversion(
                name => name.Value,
                value => Name.Create(value).Value);

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired()
            .HasColumnName("id")
            .HasConversion(
                id => id.Value,
                value => PetId.Create(value).Value);

        builder.OwnsOne(x => x.DetailsForHelps, xb =>
        {
            xb.Property(x => x.Items)
                .HasConversion(
                    value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
                    value => JsonSerializer.Deserialize<IReadOnlyList<DetailsForHelp>>(value,
                        JsonSerializerOptions.Default)!,
                    new ValueComparer<IReadOnlyList<DetailsForHelp>>(
                        (c1, c2) => c1!.SequenceEqual(c2!),
                        c => c.Aggregate(0, (current, value) => HashCode.Combine(current, value.GetHashCode())),
                        c => c.ToList()
                    )
                )
                .HasColumnName("details_for_help")
                .HasColumnType("jsonb");
        });

        builder.OwnsOne(x => x.Files, xb =>
        {
            xb.Property(x => x.Items)
                .HasConversion(
                    value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
                    value => JsonSerializer.Deserialize<IReadOnlyList<File>>(value,
                        JsonSerializerOptions.Default)!,
                    new ValueComparer<IReadOnlyList<File>>(
                        (c1, c2) => c1!.SequenceEqual(c2!),
                        c => c.Aggregate(0, (current, value) => HashCode.Combine(current, value.GetHashCode())),
                        c => c.ToList()
                    )
                )
                .HasColumnName("files")
                .HasColumnType("jsonb");
        });

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired(true);

        builder.Property(x => x.DeletedAt)
            .HasColumnName("deleted_at")
            .IsRequired(false);
    }
}