using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Presentation.Requests;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Framework.Abstractions;
using PetFamily.Framework.Extensions;
using PetFamily.Framework.Responses;
using PetFamily.SharedKernel.Errors;
using PetFemily.Accounts.Application.Command.Login;
using PetFemily.Accounts.Application.Command.Registration;

namespace PetFamily.Accounts.Presentation.Controllers;

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