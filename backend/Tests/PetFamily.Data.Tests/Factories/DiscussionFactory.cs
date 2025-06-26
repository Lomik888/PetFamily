using PetFamily.Data.Tests.Requests;
using PetFamily.Discussions.Domain;
using PetFamily.Discussions.Domain.ValueObjects;
using PetFamily.Discussions.Domain.ValueObjects.Ids;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Data.Tests.Factories;

public static class DiscussionFactory
{
    public static Discussion CreateDiscussion(RequestDiscussion requestDiscussion)
    {
        var usersIds = requestDiscussion.UsersIds.Select(x => UserId.Create(x).Value);
        var users = Users.Create(usersIds).Value;
        var relationId = RelationId.Create(requestDiscussion.RelationId).Value;

        var discussion = Discussion.Create(users, relationId);

        return discussion;
    }
}