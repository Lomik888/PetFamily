using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts.Requests.Species;
using PetFamily.API.Contracts.Response.Envelope;
using PetFamily.API.Extensions;
using PetFamily.Application;
using PetFamily.Application.Contracts;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.SpeciesUseCases.Commands.Delete;
using PetFamily.Application.SpeciesUseCases.Queries.GetBreeds;
using PetFamily.Application.SpeciesUseCases.Queries.GetSpecies;
using PetFamily.Shared.Errors;

namespace PetFamily.API.Controllers;

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
    public async Task<ActionResult> Get(
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