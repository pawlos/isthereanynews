using IsThereAnyNews.DataAccess;
using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services.Implementation
{
    public class RssChannelsService : IRssChannelsService
    {
        private readonly IRssChannelsRepository channelsesRepository;
        private readonly ISessionProvider sessionProvider;

        public RssChannelsService(
            IRssChannelsRepository channelsesRepository,
            IUserAuthentication authentication, 
            ISessionProvider sessionProvider)
        {
            this.channelsesRepository = channelsesRepository;
            this.sessionProvider = sessionProvider;
        }

        public RssChannelsIndexViewModel LoadAllChannels()
        {
            var loadAllChannels = this.channelsesRepository.LoadAllChannels();
            var viewmodel = new RssChannelsIndexViewModel(loadAllChannels);
            return viewmodel;
        }

        public RssChannelsDetailsViewModel Load(long id)
        {
            var channel = this.channelsesRepository.Load(id);
            var viewmodel = new RssChannelsDetailsViewModel(channel);
            return viewmodel;
        }

        public RssChannelsMyViewModel LoadAllChannelsOfCurrentUser()
        {
            var currentUserId = this.sessionProvider.GetCurrentUserId();
            var channels = this.channelsesRepository.LoadAllChannelsForUser(currentUserId);
            return new RssChannelsMyViewModel(channels);
        }
    }
}