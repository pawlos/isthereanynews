using System;
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

        public RssChannelsService(
            IRssChannelsRepository channelsRepository,
            ISessionProvider sessionProvider,
            IRssChannelsSubscriptionsRepository channelsSubscriptionRepository,
            IUserRepository usersRepository,
            IRssEntriesToReadRepository rssEntriesToReadRepository)
        {
            this.channelsRepository = channelsRepository;
            this.sessionProvider = sessionProvider;
            this.channelsSubscriptionRepository = channelsSubscriptionRepository;
            this.usersRepository = usersRepository;
            this.rssEntriesToReadRepository = rssEntriesToReadRepository;
        }

        public RssChannelsIndexViewModel LoadAllChannels()
        {
            var loadAllChannels = this.channelsRepository.LoadAllChannels();
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
    }
}