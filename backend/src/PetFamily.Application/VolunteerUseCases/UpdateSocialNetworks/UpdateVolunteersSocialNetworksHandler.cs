using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.Extensions;
using PetFamily.Domain.VolunteerContext.IdsVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO.Collections;
using PetFamily.Shared.Errors;

namespace PetFamily.Application.VolunteerUseCases.UpdateSocialNetworks;

public class UpdateVolunteersSocialNetworksHandler :
    ICommandHandler<ErrorCollection, UpdateVolunteersSocialNetworksCommand>
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

    public async Task<UnitResult<ErrorCollection>> Handle(
        UpdateVolunteersSocialNetworksCommand request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("Invalid Validation request");
            return UnitResult.Failure(ErrorCollection.Create(validationResult.Errors.ToErrors()));
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
        return UnitResult.Success<ErrorCollection>();
    }
}