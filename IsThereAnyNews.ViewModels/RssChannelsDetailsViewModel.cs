
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.ViewModels
{
    public class RssChannelsDetailsViewModel
    {
        public RssChannel Channel { get; set; }

        public RssChannelsDetailsViewModel(RssChannel channel)
        {
            Channel = channel;
        }
    }
}