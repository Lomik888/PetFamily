using AutoFixture;
using PetFamily.Data.Tests.Requests;

namespace PetFamily.Data.Tests.Builders;

public class RequestDiscussionBuild : DomainBuilderBase
{
    public static RequestDiscussion DiscussionBuild()
    {
        var usersIds = _autoFixture.CreateMany<Guid>(2);

        return _autoFixture
            .Build<RequestDiscussion>()
            .With(x => x.UsersIds, usersIds)
            .Create();
    }
}