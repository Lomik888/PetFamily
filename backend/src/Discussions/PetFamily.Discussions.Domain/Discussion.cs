using CSharpFunctionalExtensions;
using PetFamily.Discussions.Domain.ValueObjects;
using PetFamily.Discussions.Domain.ValueObjects.Ids;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Discussions.Domain;

public sealed class Discussion : Entity<DiscussionId>
{
    private readonly List<Message> _messages = [];

    public Users Users { get; private set; }
    public RelationId RelationId { get; private set; }
    public bool Resolved { get; private set; }
    public IReadOnlyList<Message> Messages => _messages;

    private Discussion()
    {
    }

    private Discussion(
        DiscussionId discussionId,
        Users users,
        RelationId relationId,
        bool resolved)
    {
        Id = discussionId;
        Users = users;
        RelationId = relationId;
        Resolved = resolved;
    }

    public static Discussion Create(
        Users users,
        RelationId relationId)
    {
        var id = DiscussionId.Create();
        var resolved = false;

        var discussion = new Discussion(
            id,
            users,
            relationId,
            resolved);

        return discussion;
    }

    public void Resolve()
    {
        this.Resolved = true;
    }

    public void UnResolve()
    {
        this.Resolved = false;
    }

    public UnitResult<Error> EditMessage(UserId userId, MessageId messageId, Text text)
    {
        var message = Messages.SingleOrDefault(x => x.Id == messageId && x.UserId == userId);
        if (message == null)
        {
            var error = ErrorsPreform.General.Validation($"User {userId.Value} is not owned by this message.");
            return error;
        }

        message.Edit(text);

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> RemoveMessage(UserId userId, MessageId messageId)
    {
        var message = Messages.SingleOrDefault(x => x.Id == messageId && x.UserId == userId);
        if (message == null)
        {
            var error = ErrorsPreform.General.Validation($"User {userId.Value} is not owned by this message.");
            return error;
        }

        _messages.Remove(message);

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> CreateMessage(UserId userId, Text text)
    {
        var userExistsInDiscussion = Users.Items.Contains(userId);
        if (userExistsInDiscussion == false)
        {
            var error = ErrorsPreform.General.Validation($"User {userId.Value} does not exist in discussion");
            return error;
        }

        var message = Message.Create(text, userId);
        _messages.Add(message);

        return UnitResult.Success<Error>();
    }
}