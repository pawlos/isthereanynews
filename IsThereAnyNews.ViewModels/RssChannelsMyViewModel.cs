using System.Collections.Generic;

namespace IsThereAnyNews.ViewModels
{
    public class RssChannelsMyViewModel
    {
        public List<RssChannelSubscriptionViewModel> ChannelsSubscriptions { get; set; }
        public List<ObservableUserEventsInformation> Users { get; set; }
    }
}