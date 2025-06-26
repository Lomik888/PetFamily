using FluentAssertions;
using PetFamily.Data.Tests.Builders;
using PetFamily.Data.Tests.Factories;
using PetFamily.Discussions.Domain.ValueObjects;
using PetFamily.Discussions.Domain.ValueObjects.Ids;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Discussions.Domain.UnitTests;

public class DiscussionRemoveMessageTests
{
    [Fact]
    public void Discussion_remove_message_return_valid_entity()
    {
        var discussionRequest = RequestDiscussionBuild.DiscussionBuild();
        var discussion = DiscussionFactory.CreateDiscussion(discussionRequest);

        var userIdList = discussionRequest.UsersIds.Select(x => UserId.Create(x).Value);
        var users = Users.Create(userIdList).Value;
        var relationId = RelationId.Create(discussionRequest.RelationId).Value;
        var sut = Discussion.Create(users, relationId);

        var requestMessageBuild = RequestMessageBuild.Build(discussionRequest);

        var userId = UserId.Create(requestMessageBuild.UserId).Value;
        var text = Text.Create(requestMessageBuild.Text).Value;

        discussion.CreateMessage(userId, text);
        sut.CreateMessage(userId, text);

        var messageIdDiscussion = discussion.Messages.First().Id;
        var messageIdSut = sut.Messages.First().Id;

        var removeMessageResultDiscussion = discussion.RemoveMessage(userId, messageIdDiscussion);
        var removeMessageResultSut = sut.RemoveMessage(userId, messageIdSut);

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

        removeMessageResultDiscussion.IsSuccess.Should().BeTrue();
        removeMessageResultSut.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Discussion_create_message_return_error_invalid_messageId()
    {
        var discussionRequest = RequestDiscussionBuild.DiscussionBuild();
        var discussion = DiscussionFactory.CreateDiscussion(discussionRequest);

        var userIdList = discussionRequest.UsersIds.Select(x => UserId.Create(x).Value);
        var users = Users.Create(userIdList).Value;
        var relationId = RelationId.Create(discussionRequest.RelationId).Value;
        var sut = Discussion.Create(users, relationId);

        var requestMessageBuild = RequestMessageBuild.Build(discussionRequest);

        var userId = UserId.Create(requestMessageBuild.UserId).Value;
        var text = Text.Create(requestMessageBuild.Text).Value;

        discussion.CreateMessage(userId, text);
        sut.CreateMessage(userId, text);

        var someGuid = Guid.NewGuid();
        var messageIdDiscussion = MessageId.Create(someGuid).Value;
        var messageIdSut = MessageId.Create(someGuid).Value;

        var removeMessageResultDiscussion = discussion.RemoveMessage(userId, messageIdDiscussion);
        var removeMessageResultSut = sut.RemoveMessage(userId, messageIdSut);

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

        removeMessageResultDiscussion.IsSuccess.Should().BeFalse();
        removeMessageResultSut.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Discussion_create_message_return_error_invalid_userId()
    {
        var discussionRequest = RequestDiscussionBuild.DiscussionBuild();
        var discussion = DiscussionFactory.CreateDiscussion(discussionRequest);

        var userIdList = discussionRequest.UsersIds.Select(x => UserId.Create(x).Value);
        var users = Users.Create(userIdList).Value;
        var relationId = RelationId.Create(discussionRequest.RelationId).Value;
        var sut = Discussion.Create(users, relationId);

        var requestMessageBuild = RequestMessageBuild.Build(discussionRequest);

        var userId = UserId.Create(requestMessageBuild.UserId).Value;
        var text = Text.Create(requestMessageBuild.Text).Value;

        discussion.CreateMessage(userId, text);
        sut.CreateMessage(userId, text);

        var someGuid = Guid.NewGuid();

        var someUserId = UserId.Create(someGuid).Value;

        var messageIdDiscussion = discussion.Messages.First().Id;
        var messageIdSut = sut.Messages.First().Id;

        var removeMessageResultDiscussion = discussion.RemoveMessage(someUserId, messageIdDiscussion);
        var removeMessageResultSut = sut.RemoveMessage(someUserId, messageIdSut);

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

        removeMessageResultDiscussion.IsSuccess.Should().BeFalse();
        removeMessageResultSut.IsSuccess.Should().BeFalse();
    }
}