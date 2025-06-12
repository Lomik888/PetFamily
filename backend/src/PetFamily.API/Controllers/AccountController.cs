using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Contracts.Requests.Accounts;
using PetFamily.API.Contracts.Response.Envelope;
using PetFamily.API.Extensions;
using PetFamily.Application.AccauntManagment.Command.Login;
using PetFamily.Application.AccauntManagment.Command.Registration;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Shared.Errors;

namespace PetFamily.API.Controllers;

public class AccountController : ApplicationController
{
    [AllowAnonymous]
    [HttpPost("registration")]
    public async Task<IActionResult> RegistrationAccount(
        [FromBody] AccountRegistrationRequest request,
        [FromServices] ICommandHandler<ErrorList, AccountRegistrationCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        if (result.IsFailure == true)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.OkEmpty());
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAccount(
        [FromBody] AccountLoginRequest request,
        [FromServices] ICommandHandler<string, ErrorList, AccountLoginCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        if (result.IsFailure == true)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        return Ok(Envelope.Ok(result.Value));
    }
}