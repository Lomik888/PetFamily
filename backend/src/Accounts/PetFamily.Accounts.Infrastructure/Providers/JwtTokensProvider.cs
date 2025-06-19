using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Infrastructure.Options;
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


    public Result<string, ErrorList> CreateAccessJwtToken(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email.Value ?? ""),
            new Claim(JwtRegisteredClaimNames.Typ, RolesTypes.User),
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

        return token;
    }
}