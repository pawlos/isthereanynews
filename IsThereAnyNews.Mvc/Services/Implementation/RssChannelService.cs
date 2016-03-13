using IsThereAnyNews.DataAccess;
using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.Mvc.ViewModels;

namespace IsThereAnyNews.Mvc.Services.Implementation
{
    public class RssChannelService : IRssChannelService
    {
        private readonly IRssChannelRepository rssChannelRepository;

        public RssChannelService() : this(new RssChannelRepository())
        {

        }

        public RssChannelService(IRssChannelRepository rssChannelRepository)
        {
            this.rssChannelRepository = rssChannelRepository;
        }

        public RssChannelIndexViewModel GetViewModelFormChannelId(long id)
        {
            var rssChannel = this.rssChannelRepository.LoadRssChannel(id);
            return new RssChannelIndexViewModel
            {
                Name = rssChannel.Title,
                Added = rssChannel.Created
            };
        }
    }
}