using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.EntityFramework.Models;
using IsThereAnyNews.EntityFramework.Models.Entities;

namespace IsThereAnyNews.ViewModels
{
    public class RssChannelsMyViewModel
    {
        public RssChannelsMyViewModel(List<RssChannelSubscription> subscriptions)
        {
            this.ChannelsSubscriptions = subscriptions
                .Select(subscription => 
                new RssChannelSubscriptionViewModel(subscription)).ToList();
        }

        public List<RssChannelSubscriptionViewModel> ChannelsSubscriptions { get; set; }
        public List<ObservableUserEventsInformation> Users { get; set; }
    }
}