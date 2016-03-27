using IsThereAnyNews.Dtos;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services
{
    public interface IRssSubscriptionService
    {
        RssSubscriptionIndexViewModel LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(long id);
        void MarkAllRssReadForSubscription(MarkReadForSubscriptionDto id);
        void MarkEntryViewed(long rssToReadId);
        void UnsubscribeCurrentUserFromChannelId(long subscriptionId);
        void SubscribeCurrentUserToChannel(long channelId);
    }
}