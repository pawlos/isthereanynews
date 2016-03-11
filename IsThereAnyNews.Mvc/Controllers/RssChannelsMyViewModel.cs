using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class RssChannelsMyViewModel
    {
        public RssChannelsMyViewModel(List<RssChannel> channels)
        {
            this.Channels = channels.Select(channel => new RssChannelViewModel(channel)).ToList();
        }

        public List<RssChannelViewModel> Channels { get; }
    }
}