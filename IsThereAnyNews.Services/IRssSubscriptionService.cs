namespace IsThereAnyNews.Services
{
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public interface IRssSubscriptionService
    {
        RssSubscriptionIndexViewModel LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(StreamType streamType, long id, ShowReadEntries showReadEntries);

        void MarkAllRssReadForSubscription(MarkReadForSubscriptionDto id);

        void UnsubscribeCurrentUserFromChannelId(long subscriptionId);

        void SubscribeCurrentUserToChannel(long channelId);

        void MarkRead(MarkReadDto dto);

        void SubscribeCurrentUserToChannel(AddChannelDto channelId);

        void MarkClicked(MarkClickedDto dto);

        void MarkEntriesRead(MarkReadDto dto);

        void MarkEntriesSkipped(MarkSkippedDto model);
    }
}