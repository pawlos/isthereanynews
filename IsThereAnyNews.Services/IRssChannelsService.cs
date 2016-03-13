using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services
{
    public interface IRssChannelsService
    {
        RssChannelsIndexViewModel LoadAllChannels();
        RssChannelsMyViewModel LoadAllChannelsOfCurrentUser();
    }
}