using FluentAssertions;
using PetFamily.Data.Tests.Builders;
using PetFamily.Data.Tests.Factories;
using PetFamily.Discussions.Domain.ValueObjects;
using PetFamily.Discussions.Domain.ValueObjects.Ids;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Discussions.Domain.UnitTests;

public class DiscussionEditMessageTetst
{
    [Fact]
    public void Discussion_edit_message_return_valid_entity()
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

        var newText = Text.Create("Всем камышам пис, остальным соболезную").Value;

        var editMessageResultDiscussion = discussion.EditMessage(userId, messageIdDiscussion, newText);
        var editMessageResultSut = sut.EditMessage(userId, messageIdSut, newText);

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

        editMessageResultDiscussion.IsSuccess.Should().BeTrue();
        editMessageResultSut.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Discussion_edit_message_return_error_invalid_messageId()
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

        var newText = Text.Create("Всем камышам пис, остальным соболезную").Value;

        var editMessageResultDiscussion = discussion.EditMessage(userId, messageIdDiscussion, newText);
        var editMessageResultSut = sut.EditMessage(userId, messageIdSut, newText);

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

        editMessageResultDiscussion.IsSuccess.Should().BeFalse();
        editMessageResultSut.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Discussion_edit_message_return_error_invalid_userId()
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

        var newText = Text.Create("Всем камышам пис, остальным соболезную").Value;

        var editMessageResultDiscussion = discussion.EditMessage(someUserId, messageIdDiscussion, newText);
        var editMessageResultSut = sut.EditMessage(someUserId, messageIdSut, newText);

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

        editMessageResultDiscussion.IsSuccess.Should().BeFalse();
        editMessageResultSut.IsSuccess.Should().BeFalse();
    }
}