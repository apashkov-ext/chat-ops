using System.Reflection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Passport;
using Telegram.Bot.Types.Payments;
using Xunit.Sdk;

namespace ChatOps.Api.Tests.TestData;

internal sealed class WrongMessageDataAttribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        yield return [new Message()];

        yield return
        [
            new Message { Animation = new Animation() }
        ];

        yield return
        [
            new Message { Audio = new Audio() }
        ];

        yield return
        [
            new Message { Document = new Document() }
        ];

        yield return
        [
            new Message { PaidMedia = new PaidMediaInfo() }
        ];

        yield return
        [
            new Message { Photo = [] }
        ];

        yield return
        [
            new Message { Sticker = new Sticker() }
        ];

        yield return
        [
            new Message { Story = new Story() }
        ];

        yield return
        [
            new Message { Video = new Video() }
        ];

        yield return
        [
            new Message { VideoNote = new VideoNote() }
        ];

        yield return
        [
            new Message { Voice = new Voice() }
        ];

        yield return
        [
            new Message { Checklist = new Checklist() }
        ];

        yield return
        [
            new Message { Contact = new Contact() }
        ];

        yield return
        [
            new Message { Dice = new Dice() }
        ];

        yield return
        [
            new Message { Game = new Game() }
        ];

        yield return
        [
            new Message { Poll = new Poll() }
        ];

        yield return
        [
            new Message { Venue = new Venue() }
        ];

        yield return
        [
            new Message { Location = new Location() }
        ];

        yield return
        [
            new Message { NewChatMembers = [] }
        ];

        yield return
        [
            new Message { LeftChatMember = new User() }
        ];

        yield return
        [
            new Message { NewChatTitle = "title" }
        ];

        yield return
        [
            new Message { NewChatPhoto = [] }
        ];

        yield return
        [
            new Message { DeleteChatPhoto = true }
        ];

        yield return
        [
            new Message { GroupChatCreated = true }
        ];

        yield return
        [
            new Message { SupergroupChatCreated = true }
        ];

        yield return
        [
            new Message { ChannelChatCreated = true }
        ];

        yield return
        [
            new Message { MessageAutoDeleteTimerChanged = new MessageAutoDeleteTimerChanged() }
        ];

        yield return
        [
            new Message { MigrateToChatId = 1L }
        ];

        yield return
        [
            new Message { MigrateFromChatId = 1L }
        ];

        yield return
        [
            new Message { PinnedMessage = new Message() }
        ];

        yield return
        [
            new Message { Invoice = new Invoice() }
        ];

        yield return
        [
            new Message { SuccessfulPayment = new SuccessfulPayment() }
        ];

        yield return
        [
            new Message { RefundedPayment = new RefundedPayment() }
        ];

        yield return
        [
            new Message { UsersShared = new UsersShared() }
        ];

        yield return
        [
            new Message { ChatShared = new ChatShared() }
        ];

        yield return
        [
            new Message { Gift = new GiftInfo() }
        ];

        yield return
        [
            new Message { UniqueGift = new UniqueGiftInfo() }
        ];

        yield return
        [
            new Message { ConnectedWebsite = "example.com" }
        ];

        yield return
        [
            new Message { WriteAccessAllowed = new WriteAccessAllowed() }
        ];

        yield return
        [
            new Message { PassportData = new PassportData() }
        ];

        yield return
        [
            new Message { ProximityAlertTriggered = new ProximityAlertTriggered() }
        ];

        yield return
        [
            new Message { BoostAdded = new ChatBoostAdded() }
        ];

        yield return
        [
            new Message { ChatBackgroundSet = new ChatBackground() }
        ];

        yield return
        [
            new Message { ChecklistTasksDone = new ChecklistTasksDone() }
        ];

        yield return
        [
            new Message { ChecklistTasksAdded = new ChecklistTasksAdded() }
        ];

        yield return
        [
            new Message { DirectMessagePriceChanged = new DirectMessagePriceChanged() }
        ];

        yield return
        [
            new Message { ForumTopicCreated = new ForumTopicCreated() }
        ];

        yield return
        [
            new Message { ForumTopicEdited = new ForumTopicEdited() }
        ];

        yield return
        [
            new Message { ForumTopicClosed = new ForumTopicClosed() }
        ];

        yield return
        [
            new Message { ForumTopicReopened = new ForumTopicReopened() }
        ];

        yield return
        [
            new Message { GeneralForumTopicHidden = new GeneralForumTopicHidden() }
        ];

        yield return
        [
            new Message { GeneralForumTopicUnhidden = new GeneralForumTopicUnhidden() }
        ];

        yield return
        [
            new Message { GiveawayCreated = new GiveawayCreated() }
        ];

        yield return
        [
            new Message { Giveaway = new Giveaway() }
        ];

        yield return
        [
            new Message { GiveawayWinners = new GiveawayWinners() }
        ];

        yield return
        [
            new Message { GiveawayCompleted = new GiveawayCompleted() }
        ];

        yield return
        [
            new Message { PaidMessagePriceChanged = new PaidMessagePriceChanged() }
        ];

        yield return
        [
            new Message { SuggestedPostApproved = new SuggestedPostApproved() }
        ];

        yield return
        [
            new Message { SuggestedPostApprovalFailed = new SuggestedPostApprovalFailed() }
        ];

        yield return
        [
            new Message { SuggestedPostDeclined = new SuggestedPostDeclined() }
        ];

        yield return
        [
            new Message { SuggestedPostPaid = new SuggestedPostPaid() }
        ];

        yield return
        [
            new Message { SuggestedPostRefunded = new SuggestedPostRefunded() }
        ];

        yield return
        [
            new Message { VideoChatScheduled = new VideoChatScheduled() }
        ];

        yield return
        [
            new Message { VideoChatStarted = new VideoChatStarted() }
        ];

        yield return
        [
            new Message { VideoChatEnded = new VideoChatEnded() }
        ];

        yield return
        [
            new Message { VideoChatParticipantsInvited = new VideoChatParticipantsInvited() }
        ];

        yield return
        [
            new Message { WebAppData = new WebAppData() }
        ];
    }
}