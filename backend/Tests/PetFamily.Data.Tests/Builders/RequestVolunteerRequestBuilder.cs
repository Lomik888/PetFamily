using AutoFixture;
using PetFamily.Core.Dtos;
using PetFamily.Data.Tests.Requests;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteersRequests.Domain.Enums;
using PetFamily.VolunteersRequests.Domain.ValueObjects;
using PetFemily.Accounts.Domain.ValueObjects;

namespace PetFamily.Data.Tests.Builders;

public class RequestVolunteerRequestBuilder : DomainBuilderBase
{
    private const int EXPERIENCE_MAX = Experience.VELUE_MAX_LENGHT;
    private const int EXPERIENCE_MIN = 0;
    private const int DETAILSFORHELPS_MAX = DetailsForHelps.MAX_DETAILS_COUNT;
    private const int DETAILSFORHELPS_MIN = 0;

    public static RequestVolunteerRequest RequestVolunteerBuild()
    {
        var experience = _random.Next(EXPERIENCE_MIN, EXPERIENCE_MAX + 1);
        var detailsForHelpsCount = _random.Next(DETAILSFORHELPS_MIN, DETAILSFORHELPS_MAX + 1);
        var detailsForHelps = _autoFixture.CreateMany<DetailsForHelpDto>(detailsForHelpsCount);

        var withCertificates = _autoFixture.Create<bool>();
        string? certificates = null;

        if (withCertificates == true)
        {
            var certificatesGuid = Guid.NewGuid();
            certificates = certificatesGuid.ToString();
        }

        return _autoFixture
            .Build<RequestVolunteerRequest>()
            .With(x => x.Certificates, certificates)
            .With(x => x.Experience, experience)
            .With(x => x.DetailsForHelps, detailsForHelps)
            .Create();
    }
}