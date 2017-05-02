using System.Linq;
using IsThereAnyNews.ViewModels.Subscriptions;

namespace IsThereAnyNews.ViewModels.RssChannel
{
    using System.Collections.Generic;

    public class RssChannelsMyViewModel
    {
        public RssChannelsMyViewModel()
        {
            this.ChannelsSubscriptions = new List<RssChannelSubscriptionViewModel>();
            this.Users = new List<ObservableUserEventsInformation>();
            this.ChannelEvents = new List<ISubscriptionViewModel>();
        }

        public List<RssChannelSubscriptionViewModel> ChannelsSubscriptions { get; set; }
        public List<ObservableUserEventsInformation> Users { get; set; }
        public List<ISubscriptionViewModel> ChannelEvents { get; set; }

        public ISubscriptionViewModel Creations { get; set; }
        public ISubscriptionViewModel Exceptions { get; set; }
        public ISubscriptionViewModel Updates { get; set; }

        public List<ISubscriptionViewModel> SubscriptionViewModels
        {
            get
            {
                var list = new List<ISubscriptionViewModel>();

                list.AddRange(this.ChannelsSubscriptions);
                list.AddRange(this.Users);
                list.AddRange(this.ChannelEvents);
                list.Add(this.Creations);
                list.Add(this.Exceptions);
                list.Add(this.Updates);

                return list.Where(x=>x.Count!="0"&&!string.IsNullOrEmpty(x.Count)).ToList();
            }
        }
    }
}