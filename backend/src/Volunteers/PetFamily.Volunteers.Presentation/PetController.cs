using Microsoft.AspNetCore.Mvc;
using PetFamily.Core;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Framework.Abstractions;
using PetFamily.Framework.Extensions;
using PetFamily.Framework.Responses;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Dtos.PetDtos;
using PetFamily.Volunteers.Application.Queries.GetPet;
using PetFamily.Volunteers.Application.Queries.GetPets;
using PetFamily.Volunteers.Presentation.Requests.Volunteer;

namespace PetFamily.Volunteers.Presentation;

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