using FluentValidation;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Quic;
using PetFamily.Application.Extensions;

namespace PetFamily.Application.VolunteerUseCases.Queries.Get;

public class GetQueryValidator : AbstractValidator<GetQuery>
{
    public GetQueryValidator()
    {
        RuleFor(x => x.Page).Must(x => x > 0).WithMessageCustom("Page must be greater than 0");
        RuleFor(x => x.PageSize).Must(x => x > -1).WithMessageCustom("PageSize must be greater than -1");
    }
}