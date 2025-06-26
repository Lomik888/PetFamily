using AutoFixture;
using PetFamily.Data.Tests.Requests;

namespace PetFamily.Data.Tests.Builders;

public class RequestMessageBuild : DomainBuilderBase
{
    public static RequestMessage Build(RequestDiscussion requestDiscussion)
    {
        var userIndex = _random.Next(0, requestDiscussion.UsersIds.Count());
        var userId = requestDiscussion.UsersIds.ToList()[userIndex];

        return _autoFixture
            .Build<RequestMessage>()
            .With(x => x.UserId, userId)
            .Create();
    }
}