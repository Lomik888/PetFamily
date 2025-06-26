using FluentAssertions;
using PetFamily.Data.Tests.Builders;
using PetFamily.Data.Tests.Factories;
using PetFamily.Discussions.Domain.ValueObjects;
using PetFamily.Discussions.Domain.ValueObjects.Ids;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Discussions.Domain.UnitTests;

public class DiscussionCreateMessageTests
{
    [Fact]
    public void Discussion_create_message_return_valid_entity()
    {
        var discussionRequest = RequestDiscussionBuild.DiscussionBuild();
        var discussion = DiscussionFactory.CreateDiscussion(discussionRequest);

        var requestMessageBuild = RequestMessageBuild.Build(discussionRequest);

        var userIdList = discussionRequest.UsersIds.Select(x => UserId.Create(x).Value);
        var users = Users.Create(userIdList).Value;
        var relationId = RelationId.Create(discussionRequest.RelationId).Value;

        var sut = Discussion.Create(users, relationId);

        var userId = UserId.Create(requestMessageBuild.UserId).Value;
        var text = Text.Create(requestMessageBuild.Text).Value;

        var createMessageResultDiscussion = discussion.CreateMessage(userId, text);
        var createMessageResultSut = sut.CreateMessage(userId, text);

        sut.Should().BeEquivalentTo(discussion,
            options =>
                options.Excluding(x => x.Id)
                    .ComparingByMembers<Discussion>()
                    .ComparingByMembers<Users>()
                    .ComparingByMembers<UserId>()
                    .Excluding(x => x.Messages).WithTracing());

        sut.Messages.Should().BeEquivalentTo(discussion.Messages,
            options =>
                options.Excluding(x => x.Id)
                    .Excluding(x => x.CreatedAt)
                    .ComparingByMembers<Message>().WithTracing());

        createMessageResultDiscussion.Should().BeEquivalentTo(createMessageResultSut);
        createMessageResultSut.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Discussion_create_message_return_error()
    {
        var discussionRequest = RequestDiscussionBuild.DiscussionBuild();
        var discussion = DiscussionFactory.CreateDiscussion(discussionRequest);

        var requestMessageBuild = RequestMessageBuild.Build(discussionRequest);

        var userIdList = discussionRequest.UsersIds.Select(x => UserId.Create(x).Value);
        var users = Users.Create(userIdList).Value;
        var relationId = RelationId.Create(discussionRequest.RelationId).Value;

        var sut = Discussion.Create(users, relationId);

        var someGuid = Guid.NewGuid();
        var userId = UserId.Create(someGuid).Value;
        var text = Text.Create(requestMessageBuild.Text).Value;

        var createMessageResultDiscussion = discussion.CreateMessage(userId, text);
        var createMessageResultSut = sut.CreateMessage(userId, text);

        sut.Should().BeEquivalentTo(discussion,
            options =>
                options.Excluding(x => x.Id)
                    .ComparingByMembers<Discussion>()
                    .ComparingByMembers<Users>()
                    .ComparingByMembers<UserId>()
                    .Excluding(x => x.Messages).WithTracing());

        sut.Messages.Should().BeEquivalentTo(discussion.Messages,
            options =>
                options.Excluding(x => x.Id)
                    .Excluding(x => x.CreatedAt)
                    .ComparingByMembers<Message>().WithTracing());

        createMessageResultDiscussion.IsSuccess.Should().BeFalse();
        createMessageResultSut.IsSuccess.Should().BeFalse();
    }
}