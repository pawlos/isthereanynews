using IsThereAnyNews.DataAccess;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services.Implementation
{
    public class RssSubscriptionService : IRssSubscriptionService
    {
        private readonly ISessionProvider sessionProvider;
        private readonly IRssChannelsSubscriptionsRepository rssSubscriptions;

        public RssSubscriptionService(IUserAuthentication authentication, ISessionProvider sessionProvider, IRssChannelsSubscriptionsRepository rssSubscriptions)
        {
            this.sessionProvider = sessionProvider;
            this.rssSubscriptions = rssSubscriptions;
        }

        public RssSubscriptionIndexViewModel LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(long subscriptionId)
        {
            var currentUserId = this.sessionProvider.GetCurrentUserId();
            var loadAllRssEntriesForUserAndChannel = this.rssSubscriptions.LoadAllRssEntriesForUserAndChannel(currentUserId, subscriptionId);
            var viewModel = new RssSubscriptionIndexViewModel(loadAllRssEntriesForUserAndChannel);
            return viewModel;
        }
    }
}