using System.Collections.Generic;

namespace IsThereAnyNews.ViewModels.RssChannel
{
    public class RssChannelsMyViewModel
    {
        public List<RssChannelSubscriptionViewModel> ChannelsSubscriptions { get; set; }
        public List<ObservableUserEventsInformation> Users { get; set; }
        public List<ChannelEventViewModel> ChannelEvents { get; set; }
    }
}