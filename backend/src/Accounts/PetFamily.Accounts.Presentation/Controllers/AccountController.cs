using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Accounts.Presentation.Requests;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Framework.Abstractions;
using PetFamily.Framework.Extensions;
using PetFamily.Framework.Responses;
using PetFamily.SharedKernel.Errors;
using PetFemily.Accounts.Application.Command.Login;
using PetFemily.Accounts.Application.Command.RefreshLogin;
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
        [FromServices] ICommandHandler<LoginResponseDto, ErrorList, AccountLoginCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        if (result.IsFailure == true)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        var cookieOptions = new CookieOptions
        {
            Domain = null, // тут я должен установить свой домен
            Expires = result.Value.ExpireAt,
            Secure = false, // пока у меня на сайте нет https
            SameSite = SameSiteMode.Strict,
            HttpOnly = true,
            IsEssential = true
        };

        HttpContext.Response.Cookies.Append("refresh_token", result.Value.RefreshToken, cookieOptions);

        return Ok(Envelope.Ok(result.Value.Jwt));
    }

    [AllowAnonymous]
    [HttpPost("login-refresh")]
    public async Task<IActionResult> LoginRefreshAccount(
        [FromServices] ICommandHandler<LoginResponseDto, ErrorList, RefreshLoginCommand> handler,
        CancellationToken cancellationToken)
    {
        var request = new AccountLoginRefreshRequest();
        var jwt =
            HttpContext.Request.Headers.Authorization.FirstOrDefault(x => x != null && x.StartsWith("Bearer "));
        if (jwt == null)
        {
            return Unauthorized();
        }

        var refreshToken =
            HttpContext.Request.Cookies["refresh_token"];
        if (refreshToken == null)
        {
            return Unauthorized();
        }

        var token = jwt.Split(' ').Last();

        var result = await handler.Handle(request.ToCommand(token, refreshToken), cancellationToken);

        if (result.IsFailure == true)
        {
            return result.Error.Errors.ToErrorActionResult();
        }

        var cookieOptions = new CookieOptions
        {
            Domain = null, // тут я должен установить свой домен
            Expires = result.Value.ExpireAt,
            Secure = false, // пока у меня на сайте нет https
            SameSite = SameSiteMode.Strict,
            HttpOnly = true,
            IsEssential = true
        };

        HttpContext.Response.Cookies.Append("refresh_token", result.Value.RefreshToken, cookieOptions);

        return Ok(Envelope.Ok(result.Value.Jwt));
    }
}