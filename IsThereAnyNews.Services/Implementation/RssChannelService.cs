using IsThereAnyNews.DataAccess;
using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services.Implementation
{
    public class RssChannelService : IRssChannelService
    {
        private readonly IRssChannelRepository rssChannelRepository;

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