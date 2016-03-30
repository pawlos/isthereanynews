using System.Linq;
using System.Web.Caching;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services.Implementation
{
    public class RssChannelsService : IRssChannelsService
    {
        private readonly IRssChannelsRepository channelsRepository;
        private readonly ISessionProvider sessionProvider;
        private readonly IRssChannelsSubscriptionsRepository channelsSubscriptionRepository;
        private readonly IUserRepository usersRepository;
        private readonly IRssEntriesToReadRepository rssEntriesToReadRepository;
        private readonly IUserAuthentication authentication;
        private readonly IRssChannelsSubscriptionsRepository rssSubscriptionRepository;
        private readonly ISessionProvider session;

        public RssChannelsService(
            IRssChannelsRepository channelsRepository,
            ISessionProvider sessionProvider,
            IRssChannelsSubscriptionsRepository channelsSubscriptionRepository,
            IUserRepository usersRepository,
            IRssEntriesToReadRepository rssEntriesToReadRepository,
            IUserAuthentication authentication,
            IRssChannelsSubscriptionsRepository rssSubscriptionRepository,
            ISessionProvider session)
        {
            this.channelsRepository = channelsRepository;
            this.sessionProvider = sessionProvider;
            this.channelsSubscriptionRepository = channelsSubscriptionRepository;
            this.usersRepository = usersRepository;
            this.rssEntriesToReadRepository = rssEntriesToReadRepository;
            this.authentication = authentication;
            this.rssSubscriptionRepository = rssSubscriptionRepository;
            this.session = session;
        }

        public RssChannelsIndexViewModel LoadAllChannels()
        {
            var loadAllChannels = this.channelsRepository.LoadAllChannels()
                .Select(c => new RssChannelWithStatisticsViewModel(
                    c.Id,
                    c.Created,
                    c.RssEntriesCount,
                    c.SubscriptionsCount,
                    c.Title,
                    c.Updated))
                .ToList();
            var viewmodel = new RssChannelsIndexViewModel(loadAllChannels);
            return viewmodel;
        }

        public RssChannelsDetailsViewModel Load(long id)
        {
            var channel = this.channelsRepository.Load(id);
            var viewmodel = new RssChannelsDetailsViewModel(channel);
            return viewmodel;
        }

        public RssChannelsMyViewModel LoadAllChannelsOfCurrentUser()
        {
            var currentUserId = this.sessionProvider.GetCurrentUserId();
            var rssSubscriptions = this.channelsSubscriptionRepository.LoadAllSubscriptionsForUser(currentUserId);
            this.rssEntriesToReadRepository.CopyRssThatWerePublishedAfterLastReadTimeToUser(currentUserId, rssSubscriptions);
            return new RssChannelsMyViewModel(rssSubscriptions);
        }

        public RssChannelIndexViewModel GetViewModelFormChannelId(long id)
        {
            var rssChannel = this.channelsRepository.LoadRssChannel(id);


            var rssChannelIndexViewModel = new RssChannelIndexViewModel
            {
                Name = rssChannel.Title,
                Added = rssChannel.Created,
                ChannelId = rssChannel.Id,
                Entries = rssChannel.RssEntries.Select(entry=>new RssEntryViewModel(entry)).ToList()
            };

            if (this.authentication.CurrentUserIsAuthenticated())
            {
                var userId = this.session.GetCurrentUserId();
                var subscriptionInfo = this.rssSubscriptionRepository.FindSubscriptionIdOfUserAndOfChannel(userId, id);
                rssChannelIndexViewModel.IsAuthenticatedUser = true;
                rssChannelIndexViewModel.SubscriptionInfo = new UserRssSubscriptionInfoViewModel(subscriptionInfo);
            }
            return rssChannelIndexViewModel;
        }
    }
}