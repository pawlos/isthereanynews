namespace IsThereAnyNews.ViewModels.RssChannel
{
    using System.Collections.Generic;

    public class RssChannelsMyViewModel
    {
        public RssChannelsMyViewModel()
        {
            this.ChannelsSubscriptions = new List<RssChannelSubscriptionViewModel>();
            this.Users = new List<ObservableUserEventsInformation>();
            this.ChannelEvents = new List<ChannelEventViewModel>();
            this.Events = new AdminEventsViewModel();
        }

        public List<RssChannelSubscriptionViewModel> ChannelsSubscriptions { get; set; }
        public List<ObservableUserEventsInformation> Users { get; set; }
        public List<ChannelEventViewModel> ChannelEvents { get; set; }
        public AdminEventsViewModel Events { get; set; }

        public List<ISubscriptionViewModel> SubscriptionViewModels
        {
            get
            {
                var list = new List<ISubscriptionViewModel>();

                list.AddRange(this.ChannelsSubscriptions);
                list.AddRange(this.Users);
                list.AddRange(this.ChannelEvents);
                list.Add(this.Events.Creations);
                list.Add(this.Events.Exceptions);
                list.Add(this.Events.Updates);

                return list;
            }
        }
    }
}