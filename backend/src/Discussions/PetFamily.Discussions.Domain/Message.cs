using CSharpFunctionalExtensions;
using PetFamily.Discussions.Domain.ValueObjects;
using PetFamily.Discussions.Domain.ValueObjects.Ids;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Discussions.Domain;

public sealed class Message : Entity<MessageId>
{
    public Text Text { get; private set; }
    public CreatedAt CreatedAt { get; private set; }
    public UserId UserId { get; private set; }
    public bool IsEdited { get; private set; }

    private Message()
    {
    }

    private Message(
        MessageId messageId,
        Text text,
        CreatedAt createdAt,
        UserId userId,
        bool isEdited)
    {
        Id = messageId;
        Text = text;
        CreatedAt = createdAt;
        UserId = userId;
        IsEdited = isEdited;
    }

    internal void Edit(Text text)
    {
        this.Text = text;
        this.IsEdited = true;
    }

    internal static Message Create(Text text, UserId userId)
    {
        var id = MessageId.Create();
        var createdAt = CreatedAt.UtcNow();
        var isEdited = false;

        var message = new Message(
            id,
            text,
            createdAt,
            userId,
            isEdited);

        return message;
    }
}