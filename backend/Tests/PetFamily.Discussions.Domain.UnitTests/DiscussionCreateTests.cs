using FluentAssertions;
using PetFamily.Data.Tests.Builders;
using PetFamily.Data.Tests.Factories;
using PetFamily.Discussions.Domain.ValueObjects;
using PetFamily.Discussions.Domain.ValueObjects.Ids;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Discussions.Domain.UnitTests;

public class DiscussionCreateTests
{
    [Fact]
    public void Discussion_create_return_valid_entity()
    {
        var discussionRequest = RequestDiscussionBuild.DiscussionBuild();
        var discussion = DiscussionFactory.CreateDiscussion(discussionRequest);

        var userIdList = discussionRequest.UsersIds.Select(x => UserId.Create(x).Value);
        var users = Users.Create(userIdList).Value;
        var relationId = RelationId.Create(discussionRequest.RelationId).Value;

        var sut = Discussion.Create(users, relationId);

        sut.Should().BeEquivalentTo(discussion,
            options =>
                options.Excluding(x => x.Id)
                    .ComparingByMembers<Discussion>()
                    .ComparingByMembers<Users>()
                    .ComparingByMembers<UserId>());
    }
}