using IsThereAnyNews.Mvc.ViewModels;

namespace IsThereAnyNews.Mvc.Services
{
    public interface IRssChannelsService
    {
        RssChannelsIndexViewModel LoadAllChannels();
        RssChannelsMyViewModel LoadAllChannelsOfCurrentUser();
    }
}