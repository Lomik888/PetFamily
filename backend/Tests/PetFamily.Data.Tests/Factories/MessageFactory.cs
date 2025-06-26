using PetFamily.Data.Tests.Requests;
using PetFamily.Discussions.Domain;
using PetFamily.Discussions.Domain.ValueObjects;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Data.Tests.Factories;

public static class MessageFactory
{
    public static Discussion CreatePet(Discussion discussion, RequestMessage requestMessage)
    {
        var text = Text.Create(requestMessage.Text).Value;
        var userId = UserId.Create(requestMessage.UserId).Value;

        var createMessageResult = discussion.CreateMessage(userId, text);
        if (createMessageResult.IsSuccess == false)
        {
            throw new Exception($"can't create pet {createMessageResult.Error}");
        }

        return discussion;
    }
}