using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services
{
    public interface IRssSubscriptionService
    {
        RssSubscriptionIndexViewModel LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(long id);
    }
}