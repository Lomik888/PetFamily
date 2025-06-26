using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Discussions.Domain.ValueObjects;

public class Users : ValueObjectList<UserId>
{
    public const int MIN_USERS_VALUE = 2;

    private Users(IEnumerable<UserId> items) : base(items)
    {
    }

    private Users()
    {
    }

    public static Result<Users, Error> Create(IEnumerable<UserId> items)
    {
        var userIds = items.ToList();
        if (userIds.ToList().Count < MIN_USERS_VALUE)
        {
            var error = ErrorsPreform.General.Validation("users list is too small");
            return error;
        }

        return new Users(userIds);
    }
}