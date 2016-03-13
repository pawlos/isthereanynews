using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services
{
    public interface IRssChannelService
    {
        RssChannelIndexViewModel GetViewModelFormChannelId(long id);
    }
}