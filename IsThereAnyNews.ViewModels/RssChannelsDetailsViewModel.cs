
using IsThereAnyNews.EntityFramework.Models;
using IsThereAnyNews.EntityFramework.Models.Entities;

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