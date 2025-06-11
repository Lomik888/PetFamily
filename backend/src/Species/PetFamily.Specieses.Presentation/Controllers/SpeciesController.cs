using Microsoft.AspNetCore.Mvc;
using PetFamily.Core;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Framework.Abstractions;
using PetFamily.Framework.Extensions;
using PetFamily.Framework.Responses;
using PetFamily.SharedKernel.Errors;
using PetFamily.Specieses.Application.Commands.Delete;
using PetFamily.Specieses.Application.Dtos;
using PetFamily.Specieses.Application.Queries.GetBreeds;
using PetFamily.Specieses.Application.Queries.GetSpecies;
using PetFamily.Specieses.Presentation.Requests;

namespace PetFamily.Specieses.Presentation.Controllers;

public class SpeciesController : ApplicationController
{
    [HttpGet("species")]
    public async Task<ActionResult> Get(
        [FromQuery] GetWithPaginationSpeciesRequest request,
        [FromServices] IQueryHandler<GetObjectsWithPaginationResponse<SpeciesDto>, ErrorList, GetSpeciesQuery> handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToQuery(), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.Ok(result.Value));
    }

    [HttpGet("{speciesId:guid}/breeds")]
    public async Task<ActionResult> GetBreeds(
        [FromRoute] Guid speciesId,
        [FromQuery] GetBreedsWithPaginationRequest request,
        [FromServices] IQueryHandler<GetObjectsWithPaginationResponse<BreedsDto>, ErrorList, GetBreedsQuery> handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(request.ToQuery(speciesId), cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.Ok(result.Value));
    }

    [HttpDelete("{speciesId:guid}/")]
    public async Task<IActionResult> DeletePetFiles(
        [FromRoute] Guid speciesId,
        [FromBody] DeleteSpecieAndBreedRequest request,
        [FromServices] ICommandHandler<ErrorList, DeleteSpecieAndBreedCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(speciesId), cancellationToken);

        if (result.IsFailure == true)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.OkEmpty());
    }
}