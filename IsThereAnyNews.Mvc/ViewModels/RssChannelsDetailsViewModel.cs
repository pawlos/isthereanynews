using IsThereAnyNews.Mvc.Models;

namespace IsThereAnyNews.Mvc.ViewModels
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