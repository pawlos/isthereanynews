using IsThereAnyNews.Mvc.ViewModels;

namespace IsThereAnyNews.Mvc.Controllers
{
    internal interface IRssChannelService
    {
        object LoadAllChannels();
        RssChannelsDetailsViewModel Load(long id);
        RssChannelsMyViewModel LoadAllChannelsOfCurrentUser();
    }
}