using IsThereAnyNews.DataAccess;
using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.Mvc.Services;
using IsThereAnyNews.Mvc.Services.Implementation;
using IsThereAnyNews.Mvc.ViewModels;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class RssChannelsService : IRssChannelService
    {
        private readonly IRssChannelRepository channelsRepository;
        private readonly IUserAuthentication authentication;
        private readonly ISessionProvider sessionProvider;

        public RssChannelsService() : 
            this(new RssChannelRepository(), 
                new UserAuthentication(), 
                new SessionProvider())
        {
        }

        public RssChannelsService(
            IRssChannelRepository channelsRepository,
            IUserAuthentication authentication, 
            ISessionProvider sessionProvider)
        {
            this.channelsRepository = channelsRepository;
            this.authentication = authentication;
            this.sessionProvider = sessionProvider;
        }

        public object LoadAllChannels()
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
            var channels = this.channelsRepository.LoadAllChannelsForUser(currentUserId);
            return new RssChannelsMyViewModel(channels);
        }
    }
}