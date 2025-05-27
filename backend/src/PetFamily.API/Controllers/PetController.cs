using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts.Requests;
using PetFamily.API.Contracts.Response.Envelope;
using PetFamily.API.Extensions;
using PetFamily.Application;
using PetFamily.Application.Contracts;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.VolunteerUseCases.Queries.GetPet;
using PetFamily.Application.VolunteerUseCases.Queries.GetPets;
using PetFamily.Shared.Errors;

namespace PetFamily.API.Controllers;

public class PetController : ApplicationController
{
    [HttpGet("pet/{petId:guid}")]
    public async Task<ActionResult> GetPetById(
        [FromRoute] Guid petId,
        [FromServices] IQueryHandler<PetDto, ErrorList, GetPetQuery> handler,
        CancellationToken cancellationToken = default)
    {
        var request = new GetPetRequest();

        var result = await handler.Handle(request.ToQuery(petId), cancellationToken);
        if (result.IsFailure)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.Ok(result.Value));
    }

    [HttpGet("pets")]
    public async Task<ActionResult> GetPets(
        [FromBody] GetPetsRequest request,
        [FromServices] IQueryHandler<GetObjectsWithPaginationResponse<PetDto>, ErrorList, GetPetsQuery> handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToQuery(), cancellationToken);
        if (result.IsFailure)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.Ok(result.Value));
    }
}