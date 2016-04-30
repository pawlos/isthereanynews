using IsThereAnyNews.Dtos;
using IsThereAnyNews.SharedData;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services
{
    public interface IRssSubscriptionService
    {
        RssSubscriptionIndexViewModel LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(StreamType streamType, long id);
        void MarkAllRssReadForSubscription(MarkReadForSubscriptionDto id);
        void MarkEntryViewed(long rssToReadId);
        void UnsubscribeCurrentUserFromChannelId(long subscriptionId);
        void SubscribeCurrentUserToChannel(long channelId);
    }
}