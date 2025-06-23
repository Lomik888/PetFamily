using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Core;
using PetFamily.Framework;
using PetFamily.SharedKernel.Errors;
using PetFemily.Accounts.Application.Providers;
using PetFemily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Providers;

public class JwtTokensProvider : IJwtTokensProvider
{
    private readonly IOptions<JwtOptions> _jwtOptions;

    public JwtTokensProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }

    public Result<(string jwt, string jti), ErrorList> CreateAccessJwtToken(User user)
    {
        if (user == null)
        {
            var error = ErrorsPreform.General.Validation("User is null", nameof(user));
            var errors = ErrorList.Create(error);
            return errors;
        }

        var jti = Guid.NewGuid().ToString();

        var claims = new List<Claim>()
        {
            new Claim(JwtClaimsTypesCustom.Sub, user.Id.ToString()),
            new Claim(JwtClaimsTypesCustom.Email, user.Email ?? ""),
            new Claim(JwtClaimsTypesCustom.Role, RolesTypes.Participant),
            new Claim(JwtClaimsTypesCustom.Jti, jti),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.SecurityKey));

        var jwt = new JwtSecurityToken(
            issuer: _jwtOptions.Value.Issuer,
            audience: _jwtOptions.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.Value.LifeTimeMinutes),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        if (string.IsNullOrWhiteSpace(token))
        {
            var error = ErrorsPreform.General.IternalServerError("Cant Create jwt access token");
            return ErrorList.Create(error);
        }

        return (token, jti);
    }

    public Result<bool, ErrorList> ValidateJwt(User user, string userJwt)
    {
        if (user == null || string.IsNullOrWhiteSpace(userJwt))
        {
            var error = ErrorsPreform
                .General
                .Validation("User is null or userJwt IsNullOrWhiteSpace");
            var errors = ErrorList.Create(error);
            return errors;
        }

        var claimsResult = GetClaims(userJwt);
        if (claimsResult.IsFailure == true)
        {
            return ErrorList.Create(claimsResult.Error);
        }


        var expUtcClaim = claimsResult.Value.FirstOrDefault(x => x.Type == JwtClaimsTypesCustom.Exp);
        if (expUtcClaim == null)
        {
            var error = ErrorsPreform.General.NotFound("Not found require claim");
            var errors = ErrorList.Create(error);
            return errors;
        }

        var expUtcNumber = Convert.ToInt64(expUtcClaim.Value);
        var expUtcTime = new DateTime(expUtcNumber, DateTimeKind.Utc);

        var jtiClaim = claimsResult.Value.FirstOrDefault(x => x.Type == JwtClaimsTypesCustom.Jti);
        if (jtiClaim == null)
        {
            var error = ErrorsPreform.General.NotFound("Not found require claim");
            var errors = ErrorList.Create(error);
            return errors;
        }

        var claims = new List<Claim>()
        {
            new Claim(JwtClaimsTypesCustom.Sub, user.Id.ToString()),
            new Claim(JwtClaimsTypesCustom.Email, user.Email ?? ""),
            new Claim(JwtClaimsTypesCustom.Role, RolesTypes.Participant),
            new Claim(JwtClaimsTypesCustom.Jti, jtiClaim.Value),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.SecurityKey));

        var jwt = new JwtSecurityToken(
            issuer: _jwtOptions.Value.Issuer,
            audience: _jwtOptions.Value.Audience,
            claims: claims,
            expires: expUtcTime,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        if (string.IsNullOrWhiteSpace(token))
        {
            var error = ErrorsPreform.General.IternalServerError("Cant Create jwt access token");
            return ErrorList.Create(error);
        }

        if (userJwt != token)
        {
            return false;
        }

        return true;
    }

    public Result<RefreshSessions, ErrorList> CreateRefreshToken(Guid userId, Guid jti)
    {
        if (userId == Guid.Empty)
        {
            var error = ErrorsPreform.General.Validation("UserId is empty", nameof(userId));
            var errors = ErrorList.Create(error);
            return errors;
        }

        if (jti == Guid.Empty)
        {
            var error = ErrorsPreform.General.Validation("Jti is empty", nameof(jti));
            var errors = ErrorList.Create(error);
            return errors;
        }

        var createdAt = DateTime.UtcNow;
        var expireAt = DateTime.UtcNow.AddDays(30);

        var refreshToKen = new RefreshSessions
        {
            Jti = jti,
            UserId = userId,
            CreatedAt = createdAt,
            ExpireAt = expireAt
        };

        return refreshToKen;
    }

    public Result<IReadOnlyList<Claim>, Error> GetClaims(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        var claims = token.Claims;
        if (claims == null)
        {
            var error = ErrorsPreform.General.NotFound("JWT claims is empty");
        }

        return claims!.ToImmutableList();
    }
}