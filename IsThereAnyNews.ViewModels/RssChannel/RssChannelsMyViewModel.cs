namespace IsThereAnyNews.ViewModels.RssChannel
{
    using System.Collections.Generic;

    public class RssChannelsMyViewModel
    {
        public List<RssChannelSubscriptionViewModel> ChannelsSubscriptions { get; set; }
        public List<ObservableUserEventsInformation> Users { get; set; }
        public List<ChannelEventViewModel> ChannelEvents { get; set; }
        public AdminEventsViewModel Events { get; set; }
    }
}