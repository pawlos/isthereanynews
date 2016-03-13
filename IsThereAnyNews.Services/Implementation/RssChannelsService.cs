using IsThereAnyNews.DataAccess;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services.Implementation
{
    public class RssChannelsService : IRssChannelsService
    {
        private readonly IRssChannelsRepository channelsRepository;
        private readonly ISessionProvider sessionProvider;
        private readonly IRssChannelsSubscriptionsRepository channelsSubscription;

        public RssChannelsService(
            IRssChannelsRepository channelsRepository, 
            ISessionProvider sessionProvider, 
            IRssChannelsSubscriptionsRepository channelsSubscription)
        {
            this.channelsRepository = channelsRepository;
            this.sessionProvider = sessionProvider;
            this.channelsSubscription = channelsSubscription;
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
            var rssSubscriptions = this.channelsSubscription.LoadAllSubscriptionsWithChannelsForUser(currentUserId);
            return new RssChannelsMyViewModel(rssSubscriptions);
        }
    }
}