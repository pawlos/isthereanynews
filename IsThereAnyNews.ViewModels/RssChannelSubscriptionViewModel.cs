using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.EntityFramework.Models.Entities;

namespace IsThereAnyNews.ViewModels
{
    public class RssChannelSubscriptionViewModel
    {
        public RssChannelSubscriptionViewModel(RssChannelSubscription subscription)
        {
            this.Id = subscription.Id;
            this.Title = subscription.Title;
            this.RssToRead = subscription.RssEntriesToRead.Count(x => !x.IsRead);
        }

        public int RssToRead { get; set; }

        public string ChannelUrl { get; set; }
        public string Title { get; set; }
        public long Id { get; set; }
    }
}