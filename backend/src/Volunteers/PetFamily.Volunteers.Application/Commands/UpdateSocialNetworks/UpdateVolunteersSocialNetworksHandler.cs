using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Abstractions;
using PetFamily.Volunteers.Domain.ValueObjects.IdsVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO;
using PetFamily.Volunteers.Domain.ValueObjects.VolunteerVO.Collections;

namespace PetFamily.Volunteers.Application.Commands.UpdateSocialNetworks;

public class UpdateVolunteersSocialNetworksHandler :
    ICommandHandler<ErrorList, UpdateVolunteersSocialNetworksCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<UpdateVolunteersSocialNetworksCommand> _validator;
    private readonly ILogger<UpdateVolunteersSocialNetworksHandler> _logger;

    public UpdateVolunteersSocialNetworksHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdateVolunteersSocialNetworksHandler> logger,
        IValidator<UpdateVolunteersSocialNetworksCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        UpdateVolunteersSocialNetworksCommand request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("Invalid Validation request");
            return UnitResult.Failure(ErrorList.Create(validationResult.Errors.ToErrors()));
        }

        var volunteer = await _volunteerRepository.GetByIdAsync(
            VolunteerId.Create(request.VolunteerId).Value,
            cancellationToken);

        var newSocials = request.SocialNetworkCollectionDto.SocialNetworks
            .Select(social => SocialNetwork.Create(social.Title, social.Url).Value).ToList();

        var newSocialNetworks = SocialNetworks.Create(newSocials).Value;

        volunteer.SetSocialNetworks(newSocialNetworks);

        await _volunteerRepository.UpdateAsAlreadyTrackingAsync(volunteer, cancellationToken);

        _logger.LogInformation("Socials updated");
        return UnitResult.Success<ErrorList>();
    }
}