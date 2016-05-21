using IsThereAnyNews.Dtos;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services
{
    public interface IRssChannelsService
    {
        RssChannelsIndexViewModel LoadAllChannels();
        RssChannelsMyViewModel LoadAllChannelsOfCurrentUser();
        RssChannelIndexViewModel GetViewModelFormChannelId(long id);
        void CreateNewChannelIfNotExists(AddChannelDto dto);
    }
}