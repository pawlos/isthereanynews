using IsThereAnyNews.Mvc.ViewModels;

namespace IsThereAnyNews.Mvc.Services
{
    public interface IRssChannelService
    {
        RssChannelIndexViewModel GetViewModelFormChannelId(long id);
    }
}