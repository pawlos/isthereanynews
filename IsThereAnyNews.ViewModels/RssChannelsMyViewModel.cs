namespace IsThereAnyNews.ViewModels
{
    using System.Collections.Generic;

    public class RssChannelsMyViewModel
    {
        public List<RssChannelSubscriptionViewModel> ChannelsSubscriptions { get; set; }
        public List<ObservableUserEventsInformation> Users { get; set; }
    }
}